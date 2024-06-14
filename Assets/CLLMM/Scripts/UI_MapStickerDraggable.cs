using System;
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
    public class UI_MapStickerDraggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private MapSticker _mapSticker;
        [SerializeField] private MapboxController _mapboxController;
        [SerializeField] private MapboxPinManager _mapboxPinManager;
        [SerializeField] private RectTransform _mapUIPreview;
        
        [SerializeField] private Image _stickerImage;
        [SerializeField] private TMP_Text _stickerLabel;
        
        [SerializeField] private RectTransform _uiDraggable;
        [SerializeField] private Image _uiDraggableImage;
        
        [SerializeField] private MapStickerPin _mapPinPrefab;

        private MapStickerPin _mapStickerPin;
        private bool _isDragging;
        private bool _isOver;
        
        private void Start()
        {
            _uiDraggable.gameObject.SetActive(false);
            
            _stickerImage.sprite = _mapSticker.StickerSprite;
            _uiDraggableImage.sprite = _mapSticker.StickerSprite;
            
            _stickerLabel.text = _mapSticker.StickerName;
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
            
            _mapStickerPin = Instantiate(_mapPinPrefab,
                _mapboxController.GetMapWorldPositionFromCameraUV(GetNormalisedMousePositionWithinMapRect()),
                Quaternion.identity);
            
            _mapStickerPin.StickerSprite.sprite = _mapSticker.StickerSprite;
            
            _isDragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _uiDraggable.gameObject.SetActive(false);

            if (_isOver)
            {
                _mapStickerPin.PinLatLong =
                    _mapboxController.GetMapCoordinatesFromCameraUV(GetNormalisedMousePositionWithinMapRect());
                _mapboxPinManager.RegisterMapPin(_mapStickerPin);
                _mapStickerPin = null;
            }
            else
            {
                Destroy(_mapStickerPin);
            }
            
            _isDragging = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isOver = false;
        }
        
        private Vector2 GetNormalisedMousePositionWithinMapRect()
        {
            Vector2 localMousePos = Input.mousePosition - _mapUIPreview.position;
            Vector2 normalisedMousePos = new Vector2(localMousePos.x / _mapUIPreview.rect.width, localMousePos.y / _mapUIPreview.rect.height);
            normalisedMousePos.x += 0.5f;
            normalisedMousePos.y += 0.5f;
            return normalisedMousePos;
        }
    }
}