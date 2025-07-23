using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Edelweiss.Core.FX
{
    public class HoverItemEffect : EdelMono
    {
        [SerializeField]
        private Transform handContact;

        private Transform _objectContact;

        [SerializeField]
        private LineRenderer lineRenderer;


        private void Awake()
        {
            lineRenderer.useWorldSpace = true;
            lineRenderer.positionCount = 2;
            lineRenderer.enabled       = false;
        }

        private void Update()
        {
            if (!lineRenderer.enabled) return;
            UpdateRopeState();
        }

        private void UpdateRopeState()
        {
            lineRenderer.SetPosition(0, handContact.position);
            lineRenderer.SetPosition(1, _objectContact.position);
        }

        public void CreateString(Transform objectContact)
        {
            lineRenderer.enabled = true;
            _objectContact       = objectContact;
            lineRenderer.SetPosition(0, handContact.position);
            lineRenderer.SetPosition(1, objectContact.position);
        }

        public void ResetEffect()
        {
            _objectContact       = null;
            lineRenderer.enabled = false;
        }
    }
}