using SimpleBalance.Classes;

namespace SimpleBalance.Models
{
    public class BalanceOutput
    {
        public bool SolutionFound { get; private set; }
        public double? DisbalanceOriginal { get; private set; } //double? - переменная может принимать значение null
        public double? Disbalance { get; private set; }
        public double[] Solution { get; private set; }

        public BalanceOutput(bool solutionFound, double? disbalanceOriginal, double? disbalance, double[] solution)
        {
            SolutionFound = solutionFound;
            DisbalanceOriginal = disbalanceOriginal;
            Disbalance = disbalance;
            Solution = solution;
        }

        public BalanceOutput(QPSolver solver)
        {
            SolutionFound = solver.SolutionFound;
            DisbalanceOriginal = solver.DisbalanceOriginal;
            Disbalance = solver.Disbalance;
            Solution = solver.GetSolution();
        }
    }
}
