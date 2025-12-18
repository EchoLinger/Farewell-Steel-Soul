using System.IO;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace FarewellSteelSoul;

[BepInAutoPlugin(id: "io.github.EchoLinger.FarewellSteelSoul")]
[HarmonyPatch]
public partial class FarewellSteelSoul : BaseUnityPlugin
{
    private static ManualLogSource Log;

    private static FarewellSteelSoul Instance;
    private static string Location => Path.GetDirectoryName(Instance.Info.Location)!;

    private void Awake()
    {
        Log = Logger;
        Instance = this;
        Register();
        Harmony.CreateAndPatchAll(typeof(FarewellSteelSoul).Assembly);
        Log.LogInfo($"Plugin {Name} ({Id}) has loaded!");
    }
}