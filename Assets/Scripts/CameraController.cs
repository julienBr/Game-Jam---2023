using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Animator _camAnimator;
    [SerializeField] private AppDatas _data;
    public float minX = -60f;
    public float maxX = 60f;
    public float sensitivity;
    public Camera cam;
    private float _rotY;
    private float _rotX;
    private bool _lookBack;
    private Movement _move;
    public delegate void DeathEvent();
    public static event DeathEvent PlayerDeath;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _move = GetComponent<Movement>();
    }
    
    private void Update()
    {
        _rotY += Input.GetAxis("Mouse X") * sensitivity;
        _rotX += Input.GetAxis("Mouse Y") * sensitivity;

        _rotX = Mathf.Clamp(_rotX, minX, maxX);
        if (Time.timeScale == 1f)
        {
            transform.localEulerAngles = new Vector3(0, _rotY, 0);
            cam.transform.localEulerAngles = new Vector3(-_rotX, 0, _move.tilt);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if(!_lookBack) LookBack();
            else LookForward();
        }
    }
    
    private void LookBack()
    {
        _lookBack = true;
        _camAnimator.SetBool("LookBack", true);
    }

    private void LookForward()
    {
        _lookBack = false;
        _camAnimator.SetBool("LookBack", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Death"))
        {
            PlayerDeath?.Invoke();
        }
    }
}