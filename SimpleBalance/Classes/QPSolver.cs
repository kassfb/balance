using Accord.Math;
using Accord.Math.Optimization;
using System;
using System.Collections.Generic;

namespace SimpleBalance.Classes
{
    public class QPSolver
    {
        GoldfarbIdnani qp;
        public bool SolutionFound { get; private set; }
        public double? DisbalanceOriginal { get; private set; }
        public double? Disbalance { get; private set; }
             

        public void Solve(double[] x0, double[,] A, double[] t, double[] I, double[] lower, double[] upper)
        {
            double[,] diagI = Matrix.Diagonal(I);
            double[,] diagW = Matrix.Diagonal(1.Divide(t.Pow(2)));

            double[,] H = diagI.Dot(diagW);
            double[] d = H.Dot(x0).Multiply(-1);

            QuadraticObjectiveFunction func = new QuadraticObjectiveFunction(H, d);

            List<LinearConstraint> constraints = new List<LinearConstraint>();

            double[] b = Vector.Create(A.GetLength(0), 0.0);
            // Нижние и верхние границы
            for (int j = 0; j < x0.Length; j++)
            {
                constraints.Add(new LinearConstraint(1)
                {
                    VariablesAtIndices = new[] { j },
                    ShouldBe = ConstraintType.GreaterThanOrEqualTo,
                    Value = lower[j]
                });

                constraints.Add(new LinearConstraint(1)
                {
                    VariablesAtIndices = new[] { j },
                    ShouldBe = ConstraintType.LesserThanOrEqualTo,
                    Value = upper[j]
                });
            }

            // Ограничения для решения задачи баланса
            for (var j = 0; j < b.Length; j++)
            {
                var notNullElements = Array.FindAll(A.GetRow(j), x => Math.Abs(x) > 0);
                var notNullElementsIndexes = new List<int>();
                for (var k = 0; k < x0.Length; k++)
                {
                    if (Math.Abs(A[j, k]) > 0)
                    {
                        notNullElementsIndexes.Add(k);
                    }
                }

                constraints.Add(new LinearConstraint(notNullElements.Length)
                {
                    VariablesAtIndices = notNullElementsIndexes.ToArray(),
                    CombinedAs = notNullElements,
                    ShouldBe = ConstraintType.EqualTo,
                    Value = b[j]
                });
            }
            
            qp = new GoldfarbIdnani(func, constraints);

            SolutionFound = qp.Minimize();

            if (SolutionFound)
            {
                DisbalanceOriginal = A.Dot(x0).Subtract(b).Euclidean();
                Disbalance = A.Dot(qp.Solution).Subtract(b).Euclidean();
            }
            else
            {
                DisbalanceOriginal = null;
                Disbalance = null;
            }
        }

        public double[] GetSolution()
        {
            if (qp != null && SolutionFound == true)
            {
                return qp.Solution;
            }
            return null;
        }

    }
}
