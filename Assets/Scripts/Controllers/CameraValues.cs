using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Tundayne
{
    [CreateAssetMenu(menuName = "Character Controller/Camera Values")]
    public class CameraValues : ScriptableObject
    {
        public float turnSmooth = 0.1f;
        public float moveSpeed = 9f;
        public float aimSpeed = 25f;
        public float y_rotation_speed = 8f;
        public float x_rotation_speed = 8f;
        public float minAngle = -35f;
        public float maxAngle = 35f;
        public float normalZ = -3f;
        public float normalX;
        public float normalY;
        public float aimZ = -0.5f;
        public float aimX;
        public float aimY;
        public float crouchY;
        public float adaptSpeed = 9f;
    }
}

