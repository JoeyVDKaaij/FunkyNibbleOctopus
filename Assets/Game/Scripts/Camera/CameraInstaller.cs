namespace Game.Camera
{
    public class CameraInstaller : Zenject.MonoInstaller, Zenject.IFactory<UnityEngine.Camera>
    {
        [UnityEngine.SerializeField]
        private new UnityEngine.Camera camera;

        public UnityEngine.Camera Camera => camera;

        public override void InstallBindings ()
        {
            Container.Bind<UnityEngine.Camera>().FromInstance(camera).AsSingle().NonLazy();
        }

        public UnityEngine.Camera Create ()
        {
            return camera;
        }
    }
}
