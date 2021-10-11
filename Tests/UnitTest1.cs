using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SimpleBalance.Classes;
using SimpleBalance.Controllers;
using SimpleBalance.Models;
using System;
using System.IO;

namespace Tests
{
    [TestClass]
    public class UnitTestSolver
    {
        /// <summary>
        /// “естирование POST запроса на вычисление баланса
        /// </summary>
        [TestMethod]
        public void TestBalanceController()
        {
            BalanceController controller = new BalanceController();

            BalanceInput input;
            BalanceOutput output;
            BalanceOutput expectedOutput = new BalanceOutput(true, 0.2879496483762398, 9.155133597044475e-16,
                new double[] {
                    10.055612418500504,
                    3.0144745895183522,
                    7.041137828982151,
                    1.9822547563048074,
                    5.058883072677343,
                    4.067257698582969,
                    0.9916253740943739
                }
            );

            using (StreamReader r = new StreamReader(@"..\..\..\testModel1.json"))
            {
                string json = r.ReadToEnd();
                input = JsonConvert.DeserializeObject<BalanceInput>(json);
            }

            output = controller.Post(input);

            for (var j = 0; j < output.Solution.Length; j++)
            {
                Assert.AreEqual(expectedOutput.Solution[j], output.Solution[j], 3);
            }
            
            //Assert.AreEqual(0, (double)output.Disbalance, 3);
        }

        /// <summary>
        /// “естирование солвера дл€ решени€ задачи из *Original*.xlsx
        /// </summary>
        [TestMethod]
        public void TestOriginal()
        {
            var solver = new QPSolver();

            double[] x0 = { 10.005, 3.033, 6.831, 1.985, 5.093, 4.057, 0.991 };

            double[,] A = {
                {1, -1, -1, 0, 0, 0, 0},
                {0, 0, 1, -1, -1, 0, 0},
                {0, 0, 0, 0, 1, -1, -1}
            };
            double[] i = { 1, 1, 1, 1, 1, 1, 1 };
            double[] t = { 0.200, 0.121, 0.683, 0.040, 0.102, 0.081, 0.020 };
            double[] l = { 0, 0, 0, 0, 0, 0, 0 };
            double[] u = { 10000, 10000, 10000, 10000, 10000, 10000, 10000 };

            solver.Solve(x0, A, t, i, l, u);

            var result = solver.GetSolution();

            double[] expected = { 10.05561, 3.01447, 7.04114, 1.98225, 5.05888, 4.06726, 0.99163 };

            for (var j = 0; j < result.Length; j++)
            {
                Assert.AreEqual(expected[j], result[j], 3);
            }
        }

        /// <summary>
        /// “естирование солвера дл€ решени€ задачи дл€ четного варианта
        /// </summary>
        [TestMethod]
        public void TestV2()
        {
            var solver = new QPSolver();

            double[] x0 = { 10.005, 3.033, 6.831, 1.985, 5.093, 4.057, 0.991, 6.666 };

            double[,] A = {
                {1, -1, -1, 0, 0, 0, 0, 0},
                {0, 0, 1, -1, -1, 0, 0, 0},
                {0, 0, 0, 0, 1, -1, -1, -1}
            };
            double[] i = { 1, 1, 1, 1, 1, 1, 1, 1 };
            double[] t = { 0.200, 0.121, 0.683, 0.040, 0.102, 0.081, 0.020, 0.667 };
            double[] l = { 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] u = { 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000 };

            solver.Solve(x0, A, t, i, l, u);
            var result = solver.GetSolution();

            double[] expected = { 10.15338, 2.97869, 7.17469, 1.97789, 5.19680, 3.96237, 0.98523, 0.24920 };

            for (var j = 0; j < result.Length; j++)
            {
                Assert.AreEqual(expected[j], result[j], 3);
            }
        }

