using ItemChanger.Serialization;
using ItemChanger.Silksong.Assets;
using UnityEngine;

namespace ItemChanger.Silksong.Serialization.SpecialSprites;

public class EvaHealSprite : IValueProvider<Sprite>
{
    public Sprite Value => GameObjectKeys.ANCESTRAL_ART_GET_PROMPT
        .GetGameObjectPrefab()
        .GetComponent<PowerUpGetMsg>()
        .powerUpInfos[(int)PowerUpGetMsg.PowerUps.EvaHeal]
        .PromptSprite;
}
