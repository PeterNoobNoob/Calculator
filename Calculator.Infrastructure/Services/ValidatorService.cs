using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Calculator.Infrastructure.Interfaces;

namespace Calculator.Infrastructure.Services
{
    public class ValidatorService : IValidatorService
    {
        public List<string> ValidateCalculatorInput(string str)
        {
            var listErrors = new List<string>();
            var listOperatorsAndNumbers = new List<string>();
            var previousOperatorIndex = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].Equals('+') || str[i].Equals('-') || str[i].Equals('*') || str[i].Equals('/'))
                {
                    if ((previousOperatorIndex != 0 && i - previousOperatorIndex == 1) || i == 0)
                    {
                        continue;
                    }

                    listOperatorsAndNumbers.Add(str.Substring(
                        previousOperatorIndex == 0 ? previousOperatorIndex : previousOperatorIndex + 1,
                        previousOperatorIndex == 0 ? i - previousOperatorIndex : i - previousOperatorIndex - 1));
                    listOperatorsAndNumbers.Add(str[i].ToString());
                    previousOperatorIndex = i;
                }
            }

            listOperatorsAndNumbers.Add(str.Substring(previousOperatorIndex + 1, str.Length - previousOperatorIndex - 1));
            foreach (var oper in listOperatorsAndNumbers)
            {
                if (!Regex.IsMatch(oper, @"^-?[0-9]+$"))
                {
                    if (Regex.IsMatch(oper, @"^[+-/*]{1}$"))
                    {
                        continue;
                    }
                    listErrors.Add($"Invalid character: {oper}");
                }
            }

            var operatorErrors = new List<string>();
            for (int i = 0; i < listOperatorsAndNumbers.Count; i++)
            {
                if (i != 0)
                {
                    if (IsOperator(listOperatorsAndNumbers[i - 1]) && IsOperator(listOperatorsAndNumbers[i]))
                    {
                        operatorErrors.Add($"{i}-{listOperatorsAndNumbers[i]}");
                        operatorErrors.Add($"{i-1}-{listOperatorsAndNumbers[i-1]}");
                    }
                }
            }

            if (operatorErrors.Any())
            {
                var duplicateOperatorsError = string.Concat(operatorErrors.Distinct().Select(a => a.Substring(2)));
                listErrors.Add($"Error occurred in characters: {duplicateOperatorsError}");
            }
            return listErrors;
        }

        private bool IsOperator(string str)
        {
            return str.Equals("+") || str.Equals("-") || str.Equals("*") || str.Equals("/");
        }
    }
}
