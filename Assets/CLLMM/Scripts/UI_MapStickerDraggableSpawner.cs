using System;
using System.Collections.Generic;
using UnityEngine;

namespace CLLMM.Scripts
{
    public class UI_MapStickerDraggableSpawner : MonoBehaviour
    {
        [SerializeField] private UI_MapStickerDraggable _draggablePrefab;
        [SerializeField] private Transform _container;
        [SerializeField] private List<MapSticker> _mapStickers;

        private void Start()
        {
            foreach (var mapSticker in _mapStickers)
            {
                UI_MapStickerDraggable draggable = Instantiate(_draggablePrefab, _container);
                draggable.MapSticker = mapSticker;
                draggable.gameObject.SetActive(true);
            }
        }
    }
}