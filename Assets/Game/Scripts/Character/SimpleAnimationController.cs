using UnityEngine;

namespace Game.Character
{
    public class SimpleAnimationController : MonoBehaviour
    {
        [SerializeField]
        private string idleTrigger = "Idle";

        [SerializeField]
        private string idleAnimation = "Idle";

        [SerializeField]
        private string walkTrigger = "Walk";

        [SerializeField]
        private string walkAnimation = "Walk";

        [SerializeField]
        private float speedThreshold = 1f;

        [SerializeField]
        private new Rigidbody rigidbody;

        [SerializeField]
        private Animator animator;

        private float _previousSpeedSqr;

        public void Update ()
        {
            float speedSqr = rigidbody.velocity.sqrMagnitude;
            float speedThresholdSqr = speedThreshold * speedThreshold;
            if (speedSqr > speedThresholdSqr && _previousSpeedSqr <= speedThresholdSqr) {
                animator.SetBool(idleTrigger, false);
                animator.SetBool(walkTrigger, true);
            } else if (speedSqr <= speedThresholdSqr && _previousSpeedSqr > speedThresholdSqr) {
                animator.SetBool(idleTrigger, true);
                animator.SetBool(walkTrigger, false);
            }
        }

        private void FixedUpdate ()
        {
            _previousSpeedSqr = rigidbody.velocity.sqrMagnitude;
        }

        private void Reset ()
        {
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }
    }
}
