using BepInEx.Configuration;

namespace FarewellSteelSoul;

public partial class FarewellSteelSoul
{
    private static ConfigEntry<bool> DisableJournal;
    private static ConfigEntry<bool> DisableQuest;
    private static ConfigEntry<bool> DisableCrest;
    private static ConfigEntry<bool> DisableCredits;

    private void Register()
    {
        DisableJournal = Config.Bind("Settings", "DisableJournal", false, "Disable journal updated message");
        DisableQuest = Config.Bind("Settings", "DisableQuest", false, "Disable wish granted message");
        DisableCrest = Config.Bind("Settings", "DisableCrest", false, "Disable crest bound message");
        DisableCredits = Config.Bind("Settings", "DisableCredits", false, "Disable ending credits");
    }
}