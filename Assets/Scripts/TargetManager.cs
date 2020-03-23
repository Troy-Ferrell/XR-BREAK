using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace XR.Break
{
    public class TargetManager : MonoBehaviour
    {
        [SerializeField]
        private ObjectPool markerPool;

        [SerializeField]
        private ObjectPool ringPool;

        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private float RingRadius = 0.1f;

        private const float MIN_RAYCAST_DISTANCE = 1f;
        private const float MAX_RAYCAST_DISTANCE = 10.0f;
        private const float HIT_OFFSET = 0.05f;
        private float timer;

        private void Awake()
        {
            Debug.Assert(markerPool != null);
            Debug.Assert(ringPool != null);
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer > 3.0f)
            {
                timer = 0.0f;
                PlaceMarker();
                PlaceRing();
            }
        }

        private void PlaceRing()
        {
            if (markerPool.HasAvailable())
            {
                var cam = CameraCache.Main.transform;
                var direction = GenerateRandomDirection(cam);

                if (Physics.SphereCast(
                    cam.position,
                    RingRadius,
                    direction,
                    out RaycastHit hit,
                    MAX_RAYCAST_DISTANCE,
                    layerMask))
                {
                    if (hit.distance > MIN_RAYCAST_DISTANCE)
                    {
                        var midPoint = (cam.position + hit.point ) / 2.0f;

                        foreach (var target in ringPool.ActiveObjects)
                        {
                            if ((target.GetGameObject().transform.position - midPoint).sqrMagnitude < 0.25f * 0.25f)
                            {
                                return;
                            }
                        }

                        var marker = ringPool.Request().GetGameObject();

                        marker.transform.position = midPoint;
                        marker.transform.rotation = Quaternion.LookRotation(direction);
                    }
                }
            }
        }

        private void PlaceMarker()
        {
            if (markerPool.HasAvailable())
            {
                var cam = CameraCache.Main.transform;

                if (Physics.Raycast(
                    cam.position,
                    GenerateRandomDirection(cam),
                    out RaycastHit hit,
                    MAX_RAYCAST_DISTANCE,
                    layerMask))
                {
                    if (hit.distance > MIN_RAYCAST_DISTANCE &&
                        hit.transform.gameObject.layer == 31)
                    {
                        var placementPoint = hit.point + hit.normal * HIT_OFFSET;

                        foreach (var target in markerPool.ActiveObjects)
                        {
                            if ( (target.GetGameObject().transform.position - placementPoint).sqrMagnitude < 0.25f * 0.25f)
                            {
                                return;
                            }
                        }

                        var marker = markerPool.Request().GetGameObject();

                        marker.transform.position = placementPoint;
                        marker.transform.rotation = Quaternion.LookRotation(hit.normal);
                    }
                }
            }
        }

        private static Vector3 GenerateRandomDirection(Transform transform)
        {
            var topDownForward = new Vector3(transform.forward.x, 0.0f, transform.forward.z);

            // Generate random raycast in 180 degree FOV of camera
            var raycastDir = Vector3.Lerp(topDownForward - transform.right, topDownForward + transform.right, Random.Range(0.0f, 1.0f));
            raycastDir = Vector3.Lerp(raycastDir - Vector3.up, raycastDir + Vector3.up, Random.Range(0.0f, 1.0f));

            return raycastDir;
        }
    }
}