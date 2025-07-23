using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Edelweiss.Core.FX
{
    public class StringContact : EdelMono
    {
        private const string WARN_MISSING_COMPONENTS = "LineRenderer or Contact Transform is not set.";

        private const int INDEX_LINE_START = 0;
        private const int INDEX_LINE_END   = 1;

        [SerializeField]
        private float LENGTH = 5f;

        [SerializeField]
        [Required]
        private Transform m_Contact;

        [SerializeField]
        [Required]
        private LineRenderer m_LineRenderer;

        [SerializeField]
        private Vector3 m_BottomOffset = Vector3.zero;

        [SerializeField]
        private Vector3 m_TopOffset = Vector3.zero;


        private void Awake()
        {
            Reset();
        }

        private void Update()
        {
            UpdatePoints();
        }

        [Button]
        private void UpdatePoints()
        {
            if (AnyNull(m_LineRenderer, m_Contact))
            {
                LogWarning(WARN_MISSING_COMPONENTS);
                return;
            }

            transform.position = m_Contact.position;

            Vector3 start = Vector3.zero + m_BottomOffset;
            Vector3 end   = start        + (LENGTH * Vector3.up) + m_TopOffset;

            m_LineRenderer.SetPosition(INDEX_LINE_START, start);
            m_LineRenderer.SetPosition(INDEX_LINE_END,   end);
        }

        private void Reset()
        {
            ValidateComponent(ref m_LineRenderer);
        }
    }
}