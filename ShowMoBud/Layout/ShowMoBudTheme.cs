using MudBlazor;

namespace ShowMoBud.Layout
{
    public static class ShowMoBudTheme
    {

        public static MudTheme Theme = new MudTheme()
        {
            PaletteDark = new PaletteDark()
            {
                Primary = "#00FF00", // Bright green primary color
                Secondary = "#FFFF00", // Bright yellow secondary color
                Background = "#000000", // Black background
                AppbarBackground = "#00FF00", // Bright green app bar background
                DrawerBackground = "#111111", // Dark gray drawer background
                Surface = "1C1C1C", // Dark gray surface
                TextPrimary = "#FFFFFF", // White text for primary text
                TextSecondary = "#AAAAAA", // Light gray text for secondary text    
                ActionDefault = "#00FF00", // Bright green for default actions
            }
        };

    }
}
