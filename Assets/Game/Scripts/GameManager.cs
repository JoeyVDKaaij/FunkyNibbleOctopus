using Game.Scenes;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameManager : MonoInstaller
    {
        [Header("Scenes")]
        [SerializeField]
        private SceneContainer mainMenuScene;

        [SerializeField]
        private SceneContainer gameScene;

        [Header("Score")]
        [SerializeField]
        private float highscore;
        public float Highscore
        {
            get => highscore;
            private set {
                if (value > highscore)
                    highscore = value;
            }
        }

        [Inject]
        private ScenesController _scenesController;

        public override void InstallBindings ()
        {
            Container.Bind<GameManager>().FromInstance(this).AsSingle().NonLazy();
        }

        public void FinishGame (float score)
        {
            Highscore = score;
            _scenesController.LoadScene(mainMenuScene).Forget();
        }

        private void Update ()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Backspace))
                FinishGame(0);
#endif
        }
    }
}
