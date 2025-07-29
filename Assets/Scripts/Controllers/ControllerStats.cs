using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Tundayne
{
    [CreateAssetMenu(menuName = "Controller/Stats")]
    public class ControllerStats : ScriptableObject
    {
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float SprintSpeed = 6f;
        [SerializeField] private float crouchSpeed = 2f;
        [SerializeField] private float animSpeed = 2f;
    }
}

