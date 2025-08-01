using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    public class StatesManager : MonoBehaviour
    {
        public ControllerStates controllerStates;
        public ControllerStatistics controllerStats;
        public InputVariables input;


        [System.Serializable]
        public class InputVariables
        {
            public float horizontal;
            public float vertical;
            public float moveAmount;
            public Vector3 moveDirection;
            public Vector3 animDirection;
            public Vector3 rotateDirection;
        }

        [System.Serializable]
        public class ControllerStates
        {
            public bool onGround;
            public bool isAiming;
            public bool isCrouching;
            public bool isRunning;
            public bool isInteracting;
        }

        #region References
        public Animator anim;
        public GameObject activeModel;
        [HideInInspector]
        public Rigidbody rigid;
        [HideInInspector]
        public Collider controllerColider;

        List<Collider> ragdollColiders = new List<Collider>();
        List<Rigidbody> ragdollRigidbodies = new List<Rigidbody>();
        [HideInInspector]
        public LayerMask ignorelayers;
        [HideInInspector]
        public LayerMask ignoreForGround;
        public CharState currentState;
        public float delta;
        #endregion

        #region Init
        public void Init()
        {

            SetupAnimator();

            //setup Rigidbody;
            rigid = GetComponent<Rigidbody>();
            rigid.isKinematic = false;
            rigid.drag = 4f;
            rigid.angularDrag = 999f;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            // setup Collider
            controllerColider = GetComponent<Collider>();

            SetupRagdoll();
            ignorelayers = ~(1 << 9);
            ignoreForGround = ~(1 << 9 | 1 << 10);
        }

        void SetupAnimator()
        {
            if (activeModel == null)
            {
                anim = GetComponentInChildren<Animator>();
                activeModel = anim.gameObject;
            }

            if (anim == null)
            {
                anim = GetComponentInChildren<Animator>();
            }

            anim.applyRootMotion = false;
        }

        void SetupRagdoll()
        {
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rigidbodies)
            {
                if (rb == rigid)
                {
                    continue;
                }
                Collider collider = rb.GetComponent<Collider>();
                collider.isTrigger = true;
                ragdollRigidbodies.Add(rb);
                ragdollColiders.Add(collider);
                rb.isKinematic = true; // Rất quan trọng: Tắt vật lý cho ragdoll lúc đầu
                rb.gameObject.layer = 10; // Layer 10 là layer Ragdoll
            }
        }
        #endregion

        #region FixedUpdate
        public void FixedTick(float d)
        {
            delta = d;
            switch (currentState)
            {
                case CharState.normal:
                    controllerStates.onGround = OnGround();
                    if (controllerStates.isAiming)
                    {
                        MovementAiming();
                    }
                    else
                    {
                        MovementNormal();
                    }
                    RotationNormal();

                    break;
                case CharState.onAir:
                    rigid.drag = 0f; // Bỏ drag khi đang trên không
                    controllerStates.onGround = OnGround();
                    break;
                case CharState.cover:
                    break;
                case CharState.vaulting:
                    break;
                default:
                    break;
            }
        }

        void MovementNormal()
        {
            if (input.moveAmount > 0.5f)
            {
                rigid.drag = 0f; // Bỏ drag khi di chuyển
            }
            else
            {
                rigid.drag = 4f; // Thêm drag khi không di chuyển
            }

            float speed = controllerStats.moveSpeed;
            if (controllerStates.isRunning)
            {
                speed = controllerStats.SprintSpeed;
            }
            if (controllerStates.isCrouching)
            {
                speed = controllerStats.crouchSpeed;
            }

            Vector3 dir = Vector3.zero;
            dir = transform.forward * (speed * input.moveAmount);
            rigid.velocity = dir;
        }

        void RotationNormal()
        {
            if (!controllerStates.isAiming)
            {
                input.rotateDirection = input.moveDirection;
            }
            Vector3 targetDir = input.rotateDirection;
            targetDir.y = 0; // Đảm bảo không có thành phần Y

            if (targetDir == Vector3.zero)
            {
                targetDir = transform.forward; // Nếu không có hướng di chuyển, giữ nguyên hướng hiện tại
            }

            Quaternion lookDir = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.root.rotation, lookDir, controllerStats.rotationSpeed * delta);
            transform.root.rotation = targetRotation;
        }

        void MovementAiming()
        {
            // Trong file ControllerStatistics.cs, bạn cần thêm dòng này:
            // public float aimSpeed = 2f;
            // Nếu chưa có, code sẽ báo lỗi. Giả sử bạn đã thêm.
            // float speed = controllerStats.aimSpeed;
            float speed = controllerStats.moveSpeed; // Tạm dùng movespeed vì aimSpeed chưa có
            Vector3 v = input.moveDirection * speed;
            rigid.velocity = v;
        }
        #endregion  // <---- DÒNG NÀY ĐÃ ĐƯỢC THÊM VÀO

        #region Update

        public void Tick(float d)
        {
            delta = d;
            switch (currentState)
            {
                case CharState.normal:
                    controllerStates.onGround = OnGround();
                    HandleAnimationAll(); // Gọi hàm này để xử lý tất cả animation
                    break;
                case CharState.onAir:
                    controllerStates.onGround = OnGround();
                    break;
                case CharState.cover:
                    break;
                case CharState.vaulting:
                    break;
                default:
                    break;
            }
        }

        void HandleAnimationAll()
        {
            anim.SetBool("spring", controllerStates.isRunning);
            anim.SetBool("aiming", controllerStates.isAiming);
            anim.SetBool("crouching", controllerStates.isCrouching);

            if (controllerStates.isAiming)
            {
                HandleAnimationAiming();
            }
            else
            {
                HandleAnimationNormal();
            }
        }

        void HandleAnimationNormal()
        {
            float animVertical = input.moveAmount;
            anim.SetFloat("Vertical", animVertical, 0.15f, delta);
        }

        void HandleAnimationAiming()
        {
            float v = input.vertical;
            float h = input.horizontal;

            anim.SetFloat("horizontal", h, 0.2f, delta);
            anim.SetFloat("vertical", v, 0.2f, delta);
        }

        #endregion

        // kiểm tra xem có mặt đất ngay bên dưới nhân vật không
        bool OnGround()
        {
            Vector3 origin = transform.position;
            origin.y += 0.6f; // Offset to avoid starting the raycast inside the ground
            Vector3 direction = -Vector3.up;
            float distance = 0.7f; // Khoảng cách kiểm tra va chạm
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, distance))
            {
                Vector3 touchPoint = hit.point;
                transform.position = touchPoint;
                return true;
            }
            return false;
        }
    }

    public enum CharState
    {
        normal,
        onAir,
        cover,
        vaulting

    }
}