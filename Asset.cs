using System.IO;
using UnityEngine;

namespace FarewellSteelSoul;

public partial class FarewellSteelSoul
{
    private static readonly WaitForSeconds StageGap = new(2f);

    private static Texture2D LoadTexture(string spritePath)
    {
        var filePath = Path.Combine(Location, "media", spritePath);
        var bytes = File.ReadAllBytes(filePath);
        var texture2D = new Texture2D(2, 2);
        texture2D.LoadImage(bytes);
        texture2D.wrapMode = TextureWrapMode.Clamp;
        texture2D.filterMode = FilterMode.Bilinear;
        return texture2D;
    }

    private static Sprite LoadSprite(string spritePath, Vector2? pivot = null, float? ppu = null)
    {
        var texture2D = LoadTexture(spritePath);
        var sprite = Sprite.Create(texture2D,
            new Rect(0.0f, 0.0f, texture2D.width, texture2D.height),
            pivot ?? new Vector2(0.5f, 0.5f),
            ppu ?? 100
        );
        return sprite;
    }

    private static ToolCrest? _crest;

    private static ToolCrest Crest
    {
        get
        {
            if (_crest != null)
            {
                return _crest;
            }

            var crest = Instantiate(ToolItemManager.GetCrestByName("Warrior"));
            crest.crestSprite = LoadSprite("crest.png");
            crest.displayName = I18nString(I18nKeys.CrestName);
            crest.getPromptDesc = I18nString(I18nKeys.CrestDesc);
            crest.equipText = I18nString(I18nKeys.CrestEquip);
            _crest = crest;
            return _crest;
        }
    }

    private static Quest? _quest;

    private static Quest Quest
    {
        get
        {
            if (_quest != null)
            {
                return _quest;
            }

            var quest = ScriptableObject.CreateInstance<Quest>();
            quest.questType = ScriptableObject.CreateInstance<QuestType>();
            quest.displayName = I18nString(I18nKeys.QuestName);
            var largeIcon = LoadSprite("quest.png");
            quest.questType.largeIcon = largeIcon;
            _quest = quest;
            return _quest;
        }
    }

    private static Texture[]? _animations;

    private static Texture[] Animations
    {
        get
        {
            if (_animations != null)
            {
                return _animations;
            }

            _animations = new Texture[24];

            for (var i = 1; i <= 17; i++)
            {
                var texture = LoadTexture($"animation/journal_{i:D2}.png");
                _animations[i - 1] = texture;
            }

            for (var i = 1; i <= 7; i++)
            {
                var texture = LoadTexture($"animation/journal_down_{i:D2}.png");
                _animations[i + 16] = texture;
            }

            return _animations;
        }
    }

    private static Sprite? _credits;

    private static Sprite Credits
    {
        get
        {
            if (_credits != null)
            {
                return _credits;
            }

            var credits = LoadSprite("credits.png");
            _credits = credits;
            return _credits;
        }
    }
}