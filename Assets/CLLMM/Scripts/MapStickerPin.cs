using System.Collections.Generic;
using UnityEngine;

namespace CLLMM.Scripts
{
    public class MapStickerPin : MapPin
    {
        [SerializeField] private MapSticker _mapSticker;
        [SerializeField] private SpriteRenderer _stickerSprite;
        [SerializeField] private List<ParticleSystem> _dropParticles;

        public MapSticker MapSticker
        {
            get => _mapSticker;
            set => _mapSticker = value;
        }
        public SpriteRenderer StickerSprite => _stickerSprite;

        public void TriggerDropPartciles()
        {
            // Set colour of particles to the Sticker Color
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
        
        public override void SetPinWorldPosition(Vector3 worldPosition)
        {
            base.SetPinWorldPosition(worldPosition);
        }
    }
}