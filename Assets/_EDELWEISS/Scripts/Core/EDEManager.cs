using UnityEngine;

namespace Edelweiss.Core
{

    public class EDEManager
    {
        private static  EDEManager _instance;

        public static EDEManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EDEManager();
                    _instance.Initialize();
                }

                return _instance;
            }
        }
        
        public EDEEventManager EventManager{ get; private set; }
        
        private EDEManager() {}

        private void Initialize()
        {
            var monoManager = new GameObject("MonoManager");
            monoManager.AddComponent<MonoBehaviour>();
            UnityEngine.Object.DontDestroyOnLoad(monoManager);
            EventManager = new EDEEventManager();
        }
    }
}