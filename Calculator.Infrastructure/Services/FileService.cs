using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Calculator.Infrastructure.Interfaces;

namespace Calculator.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IValidatorService _validatorService;
        private readonly ICalculationService _calculationService;

        public FileService(IValidatorService validatorService, ICalculationService calculationService)
        {
            _validatorService = validatorService;
            _calculationService = calculationService;
        }

        public async Task PublishCalculatedResultAsync(string fileName, CancellationToken cancellationToken)
        {
            var fileLines = await File.ReadAllLinesAsync(fileName, cancellationToken);
            var result = new List<string>();
            foreach (var line in fileLines)
            {
                var trimmedLine = line.Replace(" ", "");
                var errors = _validatorService.ValidateCalculatorInput(trimmedLine);
                if (errors.Any())
                {
                    var stringBuilder = new StringBuilder();
                    foreach (var error in errors)
                    {
                        stringBuilder.Append(error);
                        stringBuilder.Append(" ");
                    }
                    result.Add(stringBuilder.ToString());
                    continue;
                }

                var calculatedResult = await _calculationService.CalculateAsync(trimmedLine, cancellationToken);
                result.Add(calculatedResult);
            }

            var indexOf = fileName.LastIndexOf(".", StringComparison.Ordinal);
            if (indexOf != -1)
            {
                var resultFileName =
                    $"{fileName.Substring(0, indexOf)}_output.{fileName.Substring(indexOf + 1, fileName.Length-indexOf - 1)}";
                await File.WriteAllLinesAsync(resultFileName, result);
            }

        }
    }
}
