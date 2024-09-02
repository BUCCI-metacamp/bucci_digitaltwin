using System;
using System.Collections;
using UnityEngine;

namespace WI
{
   
    public class OrbitalController : GenericController, ISingle
    {
       
        public new OrbitalControllerOption option => base.option as OrbitalControllerOption;

        
        public override void Movement()
        {
            
             nextPosition = option.target.position;

            Zoom();
            Rotate();
            Move();
        }

        protected override void Move()
        {
            if (option.moveLimit)
            {
                MoveLimt();
                return;
            }
                
            if (!input.rightClick)
            {
                return;
            }
            moveVector = Vector3.zero;

            var mouseYsen = input.mouseY * option.elevationSensivity;

            float y = mouseYsen * ((option.maxElevation - option.minElevation) / option.currentElevation);
            float z = mouseYsen * ((option.minElevation - option.currentElevation) / option.maxElevation);

            moveVector = transform.TransformDirection(input.mouseX * option.elevationSensivity, y, z);
            moveVector.y = 0;

            var t = option.currentDistance / option.maxDistance;
            option.moveClamper = Mathf.Min(t, 1f);
            moveVector *= option.moveClamper * option.moveSpeed * option.moveSensivity;
            nextPosition = option.target.position - moveVector;
        }

        protected override void Zoom()
        {
            camera.orthographicSize = camera.orthographic ? option.currentDistance : 0f;

            if (input.mouseWheel < -0.01f || input.mouseWheel > 0.01f)
            {
                var nextDistance = option.currentDistance - input.mouseWheel * option.zoomSpeed;
                option.currentDistance = Mathf.Lerp(option.currentDistance, nextDistance, option.zoomSpeed * Time.deltaTime);
                option.currentDistance = Mathf.Clamp(option.currentDistance, option.minDistance, option.maxDistance);

                camera.orthographicSize = Mathf.Clamp(option.currentDistance, option.minDistance, option.maxDistance);
            }
        }

        protected override void Rotate()
        {
            if (option.rotateLimit)
                return;
            AzimuthControl();
            ElevationControl();
        }
        protected void AzimuthControl()
        {
            if (!input.wheelClick || option.azimuthRotateLimit)
                return;
            if (input.mouseX > 0 || input.mouseX < 0)
            {
                option.currentAzimuth += input.mouseX * option.azimuthSensivity;
                if (option.currentAzimuth > 360)
                    option.currentAzimuth -= 360;
                else if (option.currentAzimuth < 0)
                    option.currentAzimuth += 360;
            }
        }

        protected void ElevationControl()
        {
            if (!input.wheelClick || option.elevationRotateLimit)
                return;
            if (input.mouseY > 0.01f || input.mouseY < -0.01f)
            {
                option.currentElevation -= input.mouseY * option.elevationSensivity;
                option.currentElevation = Mathf.Clamp(option.currentElevation, option.minElevation, option.maxElevation);
            }
        }

        public override void LastPositioning(bool limit)
        {
            if (!limit)
                return;

            option.target.position = nextPosition;
            var dist = new Vector3(0, 0, -option.currentDistance);

            cameraPosition = nextPosition + Quaternion.Euler(option.currentElevation, option.currentAzimuth, 0f) * dist;
            camera.transform.position = cameraPosition;

            if (option.currentElevation <= 90f)
            {
                camera.transform.rotation = Quaternion.Euler(option.currentElevation, option.currentAzimuth, 0f);
            }
            else
            {
                camera.transform.LookAt(option.target);
            }
        }

        public override void Rewind()
        {
            option.target.position = option.originTargetPos;
            option.target.eulerAngles = option.originTargetRot;
            option.currentDistance = option.originDistance;
            option.currentElevation = option.originElevation;
            option.currentAzimuth = option.originAzimuth;
            var dist = new Vector3(0, 0, -option.currentDistance);
            cameraPosition = option.target.position + Quaternion.Euler(option.currentElevation, option.currentAzimuth, 0f) * dist;
            camera.transform.position = cameraPosition;
            camera.transform.LookAt(option.target);

        }
        public void MoveLimt()
        {
            Rotate();
            Zoom();
            MoveCamera();
        }

        protected void LimitRotate()
        {
            if (!input.rightClick)
                return;
            if (input.mouseY > 0.01f || input.mouseY < -0.01f)
            {
                option.currentElevation -= input.mouseY * option.elevationSensivity;
                option.currentElevation = Mathf.Clamp(option.currentElevation, 10, 70);
            }
        }

        public float duration_MoveToCamera;
        public float speed_MoveToCamera;
        public float process;
        float timer;

        void ProcessWinding()
        {
            if (process > 0f)
            {
                process = 1f - process;
            }
        }
        public void SmoothMove(Vector3 pos, float distance, float elevation, float azimuth)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothMoving(pos, distance, elevation, azimuth));
        }
        IEnumerator SmoothMoving(Vector3 pos, float distance, float elevation, float azimuth)
        {
            ProcessWinding();
            while (process < 1f)
            {
                timer += speed_MoveToCamera * Time.deltaTime;
                process += timer / duration_MoveToCamera;
                option.target.position = Vector3.Lerp(option.target.position, pos, process);
                option.currentDistance = Mathf.Lerp(option.currentDistance, distance, process);
                option.currentElevation = Mathf.LerpAngle(option.currentElevation, elevation, process);
                option.currentAzimuth = Mathf.LerpAngle(option.currentAzimuth, azimuth, process);
                MoveCamera();
                yield return null;
            }
            timer = 0f;
        }

        public void MoveCamera()
        {
            //option.target.position = nextPostion;
            var dist = new Vector3(0, 0, -option.currentDistance);
            //option.outlineCamera.orthographicSize = option.currentDistance;
            cameraPosition = option.target.position + Quaternion.Euler(option.currentElevation, option.currentAzimuth, 0f) * dist;
            camera.transform.position = cameraPosition;
            camera.transform.LookAt(option.target);
        }
        

        public void MoveTo(Transform camPos, float distance)
        {
            option.target.position = camPos.transform.position;
            option.currentElevation = option.originElevation;
            option.currentAzimuth = option.originAzimuth;
            option.currentDistance = distance;
            MoveCamera();
        }
    }
}