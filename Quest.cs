using System.Collections;
using UnityEngine;

namespace FarewellSteelSoul;

public partial class FarewellSteelSoul
{
    private static IEnumerator ShowQuestMsg()
    {
        var completed = false;
        QuestManager.ShowQuestCompleted(Quest, () => { completed = true; });
        yield return new WaitUntil(() => completed);
    }
}