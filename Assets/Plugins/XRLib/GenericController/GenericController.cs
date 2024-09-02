using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WI
{
    [RequireComponent(typeof(Camera))]
    public abstract class GenericController : MonoBehaviour
    {
        public new Camera camera;
        public GenericControllerOption option;
        public MaxRangeLimitter maxRangeLimitter = new MaxRangeLimitter();
        protected Vector3 moveVector;
        protected Vector3 cameraPosition;
        public Vector3 nextPosition;

       

        public override void AfterAwake()
        {
            camera = GetComponent<Camera>();
            option.Apply(this);
            Collider maxRange = transform.parent.Find("MaxRange").GetComponent<BoxCollider>();
            maxRangeLimitter.SetRange(maxRange);
        }

        public virtual void Movement()
        {
            Move();
            Zoom();
            Rotate();
        }
        protected abstract void Move();
        protected abstract void Zoom();
        protected abstract void Rotate();
        public abstract void LastPositioning(bool limit);
        public abstract void Rewind();

        protected UserInput input;
        public bool isClickUI;
        public bool IsClickUI()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isClickUI = EventSystem.current.IsPointerOverGameObject();
            }
            else if(Input.GetMouseButtonUp(0))
            {
                isClickUI = false;
            }
            return isClickUI;
        }

        public bool onControl
        {
            get
            {
                bool onClick;
                onClick = input.leftClick || input.rightClick || input.wheelClick;

                if (input.mouseWheel != 0)
                {
                    onClick = true;
                }
                return onClick;
            }
        }

        protected virtual void LateUpdate()
        {
            if (IsClickUI())
                return;

            input.GetInput();
            Movement();
            var limitCheck = maxRangeLimitter.MoveRangeLimit(nextPosition);
            LastPositioning(limitCheck);
        }
    }
}