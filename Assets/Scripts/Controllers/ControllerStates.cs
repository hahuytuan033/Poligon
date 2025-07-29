using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    [CreateAssetMenu(menuName = "Controller/States")]
    public class ControllerStates : ScriptableObject
    {
        public bool onGround;
        public bool isAiming;
        public bool isCrouching;
        public bool isRunning;

        public bool isInteracting;
    }
}


