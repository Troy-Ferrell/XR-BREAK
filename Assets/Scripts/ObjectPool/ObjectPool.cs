using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XR.Break
{
    public class ObjectPool : MonoBehaviour
    {
        public GameObject Prefab;

        [SerializeField]
        private int objectPoolSize = 10;

        public int PoolSize
        {
            get => objectPoolSize;
        }

        public IReadOnlyCollection<IObjectPoolItem> ActiveObjects
        {
            get => activeObjects;
        }

        public IReadOnlyCollection<IObjectPoolItem> AvailableObjects
        {
            get => availableObjects;
        }

        private HashSet<IObjectPoolItem> activeObjects = new HashSet<IObjectPoolItem>();
        private Queue<IObjectPoolItem> availableObjects = new Queue<IObjectPoolItem>();

        public bool HasAvailable()
        {
            return availableObjects.Count > 0;
        }

        private void Awake()
        {
            for (int i = 0; i < PoolSize; i++)
            {
                var gameObject = Instantiate(Prefab, transform);
                var instance = gameObject.GetComponent<IObjectPoolItem>();
                if (instance != null)
                {
                    instance.RegisterPool(this);
                    availableObjects.Enqueue(instance);
                }
                else
                {
                    Debug.LogError($"{gameObject} does not implement required interface {typeof(IObjectPoolItem).Name}");
                    Destroy(gameObject);
                }
            }
        }

        public IObjectPoolItem Request()
        {
            if (!HasAvailable())
            {
                return null;
            }

            var result = availableObjects.Dequeue();

            result.OnRequest();

            activeObjects.Add(result);

            return result;
        }

        public void RecycleObject(IObjectPoolItem returnObject)
        {
            if (activeObjects.Contains(returnObject))
            {
                activeObjects.Remove(returnObject);

                returnObject.OnRecycle();

                availableObjects.Enqueue(returnObject);
            }
        }
    }
}
