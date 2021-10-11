using Microsoft.AspNetCore.Mvc;
using SimpleBalance.Classes;
using SimpleBalance.Models;

namespace SimpleBalance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        // POST api/<BalanceController>
        [HttpPost]
        public BalanceOutput Post(BalanceInput value)
        {
            InputConverter converter = new InputConverter(value);
            converter.Convert();

            QPSolver solver = new QPSolver();
            solver.Solve(converter.X0, converter.A, converter.T, 
            converter.I, converter.Lower, converter.Upper);

            BalanceOutput output = new BalanceOutput(solver);
            return output;
        }
    }
}
