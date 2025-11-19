using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace FarewellSteelSoul;

public partial class FarewellSteelSoul
{
    private static void ShowJournalMsg()
    {
        var go = EnemyJournalManager._instance.journalUpdateMessage;
        go.SetActive(true);
        var fsm = PlayMakerFSM.FindFsmOnGameObject(go, "Journal Msg");
        FSMUtility.SetBool(fsm, "Full", true);
        FSMUtility.SetBool(fsm, "Should Recycle", true);
    }

    private static readonly (int oldId, int newId)?[] DefIds = new (int oldId, int newId)?[24];

    [HarmonyPatch(typeof(tk2dSpriteAnimator), nameof(tk2dSpriteAnimator.Play), typeof(tk2dSpriteAnimationClip))]
    [HarmonyPrefix]
    public static void ReplaceAnimation(tk2dSpriteAnimationClip clip)
    {
        if (clip is not { name: "Journal Full" or "Journal Full Down" })
        {
            return;
        }

        var isDown = clip.name == "Journal Full Down";
        var baseFrame = isDown ? clip.frames.First() : clip.frames.Last();
        var basePos = baseFrame.spriteCollection.spriteDefinitions[baseFrame.spriteId].positions;

        for (var i = 0; i < clip.frames.Length; i++)
        {
            var frame = clip.frames[i];
            var def = frame.spriteCollection.spriteDefinitions[frame.spriteId];
            var key = isDown ? i + 17 : i;

            if (IsPermaDeath)
            {
                if (DefIds[key] == null)
                {
                    var newDef = JsonUtility.FromJson<tk2dSpriteDefinition>(JsonUtility.ToJson(def));
                    newDef.positions = basePos;
                    newDef.uvs =
                    [
                        new Vector2(0, 0),
                        new Vector2(1, 0),
                        new Vector2(0, 1),
                        new Vector2(1, 1)
                    ];
                    newDef.materialInst = new Material(def.materialInst)
                    {
                        mainTexture = Animations[key]
                    };
                    frame.spriteCollection.spriteDefinitions =
                        frame.spriteCollection.spriteDefinitions.AddToArray(newDef);
                    var oldId = frame.spriteId;
                    var newId = frame.spriteCollection.spriteDefinitions.Length - 1;
                    DefIds[key] = (oldId, newId);
                }

                frame.spriteId = DefIds[key]!.Value.newId;
            }
            else if (DefIds[key] != null)
            {
                frame.spriteId = DefIds[key]!.Value.oldId;
            }
        }
    }
}