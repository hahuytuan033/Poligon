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
        bool aimInput;

        public StatesManager statesManager;
        public CameraHandler cameraHandler;

        void Start()
        {
            statesManager.Init();
            cameraHandler.Init(this);
            isInit = true;
        }

        public void InitInGame()
        {
            statesManager.Init();
            cameraHandler.Init(this);
            isInit = true;
        }


        #region FixedUpdate
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

            cameraHandler.FixedTick(delta);
        }

        void GetInput_FixedUpdate()
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");

        }

        void InGame_UpdateStates_FixedUpdate()
        {
            statesManager.input.rotateDirection = cameraHandler.camTrans.forward;
            statesManager.input.horizontal = horizontal;
            statesManager.input.vertical = vertical;

            statesManager.input.moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            Vector3 moveDir = cameraHandler.camTrans.forward * vertical;
            moveDir += cameraHandler.camTrans.right * horizontal;
            moveDir.Normalize();
            statesManager.input.moveDirection = moveDir;

            statesManager.input.rotateDirection = cameraHandler.mTransform.forward;

        }
        #endregion

        #region Update 
        void Update()
        {
            if (!isInit)
            {
                return;
            }

            delta = Time.deltaTime;
            GetInput_Update();
            InGame_UpdateStates_Update();
            statesManager.Tick(delta);

        }

        void GetInput_Update()
        {
            aimInput = Input.GetMouseButton(1);
        }

        void InGame_UpdateStates_Update()
        {
            statesManager.controllerStates.isAiming = aimInput;
        }
        #endregion
    }

    public enum GamePhase
    {
        inGame,
        inMenu
    }
    
    
}
