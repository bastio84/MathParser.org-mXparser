/*
 * @(#)NumericalAnalysis.cs        2.0.0    2015-12-29
 * 
 * You may use this software under the condition of "Simplified BSD License"
 * 
 * Copyright 2010-2015 MARIUSZ GROMADA. All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 * 
 *    1. Redistributions of source code must retain the above copyright notice, this list of
 *       conditions and the following disclaimer.
 * 
 *    2. Redistributions in binary form must reproduce the above copyright notice, this list
 *       of conditions and the following disclaimer in the documentation and/or other materials
 *       provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED BY <MARIUSZ GROMADA> ``AS IS'' AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 * The views and conclusions contained in the software and documentation are those of the
 * authors and should not be interpreted as representing official policies, either expressed
 * or implied, of MARIUSZ GROMADA.
 * 
 * If you have any questions/bugs feel free to contact:
 * 
 *     Mariusz Gromada
 *     mariusz.gromada@mathspace.pl
 *     http://mathspace.pl/
 *     http://mathparser.org/
 *     http://github.com/mariuszgromada/mXparser/
 *     http://mariuszgromada.github.io/mXparser/
 *     http://mxparser.sourceforge.net/
 * 
 *                              Asked if he believes in one God, a mathematician answered: 
 *                              "Yes, up to isomorphism."  
 */

using System;

namespace org.mariuszgromada.math.mxparser.mathcollection {


    /**
     * NumericalAnalysis - numerical integration, differentiation, etc... 
     * 
     * @author         <b>Mariusz Gromada</b><br/>
     *                 <a href="mailto:mariusz.gromada@mathspace.pl">mariusz.gromada@mathspace.pl</a><br>
     *                 <a href="http://mathspace.pl/" target="_blank">MathSpace.pl</a><br>
     *                 <a href="http://mathparser.org/" target="_blank">MathParser.org - mXparser project page</a><br>
     *                 <a href="http://github.com/mariuszgromada/mXparser/" target="_blank">mXparser on GitHub</a><br>
     *                 <a href="http://mariuszgromada.github.io/mXparser/" target="_blank">mXparser on GitHub pages</a><br>
     *                 <a href="http://mxparser.sourceforge.net/" target="_blank">mXparser on SourceForge/</a><br>
     *                         
     * @version        2.0.0
     */
    [CLSCompliant(true)]
    public sealed class NumericalAnalysis {

	    /**
	     * Derivative type specification
	     */
	    public const int LEFT_DERIVATIVE = 1;
	    public const int RIGHT_DERIVATIVE = 2;
	    public const int GENERAL_DERIVATIVE = 3;
    	
