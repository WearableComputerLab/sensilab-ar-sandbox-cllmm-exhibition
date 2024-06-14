using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CLLMM.Scripts
{
    /// <summary>
    /// Controller for AbstractMap and Mapbox API. Handles smooth map translations and scaling to target locations,
    /// map pins etc. 
    /// </summary>
    public class MapboxController : MonoBehaviour
    {
        [SerializeField] private AbstractMap _map;
        [SerializeField] private Camera _mapCamera;
        [SerializeField] private float _transitionZoomSmoothTime = 1.0f;
        [SerializeField] private float _transitionLatLongSmoothTime = 1.0f;
        [SerializeField] private Text _coordinatesText;
        
        private bool _isTransitionActive;
        private float _transitionTargetZoom;
        private Vector2d _transitionTargetLatLong;
        private float _transitionZoomCurrentVel;

        private void OnEnable()
        {
            _map.OnUpdated += OnMapUpdated;
        }

        private void OnDisable()
        {
            _map.OnUpdated -= OnMapUpdated;
        }

        private void Start()
        {
            OnMapUpdated();
        }

        private void Update()
        {
            if (_isTransitionActive)
            {
                _isTransitionActive = !ProcessMapTransition();
            }
        }
        
        public void TransitionToLatLong(Vector2d latLong, float zoom)
        {
            _transitionTargetZoom = zoom;
            _transitionTargetLatLong = latLong;
            
            _isTransitionActive = true;
        }

        public void CancelCurrentTransition()
        {
            _isTransitionActive = false;
        }

        public void TransitionToLocation(MapboxLocation location)
        {
            TransitionToLatLong(location.LatLong, location.Zoom);
        }
        
        public Vector3 GetMapWorldPositionFromCameraUV(Vector2 uv)
        {
            // TODO: Consider map tile position, 3D etc
            Vector3 viewportPos = new Vector3(uv.x, uv.y, 0);
            Vector3 worldPos = _mapCamera.ViewportToWorldPoint(viewportPos);
            worldPos.y = 0f;
            return worldPos;
        }

        public Vector2d GetMapCoordinatesFromCameraUV(Vector2 uv)
        {
            Vector3 worldPos = GetMapWorldPositionFromCameraUV(uv);
            return _map.WorldToGeoPosition(worldPos);
        }
        
        /// <summary>
        /// Process map translation and zoom smooth transition. Returns true once the transition is complete.
        /// </summary>
        /// <returns>True if transition is complete</returns>
        private bool ProcessMapTransition()
        {
            float mapZoom = _map.Zoom;
            mapZoom = Mathf.SmoothDamp(mapZoom, _transitionTargetZoom, ref _transitionZoomCurrentVel, _transitionZoomSmoothTime);
                
            Vector2d mapLatLong = _map.CenterLatitudeLongitude;
            mapLatLong = Vector2d.Lerp(mapLatLong, _transitionTargetLatLong, Time.deltaTime / _transitionLatLongSmoothTime);
                
            _map.UpdateMap(mapLatLong, mapZoom);

            return (Mathf.Abs(mapZoom - _transitionTargetZoom) < 0.005f &&
                    (mapLatLong - _transitionTargetLatLong).magnitude < 0.005f);
        }
        
        private void OnMapUpdated()
        {
            _coordinatesText.text = $"{_map.CenterLatitudeLongitude.x:F4}, {_map.CenterLatitudeLongitude.y:F4}, {_map.Zoom:F2}";
        }
    }
}