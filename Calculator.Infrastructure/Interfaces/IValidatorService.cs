using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Infrastructure.Interfaces
{
    public interface IValidatorService
    {
        List<string> ValidateCalculatorInput(string str);
    }
}
