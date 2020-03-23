using UnityEngine;
using UnityEngine.Events;

namespace XR.Break
{
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

        protected bool active = false;
        private float timer = 0.0f;

        private const string SpinAnimationTrigger = "Spin";
        private const string SpinOutAnimationTrigger = "SpinOut";

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

        protected virtual void OnDisable()
        {
            active = false;
        }

        protected virtual void Update()
        {
            if (active && AutoDestruct)
            {
                timer += Time.deltaTime;
                if (timer > Lifetime)
                {
                    Captured();
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (active)
            {
                Captured();
            }
        }

        protected virtual void Captured()
        {
            active = false;
            PlayAnimation(true);
            OnCapture?.Invoke();
        }

        #endregion
    }
}
