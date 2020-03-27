using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace XR.Break
{
    /// <summary>
    /// Class that manages spawning and placement of Ring/Marker Targets into the environment
    /// </summary>
    public class TargetManager : Singleton<TargetManager>
    {
        [SerializeField]
        private ObjectPool markerPool;
        
        [SerializeField]
        private ObjectPool ringPool;

        [SerializeField]
        private LayerMask layerMask;

        [Min(0.0f)]
        [SerializeField]
        private float MinSpawnThreshold = 3.0f;

        [SerializeField]
        private float MaxSpawnThreshold = 6.0f;

        [SerializeField]
        private float MinDistanceSpace = 0.25f;

        [SerializeField]
        private float SphereCastRadius = 0.2f;

        private const float MIN_RAYCAST_DISTANCE = 1f;
        private const float MAX_RAYCAST_DISTANCE = 10.0f;
        private const float HIT_OFFSET = 0.05f;
        
        private float markerTimer;
        private float markerTimeLimit;

        private float ringTimer;
        private float ringTimeLimit;

        private float minDistanceSpaceSqr;

        // Prevent Non-singleton construction
        protected TargetManager() { }

        private void Awake()
        {
            Debug.Assert(MinSpawnThreshold < MaxSpawnThreshold, "MinSpawnThreshold is not less than MaxSpawnThreshold");

            markerTimeLimit = Random.Range(MinSpawnThreshold, MaxSpawnThreshold);
            ringTimeLimit = Random.Range(MinSpawnThreshold, MaxSpawnThreshold);

            minDistanceSpaceSqr = MinDistanceSpace * MinDistanceSpace;
        }

        private void Update()
        {
            markerTimer += Time.deltaTime;

            if (markerTimer > markerTimeLimit)
            {
                markerTimer = 0.0f;
                PlaceMarker();
            }

            ringTimer += Time.deltaTime;
            if (ringTimer > ringTimeLimit)
            {
                ringTimer = 0.0f;
                PlaceRing();
            }
        }

        private void PlaceRing()
        {
            if (ringPool.HasAvailable())
            {
                var cam = CameraCache.Main.transform;
                var direction = GenerateRandomDirection(cam);

                if (Physics.SphereCast(
                    cam.position,
                    SphereCastRadius,
                    direction,
                    out RaycastHit hit,
                    MAX_RAYCAST_DISTANCE,
                    layerMask))
                {
                    if (hit.distance > MIN_RAYCAST_DISTANCE)
                    {
                        var midPoint = (cam.position + hit.point ) / 2.0f;

                        if (IsValidPlacement(midPoint))
                        {
                            PlaceTarget(ringPool.Request(),
                                        midPoint,
                                        Quaternion.LookRotation(direction));
                        }
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
                    if (hit.distance > MIN_RAYCAST_DISTANCE)
                    {
                        var placementPoint = hit.point + hit.normal * HIT_OFFSET;

                        if (IsValidPlacement(placementPoint))
                        {
                            PlaceTarget(markerPool.Request(),
                                        placementPoint,
                                        Quaternion.LookRotation(hit.normal));
                        }
                    }
                }
            }
        }

        private void PlaceTarget(IObjectPoolItem item, Vector3 placementPoint, Quaternion rotation)
        {
            var targetObj = item.GetGameObject();
            targetObj.transform.position = placementPoint;
            targetObj.transform.rotation = rotation;

            var target = targetObj.GetComponentInChildren<Target>(true);
            if (target != null)
            {
                target.Lock();
            }
        }

        private bool IsValidPlacement(Vector3 pos)
        {
            foreach (var target in markerPool.ActiveObjects)
            {
                if (WithinDistance(target.GetGameObject().transform.position, pos, minDistanceSpaceSqr))
                {
                    return false;
                }
            }

            foreach (var target in ringPool.ActiveObjects)
            {
                if (WithinDistance(target.GetGameObject().transform.position, pos, minDistanceSpaceSqr))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool WithinDistance(Vector3 p1, Vector3 p2, float sqrDistance)
        {
            return (p1 - p2).sqrMagnitude < sqrDistance;
        }

        private static Vector3 GenerateRandomDirection(Transform transform)
        {
            var topDownForward = new Vector3(transform.forward.x, 0.0f, transform.forward.z);
            var topDownRight = new Vector3(transform.right.x, 0.0f, transform.right.z);

            // Generate random raycast in 180 degree FOV of camera
            var raycastDir = Vector3.Lerp(topDownForward - topDownRight, topDownForward + topDownRight, Random.Range(0.0f, 1.0f));
            raycastDir = Vector3.Lerp(raycastDir - Vector3.up, raycastDir + Vector3.up, Random.Range(0.0f, 1.0f));

            return raycastDir;
        }
    }
}