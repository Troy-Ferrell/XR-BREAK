using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.WSA;

namespace XR.Break
{
    /// <summary>
    /// Class manages life-cycle of a Target and whether the item has been captured via collision
    /// </summary>
    public class Target : MonoBehaviour
    {
        [Header("References")]

        [SerializeField]
        private DefaultObjectPoolItem PoolItem;

        [SerializeField]
        private Animator SpinAnimator;

        [Header("Lifetime")]

        public bool AutoDestruct = true;

        public float Lifetime = 15f;

        [Header("Events")]

        public UnityEvent OnSpawn = new UnityEvent();
        public UnityEvent OnCapture = new UnityEvent();
        public UnityEvent OnRelease = new UnityEvent();

        protected bool active = false;
        private float timer = 0.0f;

        private const string SpinAnimationTrigger = "Spin";
        private const string SpinOutAnimationTrigger = "SpinOut";

        protected const uint DEFAULT_SCORE = 10;

        private WorldAnchor anchor = null;

        public void SpinStart()
        {
            if (!active)
            {
                // We are playing an animation in reverse and it has ended
                PoolItem.Reset();
            }
        }

        public void PlayAnimation(bool inReverse = false)
        {
            SpinAnimator.SetTrigger(inReverse ? SpinOutAnimationTrigger : SpinAnimationTrigger);
        }

        #region MonoBehaviour implementations

        private void Awake()
        {
            Debug.Assert(SpinAnimator != null);
        }

        protected virtual void OnEnable()
        {
            active = true;
            timer = 0.0f;

            PlayAnimation();
            OnSpawn?.Invoke();
        }

        public void Lock()
        {
            if (anchor == null)
            {
                anchor = transform.parent.gameObject.AddComponent<WorldAnchor>();
            }
        }

        public void Unlock()
        {
            if (anchor != null)
            {
                Destroy(anchor);
                anchor = null;
            }
        }

        protected virtual void OnDisable()
        {
            active = false;
            Unlock();
        }

        protected virtual void Update()
        {
            if (active && AutoDestruct)
            {
                timer += Time.deltaTime;
                if (timer > Lifetime)
                {
                    Release();
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (active)
            {
                ScoreManager.Instance.AddScore(DEFAULT_SCORE);
                OnCapture?.Invoke();
                Release();
            }
        }

        protected virtual void Release()
        {
            active = false;
            PlayAnimation(true);
            OnRelease?.Invoke();
        }

        #endregion
    }
}
