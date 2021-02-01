using System.Threading;
using System.Threading.Tasks;

namespace Calculator.Infrastructure.Interfaces
{
    public interface IFileService
    {
        Task PublishCalculatedResultAsync(string fileName, CancellationToken cancellationToken);
    }
}
