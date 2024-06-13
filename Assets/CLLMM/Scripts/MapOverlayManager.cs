using System;
using UnityEngine;

namespace CLLMM.Scripts
{
    public class MapOverlayManager : MonoBehaviour
    {
        [SerializeField] private Camera _overlayProjectorCamera;
        [SerializeField] private GameObject _overlayTouchscreenUI;
        [SerializeField] private bool _showOnStart;
        
        private void Start()
        {
            if (_showOnStart)
            {
                ShowMapOverlay();
            }
            else
            {
                HideMapOverlay();
            }
        }

        public void ShowMapOverlay()
        {
            _overlayProjectorCamera.gameObject.SetActive(true);
            _overlayTouchscreenUI.gameObject.SetActive(true);
        }

        public void HideMapOverlay()
        {
            _overlayProjectorCamera.gameObject.SetActive(false);
            _overlayTouchscreenUI.gameObject.SetActive(false);
        }
    }
}