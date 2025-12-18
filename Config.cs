using BepInEx.Configuration;

namespace FarewellSteelSoul;

public partial class FarewellSteelSoul
{
    private static ConfigEntry<bool> ShowJournal;
    private static ConfigEntry<bool> ShowQuest;
    private static ConfigEntry<bool> ShowCrest;
    private static ConfigEntry<bool> ShowCredits;

    private void Register()
    {
        ShowJournal = Config.Bind("Settings", "ShowJournal", true, "Show journal updated message");
        ShowQuest = Config.Bind("Settings", "ShowQuest", true, "Show wish granted message");
        ShowCrest = Config.Bind("Settings", "ShowCrest", true, "Show crest bound message");
        ShowCredits = Config.Bind("Settings", "ShowCredits", true, "Show ending credits");
    }
}