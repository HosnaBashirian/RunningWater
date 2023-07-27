using _Scripts.Controller.General;
using _Scripts.Models.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Utils
{
    [RequireComponent(typeof(RectTransform))]
    public class AnimatedButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe[2]
        {
            new Keyframe(0, 0, 2, 2),
            new Keyframe(1, 1, 0.0f, 0.0f)
        });

        [SerializeField, Range(0.8f, 1.2f)] private float onHoldSizeMultiplier = .9f;

        [SerializeField] private bool animateOnEnable = true;

        [SerializeField, Range(0, 25)] private int order = 0;

        public int Order
        {
            get => order;
            set => order = value;
        }

        [SerializeField] public bool interactable = true;

        public float duration = 0.2f;
        private const float DelayDuration = 0.1f;

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

        private void OnEnable()
        {
            if (gameObject.name.Equals("OverlayNoticeButton"))
            {
                return;
            }

            if (!animateOnEnable)
            {
                RectTransform.DOComplete();
                RectTransform.localScale = Vector3.one;
                return;
            }

            RectTransform.DOComplete();
            RectTransform.localScale = Vector3.zero;
            RectTransform.DOScale(Vector3.one, duration).SetEase(curve).SetDelay(order * DelayDuration);
        }

        public void Disable()
        {
            RectTransform.DOComplete();
            RectTransform.localScale = Vector3.one;
            RectTransform.DOScale(Vector3.zero, duration).SetEase(curve).OnComplete(() => gameObject.SetActive(false));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable) return;
            RectTransform.DOKill();
            RectTransform.DOScale(Vector3.one * onHoldSizeMultiplier, duration).SetEase(curve);
            GameHub.Instance.AudioPlayer.PlayOneShot(SoundType.Click);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable) return;
            RectTransform.DOKill();
            RectTransform.DOScale(Vector3.one, duration).SetEase(curve);
        }
    }
}