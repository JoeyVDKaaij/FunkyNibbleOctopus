using UnityEngine;

namespace Game.Tables
{
    public class TableCustomer : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        
        private TableController _tableController;

        private bool _doNotInterrupt;
        private float _nextChatterMoment;
        
        private void Start ()
        {
            animator.speed = UnityEngine.Random.Range(1f, 1.1f);
            // random animation start
            animator.Play("Idle", 0, UnityEngine.Random.Range(0f, 1f));

            _nextChatterMoment = Time.time + UnityEngine.Random.Range(1f, 5f);
        }

        private void Update ()
        {
            if (Time.time < _nextChatterMoment) {
                animator.SetBool("Chatter", false);
                return;
            }

            if (!_doNotInterrupt)
                animator.SetBool("Chatter", true);

            _nextChatterMoment = Time.time + UnityEngine.Random.Range(1f, 5f);
        }

        public void SetTableController (TableController tableController)
        {
            _tableController = tableController;
        }

        public void SignalHappy ()
        {
            _doNotInterrupt = true;
            animator.SetBool("Happy", true);
            animator.SetBool("Angry", false);
            animator.SetBool("Idle", false);
            animator.SetBool("Chatter", false);
        }

        public void SignalAngry ()
        {
            _doNotInterrupt = true;
            animator.SetBool("Happy", false);
            animator.SetBool("Angry", true);
            animator.SetBool("Idle", false);
            animator.SetBool("Chatter", false);
        }

        public void SignalIdle ()
        {
            _doNotInterrupt = false;
            animator.SetBool("Happy", false);
            animator.SetBool("Angry", false);
            animator.SetBool("Idle", true);
            animator.SetBool("Chatter", false);
        }
    }
}
