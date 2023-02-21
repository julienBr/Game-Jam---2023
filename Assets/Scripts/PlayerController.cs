using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speedH;
    [SerializeField] private float _speedV;
    [SerializeField] private float _jump;
    private Animator _animator;
    private Rigidbody _body;
    private bool _lookBack;
    private Vector3 _currentPosition;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _currentPosition = transform.position;
        Move();
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(!_lookBack) LookBack();
            else LookForward();
        }
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
    }

    private void Move()
    {
        transform.Translate(Input.GetAxis("Horizontal") * _speedH * Time.deltaTime, 0f, _speedV * Time.deltaTime);
    }

    private void LookBack()
    {
        _lookBack = true;
        _animator.SetBool("LookBack", true);
    }

    private void LookForward()
    {
        _lookBack = false;
        _animator.SetBool("LookBack", false);
    }

    private void Jump()
    {
        _body.AddForce(0f, _jump, 0f);
    }
}