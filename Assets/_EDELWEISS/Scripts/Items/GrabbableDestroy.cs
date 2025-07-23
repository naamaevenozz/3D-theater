using System;
using Edelweiss.Core;
using UnityEngine;

namespace Edelweiss.Items
{
    public class GrabbableDestroy : EdelMono
    {
        [SerializeField] private float destroyTimer = 30f;
        [SerializeField] private GrabbableItem item;
        private void Start()
        {
            //DestroyCoroutine(destroyTimer);
        }

        /*private Coroutine DestroyCoroutine(float delay)
        {
            
        }*/
    }
}