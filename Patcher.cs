using System.Collections;
using GlobalEnums;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarewellSteelSoul;

public partial class FarewellSteelSoul
{
    private static readonly WaitForSeconds _waitForSeconds1 = new(1f);
    private static readonly WaitForSeconds _waitForSeconds2 = new(2f);

    private static IEnumerator Chain()
    {
        if (ShowJournal.Value)
        {
            var go = EnemyJournalManager._instance.journalUpdateMessage;
            var fsm = PlayMakerFSM.FindFsmOnGameObject(go, "Journal Msg");
            yield return new WaitUntil(() => fsm == null || fsm is { ActiveStateName: "End" or "Ended" });
        }

        if (ShowQuest.Value)
        {
            yield return _waitForSeconds1;
            yield return ShowQuestMsg();
        }

        if (ShowCrest.Value)
        {
            yield return _waitForSeconds2;
            yield return ShowCrestMsg();
        }

        if (ShowCredits.Value)
        {
            yield return _waitForSeconds2;
            GameManager.ReportUnload(SceneManager.GetActiveScene().name);
            GameManager.instance.LoadScene("End_Credits");
        }
        else
        {
            IsPermaDeath = false;
            GameManager.instance.LoadScene("PermaDeath");
        }
    }

    private static bool IsPermaDeath;

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.PlayerDead))]
    public static class PatchPlayerDead
    {
        [HarmonyPrefix]
        public static void Prefix(GameManager __instance)
        {
            IsPermaDeath = __instance.playerData.permadeathMode == PermadeathModes.Dead;
            if (IsPermaDeath && ShowJournal.Value)
            {
                ShowJournalMsg();
            }
        }

        [HarmonyPostfix]
        public static IEnumerator Postfix(IEnumerator __result)
        {
            yield return __result;
            if (IsPermaDeath)
            {
                Instance.StartCoroutine(Chain());
            }
        }
    }


    [HarmonyPatch(typeof(GameManager), nameof(GameManager.LoadScene))]
    [HarmonyPrefix]
    public static bool PatchLoadScene(GameManager __instance, ref string destScene)
    {
        return !(destScene == "PermaDeath" && IsPermaDeath);
    }
}