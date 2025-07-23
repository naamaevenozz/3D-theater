using System;
using Edelweiss.Core;
using UnityEngine;

namespace Edelweiss.UI
{
    public class GrabbablePlayButton : EdelMono
    {
        private bool isActive;

        private GameManager gameManager;

        private Transform anchorTransform;

        private void Start()
        {
            isActive    = true;
            gameManager = GameManager.Instance;

            MyMethod();
        }

        private void MyMethod()
        {
            LogInfo("Successfully called MyMethod!");
        }
    }
}