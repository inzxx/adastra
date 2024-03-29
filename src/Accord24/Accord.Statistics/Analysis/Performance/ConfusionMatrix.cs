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

namespace Accord.Statistics.Analysis
{
    using System;

    /// <summary>
    ///   Binary decision confusion matrix.
    /// </summary>
    /// 
    /// <example>
    ///   <code>
    ///   // The correct and expected output values (as confirmed by a Gold
    ///   //  standard rule, actual experiment or true verification)
    ///   int[] expected = { 0, 0, 1, 0, 1, 0, 0, 0, 0, 0 };
    ///   
    ///   // The values as predicted by the decision system or
    ///   //  the test whose performance is being measured.
    ///   int[] predicted = { 0, 0, 0, 1, 1, 0, 0, 0, 0, 1 };
    ///   
    ///   
    ///   // In this test, 1 means positive, 0 means negative
    ///   int positiveValue = 1;
    ///   int negativeValue = 0;
    ///   
    ///   // Create a new confusion matrix using the given parameters
    ///   ConfusionMatrix matrix = new ConfusionMatrix(predicted, expected,
    ///       positiveValue, negativeValue);
    ///
    ///   // At this point,
    ///   //   True Positives should be equal to 1;
    ///   //   True Negatives should be equal to 6;
    ///   //   False Negatives should be equal to 1;
    ///   //   False Positives should be equal to 2.
    ///   </code>
    /// </example>
    /// 
    [Serializable]
    public class ConfusionMatrix
    {

        //  2x2 confusion matrix
        private int truePositives;
        private int trueNegatives;
        private int falsePositives;
        private int falseNegatives;


        /// <summary>
        ///   Constructs a new Confusion Matrix.
        /// </summary>
        /// 
        public ConfusionMatrix(int truePositives, int trueNegatives,
            int falsePositives, int falseNegatives)
        {
            this.truePositives = truePositives;
            this.trueNegatives = trueNegatives;
            this.falsePositives = falsePositives;
            this.falseNegatives = falseNegatives;
        }

        /// <summary>
        ///   Constructs a new Confusion Matrix.
        /// </summary>
        /// 
        /// <param name="predicted">The values predicted by the model.</param>
        /// <param name="expected">The actual, truth values from the data.</param>
        /// 
        public ConfusionMatrix(bool[] predicted, bool[] expected)
        {
            // Initial argument checking
            if (predicted == null) throw new ArgumentNullException("predicted");
            if (expected == null) throw new ArgumentNullException("expected");
            if (predicted.Length != expected.Length)
                throw new DimensionMismatchException("expected", "The size of the predicted and expected arrays must match.");


            // For each of the predicted values,
            for (int i = 0; i < predicted.Length; i++)
            {
                bool prediction = predicted[i];
                bool expectation = expected[i];


                // If the prediction equals the true measured value
                if (expectation == prediction)
                {
                    // We have a hit. Now we have to see
                    //  if it was a positive or negative hit
                    if (prediction == true)
                    {
                        truePositives++; // Positive hit
                    }
                    else
                    {
                        trueNegatives++; // Negative hit
                    }
                }
                else
                {
                    // We have a miss. Now we have to see
                    //  if it was a positive or negative miss
                    if (prediction == true)
                    {
                        falsePositives++; // Positive hit
                    }
                    else
                    {
                        falseNegatives++; // Negative hit
                    }
                }
            }
        }

