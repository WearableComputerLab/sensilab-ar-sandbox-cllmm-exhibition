using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CLLMM.Scripts
{
    public class UI_EventTweens : MonoBehaviour,
        IPointerEnterHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerExitHandler,
        IPointerClickHandler
    {
        public enum EventType
        {
            PointerEnter,
            PointerDown,
            PointerUp,
            PointerExit,
            PointerClick
        }

        public enum TweenType
        {
            Scale,
            Rotate,
            Move
        }
        
        [Serializable]
        public class EventTweenSettings
        {
            public EventType eventType;
            public TweenType tweenType;
            public Ease ease;
            public Vector3 tweenAmount;
            public float tweenDuration;
        }
        
        [SerializeField] private List<EventTweenSettings> _eventTweenSettings;

        // Events/delagtes for each event type
        private UnityEvent OnPointerEnterInternal = new UnityEvent();
        private UnityEvent OnPointerDownInternal = new UnityEvent();
        private UnityEvent OnPointerUpInternal = new UnityEvent();
        private UnityEvent OnPointerExitInternal = new UnityEvent();
        private UnityEvent OnPointerClickInternal = new UnityEvent();
        
        private void OnEnable()
        {
            foreach (EventTweenSettings tween in _eventTweenSettings)
            {
                RegisterTween(tween);
            }
        }

        private void OnDisable()
        {
            OnPointerEnterInternal.RemoveAllListeners();
            OnPointerDownInternal.RemoveAllListeners();
            OnPointerUpInternal.RemoveAllListeners();
            OnPointerExitInternal.RemoveAllListeners();
            OnPointerClickInternal.RemoveAllListeners();
        }

        private void RegisterTween(EventTweenSettings tweenSettings)
        {
            Action tweenAction = null;
            
            switch (tweenSettings.tweenType)
            {
                case TweenType.Scale:
                    tweenAction = ConfigureScaleTween(tweenSettings);
                    break;
                case TweenType.Rotate:
                    tweenAction = ConfigureRotateTween(tweenSettings);
                    break;
                case TweenType.Move:
                    tweenAction = ConfigureMoveTween(tweenSettings);
                    break;
            }

            if (tweenAction != null)
            {
                GetEventFromType(tweenSettings.eventType).AddListener(tweenAction.Invoke);
            }
        }

        private Action ConfigureMoveTween(EventTweenSettings tweenSettings)
        {
            return () =>
            {
                transform.DOLocalMove(tweenSettings.tweenAmount, tweenSettings.tweenDuration)
                    .SetEase(tweenSettings.ease);
            };
        }
        
        private Action ConfigureRotateTween(EventTweenSettings tweenSettings)
        {
            return () =>
            {
                transform.DOLocalRotate(tweenSettings.tweenAmount, tweenSettings.tweenDuration)
                    .SetEase(tweenSettings.ease);
            };
        }
        
        private Action ConfigureScaleTween(EventTweenSettings tweenSettings)
        {
            return () =>
            {
                transform.DOScale(tweenSettings.tweenAmount, tweenSettings.tweenDuration)
                    .SetEase(tweenSettings.ease);
            };
        }
        
        private UnityEvent GetEventFromType(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.PointerEnter:
                    return OnPointerEnterInternal;
                case EventType.PointerDown:
                    return OnPointerDownInternal;
                case EventType.PointerUp:
                    return OnPointerUpInternal;
                case EventType.PointerExit:
                    return OnPointerExitInternal;
                case EventType.PointerClick:
                    return OnPointerClickInternal;
                default:
                    return null;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterInternal.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownInternal.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpInternal.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitInternal.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnPointerClickInternal.Invoke();
        }
    }
}