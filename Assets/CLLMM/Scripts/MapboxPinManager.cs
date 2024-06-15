using System;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using UnityEngine;

namespace CLLMM.Scripts
{
    /// <summary>
    /// Manages registered MapPins by updating their positions based on the MapBox map updating. 
    /// </summary>
    public class MapboxPinManager : MonoBehaviour
    {
        [SerializeField] private AbstractMap _map;
        [SerializeField] private List<MapPin> _initialPins;
        
        private readonly HashSet<MapPin> _mapPins = new HashSet<MapPin>();

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
            foreach (MapPin mapPin in _initialPins)
            {
                RegisterMapPin(mapPin);
            }
        }

        public void RegisterMapPin(MapPin mapPin)
        {
            if (_mapPins.Contains(mapPin))
            {
                Debug.LogWarning("MapPin already registered.");
                return;
            }
            
            _mapPins.Add(mapPin);
            UpdateMapPins();
        }
        
        public void UnregisterMapPin(MapPin mapPin)
        {
            if (!_mapPins.Contains(mapPin))
            {
                Debug.LogWarning("MapPin not registered.");
                return;
            }
            
            _mapPins.Remove(mapPin);
        }

        public void UpdateMapPins()
        {
            foreach (MapPin mapPin in _mapPins)
            {
                mapPin.SetPinWorldPosition(_map.GeoToWorldPosition(mapPin.PinLatLong));
            }
        }

        public void DestroyAllMapPins()
        {
            foreach (MapPin mapPin in _mapPins)
            {
                Destroy(mapPin.gameObject);
            }
            
            _mapPins.Clear();
        }

        // TODO: Inefficient
        public MapPin GetMapPinClosestToWorldPoint(Vector3 worldPos, out float distance)
        {
            MapPin closestMapPin = null;
            float closestDistance = float.MaxValue;
            
            foreach (MapPin mapPin in _mapPins)
            {
                float pinDistance = Vector3.Distance(mapPin.transform.position, worldPos);
                if (pinDistance < closestDistance)
                {
                    closestMapPin = mapPin;
                    closestDistance = pinDistance;
                }
            }

            distance = closestDistance;
            return closestMapPin;
        }

        // TODO: Inefficient
        public T GetMapPinClosestToWorldPoint<T>(Vector3 worldPos, out float distance) where T : MapPin
        {
            T closestMapPin = null;
            float closestDistance = float.MaxValue;
            
            foreach (MapPin mapPin in _mapPins)
            {
                if (mapPin is T typedMapPin)
                {
                    float pinDistance = Vector3.Distance(typedMapPin.transform.position, worldPos);
                    if (pinDistance < closestDistance)
                    {
                        closestMapPin = typedMapPin;
                        closestDistance = pinDistance;
                    }
                }
            }

            distance = closestDistance;
            return closestMapPin;
        }

        private void OnMapUpdated()
        {
            UpdateMapPins();
        }
    }
}