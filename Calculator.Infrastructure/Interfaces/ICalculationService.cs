using System.Threading;
using System.Threading.Tasks;

namespace Calculator.Infrastructure.Interfaces
{
    public interface ICalculationService
    {
        string Calculate(string str);

        Task<string> CalculateAsync(string str, CancellationToken cancellationToken);
    }
}
