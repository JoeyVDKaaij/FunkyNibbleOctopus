using System;
using Game.Scenes;
using Unity.VisualScripting;
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
            private set
            {
                if (value < highscore && _isHighscoreValid)
                    return;
    
                highscore = value;

                PlayerPrefs.SetFloat("funky-nibble-octopus-highscore", highscore);
            }
        }
        private bool _isHighscoreValid;

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

        private void Awake ()
        {
            _isHighscoreValid = PlayerPrefs.HasKey("funky-nibble-octopus-highscore");
            Highscore = PlayerPrefs.GetFloat("funky-nibble-octopus-highscore", 0f);
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
