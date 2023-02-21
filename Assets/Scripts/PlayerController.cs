using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speedH;
    [SerializeField] private float _speedV;
    [SerializeField] private float _jump;
    private Animator _camAnimator;
    private Animator _animator;
    private Rigidbody _body;
    private bool _lookBack;

    private void Awake()
    {
        _camAnimator = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        _animator = GetComponent<Animator>();
        _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(!_lookBack) LookBack();
            else LookForward();
        }
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.LeftShift)) Slide();
    }

    private void Move()
    {
        _body.velocity = transform.right * _speedV;
        //transform.Translate(_speedV * Time.deltaTime,0f, Input.GetAxis("Horizontal") * -_speedH * Time.deltaTime);
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

    private void Jump()
    {
        _body.AddForce(0f, _jump, 0f);
    }

    private void Slide()
    {
        _animator.SetTrigger("Slide");
    }
}