	    /**
	     * Trapezoid numerical integration
	     * 
	     * @param      f                   the expression
	     * @param      x                   the argument
	     * @param      a                   form a ...
	     * @param      b                   ... to b 
	     * @param      eps                 the epsilon (error)
	     * @param      maxSteps            the maximum number of steps
	     * 
	     * @return     Integral value as double.
	     * 
	     * @see        Expression
	     */
	    public static double integralTrapezoid(Expression f, Argument x, double a, double b,
			    double eps, int maxSteps) {
    		
		    double h = 0.5*(b-a);
		    double s = mXparser.getFunctionValue(f, x, a)
					    + mXparser.getFunctionValue(f, x, b)
					    + 2 * mXparser.getFunctionValue(f, x, a + h);
    		
		    double intF = s*h*0.5;
		    double intFprev = 0;
		    double t = a;
		    int i, j;
		    int n = 1;
    		
		    for (i = 1; i <= maxSteps; i++) {
			    n += n;
			    t = a + 0.5*h;
			    intFprev = intF;
    			
			    for (j = 1; j <= n; j++) {
				    s += 2 * mXparser.getFunctionValue(f, x, t);
				    t += h;
			    }
			    h *= 0.5;
    			
			    intF = s*h*0.5;
    			

    			
			    if (Math.Abs(intF - intFprev) <= eps)
				    return intF;
    			
		    }
    		
		    return intF;

	    }	
    	
    	
	    /**
	     * Numerical derivative at x = x0
	     * 
	     * @param      f                   the expression
	     * @param      x                   the argument
	     * @param      x0                  at point x = x0
	     * @param      derType             derivative type (LEFT_DERIVATIVE, RIGHT_DERIVATIVE,
	     *                                 GENERAL_DERIVATIVE
	     * @param      eps                 the epsilon (error)
	     * @param      maxSteps            the maximum number of steps
	     * 
	     * @return     Derivative value as double.
	     * 
	     * @see        Expression
	     */
	    public static double derivative(Expression f, Argument x, double x0,
			    int derType, double eps, int maxSteps) {

		    const double START_DX = 0.1;
    		
		    int step = 0;
		    double error = 2*eps;
		    double y0 = 0;
    		
		    double derF = 0;
		    double derFprev = 0;
    		
		    double dx = 0;
		    if (derType == LEFT_DERIVATIVE)
			    dx = -START_DX;
		    else
			    dx = START_DX;
    		
		    double dy = 0;
		    if ( (derType == LEFT_DERIVATIVE) || (derType == RIGHT_DERIVATIVE) ) {
			    y0 = mXparser.getFunctionValue(f, x, x0);
			    dy = mXparser.getFunctionValue(f, x, x0+dx) - y0;
			    derF = dy/dx;
		    } else
			    derF = ( mXparser.getFunctionValue(f, x, x0+dx) - mXparser.getFunctionValue(f, x, x0-dx) ) / (2*dx);
    		
		    do {
    			
			    derFprev = derF;
    			
			    dx = dx/2.0;

			    if ( (derType == LEFT_DERIVATIVE) || (derType == RIGHT_DERIVATIVE) ) {
				    dy = mXparser.getFunctionValue(f, x, x0+dx) - y0;
				    derF = dy/dx;
			    } else
				    derF = ( mXparser.getFunctionValue(f, x, x0+dx) - mXparser.getFunctionValue(f, x, x0-dx) ) / (2*dx);
    						
			    error = Math.Abs(derF - derFprev);
    			
			    step++;
    			
		    } while ( (step < maxSteps) && ( (error > eps) || Double.IsNaN(derF) ));
    				
		    return derF;
    		
	    }
    	
    	
	    /**
	     * Numerical n-th derivative at x = x0 (you should avoid calculation
	     * of derivatives with order higher than 2).
	     * 
	     * @param      f                   the expression
	     * @param      n                   the deriviative order
	     * @param      x                   the argument
	     * @param      x0                  at point x = x0
	     * @param      derType             derivative type (LEFT_DERIVATIVE, RIGHT_DERIVATIVE,
	     *                                 GENERAL_DERIVATIVE
	     * @param      eps                 the epsilon (error)
	     * @param      maxSteps            the maximum number of steps
	     * 
	     * @return     Derivative value as double.
	     * 
	     * @see        Expression
	     */
	    public static double derivativeNth(Expression f, double n, Argument x,
			    double x0, int derType, double eps, int maxSteps) {

		    n = Math.Round(n);
		    int step = 0;
		    double error = 2*eps;
    		
		    double derFprev = 0;
		    double dx = 0.01;		
		    double derF = 0;
    		
		    if (derType == RIGHT_DERIVATIVE)
			    for (int i = 1; i <= n; i++)
				    derF += MathFunctions.binomCoeff(-1,n-i) * MathFunctions.binomCoeff(n,i) * mXparser.getFunctionValue(f,x,x0+i*dx);
		    else
			    for (int i = 1; i <= n; i++)
				    derF += MathFunctions.binomCoeff(-1,i)*MathFunctions.binomCoeff(n,i) * mXparser.getFunctionValue(f,x,x0-i*dx);
    			
		    derF = derF / Math.Pow(dx, n);
    		
		    do {
    			
			    derFprev = derF;
    			
			    dx = dx/2.0;

			    derF = 0;
    			
			    if (derType == RIGHT_DERIVATIVE)
				    for (int i = 1; i <= n; i++)
					    derF += MathFunctions.binomCoeff(-1,n-i) * MathFunctions.binomCoeff(n,i) * mXparser.getFunctionValue(f,x,x0+i*dx);
			    else
				    for (int i = 1; i <= n; i++)
					    derF += MathFunctions.binomCoeff(-1,i)*MathFunctions.binomCoeff(n,i) * mXparser.getFunctionValue(f,x,x0-i*dx);
    			
			    derF = derF / Math.Pow(dx, n);
    			
			    error = Math.Abs(derF - derFprev);
    			
			    step++;
    			
		    } while ( (step < maxSteps) && ( (error > eps) || Double.IsNaN(derF) ));
    				
		    return derF;

	    }		
    	
    }

}