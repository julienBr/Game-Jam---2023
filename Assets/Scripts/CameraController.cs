using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minX = -60f;

    public float maxX = 60f;

    public float sensitivity;

    public Camera cam;

    private float _rotY = 0f;

    private float _rotX = 0f;
    
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    void Update()
    {
        _rotY += Input.GetAxis("Mouse X") * sensitivity;
        _rotX += Input.GetAxis("Mouse Y") * sensitivity;

        _rotX = Mathf.Clamp(_rotX, minX, maxX);

        transform.localEulerAngles = new Vector3(0, _rotY, 0);
        cam.transform.localEulerAngles = new Vector3(-_rotX, 0, 0);
    }
}