        /// <summary>
        /// “естирование солвера дл€ решени€ задачи при существенной разнице потоков
        /// </summary>
        [TestMethod]
        public void Test1x10()
        {
            var solver = new QPSolver();

            double[] x0 = { 10.005, 3.033, 6.831, 1.985, 5.093, 4.057, 0.991 };

            double[,] A = {
                {1, -1, -1, 0, 0, 0, 0},
                {0, 0, 1, -1, -1, 0, 0},
                {0, 0, 0, 0, 1, -1, -1},
                {1, -10, 0, 0, 0, 0, 0}
            };
            double[] i = { 1, 1, 1, 1, 1, 1, 1 };
            double[] t = { 0.200, 0.121, 0.683, 0.040, 0.102, 0.081, 0.020 };
            double[] l = { 0, 0, 0, 0, 0, 0, 0 };
            double[] u = { 10000, 10000, 10000, 10000, 10000, 10000, 10000 };

            solver.Solve(x0, A, t, i, l, u);
            var result = solver.GetSolution();

            Assert.AreEqual(result[0], result[1] * 10, 3);
        }

        /// <summary>
        /// “естирование запроса на ненахождение баланса
        /// </summary>
        [TestMethod]
        public void TestFoundSolution()
        {
            var solver = new QPSolver();

            double[] x0 = { 10.005, 3.033, 6.831, 1.985, 5.093, 4.057, 0.991 };

            double[,] A = {
                {1, -1, -1, 0, 0, 0, 0},
                {0, 0, 1, -1, -1, 0, 0},
                {0, 0, 0, 0, 1, -1, -1}
            };
            double[] i = { 1, 1, 1, 1, 1, 1, 1 };
            double[] t = { 0.200, 0.121, 0.683, 0.040, 0.102, 0.081, 0.020 };
            double[] l = { 0, 0, 0, 0, 0, 0, 10000 };
            double[] u = { 10000, 10000, 10000, 10000, 10000, 10000, 10000 };

            solver.Solve(x0, A, t, i, l, u);

            var result = solver.SolutionFound;

            bool expected = false;

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// “естирование запроса с неполными данными #1
        /// </summary>
        [TestMethod]
        public void TestError1()
        {
            BalanceController controller = new BalanceController();
            BalanceInput input;
            Exception expectedExcetpion = null;

            try
            {
                using (StreamReader r = new StreamReader(@"..\..\..\testErrorModel1.json"))
                {
                    string json = r.ReadToEnd();
                    input = JsonConvert.DeserializeObject<BalanceInput>(json);
                }
                BalanceOutput output = controller.Post(input);
            }
            catch (ArgumentNullException ex)
            {
                expectedExcetpion = ex;
            }

            Assert.IsNotNull(expectedExcetpion);
        }

        /// <summary>
        /// “естирование запроса с неполными данными #2
        /// </summary>
        [TestMethod]
        public void TestError2()
        {
            BalanceController controller = new BalanceController();
            BalanceInput input;
            Exception expectedExcetpion = null;

            try
            {
                using (StreamReader r = new StreamReader(@"..\..\..\testErrorModel2.json"))
                {
                    string json = r.ReadToEnd();
                    input = JsonConvert.DeserializeObject<BalanceInput>(json);
                }
                BalanceOutput output = controller.Post(input);
            }
            catch (ArgumentNullException ex)
            {
                expectedExcetpion = ex;
            }

            Assert.IsNotNull(expectedExcetpion);
        }
        /// <summary>
        /// “естирование запроса в котором баланс не сводитс€ из-за зажатых границ потока
        /// </summary>
        [TestMethod]
        public void TestError3()
        {
            BalanceController controller = new BalanceController();
            BalanceInput input;
            BalanceOutput output;

            using (StreamReader r = new StreamReader(@"..\..\..\testErrorConstraints.json"))
            {
                string json = r.ReadToEnd();
                input = JsonConvert.DeserializeObject<BalanceInput>(json);
            }

            output = controller.Post(input);

            bool expected = false;

            Assert.AreEqual(expected, output.SolutionFound);
        }

    }
}
