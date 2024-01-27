using System;
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
        private int highscore;

        [Inject]
        private ScenesController _scenesController;

        public int Highscore
        {
            get => highscore;
            set {
                if (value > highscore)
                    highscore = value;
            }
        }

        public override void InstallBindings ()
        {
            Container.Bind<GameManager>().FromInstance(this).AsSingle().NonLazy();
        }

        public void FinishGame ()
        {
            _scenesController.LoadScene(mainMenuScene).Forget();
        }

        private void Update ()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Backspace))
                FinishGame();
#endif
        }
    }
}
