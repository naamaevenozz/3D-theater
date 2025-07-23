using System.Collections.Generic;
using System.Linq;
using Edelweiss.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Edelweiss.Utils
{
    public class RigSync : EdelMono

    {
#if UNITY_EDITOR
        [SerializeField]
        private Transform prefabRoot;

        [Button]
        public void Sync()
        {
            var prefabMap = GetMap(prefabRoot);
            var thisMap   = GetMap(transform);

            foreach (string key in thisMap.Keys)
            {
                if (!prefabMap.ContainsKey(key)) continue;

                DoSingleSync(prefabMap[key], thisMap[key]);
            }
        }

        private void DoSingleSync(Transform source, Transform target)
        {
            foreach (var component in source.GetComponents<Component>())
            {
                if (component is Transform) continue;

                var targetComponent = target.GetComponent(component.GetType());
                if (targetComponent == null)
                {
                    targetComponent = target.gameObject.AddComponent(component.GetType());
                }

                // Copy values from source to target
                UnityEditor.EditorUtility.CopySerialized(component, targetComponent);
            }
        }

        private Dictionary<string, Transform> GetMap(Transform root)
        {
            Dictionary<string, Transform> map           = new();
            List<Transform>               allTransforms = root.GetComponentsInChildren<Transform>(true).ToList();

            allTransforms.ForEach((t) =>
            {
                t.gameObject.name = t.gameObject.name.Replace("mixamorig_", "mixamorig:");
                map[t.gameObject.name] = t;
            });
            return map;
        }

#endif
    }
}