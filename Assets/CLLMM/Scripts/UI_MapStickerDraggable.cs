using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CLLMM.Scripts
{
    /// <summary>
    /// UI element for dragging a map sticker onto a UI map preview rect, which will then be previewed/placed
    /// on the map as a map pin. 
    /// </summary>
    public class UI_MapStickerDraggable : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IDragHandler
    {
        [SerializeField] private MapSticker _mapSticker;
        [SerializeField] private MapboxController _mapboxController;
        [SerializeField] private MapboxPinManager _mapboxPinManager;
        [SerializeField] private RectTransform _mapUIPreview;
        
        [SerializeField] private Transform _spriteContainer;
        [SerializeField] private Image _stickerImage;
        [SerializeField] private TMP_Text _stickerLabel;
        
        [SerializeField] private RectTransform _uiDraggable;
        [SerializeField] private Image _uiDraggableImage;
        
        [SerializeField] private MapStickerPin _mapPinPrefab;

        private MapStickerPin _mapStickerPin;
        private bool _isDragging;
        private Transform _topLevelParent;

        public MapSticker MapSticker
        {
            get => _mapSticker;
            set => _mapSticker = value;
        }
        
        public RectTransform MapUIPreview
        {
            get => _mapUIPreview;
            set => _mapUIPreview = value;
        }
        
        private void Start()
        {
            _uiDraggable.gameObject.SetActive(false);
            
            _stickerImage.sprite = _mapSticker.StickerSprite;
            _uiDraggableImage.sprite = _mapSticker.StickerSprite;
            
            _stickerLabel.text = _mapSticker.StickerName;

            _topLevelParent = GetComponentInParent<Canvas>().transform;
        }

        private void Update()
        {
            if (_isDragging)
            {
                // Set draggable to mouse pos
                _uiDraggable.position = Input.mousePosition;
                
                _mapStickerPin.transform.position =
                    _mapboxController.GetMapWorldPositionFromCameraUV(GetNormalisedMousePositionWithinMapRect());
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _uiDraggable.gameObject.SetActive(true);
            _uiDraggable.transform.SetParent(_topLevelParent);

            _mapStickerPin = Instantiate(_mapPinPrefab,
                _mapboxController.GetMapWorldPositionFromCameraUV(GetNormalisedMousePositionWithinMapRect()),
                Quaternion.identity);
            
            _mapStickerPin.MapSticker = _mapSticker;
            _mapStickerPin.StickerSprite.sprite = _mapSticker.StickerSprite;
            
            _isDragging = true;

            _spriteContainer.DOScale(Vector3.one * 1.25f, 0.25f);
            
            eventData.Use();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _uiDraggable.gameObject.SetActive(false);
            _uiDraggable.transform.SetParent(transform);
            
            if (IsMouseOverMapRect())
            {
                _mapStickerPin.PinLatLong =
                    _mapboxController.GetMapCoordinatesFromCameraUV(GetNormalisedMousePositionWithinMapRect());
                _mapboxPinManager.RegisterMapPin(_mapStickerPin);
                _mapStickerPin.TriggerDropEffect(true);
                _mapStickerPin = null;
            }
            else
            {
                Destroy(_mapStickerPin.gameObject);
                _mapStickerPin = null;
            }
            
            _isDragging = false;
            
            _spriteContainer.DOScale(Vector3.one, 0.25f);
            
            eventData.Use();
        }

        public void OnDrag(PointerEventData eventData)
        {
            eventData.Use();
        }
        
        private Vector2 GetNormalisedMousePositionWithinMapRect()
        {
            Vector2 localMousePos = Input.mousePosition - _mapUIPreview.position;
            Vector2 normalisedMousePos = new Vector2(localMousePos.x / _mapUIPreview.rect.width, localMousePos.y / _mapUIPreview.rect.height);
            normalisedMousePos.x += 0.5f;
            normalisedMousePos.y += 0.5f;
            return normalisedMousePos;
        }
        
        private bool IsMouseOverMapRect()
        {
            Vector2 normPos = GetNormalisedMousePositionWithinMapRect();
            return normPos.x >= 0 && normPos.x <= 1 && normPos.y >= 0 && normPos.y <= 1;
        }
    }
}