        /// <summary>
        ///   Constructs a new Confusion Matrix.
        /// </summary>
        /// 
        /// <param name="predicted">The values predicted by the model.</param>
        /// <param name="expected">The actual, truth values from the data.</param>
        /// <param name="positiveValue">The integer label which identifies a value as positive.</param>
        /// 
        public ConfusionMatrix(int[] predicted, int[] expected, int positiveValue)
        {
            // Initial argument checking
            if (predicted == null) throw new ArgumentNullException("predicted");
            if (expected == null) throw new ArgumentNullException("expected");
            if (predicted.Length != expected.Length)
                throw new DimensionMismatchException("expected", "The size of the predicted and expected arrays must match.");


            for (int i = 0; i < predicted.Length; i++)
            {
                bool prediction = predicted[i] == positiveValue;
                bool expectation = expected[i] == positiveValue;


                // If the prediction equals the true measured value
                if (expectation == prediction)
                {
                    // We have a hit. Now we have to see
                    //  if it was a positive or negative hit
                    if (prediction == true)
                    {
                        truePositives++; // Positive hit
                    }
                    else
                    {
                        trueNegatives++; // Negative hit
                    }
                }
                else
                {
                    // We have a miss. Now we have to see
                    //  if it was a positive or negative miss
                    if (prediction == true)
                    {
                        falsePositives++; // Positive hit
                    }
                    else
                    {
                        falseNegatives++; // Negative hit
                    }
                }
            }

        }


        /// <summary>
        ///   Constructs a new Confusion Matrix.
        /// </summary>
        /// 
        /// <param name="predicted">The values predicted by the model.</param>
        /// <param name="expected">The actual, truth values from the data.</param>
        /// <param name="positiveValue">The integer label which identifies a value as positive.</param>
        /// <param name="negativeValue">The integer label which identifies a value as negative.</param>
        /// 
        public ConfusionMatrix(int[] predicted, int[] expected, int positiveValue, int negativeValue)
        {
            // Initial argument checking
            if (predicted == null) throw new ArgumentNullException("predicted");
            if (expected == null) throw new ArgumentNullException("expected");
            if (predicted.Length != expected.Length)
                throw new DimensionMismatchException("expected", "The size of the predicted and expected arrays must match.");


            for (int i = 0; i < predicted.Length; i++)
            {

                // If the prediction equals the true measured value
                if (predicted[i] == expected[i])
                {
                    // We have a hit. Now we have to see
                    //  if it was a positive or negative hit
                    if (predicted[i] == positiveValue)
                    {
                        truePositives++; // Positive hit
                    }
                    else if (predicted[i] == negativeValue)
                    {
                        trueNegatives++; // Negative hit
                    }
                }
                else
                {
                    // We have a miss. Now we have to see
                    //  if it was a positive or negative miss
                    if (predicted[i] == positiveValue)
                    {
                        falsePositives++; // Positive hit
                    }
                    else if (predicted[i] == negativeValue)
                    {
                        falseNegatives++; // Negative hit
                    }
                }
            }
        }




        /// <summary>
        ///   Gets the number of observations for this matrix
        /// </summary>
        /// 
        public int Observations
        {
            get
            {
                return trueNegatives + truePositives +
                    falseNegatives + falsePositives;
            }
        }

        /// <summary>
        ///   Gets the number of actual positives.
        /// </summary>
        /// 
        /// <remarks>
        ///   The number of positives cases can be computed by
        ///   taking the sum of true positives and false negatives.
        /// </remarks>
        /// 
        public int ActualPositives
        {
            get { return truePositives + falseNegatives; }
        }

        /// <summary>
        ///   Gets the number of actual negatives
        /// </summary>
        /// 
        /// <remarks>
        ///   The number of negatives cases can be computed by
        ///   taking the sum of true negatives and false positives.
        /// </remarks>
        /// 
        public int ActualNegatives
        {
            get { return trueNegatives + falsePositives; }
        }

        /// <summary>
        ///   Gets the number of predicted positives.
        /// </summary>
        /// 
        /// <remarks>
        ///   The number of cases predicted as positive by the
        ///   test. This value can be computed by adding the
        ///   true positives and false positives.
        /// </remarks>
        /// 
        public int PredictedPositives
        {
            get { return truePositives + falsePositives; }
        }

        /// <summary>
        ///   Gets the number of predicted negatives.
        /// </summary>
        /// 
        /// <remarks>
        ///   The number of cases predicted as negative by the
        ///   test. This value can be computed by adding the
        ///   true negatives and false negatives.
        /// </remarks>
        /// 
        public int PredictedNegatives
        {
            get { return trueNegatives + falseNegatives; }
        }

