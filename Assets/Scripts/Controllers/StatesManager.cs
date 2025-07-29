using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    public class StatesManager : MonoBehaviour
    {
        public ControllerStates controllerStates;
        public ControllerStats controllerStats;


        public Animator anim;
        public GameObject activeModel;
        [HideInInspector]
        public Rigidbody rigid;
        [HideInInspector]
        public Collider controllerColider;

        List<Collider> ragdollColiders = new List<Collider>();
        List<Rigidbody> ragdollRigidbodies = new List<Rigidbody>();
        public LayerMask ignorelayers;
        public LayerMask ignoreForGround;
        public Transform transform;
        public CharState currentState;

        public void Init()
        {
            transform = this.transform;

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

        public void FixedTick()
        {
            switch (currentState)
            {
                case CharState.normal:
                    break;
                case CharState.onAir:
                    break;
                case CharState.cover:
                    break;
                case CharState.vaulting:
                    break;
                default:
                    break;
            }
        }

        public void Tick()
        {
            switch (currentState)
            {
                case CharState.normal:
                    break;
                case CharState.onAir:
                    break;
                case CharState.cover:
                    break;
                case CharState.vaulting:
                    break;
                default:
                    break;
            }
        }

        // kiểm tra xem có mặt đất ngay bên dưới nhân vật không
        bool OnGround()
        {
            Vector3 origin = transform.position;
            origin.y += 0.6f; // Tăng một chút để tránh va chạm với mặt đất
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
