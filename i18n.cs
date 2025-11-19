using System;
using System.IO;
using System.Text;
using HarmonyLib;
using TeamCherry.Localization;
using UnityEngine;

namespace FarewellSteelSoul;

public partial class FarewellSteelSoul
{
    [Serializable]
    public class I18n
    {
        public string QuestName = "钢魂碎档";
        public string CrestName = "碎档者";
        public string CrestDesc = "在尝试完成这项壮举的过程中，你证明了自己的勇气";
        public string CrestEquip = "愿你的灵魂最终得以安息";
        public static string ModName => "Farewell Steel Soul";
        public static string Author => "HuangYunOCN\nEchoLinger";
    }

    public static class I18nKeys
    {
        public const string QuestName = "QuestName";
        public const string CrestName = "CrestName";
        public const string CrestDesc = "CrestDesc";
        public const string CrestEquip = "CrestEquip";
        public const string ModName = "ModName";
        public const string Author = "Author";
    }

    private static LanguageCode _currentLanguage = LanguageCode.ZH;

    private static I18n _i18n = new();

    [HarmonyPatch(typeof(LocalizationProjectSettings), nameof(LocalizationProjectSettings.OnSwitchedLanguage))]
    [HarmonyPostfix]
    public static void LoadI18n(LanguageCode newLang)
    {
        if (newLang == _currentLanguage)
        {
            return;
        }

        try
        {
            var filePath = Path.Combine(Location, "i18n", $"{newLang}.json");
            var text = File.ReadAllText(filePath, Encoding.UTF8);
            var i18N = JsonUtility.FromJson<I18n>(text);
            _i18n = i18N;
            _currentLanguage = newLang;
        }
        catch (Exception e)
        {
            Log.LogError(e);
        }
    }

    [HarmonyPatch(typeof(LocalisedString), nameof(LocalisedString.ToString), typeof(bool))]
    [HarmonyPostfix]
    public static void PatchLocalisedString(ref LocalisedString __instance, ref string __result)
    {
        if (__instance.Sheet == Id)
        {
            __result = __instance.Key switch
            {
                I18nKeys.QuestName => _i18n.QuestName,
                I18nKeys.CrestName => _i18n.CrestName,
                I18nKeys.CrestDesc => _i18n.CrestDesc,
                I18nKeys.CrestEquip => _i18n.CrestEquip,
                I18nKeys.ModName => I18n.ModName,
                I18nKeys.Author => I18n.Author,
                _ => __result
            };
        }
    }

    private static LocalisedString I18nString(string key)
    {
        return new LocalisedString(Id, key);
    }
}