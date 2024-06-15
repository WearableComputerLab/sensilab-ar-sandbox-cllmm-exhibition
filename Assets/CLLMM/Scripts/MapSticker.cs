using UnityEngine;

namespace CLLMM.Scripts
{
    [CreateAssetMenu(fileName = "MapSticker", menuName = "CLLMM/MapSticker", order = 0)]
    public class MapSticker : ScriptableObject
    {
        public string StickerName;
        public Sprite StickerSprite;
        public Color StickerColor = Color.white;
    }
}