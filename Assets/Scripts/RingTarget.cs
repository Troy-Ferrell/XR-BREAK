using UnityEngine;

namespace XR.Break
{
    public class RingTarget : Target
    {
        private bool positiveSideStart;

        private void OnTriggerEnter(Collider other)
        {
            if (active)
            {
                positiveSideStart = Vector3.Dot((other.transform.position - transform.position).normalized, transform.forward) > 0;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (active)
            {
                bool positiveSideEnd = Vector3.Dot((other.transform.position - transform.position).normalized, transform.forward) > 0;

                // We started on one side of the sphere collider but passed through the ring to the other side on exit
                if (positiveSideStart != positiveSideEnd)
                {
                    ScoreManager.Instance.AddScore(2 * DEFAULT_SCORE);
                    OnCapture?.Invoke();
                    Release();
                }
            }
        }
    }
}