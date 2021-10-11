using System.Collections.Generic;

namespace SimpleBalance.Models
{
    public class BalanceInput
    {
        public List<Flow> Flows { get; set; }

        public BalanceInput(List<Flow> flows)
        {
                Flows = flows;
        }

    }
}
