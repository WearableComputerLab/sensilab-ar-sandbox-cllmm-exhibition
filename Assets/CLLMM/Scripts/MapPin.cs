using Mapbox.Utils;
using UnityEngine;

namespace CLLMM.Scripts
{
    public class MapPin : MonoBehaviour
    {
        [SerializeField] private Vector2d _pinLatLong;

        public Vector2d PinLatLong
        {
            get => _pinLatLong;
            set => _pinLatLong = value;
        }
        
        public virtual void SetPinWorldPosition(Vector3 worldPosition)
        {
            transform.position = worldPosition;
        }
    }
}