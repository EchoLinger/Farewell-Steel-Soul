using System;
using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace FarewellSteelSoul;

public partial class FarewellSteelSoul
{
    private static GameObject? FindDeepChildRecursive(GameObject parent, string name)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.name == name)
                return child.gameObject;
            var result = FindDeepChildRecursive(child.gameObject, name);
            if (result != null)
                return result;
        }

        return null;
    }

    private static GameObject FindDeepChild(GameObject parent, string name)
    {
        var find = FindDeepChildRecursive(parent, name);
        return find != null
            ? find
            : throw new Exception($"GameObject {name} not found among {parent}");
    }

    private static void InsertSection(CreditsHelper creditsHelper)
    {
        var sections = creditsHelper.creditsSections;
        var original = sections[10].gameObject;
        var saveFileBreak = Instantiate(original, creditsHelper.gameObject.transform);
        saveFileBreak.name = "SaveFileBreak";
        Destroy(FindDeepChild(saveFileBreak, "Unity Technical Support"));
        Destroy(FindDeepChild(saveFileBreak, "Knights of U"));
        Destroy(FindDeepChild(saveFileBreak, "credits_plates__0002_THANKS"));
        Destroy(FindDeepChild(saveFileBreak, "Thanks"));
        var author = Instantiate(
            FindDeepChild(sections[5].gameObject, "Dressing"),
            saveFileBreak.transform);
        author.name = "Author";
        author.transform.localPosition = new Vector3(
            -author.transform.localPosition.x + 1,
            author.transform.localPosition.y - 1,
            author.transform.localPosition.z);
        var title = author.GetComponent<SetTextMeshProGameText>();
        title.Text = I18nString(I18nKeys.ModName);
        var authorValue = FindDeepChild(author, "NathAndJames");
        authorValue.name = "Authors";
        authorValue.GetComponent<SetTextMeshProGameText>().Text = I18nString(I18nKeys.Author);

        var icon = Instantiate(FindDeepChild(sections[2].gameObject, "credits_plates_LACE"),
            FindDeepChild(saveFileBreak, "bg").transform);
        icon.name = "Icon";
        var spriteRenderer = icon.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Credits;

        sections.Insert(0, saveFileBreak.GetComponent<CreditsSectionBase>());
    }

    private static IEnumerator ModSequence(CreditsHelper creditsHelper)
    {
        creditsHelper.screenFader.AlphaSelf = 1f;
        if (creditsHelper.silentSnapshot != null)
            creditsHelper.silentSnapshot.TransitionTo(0.0f);
        creditsHelper.musicSource.Play();

        InsertSection(creditsHelper);

        for (var i = 0; i < creditsHelper.creditsSections.Count; ++i)
        {
            var creditsSection = creditsHelper.creditsSections[i];
            creditsSection.gameObject.SetActive(true);
            if (i == 0 && creditsHelper.musicSnapshot != null)
                creditsHelper.musicSnapshot.TransitionTo(creditsSection.FadeUpDuration);
            yield return new WaitForSeconds(creditsHelper.screenFader.FadeTo(0.0f, creditsSection.FadeUpDuration));
            yield return creditsSection.Show();
            if (i >= creditsHelper.creditsSections.Count - 1 && creditsHelper.silentSnapshot != null)
                creditsHelper.silentSnapshot.TransitionTo(creditsSection.FadeDownDuration +
                                                          creditsHelper.timeBetweenScreens);
            yield return new WaitForSeconds(creditsHelper.screenFader.FadeTo(1f, creditsSection.FadeDownDuration));
            creditsSection.gameObject.SetActive(false);
            yield return new WaitForSeconds(creditsHelper.timeBetweenScreens);
        }

        yield return _waitForSeconds1;
        creditsHelper.cutSceneHelper.nextSceneType = CutsceneHelper.NextScene.SpecifyScene;
        creditsHelper.cutSceneHelper.nextScene = "PermaDeath";
        yield return creditsHelper.cutSceneHelper.Skip();
        GameCameras.instance.cameraController.IsBloomForced = false;
    }

    [HarmonyPatch(typeof(CreditsHelper), nameof(CreditsHelper.Sequence))]
    [HarmonyPostfix]
    public static IEnumerator ShowModCredits(IEnumerator __result, CreditsHelper __instance)
    {
        if (!IsPermaDeath)
        {
            return __result;
        }

        IsPermaDeath = false;
        return ModSequence(__instance);
    }
}