using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Calculator.Infrastructure.Interfaces;
using Calculator.Infrastructure.Services;
using Microsoft.Win32;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICalculationService _calculationService;
        private readonly IValidatorService _validatorService;
        private readonly IFileService _fileService;
        private static bool _isError = false;
        public MainWindow()
        {
            InitializeComponent();
            AddClickEventToButtons();
            _calculationService = new CalculationService();
            _validatorService = new ValidatorService();
            _fileService = new FileService(_validatorService, _calculationService);
        }

        private void AddClickEventToButtons()
        {
            foreach (UIElement element in LayoutKeypad.Children)
            {
                var button = element as Button;
                if (button!=null)
                {
                    if ((string) button.Content == "=")
                    {
                        button.Click += Result_Click;
                        continue;
                    }
                    if ((string)button.Content == "C")
                    {
                        button.Click += Clear_Click;
                        continue;
                    }
                    if (button.Name == "file")
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty((string) button.Content))
                    {
                        continue;
                    }

                    button.Click += Calculator_Click;
                }
            }
        }

        private void Calculator_Click(object sender, RoutedEventArgs e)
        {
            if (_isError)
            {
                return;
            }
            var button = e.OriginalSource as Button;
            var content = (string)button?.Content;
            textBlock.Text += content;
        }

        private async void Result_Click(object sender, RoutedEventArgs e)
        {
            if (_isError)
            {
                return;
            }
            var str = textBlock.Text;
            if (string.IsNullOrEmpty(str))
            {
                return;
            }
            var errorList = _validatorService.ValidateCalculatorInput(str);
            if (!errorList.Any())
            {
                textBlock.Text = await _calculationService.CalculateAsync(str, CancellationToken.None);
            }
            else
            {
                _isError = true;
                var sb = new StringBuilder();
                errorList.ForEach(a => sb.Append($"{a} "));
                textBlock.Text = sb.ToString();
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _isError = false;
            textBlock.Text = string.Empty;
        }

        private async void OpenFileStorage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                await _fileService.PublishCalculatedResultAsync(openFileDialog.FileName, CancellationToken.None);
            }
        }
    }
}
