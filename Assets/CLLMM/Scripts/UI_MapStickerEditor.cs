using System;
using DG.Tweening;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CLLMM.Scripts
{
    /// <summary>
    /// UI editor counterpart to select and drag around MapStcikerPins from the map UI preview rect. 
    /// </summary>
    // TODO: UI Map preview rect input handler
    public class UI_MapStickerEditor : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _mapUIPreview;
        [SerializeField] private MapboxController _mapController;
        [SerializeField] private MapboxPinManager _pinManager;

        [SerializeField] private float _maxSelectionDistance = 20f;

        [SerializeField] private RectTransform _deleteIconTransform;

        public UnityEvent OnStickerPickedUp;
        public UnityEvent OnStickerDropped;
        
        private bool _hasDraggedPin;
        private MapStickerPin _draggedPin;
        private Vector2d _startPinLatLong;
        private bool _pointerOver;
        private bool _pointerDown;
        private bool _isBinHovered;

        private void Start()
        {
            TransitionDeleteIcon(false);
        }

        private void Update()
        {
            if (_hasDraggedPin)
            {
                Vector3 targetPos = _mapController.GetMapWorldPositionFromCameraUV(GetNormalisedMousePositionWithinMapRect());
                _draggedPin.transform.position = targetPos;
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _pointerOver = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector2 mousePos = GetNormalisedMousePositionWithinMapRect();
            Vector3 mapWorldPos = _mapController.GetMapWorldPositionFromCameraUV(mousePos);
            MapStickerPin mapPin = _pinManager.GetMapPinClosestToWorldPoint<MapStickerPin>(mapWorldPos, out float distance);

            if (mapPin != null && distance <= _maxSelectionDistance)
            {
                _startPinLatLong = mapPin.PinLatLong;
                _draggedPin = mapPin;
                _hasDraggedPin = true;
                _pinManager.UnregisterMapPin(_draggedPin);
                
                TransitionDeleteIcon(true);
                
                OnStickerPickedUp?.Invoke();
            }
            
            _pointerDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_hasDraggedPin && _isBinHovered)
            {
                DeleteDraggedPin();
            }
            
            if (_hasDraggedPin)
            {
                _draggedPin.PinLatLong = _pointerOver
                    ? _mapController.GetMapCoordinatesFromCameraUV(GetNormalisedMousePositionWithinMapRect())
                    : _startPinLatLong;
                
                _pinManager.RegisterMapPin(_draggedPin);
                _pinManager.UpdateMapPins();
                
                _draggedPin.TriggerDropEffect(false);
                
                _draggedPin = null;
                _hasDraggedPin = false;
                
                TransitionDeleteIcon(false);
                
                OnStickerDropped?.Invoke();
            }

            _pointerDown = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerOver = false;
        }

        public void DeleteDraggedPin()
        {
            if (_hasDraggedPin)
            {
                Destroy(_draggedPin.gameObject);
                _draggedPin = null;
                _hasDraggedPin = false;
                
                TransitionDeleteIcon(false);
                
                OnStickerDropped?.Invoke();
            }
        }
        
        public void OnBinEnter()
        {
            if (_hasDraggedPin)
            {
                _deleteIconTransform.DOScale(1.25f, 0.25f).SetEase(Ease.OutBack);
            }
            _isBinHovered = true;
        }

        public void OnBinExit()
        {
            if (_hasDraggedPin)
            {
                _deleteIconTransform.DOScale(1f, 0.25f).SetEase(Ease.InBack);
            }            
            _isBinHovered = false;
        }
        
        private void TransitionDeleteIcon(bool show)
        {
            if (show)
            {
                _deleteIconTransform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
            }
            else
            {
                _deleteIconTransform.DOScale(0f, 0.25f).SetEase(Ease.InBack);
            }
        }
        
        // TODO: UI Map preview rect input handler
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