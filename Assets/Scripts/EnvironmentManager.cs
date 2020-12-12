using Microsoft.MixedReality.Toolkit;

namespace XR.Break
{
    public class EnvironmentManager : Singleton<EnvironmentManager>
    {
        public bool TestSpatialObserver = false;

        private void Awake()
        {
            if (!CoreServices.CameraSystem.IsOpaque || TestSpatialObserver) // AR
            {
                // Disable virtual environment scene (i.e all children will be disabled)
                this.gameObject.SetActive(false);
            }
            else // VR
            {
                // Disable spatial mesh system if not already disabled on this platform
                CoreServices.SpatialAwarenessSystem.Disable();
            }
        }
    }
}
