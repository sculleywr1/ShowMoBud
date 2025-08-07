using MudBlazor;

namespace ShowMoBud.Layout
{
    public static class ShowMoBudTheme
    {

        public static MudTheme Theme = new MudTheme()
        {
            PaletteDark = new PaletteDark()
            {
                Primary = "#31a924", // Light primary color
                Secondary = "#ac840a", // Orange secondary color
                Background = "#060d07", // Dark background
                AppbarBackground = "#E5E2D9", // Light gray app bar background
                DrawerBackground = "#111111", // Dark gray drawer background
                Surface = "#1C1C1C", // Dark gray surface
                TextPrimary = "#FFFFFF", // White text for primary text
                TextSecondary = "#31a924", // Light gray text for secondary text    
                ActionDefault = "#31a924", // Bright green for default actions
            }
        };

    }
}
