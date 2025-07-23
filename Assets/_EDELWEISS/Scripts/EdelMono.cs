using System.Linq;
using Edelweiss.Utils;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Edelweiss.Core
{
    public class EdelMono : MonoBehaviour
    {
        private EDEEventManager _events;

        protected EDEEventManager GameEvents
        {
            get
            {
                if (_events == null) _events = EDEEventManager.Instance;

                return _events;
            }
        }

        private EdelweissConfiguration _config;

        protected EdelweissConfiguration Config
        {
            get
            {
                if (_config == null) _config = EdelweissConfiguration.Instance;
                return _config;
            }
        }

        [SerializeField, HideInNormalInspector]
        private bool debug = false;

        protected void ValidateRawComponent<T>(ref T component,        bool self     = true, bool parents = false,
                                               bool  children = false, bool optional = false)
        {
            if (component == null)
            {
                component = GetComponent<T>();
                if (self     && component == null) component = GetComponent<T>();
                if (parents  && component == null) component = GetComponentInParent<T>();
                if (children && component == null) component = GetComponentInChildren<T>();

                if (component == null && !optional)
                {
                    Debug.LogError($"Missing component of type {typeof(T)} on {gameObject.name}");
                }
            }
        }

        protected void ValidateComponent<T>(ref T component,
                                            bool  self     = true,
                                            bool  parents  = false,
                                            bool  children = false,
                                            bool  optional = false)
            where T : Component
        {
            if (component == null)
            {
                component = GetComponent<T>();
                if (self     && component == null) component = GetComponent<T>();
                if (parents  && component == null) component = GetComponentInParent<T>();
                if (children && component == null) component = GetComponentInChildren<T>();

                if (component == null && !optional)
                {
                    Debug.LogError($"Missing component of type {typeof(T)} on {gameObject.name}");
                }
            }
        }

        protected void LogInfo(object                    message,
                               [CallerMemberName] string memberName = "",
                               [CallerFilePath]   string filePath   = "",
                               [CallerLineNumber] int    lineNumber = 0)
        {
            Debug.Log($"[{System.IO.Path.GetFileName(filePath)}:{lineNumber}::{memberName}] " + message, this);
        }

        protected void LogWarning(object                    message,
                                  [CallerMemberName] string memberName = "",
                                  [CallerFilePath]   string filePath   = "",
                                  [CallerLineNumber] int    lineNumber = 0)
        {
            Debug.LogWarning($"[{System.IO.Path.GetFileName(filePath)}:{lineNumber}::{memberName}] " + message, this);
        }

        protected void LogError(object                    message,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath]   string filePath   = "",
                                [CallerLineNumber] int    lineNumber = 0)
        {
            Debug.LogError($"[{System.IO.Path.GetFileName(filePath)}:{lineNumber}::{memberName}] " + message, this);
        }

        #region HELPER FUNCTIONS

        protected bool IsNull(object obj)
        {
            if (obj == null) return true;

            if (obj is GameObject gameObject)
                return gameObject == null;

            if (obj is Component component)
                return component == null;

            return false;
        }

        protected bool IsNotNull(object obj)
        {
            if (obj == null) return false;

            if (obj is GameObject gameObject)
                return gameObject != null;

            if (obj is Component component)
                return component != null;

            return true;
        }

        protected bool AnyNull(params    object[] objects) => objects.Any(IsNull);
        protected bool AnyNotNull(params object[] objects) => objects.Any(IsNotNull);
        protected bool AllNull(params    object[] objects) => objects.All(IsNull);
        protected bool AllNotNull(params object[] objects) => objects.All(IsNotNull);

        #endregion
    }
}