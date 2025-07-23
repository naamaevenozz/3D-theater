using System;
using Edelweiss.Core;
using Unity.Mathematics;
using UnityEngine;

namespace Edelweiss.Core.Player
{
    public class PlayerRotate : EdelMono
    {
        [SerializeField] private Transform leftPlayer;
        [SerializeField] private Transform rightPlayer;
        private bool rotated = false; 
        
        
        void Update()
        {
            if (leftPlayer.position.x > rightPlayer.position.x && rotated == false)
            {
                rotated = true;
                leftPlayer.rotation = Quaternion.Euler(0, -90, 0);
                rightPlayer.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if(rotated)
            {
                rotated = false;
                leftPlayer.rotation = Quaternion.Euler(0, 90, 0);
                rightPlayer.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
    }
}
