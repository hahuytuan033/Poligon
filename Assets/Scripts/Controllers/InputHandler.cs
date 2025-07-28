using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    public class InputHandler : MonoBehaviour
    {
        float horizontal;
        float vertical;

        bool animInput; // Animation input for character movement
        bool sprintInput; // Sprint input for character movement
        bool shootInput; 
        bool crouchInput; 
        bool reloadInput; // Reload Gun
        bool switchInput; // Switch Weapon
        bool pivotInput;

        bool isInit;

        float delta;

        void Start()
        {
            InitinGame();
        }

        public void InitinGame()
        {
            isInit = true;
        }

        void FixedUpdate()
        {
            if (!isInit)
            {
                return;
            }

            delta = Time.fixedDeltaTime;
        }

        void Update()
        {
            if (!isInit)
            {
                return;
            }

            delta = Time.deltaTime;

            // GetInput(); // Sẽ thêm logic lấy input ở đây sau
        }
    }
}
