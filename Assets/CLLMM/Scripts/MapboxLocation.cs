using Mapbox.Utils;
using UnityEngine;

namespace CLLMM.Scripts
{
    [CreateAssetMenu(fileName = "MapboxLocation", menuName = "CLLMM/MapboxLocation", order = 0)]
    public class MapboxLocation : ScriptableObject
    {
        public string LocationName;
        public Vector2d LatLong;
        public float Zoom;
    }
}