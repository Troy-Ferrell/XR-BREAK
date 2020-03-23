using UnityEngine;

namespace XR.Break
{
    public class DefaultObjectPoolItem : MonoBehaviour, IObjectPoolItem
    {
        #region IObjectPoolItem interface
        private ObjectPool myPool;

        public void RegisterPool(ObjectPool pool)
        {
            myPool = pool;
        }

        public void OnRequest()
        {
            gameObject.SetActive(true);
        }

        public void OnRecycle()
        {
            gameObject.SetActive(false);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        #endregion

        public void Reset()
        {
            myPool.RecycleObject(this);
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}