using UnityEngine;
using Zenject;

namespace Game.Plates
{
    [CreateAssetMenu(fileName = "PlatesRepository", menuName = "Plates/Repository", order = 0)]
    public class PlatesRepository : ScriptableObjectInstaller
    {
        [SerializeField]
        private PlateContainer[] plateContainers;

        public PlateContainer[] PlateContainers => plateContainers;
        
        public PlateContainer GetByType (string type)
        {
            foreach (var plateContainer in plateContainers)
                if (plateContainer.Type == type)
                    return plateContainer;

            return null;
        }

        public override void InstallBindings ()
        {
            Container.Bind<PlatesRepository>().FromNewScriptableObject(this).AsSingle().NonLazy();
        }
    }
}
