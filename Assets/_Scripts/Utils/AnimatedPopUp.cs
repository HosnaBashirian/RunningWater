using System;
using DG.Tweening;
using UnityEngine;


namespace _Scripts.Utils
{
    [Flags]
    public enum PopUpBehaviour
    {
        None = 0,
        SlideDown = 1 << 0,
        SlideUp = 1 << 1,
        SlideRight = 1 << 2,
        SlideLeft = 1 << 3,
        Fade = 1 << 4,
        Scale = 1 << 5,
    }

    [RequireComponent(typeof(RectTransform))]
    public class AnimatedPopUp : MonoBehaviour
    {
      
        [SerializeField] private AnimationCurve enableCurve = new AnimationCurve(new Keyframe[2]
        {
            new Keyframe(0, 0, 2, 2),
            new Keyframe(1, 1, 0.0f, 0.0f)
        });

     
        [SerializeField] private AnimationCurve disableCurve = new AnimationCurve(new Keyframe[2]
        {
            new Keyframe(0, 0, 0, 0),
            new Keyframe(1, 1, 2.0f, 2.0f)
        });

        
        [SerializeField] private PopUpBehaviour enableBehaviour = PopUpBehaviour.Fade | PopUpBehaviour.Scale;

      
        [SerializeField] private PopUpBehaviour disableBehaviour = PopUpBehaviour.Fade | PopUpBehaviour.SlideDown;

        [SerializeField] private Vector2 offset = Vector2.zero;
        [SerializeField] private Vector2 disableOffset = Vector2.zero;

        public Action ONEnable;
        public Action onDisable;
        public bool IsEnable { get; private set; }
        private const float Duration = 0.3f;
        private RectTransform _rectTransform;

        private RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }

        private CanvasGroup _canvasGroup;

        private CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup != null) return _canvasGroup;
                _canvasGroup = GetComponent<CanvasGroup>();
                if (_canvasGroup != null) return _canvasGroup;
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }

        private void OnEnable()
        {
            Enable();
        }

        public void Enable()
        {
            IsEnable = true;
            ONEnable?.Invoke();

            if (enableBehaviour == PopUpBehaviour.None) return;
            RectTransform.DOKill();
            RectTransform.anchoredPosition = ComputeDisablePos(enableBehaviour) + disableOffset;
            RectTransform.DOAnchorPos(Vector2.zero + offset, Duration).SetEase(enableCurve);
            if ((enableBehaviour & PopUpBehaviour.Scale) != 0)
            {
                RectTransform.localScale = Vector3.zero;
                RectTransform.DOScale(Vector3.one, Duration).SetEase(enableCurve);
            }
            else
            {
                RectTransform.localScale = Vector3.one;
            }

            if ((enableBehaviour & PopUpBehaviour.Fade) != 0)
            {
                CanvasGroup.DOKill();
                CanvasGroup.alpha = 0;
                CanvasGroup.DOFade(1, Duration).SetEase(enableCurve);
            }
            else
            {
                CanvasGroup.alpha = 1;
            }
        }
        
        public void Disable()
        {
            IsEnable = false;

            if (disableBehaviour == PopUpBehaviour.None)
            {
                onDisable?.Invoke();
                return;
            }

            RectTransform.DOKill();
            var onCompleteCalled = false;
            if ((disableBehaviour & ~(PopUpBehaviour.Fade | PopUpBehaviour.Scale)) != 0)
            {
                RectTransform.anchoredPosition = Vector2.zero + offset;
                RectTransform.DOAnchorPos(ComputeDisablePosReverse(disableBehaviour) + disableOffset, Duration).SetEase(disableCurve)
                    .OnComplete(() =>
                    {
                        onDisable?.Invoke();
                    });
                onCompleteCalled = true;
            }

            if ((disableBehaviour & PopUpBehaviour.Scale) != 0)
            {
                RectTransform.localScale = Vector3.one;
                if (onCompleteCalled)
                    RectTransform.DOScale(Vector3.zero, Duration).SetEase(disableCurve);
                else
                {
                    RectTransform.DOScale(Vector3.zero, Duration).SetEase(disableCurve)
                        .OnComplete(() =>
                        {
                            onDisable?.Invoke();
                        });

                    onCompleteCalled = true;
                }
            }
            else
            {
                RectTransform.localScale = Vector3.one;
            }

            if ((disableBehaviour & PopUpBehaviour.Fade) != 0)
            {
                CanvasGroup.DOKill();
                CanvasGroup.alpha = 1;
                if (onCompleteCalled)
                    CanvasGroup.DOFade(0, Duration).SetEase(disableCurve);
                else
                {
                    CanvasGroup.DOFade(0, Duration).SetEase(disableCurve).OnComplete(() =>
                    {
                        onDisable?.Invoke();
                    });
                }
            }
            else
            {
                CanvasGroup.alpha = 1;
            }
        }

        private Vector2 ComputeDisablePos(PopUpBehaviour behaviour)
        {
            var disableX = (behaviour & PopUpBehaviour.SlideRight) != 0 ? -2500
                : (behaviour & PopUpBehaviour.SlideLeft) != 0 ? 2500 : 0;
            var disableY = (behaviour & PopUpBehaviour.SlideUp) != 0 ? -4000
                : (behaviour & PopUpBehaviour.SlideDown) != 0 ? 4000 : 0;

            return new Vector2(disableX, disableY);
        }

        private Vector2 ComputeDisablePosReverse(PopUpBehaviour behaviour)
        {
            var disableX = (behaviour & PopUpBehaviour.SlideRight) != 0 ? 2500
                : (behaviour & PopUpBehaviour.SlideLeft) != 0 ? -2500 : 0;
            var disableY = (behaviour & PopUpBehaviour.SlideUp) != 0 ? 4000
                : (behaviour & PopUpBehaviour.SlideDown) != 0 ? -4000 : 0;

            return new Vector2(disableX, disableY);
        }
    }
}