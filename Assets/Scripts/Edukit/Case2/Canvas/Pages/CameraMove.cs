using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public enum CameraMode { Basic, WASD };
    public CameraMode mode;
    public float speed;
    public float rotationSpeed;
    public float speedWheel;

    public UIBlock rightBlockZone;
    public UIBlock fullBlockZone;

    public int Inventory_wheel;

    public static CameraMove Instance { get; private set; }

    Vector3 forwardPosition { get; set; }
    
    bool IsBlockInside {  get { return rightBlockZone.IsIn; } }
    bool IsFullBlockInside { get { return fullBlockZone.IsIn; } }


    float mx { get; set; }
    float my { get; set; }

    public bool IsBlockRightInspectZone
    {
        get { return rightBlockZone.gameObject.activeSelf; }
        set { rightBlockZone.gameObject.SetActive(value); }
    }
    public bool IsFullBlockZone
    {
        get { return fullBlockZone.gameObject.activeSelf; }
        set { fullBlockZone.gameObject.SetActive(value); }
    }
    private void Awake()
    {
        Instance = this;        
    }
    void Start()
    {
        IsBlockRightInspectZone = false;        
        IsFullBlockZone = false;

        Vector3 angles = transform.eulerAngles;
        mx = angles.y;
        my = angles.x;
    }
    void BasicMove()
    {
        //PageMessage.Instance.Show("", "¸¶¿ì½º");
        if (Input.GetMouseButton(0) == true)
        {
            var x = Input.GetAxis("Mouse X") * speed * 0.02f;
            var y = Input.GetAxis("Mouse Y") * speed * 0.02f;
            transform.position += transform.right * (-x) + transform.up * (-y);
            
        }

        if (Input.GetMouseButtonDown(1) == true)
        {
            var rotateLength = transform.position.y * 1.5f;
            forwardPosition = transform.position + transform.forward * rotateLength;

            
        }
        if (Input.GetMouseButton(1) == true)
        {
            mx += Input.GetAxis("Mouse X") * rotationSpeed * 0.02f;
            my -= Input.GetAxis("Mouse Y") * rotationSpeed * 0.02f;

            Quaternion rotation = Quaternion.Euler(my, mx, 0);
            float distance = Vector3.Distance(forwardPosition, transform.position);
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + forwardPosition;

            transform.rotation = rotation;
            transform.position = position;


            
        }

        if (Inventory_wheel == 0)
        {
            var wheel = Input.GetAxis("Mouse ScrollWheel");

            if (wheel != 0.0f)
            {
                var nextPosition = transform.position + transform.forward * (wheel * speedWheel);

                if (nextPosition.magnitude < 120.0f && nextPosition.magnitude > 5.0f)
                {
                    transform.position += transform.forward * (wheel * speedWheel);

                }


            }
        }

        KeyboardMove();
    }
    void WASDMove()
    {
        if (Input.GetMouseButton(0) == true)
        {
            var x = Input.GetAxis("Mouse X") * speed * 0.02f;
            var y = Input.GetAxis("Mouse Y") * speed * 0.02f;
            transform.position += transform.right * (-x) + transform.up * (-y);
            
        }

        if (Input.GetMouseButton(1) == true)
        {
            mx += Input.GetAxis("Mouse X") * rotationSpeed * 0.02f;
            my -= Input.GetAxis("Mouse Y") * rotationSpeed * 0.02f;

            Quaternion rotation = Quaternion.Euler(my, mx, 0);
            transform.rotation = rotation;

           
        }
        KeyboardMove();
    }
    public void KeyboardMove()
    {
        if (Input.GetKey(KeyCode.W) == true)
            transform.position += transform.forward * speed * 0.8f * Time.deltaTime;

        if (Input.GetKey(KeyCode.S) == true)
            transform.position -= transform.forward * speed * 0.8f * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) == true)
            transform.position -= transform.right * speed * 0.8f * Time.deltaTime;

        if (Input.GetKey(KeyCode.D) == true)
            transform.position += transform.right * speed * 0.8f * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q) == true)
            transform.position += transform.up * speed * 0.8f * Time.deltaTime;

        if (Input.GetKey(KeyCode.E) == true)
            transform.position -= transform.up * speed * 0.8f * Time.deltaTime;

    }
    // Update is called once per frame
    void Update()
    {
        




        if (mode == CameraMode.Basic) BasicMove();
        if (mode == CameraMode.WASD) WASDMove();
    }
}
