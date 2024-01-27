using UnityEngine;
using UnityEngine.UI;

namespace Game.Application.UI
{
    [RequireComponent(typeof(Button))]
    public class ApplicationQuitter : MonoBehaviour
    {
        private Button _button;

        private void Awake ()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable ()
        {
            _button.onClick.AddListener(Exit);
        }

        private void OnDisable ()
        {
            _button.onClick.RemoveListener(Exit);
        }

        public void Exit ()
        {
            UnityEngine.Application.Quit();
        }
    }
}
