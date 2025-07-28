using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    public class StatesManager : MonoBehaviour
    {
        public Animator anim;
        public GameObject activeModel;
        public Rigidbody rigid;
        public Collider controllerColider;

        List<Collider> ragdollColiders = new List<Collider>();
        List<Rigidbody> ragdollRigidbodies = new List<Rigidbody>();
        public LayerMask ignorelayers;
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

                rb.isKinematic = true; // Rất quan trọng: Tắt vật lý cho ragdoll lúc đầu
                Collider collider = rb.GetComponent<Collider>();
                collider.isTrigger = true;
                ragdollRigidbodies.Add(rb);
                ragdollColiders.Add(collider);
            }
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

        public void FixedTick()
        {

        }

        public void Tick()
        {

        }
    }
}
