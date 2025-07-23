using System;
using Edelweiss.Utils;
using UnityEngine;

namespace Edelweiss.Core.tempScripts
{
    public class PlayerZHandler : EdelMono
    {
        [SerializeField]
        private Rigidbody rootRigidbody;

        private void FixedUpdate()
        {
            rootRigidbody.MovePosition(rootRigidbody.position.With(z: 0f));
            rootRigidbody.linearVelocity = rootRigidbody.linearVelocity.With(z: 0f);
        }
    }
}