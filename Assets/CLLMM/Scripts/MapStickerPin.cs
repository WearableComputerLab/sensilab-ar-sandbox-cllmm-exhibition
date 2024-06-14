using UnityEngine;

namespace CLLMM.Scripts
{
    public class MapStickerPin : MapPin
    {
        [SerializeField] private SpriteRenderer _stickerSprite;
        
        public SpriteRenderer StickerSprite => _stickerSprite;
    }
}