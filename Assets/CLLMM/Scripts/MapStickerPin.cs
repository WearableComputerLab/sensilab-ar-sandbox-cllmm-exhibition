using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace CLLMM.Scripts
{
    public class MapStickerPin : MapPin
    {
        [SerializeField] private MapSticker _mapSticker;
        [SerializeField] private SpriteRenderer _stickerSprite;
        [SerializeField] private List<ParticleSystem> _dropParticles;
        [SerializeField] private Transform _spriteContainer;

        public MapSticker MapSticker
        {
            get => _mapSticker;
            set => _mapSticker = value;
        }
        public SpriteRenderer StickerSprite => _stickerSprite;

        public void TriggerDropEffect(bool playPartcles)
        {
            if (playPartcles)
            {
                foreach (ParticleSystem dropParticle in _dropParticles)
                {
                    var mainModule = dropParticle.main;
                    mainModule.startColor = _mapSticker.StickerColor;
                }
            
                foreach (ParticleSystem dropParticle in _dropParticles)
                {
                    dropParticle.Play();
                }
            }
            
            // Animate sticker sprite scale by popping out and back
            _spriteContainer.DOScale(Vector3.one * 1.5f, 0.25f)
                .OnComplete(() => _spriteContainer.DOScale(Vector3.one, 0.25f));
        }
        
        public override void SetPinWorldPosition(Vector3 worldPosition)
        {
            base.SetPinWorldPosition(worldPosition);
        }
    }
}