        /// <summary>
        ///   Cases correctly identified by the system as positives.
        /// </summary>
        /// 
        public int TruePositives
        {
            get { return truePositives; }
        }

        /// <summary>
        ///   Cases correctly identified by the system as negatives.
        /// </summary>
        /// 
        public int TrueNegatives
        {
            get { return trueNegatives; }
        }

        /// <summary>
        ///   Cases incorrectly identified by the system as positives.
        /// </summary>
        /// 
        public int FalsePositives
        {
            get { return falsePositives; }
        }

        /// <summary>
        ///   Cases incorrectly identified by the system as negatives.
        /// </summary>
        /// 
        public int FalseNegatives
        {
            get { return falseNegatives; }
        }

        /// <summary>
        ///   Sensitivity, also known as True Positive Rate
        /// </summary>
        /// 
        /// <remarks>
        ///   The Sensitivity is calculated as <c>TPR = TP / (TP + FN)</c>.
        /// </remarks>
        /// 
        public double Sensitivity
        {
            get
            {
                return (truePositives == 0) ?
                    0 : (double)truePositives / (truePositives + falseNegatives);
            }
        }

        /// <summary>
        ///   Specificity, also known as True Negative Rate
        /// </summary>
        /// 
        /// <remarks>
        ///   The Specificity is calculated as <c>TNR = TN / (FP + TN)</c>.
        ///   It can also be calculated as: <c>TNR = (1-False Positive Rate)</c>.
        /// </remarks>
        /// 
        public double Specificity
        {
            get
            {
                return (trueNegatives == 0) ?
                    0 : (double)trueNegatives / (trueNegatives + falsePositives);
            }
        }

        /// <summary>
        ///  Efficiency, the arithmetic mean of sensitivity and specificity
        /// </summary>
        /// 
        public double Efficiency
        {
            get { return (Sensitivity + Specificity) / 2.0; }
        }

        /// <summary>
        ///   Accuracy, or raw performance of the system
        /// </summary>
        /// 
        /// <remarks>
        ///   The Accuracy is calculated as 
        ///   <c>ACC = (TP + TN) / (P + N).</c>
        /// </remarks>
        /// 
        public double Accuracy
        {
            get { return 1.0 * (truePositives + trueNegatives) / Observations; }
        }

        /// <summary>
        ///   Positive Predictive Value, also known as Positive Precision
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        ///   The Positive Predictive Value tells us how likely is 
        ///   that a patient has a disease, given that the test for
        ///   this disease is positive.</para>
        /// <para>
        ///   The Positive Predictive Rate is calculated as
        ///   <c>PPV = TP / (TP + FP)</c>.</para>
        /// </remarks>
        /// 
        public double PositivePredictiveValue
        {
            get
            {
                double f = truePositives + FalsePositives;
                if (f != 0) return truePositives / f;
                return 1.0;
            }
        }

        /// <summary>
        ///   Negative Predictive Value, also known as Negative Precision
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        ///   The Negative Predictive Value tells us how likely it is
        ///   that the disease is NOT present for a patient, given that
        ///   the patient's test for the disease is negative.</para>
        /// <para>
        ///   The Negative Predictive Value is calculated as 
        ///   <c>NPV = TN / (TN + FN)</c>.</para> 
        /// </remarks>
        /// 
        public double NegativePredictiveValue
        {
            get
            {
                double f = (trueNegatives + falseNegatives);
                if (f != 0) return trueNegatives / f;
                else return 1.0;
            }
        }


        /// <summary>
        ///   False Positive Rate, also known as false alarm rate.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        ///   The rate of false alarms in a test.</para>
        /// <para>
        ///   The False Positive Rate can be calculated as
        ///   <c>FPR = FP / (FP + TN)</c> or <c>FPR = (1-specifity)</c>.
        /// </para>
        /// </remarks>
        /// 
        public double FalsePositiveRate
        {
            get
            {
                return (double)falsePositives / (falsePositives + trueNegatives);
            }
        }

