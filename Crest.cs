using System.Collections;
using System.Linq;
using UnityEngine;

namespace FarewellSteelSoul;

public partial class FarewellSteelSoul
{
    private static IEnumerator ShowCrestMsg()
    {
        var completed = false;
        const string prefabPath = "Assets/Prefabs/UI/Messages/Tool Crest UI Msg.prefab";
        var go = AssetBundle.GetAllLoadedAssetBundles()
            .First(b => b.GetAllAssetNames().Contains(prefabPath))
            .LoadAsset<GameObject>(prefabPath);

        ToolCrestUIMsg.Spawn(Crest, go, () => { completed = true; });
        yield return new WaitUntil(() => completed);
    }
}