﻿// Accord Statistics Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2012
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace Accord.Statistics.Models.Regression.Linear
{
    using System;
    using System.Text;
    using Accord.Math.Decompositions;

    /// <summary>
    ///   Multiple Linear Regression.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    ///   In multiple linear regression, the model specification is that the dependent
    ///   variable, denoted y_i, is a linear combination of the parameters (but need not
    ///   be linear in the independent x_i variables). As the linear regression has a
    ///   closed form solution, the regression coefficients can be computed by calling
    ///   the <see cref="Regress"/> method only once.</para>
    /// </remarks>
    /// 
    /// <example>
    ///  <para>
    ///   The following example shows how to fit a multiple linear regression model
    ///   to model a plane as an equation in the form ax + by + c = z. </para>
    ///   
    ///   <code>
    ///   // We will try to model a plane as an equation in the form
    ///   // "ax + by + c = z". We have two input variables (x and y)
    ///   // and we will be trying to find two parameters a and b and 
    ///   // an intercept term c.
    ///   
    ///   // Create a multiple linear regression for two input and an intercept
    ///   MultipleLinearRegression target = new MultipleLinearRegression(2, true);
    ///   
    ///   // Now suppose we have some points
    ///   double[][] inputs = 
    ///   {
    ///       new double[] { 1, 1 },
    ///       new double[] { 0, 1 },
    ///       new double[] { 1, 0 },
    ///       new double[] { 0, 0 },
    ///   };
    ///   
    ///   // located in the same Z (z = 1)
    ///   double[] outputs = { 1, 1, 1, 1 };
    ///   
    ///   
    ///   // Now we will try to fit a regression model
    ///   double error = target.Regress(inputs, outputs);
    ///   
    ///   // As result, we will be given the following:
    ///   double a = target.Coefficients[0]; // a = 0
    ///   double b = target.Coefficients[1]; // b = 0
    ///   double c = target.Coefficients[2]; // c = 1
    ///   
    ///   // Now, considering we were trying to find a plane, which could be
    ///   // described by the equation ax + by + c = z, and we have found the
    ///   // aforementioned coefficients, we can conclude the plane we were
    ///   // trying to find is giving by the equation:
    ///   //
    ///   //   ax + by + c = z
    ///   //     -> 0x + 0y + 1 = z
    ///   //     -> 1 = z.
    ///   //
    ///   // The plane containing the aforementioned points is, in fact,
    ///   // the plane given by z = 1.
    ///   </code>
    /// </example>
    /// 
    [Serializable]
    public class MultipleLinearRegression : ILinearRegression
    {

        private double[] coefficients;
        private bool insertConstant;


        /// <summary>
        ///   Creates a new Multiple Linear Regression.
        /// </summary>
        /// 
        /// <param name="inputs">The number of inputs for the regression.</param>
        /// 
        public MultipleLinearRegression(int inputs)
            : this(inputs, false)
        {
        }

        /// <summary>
        ///   Creates a new Multiple Linear Regression.
        /// </summary>
        /// 
        /// <param name="inputs">The number of inputs for the regression.</param>
        /// <param name="intercept">True to use an intercept term, false otherwise. Default is false.</param>
        /// 
        public MultipleLinearRegression(int inputs, bool intercept)
        {
            if (intercept) inputs++;
            this.coefficients = new double[inputs];
            this.insertConstant = intercept;
        }


        /// <summary>
        ///   Gets the coefficients used by the regression model. If the model
        ///   contains an intercept term, it will be in the end of the vector.
        /// </summary>
        /// 
        public double[] Coefficients
        {
            get { return coefficients; }
        }

        /// <summary>
        ///   Gets the number of inputs for the regression model.
        /// </summary>
        /// 
        public int Inputs
        {
            get { return coefficients.Length; }
        }

        /// <summary>
        ///   Performs the regression using the input vectors and output
        ///   data, returning the sum of squared errors of the fit.
        /// </summary>
        /// 
        /// <param name="inputs">The input vectors to be used in the regression.</param>
        /// <param name="outputs">The output values for each input vector.</param>
        /// <returns>The Sum-Of-Squares error of the regression.</returns>
        /// 
        public virtual double Regress(double[][] inputs, double[] outputs)
        {
            if (inputs.Length != outputs.Length)
                throw new ArgumentException("Number of input and output samples does not match", "outputs");

            int N = inputs[0].Length;     // inputs
            int M = inputs.Length;        // points

            if (insertConstant) N++;

            double[] B = new double[N];
            double[,] V = new double[N, N];


            // Compute V and B matrices
            for (int i = 0; i < N; i++)
            {
                // Least Squares Matrix
                for (int j = 0; j < N; j++)
                {
                    for (int k = 0; k < M; k++)
                    {
                        if (insertConstant)
                        {
                            double a = (i == N - 1) ? 1 : inputs[k][i];
                            double b = (j == N - 1) ? 1 : inputs[k][j];

                            V[i, j] += a * b;
                        }
                        else
                        {
                            V[i, j] += inputs[k][i] * inputs[k][j];
                        }
                    }
                }

                // Function to minimize
                for (int k = 0; k < M; k++)
                {
                    if (insertConstant && (i == N - 1))
                    {
                        B[i] += outputs[k];
                    }
                    else
                    {
                        B[i] += inputs[k][i] * outputs[k];
                    }
                }
            }


            // Solve V*C = B to find C (the coefficients)
            coefficients = new SingularValueDecomposition(V).Solve(B);

            // Calculate Sum-Of-Squares error
            double error = 0.0;
            double e;
            for (int i = 0; i < outputs.Length; i++)
            {
                e = outputs[i] - Compute(inputs[i]);
                error += e * e;
            }

            return error;
        }

        /// <summary>
        ///   Gets the coefficient of determination, as known as R² (r-squared).
        /// </summary>
        /// <remarks>
        ///   <para>
        ///    The coefficient of determination is used in the context of statistical models
        ///    whose main purpose is the prediction of future outcomes on the basis of other
        ///    related information. It is the proportion of variability in a data set that
        ///    is accounted for by the statistical model. It provides a measure of how well
        ///    future outcomes are likely to be predicted by the model.</para>
        ///   <para>
        ///    The R² coefficient of determination is a statistical measure of how well the
        ///    regression line approximates the real data points. An R² of 1.0 indicates
        ///    that the regression line perfectly fits the data.</para> 
        /// </remarks>
        /// <returns>The R² (r-squared) coefficient for the given data.</returns>
        public double CoefficientOfDetermination(double[][] inputs, double[] outputs)
        {
            return CoefficientOfDetermination(inputs, outputs, false);
        }

        /// <summary>
        ///   Gets the coefficient of determination, as known as R² (r-squared).
        /// </summary>
        /// <remarks>
        ///   <para>
        ///    The coefficient of determination is used in the context of statistical models
        ///    whose main purpose is the prediction of future outcomes on the basis of other
        ///    related information. It is the proportion of variability in a data set that
        ///    is accounted for by the statistical model. It provides a measure of how well
        ///    future outcomes are likely to be predicted by the model.</para>
        ///   <para>
        ///    The R² coefficient of determination is a statistical measure of how well the
        ///    regression line approximates the real data points. An R² of 1.0 indicates
        ///    that the regression line perfectly fits the data.</para> 
        /// </remarks>
        /// <returns>The R² (r-squared) coefficient for the given data.</returns>
        public double CoefficientOfDetermination(double[][] inputs, double[] outputs, bool adjust)
        {
            // R-squared = 100 * SS(regression) / SS(total)

            int N = inputs.Length;
            int P = coefficients.Length - 1;
            double SSe = 0.0;
            double SSt = 0.0;
            double avg = 0.0;
            double d;

            // Calculate output mean
            for (int i = 0; i < N; i++)
                avg += outputs[i];
            avg /= inputs.Length;

            // Calculate SSe and SSt
            for (int i = 0; i < N; i++)
            {
                d = outputs[i] - Compute(inputs[i]);
                SSe += d * d;

                d = outputs[i] - avg;
                SSt += d * d;
            }

            // Calculate R-Squared
            double r2 = (SSt != 0) ? 1.0 - (SSe / SSt) : 1.0;

            if (!adjust)
            {
                // Return ordinary R-Squared
                return r2;
            }
            else
            {
                if (r2 == 1)
                    return 1;

                if (N == P + 1)
                {
                    return double.NaN;
                }
                else
                {
                    // Return adjusted R-Squared
                    return 1.0 - (1.0 - r2) * ((N - 1.0) / (N - P - 1.0));
                }
            }
        }

        /// <summary>
        ///   Computes the Multiple Linear Regression for an input vector.
        /// </summary>
        /// <param name="input">The input vector.</param>
        /// <returns>The calculated output.</returns>
        public double Compute(double[] input)
        {
            double output = 0.0;

            for (int i = 0; i < input.Length; i++)
                output += coefficients[i] * input[i];

            if (insertConstant) output += coefficients[input.Length];

            return output;
        }

        /// <summary>
        ///   Computes the Multiple Linear Regression for input vectors.
        /// </summary>
        /// <param name="input">The input vector data.</param>
        /// <returns>The calculated outputs.</returns>
        public double[] Compute(double[][] input)
        {
            double[] output = new double[input.Length];

            for (int j = 0; j < input.Length; j++)
            {
                output[j] = Compute(input[j]);
            }

            return output;
        }

        /// <summary>
        ///   Returns a System.String representing the regression.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            int inputs = (insertConstant) ? coefficients.Length - 1 : coefficients.Length;

            sb.Append("y(");
            for (int i = 0; i < inputs; i++)
            {
                sb.AppendFormat("x{0}", i);

                if (i < inputs - 1)
                    sb.Append(", ");
            }

            sb.Append(") = ");

            for (int i = 0; i < inputs; i++)
            {
                sb.AppendFormat("{0}*x{1}", Coefficients[i], i);

                if (i < inputs - 1)
                    sb.Append(" + ");
            }

            if (insertConstant)
                sb.AppendFormat(" + {0}", coefficients[inputs]);

            return sb.ToString();
        }



        #region ILinearRegression Members
        double[] ILinearRegression.Compute(double[] inputs)
        {
            return new double[] { this.Compute(inputs) };
        }
        #endregion
    }
}