        /// <summary>
        ///   False Discovery Rate, or the expected false positive rate.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        ///   The False Discovery Rate is actually the expected false positive rate.</para>
        /// <para>
        ///   For example, if 1000 observations were experimentally predicted to
        ///   be different, and a maximum FDR for these observations was 0.10, then
        ///   100 of these observations would be expected to be false positives.</para>
        /// <para>
        ///   The False Discovery Rate is calculated as
        ///   <c>FDR = FP / (FP + TP)</c>.</para>
        /// </remarks>
        /// 
        public double FalseDiscoveryRate
        {
            get
            {
                double d = falsePositives + truePositives;
                if (d != 0.0) return falsePositives / d;
                else return 1.0;
            }
        }

        /// <summary>
        ///   Matthews Correlation Coefficient, also known as Phi coefficient
        /// </summary>
        /// 
        /// <remarks>
        ///   A coefficient of +1 represents a perfect prediction, 0 an
        ///   average random prediction and −1 an inverse prediction.
        /// </remarks>
        /// 
        public double MatthewsCorrelationCoefficient
        {
            get
            {
                double s = System.Math.Sqrt(
                    (truePositives + falsePositives) *
                    (truePositives + falseNegatives) *
                    (trueNegatives + falsePositives) *
                    (trueNegatives + falseNegatives));

                if (s != 0.0)
                    return (truePositives * trueNegatives) / s;
                else return 0.0;
            }
        }

        
        /// <summary>
        ///   Odds-ratio.
        /// </summary>
        /// 
        /// <remarks>
        ///   References: http://www.iph.ufrgs.br/corpodocente/marques/cd/rd/presabs.htm
        /// </remarks>
        /// 
        public double OddsRatio
        {
            get
            {
                return (double)(truePositives * trueNegatives) / (falsePositives * falseNegatives);
            }
        }

        /// <summary>
        ///   Kappa coefficient.
        /// </summary>
        ///
        /// <remarks>
        ///   References: http://www.iph.ufrgs.br/corpodocente/marques/cd/rd/presabs.htm
        /// </remarks>
        ///
        public double Kappa
        {
            get
            {
                double a = truePositives;
                double b = falsePositives;
                double c = falseNegatives;
                double d = trueNegatives;
                double N = Observations;

                return (double)((a + d) - (((a + c) * (a + b) + (b + d) * (c + d)) / N))
                    / (N - (((a + c) * (a + b) + (b + d) * (c + d)) / N));
            }
        }

        /// <summary>
        ///   Diagnostic power.
        /// </summary>
        /// 
        public double OverallDiagnosticPower
        {
            get { return (double)(falsePositives + trueNegatives) / Observations; }
        }

        /// <summary>
        ///   Normalized Mutual Information.
        /// </summary>
        /// 
        public double NormalizedMutualInformation
        {
            get
            {
                double a = truePositives;
                double b = falsePositives;
                double c = falseNegatives;
                double d = trueNegatives;
                double N = Observations;

                double num = a * Math.Log(a) + b * Math.Log(b) + c * Math.Log(c) + d * Math.Log(d)
                           - (a + b) * Math.Log(a + b) - (c + d) * Math.Log(c + d);

                double den = N * Math.Log(N) - ((a + c) * Math.Log(a + c) + (b + d) * Math.Log(b + d));

                return 1 + num / den;
            }
        }

        /// <summary>
        ///   Returns a <see cref="System.String"/> representing this confusion matrix.
        /// </summary>
        /// 
        /// <returns>
        ///   A <see cref="System.String"/> representing this confusion matrix.
        /// </returns>
        /// 
        public override string ToString()
        {
            return String.Format(System.Globalization.CultureInfo.CurrentCulture,
                "TP:{0} TN:{1} FP:{2} FN:{3}",
                truePositives, trueNegatives, falsePositives, falseNegatives);
        }
    }
}
