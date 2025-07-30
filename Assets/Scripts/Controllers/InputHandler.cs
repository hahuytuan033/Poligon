using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Tundayne
{
    public class InputHandler : MonoBehaviour
    {
        float horizontal;
        float vertical;

        bool isInit;

        float delta;

        public StatesManager statesManager;
        public Transform cameraHolder;

        void Start()
        {
            statesManager.Init();
            isInit = true;
        }

        void FixedUpdate()
        {
            if (!isInit)
            {
                return;
            }

            delta = Time.fixedDeltaTime;
            GetInput_FixedUpdate();
            InGame_UpdateStates_FixedUpdate(); 
            statesManager.FixedTick(delta);
        }

        void GetInput_FixedUpdate()
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");

        }

        void InGame_UpdateStates_FixedUpdate()
        {
            statesManager.input.horizontal = horizontal;
            statesManager.input.vertical = vertical;

            statesManager.input.moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            Vector3 moveDir = cameraHolder.forward * vertical;
            moveDir += cameraHolder.right * horizontal;
            moveDir.Normalize();
            statesManager.input.moveDirection = moveDir;

        }

        void Update()
        {
            if (!isInit)
            {
                return;
            }

            delta = Time.deltaTime;
            statesManager.Tick(delta);

        }
    }
}
