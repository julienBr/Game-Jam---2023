using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;
    [SerializeField] private Transform _orientation;
    public static float _xRotation;
    private float _yRotation;

    private void Start()
    {
        Cursor.visible = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void Update()
    {
        Cursor.lockState = PlayerMovement._gameCondition ? CursorLockMode.None : CursorLockMode.Locked;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * _sensX;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * _sensY;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        _yRotation += mouseX;
        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        _orientation.rotation = Quaternion.Euler(0f, _yRotation, 0f);
        
    }
}