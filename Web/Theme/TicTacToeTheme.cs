using MudBlazor;

namespace Web.Theme;

public static class TicTacToeTheme
{
    private const string FontFamily = "Montserrat";

    public static MudTheme Default { get; } = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#6053ee",
            PrimaryContrastText = "#ffffff",
            TextPrimary = "#505050",
            // Rôles joueur / ordinateur (voir --ttt-player-color et --ttt-computer-color)
            Info = "#459aee",
            Warning = "#f98100",
            LinesDefault = "#ddd",
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "5px",
        },
        Typography = new Typography
        {
            Default = new DefaultTypography { FontFamily = [FontFamily, "sans-serif"] },
            H1 = new H1Typography { FontSize = "50px", FontWeight = "500", LineHeight = "1.2" },
            H4 = new H4Typography { FontSize = "32px", FontWeight = "400", LineHeight = "1.2" },
            Button = new ButtonTypography { FontSize = "16px", TextTransform = "none" },
        },
    };
}
