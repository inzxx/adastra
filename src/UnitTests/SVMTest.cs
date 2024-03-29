﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Adastra;
using Adastra.Algorithms;

namespace UnitTests
{
    [TestFixture]
    public class SVMTest
    {
        [Test]
        public void Process()
        {
            Console.WriteLine(DbSettings.fullpath);

            EEGRecordStorage s = new EEGRecordStorage();

            EEGRecord r = s.LoadModel("MLPdata");

            Console.WriteLine("Data loaded");

            LdaSVM model = new LdaSVM();

            for (int k = 0; k < 1; k++)
            {
                model.Train(new EEGRecord(r.FeatureVectorsOutputInput));
            }

            int i = 0;
            int ok = 0;
            foreach (double[] vector in r.FeatureVectorsOutputInput)
            {
                i++;
                double[] input = new double[vector.Length - 1];

                Array.Copy(vector, 1, input, 0, vector.Length - 1);

                int result = model.Classify(input);

                if (result == vector[0])
                {
                    ok++;
                    Console.WriteLine("Result " + result + " Expected " + vector[0] + " OK");
                }
                else
                {
                    Console.WriteLine("Result " + result + " Expected " + vector[0]);
                }
            }

            Console.WriteLine(i);
            Console.WriteLine(ok);
            Console.ReadKey();
        }
    }
}
