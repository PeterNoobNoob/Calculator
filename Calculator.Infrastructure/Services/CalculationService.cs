using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Calculator.Infrastructure.Interfaces;
using Microsoft.VisualBasic.CompilerServices;

namespace Calculator.Infrastructure.Services
{
    public class CalculationService : ICalculationService
    {
        public string Calculate(string str)
        {
            if (IsCalculatedResult(str))
            {
                return str;
            }
            var listOperatorsAndNumbers = GetNumbersAndOperatorsSeparately(str);
            ChangeMultipleAndDivided(listOperatorsAndNumbers);
            ChangePlusAndMinus(listOperatorsAndNumbers);
            return listOperatorsAndNumbers.FirstOrDefault();
        }

        public Task<string> CalculateAsync(string str, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (IsCalculatedResult(str))
                {
                    return str;
                }
                var listOperatorsAndNumbers = GetNumbersAndOperatorsSeparately(str);
                ChangeMultipleAndDivided(listOperatorsAndNumbers);
                ChangePlusAndMinus(listOperatorsAndNumbers);
                return listOperatorsAndNumbers.FirstOrDefault();
            });
        }

        private void ChangeMultipleAndDivided(List<string> ints)
        {
            while (ints.Contains("*") || ints.Contains("/"))
            {
                var indexOfMultiple = ints.IndexOf("*");
                var indexOfDivided = ints.IndexOf("/");
                if (indexOfDivided != -1 && (indexOfDivided < indexOfMultiple || indexOfMultiple == -1))
                {
                    var result = BigInteger.Parse(ints[indexOfDivided - 1]) /
                                 BigInteger.Parse(ints[indexOfDivided + 1]);
                        ints[indexOfDivided - 1] = result.ToString();
                        ints.RemoveRange(indexOfDivided, 2);
                        continue;
                }
                else
                {
                    if (indexOfMultiple != -1)
                    {
                        var result = BigInteger.Parse(ints[indexOfMultiple - 1]) *
                                     BigInteger.Parse(ints[indexOfMultiple + 1]);
                        ints[indexOfMultiple - 1] = result.ToString();
                        ints.RemoveRange(indexOfMultiple, 2);
                        continue;
                    }
                }
            }
        }

        private void ChangePlusAndMinus(List<string> ints)
        {
            while (ints.Contains("+") || ints.Contains("-"))
            {
                var indexOfPlus = ints.IndexOf("+");
                if (indexOfPlus != -1)
                {
                    var result = BigInteger.Parse(ints[indexOfPlus - 1]) +
                                 BigInteger.Parse(ints[indexOfPlus + 1]);
                    ints[indexOfPlus - 1] = result.ToString();
                    ints.RemoveRange(indexOfPlus, 2);
                    continue;
                }

                var indexOfMinus = ints.IndexOf("-");
                if (indexOfMinus != -1)
                {
                    var result = BigInteger.Parse(ints[indexOfMinus - 1]) -
                                 BigInteger.Parse(ints[indexOfMinus + 1]);
                    ints[indexOfMinus - 1] = result.ToString();
                    ints.RemoveRange(indexOfMinus, 2);

                }
            }
        }

        private List<string> GetNumbersAndOperatorsSeparately(string str)
        {
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
            return listOperatorsAndNumbers;
        }

        private bool IsCalculatedResult(string result)
        {
            if (Regex.IsMatch(result, @"^[0-9]+$"))
            {
                return true;
            }

            return false;
        }
    }
}
