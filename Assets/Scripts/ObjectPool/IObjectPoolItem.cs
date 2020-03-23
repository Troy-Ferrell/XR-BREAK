using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XR.Break
{
    public interface IObjectPoolItem
    {
        void OnRequest();

        void OnRecycle();

        void RegisterPool(ObjectPool pool);

        GameObject GetGameObject();
    }
}
