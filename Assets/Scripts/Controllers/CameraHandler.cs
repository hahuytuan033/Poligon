using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform camTrans;
        public Transform target;
        public Transform pivot;
        public bool leftPivot;
        float delta;

        float mouseX;
        float mouseY;
        float smoothX;
        float smoothY;
        float smoothXVelocity;
        float smoothYVelocity;
        float lookAngle;
        float tiltAngle;

        public CameraValues cameraValues;

        StatesManager statesManager;

        public void Init(InputHandler inputHandler)
        {
            statesManager = inputHandler.statesManager;
            target =  statesManager.transform;
        }

        public void FixedTick(float d)
        {
            delta = d;
            if (target == null)
            {
                return;
            }

            HandlePosition();
        }

        void HandlePosition()
        {
            float targetX = cameraValues.normalX;
            float targetZ = cameraValues.normalZ;
            float targetY = cameraValues.normalY;

            if (statesManager.controllerStates.isCrouching)
            {
                targetY = cameraValues.crouchY;
            }

            if (statesManager.controllerStates.isAiming)
            {
                targetX = cameraValues.aimX;
                targetZ = cameraValues.aimZ;
            }

            if (leftPivot)
            {
                targetX = -targetX;
            }

            Vector3 newPivpotPos = pivot.localPosition;
            newPivpotPos.x = targetX;












            newPivpotPos.z = targetY;

            Vector3 newCampos = camTrans.localPosition;
            newCampos.z = targetZ;

            float t = delta * cameraValues.adaptSpeed;
            pivot.localPosition = Vector3.Lerp(pivot.localPosition, newPivpotPos, t);
            camTrans.localPosition = Vector3.Lerp(camTrans.localPosition, newCampos, t);
        }
    }
}

