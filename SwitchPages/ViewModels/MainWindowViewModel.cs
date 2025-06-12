using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SwitchPages.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase _currentPageViewModel;

        // We can pre-create instances of our page ViewModels
        // or create them on-the-fly. Pre-creating them preserves their state.
        private readonly HomePageViewModel _homePageViewModel;
        private readonly SettingsPageViewModel _settingsPageViewModel;

        public MainWindowViewModel()
        {
            // Initialize our page ViewModels
            _homePageViewModel = new HomePageViewModel();
            _settingsPageViewModel = new SettingsPageViewModel();

            // Set the initial page
            _currentPageViewModel = _homePageViewModel;
        }

        // A command to navigate to the Home page.
        [RelayCommand]
        private void GoToHome()
        {
            CurrentPageViewModel = _homePageViewModel;
        }

        // A command to navigate to the Settings page.
        [RelayCommand]
        private void GoToSettings()
        {
            CurrentPageViewModel = _settingsPageViewModel;
        }
    }
}
