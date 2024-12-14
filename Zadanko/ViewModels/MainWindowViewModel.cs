using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Zadanko.Services;

namespace Zadanko.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IApiService _apiService;

        public string Greeting { get; } = "Welcome to Zadanko!";

        [ObservableProperty]
        private string _firstNumberInput = string.Empty;

        [ObservableProperty]
        private string _secondNumberInput = string.Empty;

        [ObservableProperty]
        private int _result;

        [ObservableProperty]
        private bool _isBusy;

        [RelayCommand(CanExecute = nameof(CanExecuteButton))]
        private async Task AddNumbers()
        {
            if (IsBusy) return;
            
            IsBusy = true;

            try
            {
                if (int.TryParse(FirstNumberInput, out int first) && 
                    int.TryParse(SecondNumberInput, out int second))
                {
                    Result = await _apiService.AddNumbersAsync(first, second);
                }
                else
                {
                    Result = 0;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanExecuteButton() => !IsBusy;

        public MainWindowViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        partial void OnFirstNumberInputChanged(string value)
        {
            if (!string.IsNullOrEmpty(value) && !int.TryParse(value, out _))
            {
                FirstNumberInput = string.Empty;
            }
        }

        partial void OnSecondNumberInputChanged(string value)
        {
            if (!string.IsNullOrEmpty(value) && !int.TryParse(value, out _))
            {
                SecondNumberInput = string.Empty;
            }
        }
    }
}
