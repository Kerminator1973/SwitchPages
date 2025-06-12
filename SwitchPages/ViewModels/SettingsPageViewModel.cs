namespace SwitchPages.ViewModels
{
    internal class SettingsPageViewModel : ViewModelBase
    {
        public string SettingsTitle => "Application Settings";
        public bool EnableNotifications { get; set; } = true;
    }
}
