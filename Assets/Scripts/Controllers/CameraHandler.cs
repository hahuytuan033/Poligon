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
        public Transform lookAt;
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
            lookAt = this.transform;
            statesManager = inputHandler.statesManager;
            target = statesManager.transform;
        }

        public void FixedTick(float d)
        {
            delta = d;
            if (target == null)
            {
                return;
            }

            HandlePosition();
            HandleRotation();

            float speed = cameraValues.moveSpeed;
            if (statesManager.controllerStates.isAiming)
            {
                speed = cameraValues.aimSpeed;
            }

            Vector3 targetPos = Vector3.Lerp(lookAt.position, target.position, delta * speed);
            lookAt.position = targetPos;
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
            newPivpotPos.y = targetY;

            Vector3 newCampos = camTrans.localPosition;
            newCampos.z = targetZ;

            float t = delta * cameraValues.adaptSpeed;
            pivot.localPosition = Vector3.Lerp(pivot.localPosition, newPivpotPos, t);
            camTrans.localPosition = Vector3.Lerp(camTrans.localPosition, newCampos, t);
        }

        void HandleRotation()
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            if (cameraValues.turnSmooth > 0)
            {
                smoothX = Mathf.SmoothDamp(smoothX, mouseX, ref smoothXVelocity, cameraValues.turnSmooth);
                smoothY = Mathf.SmoothDamp(smoothY, mouseY, ref smoothYVelocity, cameraValues.turnSmooth);
            }
            else
            {
                smoothX = mouseX;
                smoothY = mouseY;
            }

            lookAngle += smoothX * cameraValues.y_rotation_speed * delta;
            Quaternion targetRotation = Quaternion.Euler(0, lookAngle, 0);
            lookAt.rotation = targetRotation;

            tiltAngle -= smoothY * cameraValues.x_rotation_speed * delta;
            tiltAngle = Mathf.Clamp(tiltAngle, cameraValues.minAngle, cameraValues.maxAngle);
            pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
         }
    }
}

