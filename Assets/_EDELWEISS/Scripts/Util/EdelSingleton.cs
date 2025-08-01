using Edelweiss.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Edelweiss.Utils
{
    /// <summary>
    /// A generic Singleton class for MonoBehaviours.
    /// Example usage: public class GameManager : MonoSingleton<GameManager>
    /// </summary>
    public class EdelSingleton<T> : EdelMono where T : EdelMono
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    var singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                    DontDestroyOnLoad(singletonObject); // Don't destroy the object when loading a new scene
                }

                return _instance;
            }
        }

        // Ensure no other instances can be created by having the constructor as protected
        protected EdelSingleton()
        {
        }

        protected void SetDestroyOnLoad() =>
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }
}