using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Tundayne
{
    [CreateAssetMenu(menuName = "Character Controller/Character Statistics")]
    public class ControllerStatistics : ScriptableObject
    {
        [Header("Movement Speeds")]
        public float moveSpeed = 4f;
        public float SprintSpeed = 6f;
        public float crouchSpeed = 2f;
        public float aimSpeed = 2f;
        public float rotationSpeed = 8f;
        
    }
}

