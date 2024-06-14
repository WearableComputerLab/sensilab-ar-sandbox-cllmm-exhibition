using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CLLMM.Scripts
{
    [RequireComponent(typeof(ScrollRect))]
    public class UI_DisableDragScroll : MonoBehaviour, IEndDragHandler, IBeginDragHandler
    {
        private ScrollRect EdgesScroll;

        private void Awake()
        {
            EdgesScroll = GetComponent<ScrollRect>();
        }

        public void OnBeginDrag(PointerEventData data)
        {
            EdgesScroll.StopMovement();
            EdgesScroll.enabled = false;
        }

        public void OnEndDrag(PointerEventData data)
        {
            EdgesScroll.enabled = true;
        }
    }
}