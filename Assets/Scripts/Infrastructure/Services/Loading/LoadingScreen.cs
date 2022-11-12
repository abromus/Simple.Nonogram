using System;
using DG.Tweening;
using Simple.Nonogram.Extensions;
using Simple.Nonogram.Infrastructure.Delegates;
using UniRx;
using UnityEngine;

namespace Simple.Nonogram.Infrastructure.Services.Loading
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class LoadingScreen : MonoBehaviour
    {
        private Tween _tween;
        private float _timeout;
        private Block _timeoutBlock;
        private Subject<LoadingScreen> _onScreenHided = new Subject<LoadingScreen>();

        private float _transitionTime;

        protected CanvasGroup Group { get; private set; }

        public ScreenState State { get; protected set; }

        public bool IsShowing => State == ScreenState.Showed || State == ScreenState.Showing;

        public float TransitionTime
        {
            get { return _transitionTime; }
            set
            {
                if (value < 0)
                    value = 0;

                _transitionTime = value;
            }
        }

        public IObservable<LoadingScreen> OnScreenHided => _onScreenHided;

        public abstract LoaderType Type { get; }

        protected virtual void Awake()
        {
            Group = GetComponent<CanvasGroup>();
            Group.alpha = 1;
        }

        public abstract void SetParameters(LoaderParams loaderParams);

        public virtual void Show(Block onComplete, bool withFade = true, int timeout = 0, Block timeoutBlock = null)
        {
            _timeout = timeout;
            _timeoutBlock = timeoutBlock;

            if (IsShowing)
            {
                onComplete.SafeInvoke();
                return;
            }

            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            State = ScreenState.Showing;

            StartShowing(onComplete, withFade);
        }

        public virtual void Hide(bool force = false)
        {
            if (force)
            {
                if (State == ScreenState.Hided)
                    return;

                State = ScreenState.Hiding;
                Hided();
            }
            else
            {
                if (!IsShowing)
                    return;

                var oldState = State;
                State = ScreenState.Hiding;

                if (oldState == ScreenState.Showed)
                    StartHiding();
            }
        }
        protected virtual void StartShowing(Block onComplete, bool withFade = true)
        {
            if (withFade)
            {
                _tween = Group.DOFade(1f, _transitionTime).OnComplete(() =>
                {
                    Showed(onComplete);
                    _tween = null;
                });
            }
            else
                Showed(onComplete);
        }

        protected virtual void StartHiding()
        {
            _tween = Group.DOFade(0, _transitionTime).OnComplete(() =>
            {
                Hided();
                _tween = null;
            });
        }

        protected virtual void Showed(Block onComplete)
        {
            if (State == ScreenState.Showing)
            {
                State = ScreenState.Showed;
                Group.alpha = 1f;
                onComplete.SafeInvoke();
            }
            else if (State == ScreenState.Hiding)
            {
                State = ScreenState.Showed;
                Hide();
            }
        }

        protected virtual void Hided()
        {
            if (State == ScreenState.Hiding)
            {
                State = ScreenState.Hided;
                gameObject.SetActive(false);
                Group.alpha = 0f;
            }
            else if (State == ScreenState.Showing)
            {
                Show(null);
            }
        }

        private void Update()
        {
            if (_timeout > 0)
            {
                _timeout -= Time.deltaTime;

                if (_timeout <= 0)
                {
                    _timeoutBlock.SafeInvoke();
                    _timeout = 0;
                }
            }
        }

        private void OnDisable()
        {
            _onScreenHided.OnNext(this);
        }

        public enum ScreenState
        {
            Hided,
            Showed,
            Hiding,
            Showing
        }
    }
}
