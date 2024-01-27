using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scenes.UI
{
    [RequireComponent(typeof(Button))]
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private SceneContainer targetScene;

        [Inject]
        private ScenesController _scenesController;

        private Button _button;

        private void Awake ()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable ()
        {
            _button.onClick.AddListener(LoadScene);
        }

        private void OnDisable ()
        {
            _button.onClick.RemoveListener(LoadScene);
        }

        public void LoadScene ()
        {
            _scenesController.LoadScene(targetScene).Forget();
        }
    }
}
