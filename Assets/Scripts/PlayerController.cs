using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player movement")]
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _moveAcceleration = 5f;
    [SerializeField] private float _jumpForce = 2f;
    private Vector3 _moveDirection;
    private bool _isGrounded = true;
    private bool _canJump = true;
    Rigidbody _rigidbody;
    Vector2 _moveInput;
    
    [Header("Camera controls")]
    [SerializeField] private Transform _camera;
    [SerializeField] private float _pitchClamp = 80f;
    [SerializeField] private float _cameraspeed = 1f;
    
    private Vector2 _lookInput;
    private float _pitch;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision other)
    {
        foreach (ContactPoint contact in other.contacts) 
        {
            if (contact.normal.y > 0.5f) { 
                _isGrounded = true;
                return; }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        _isGrounded = false;
    }

    public void OnJump()
    {
        if (_isGrounded && _canJump)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            Debug.Log("isJumping");
            StartCoroutine(JumpCooldown());
        }
    }
    
    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        
    }
    
    private IEnumerator JumpCooldown()
    {
        _canJump = false;
        yield return new WaitForSeconds(0.2f);
        _canJump = true;
    }
    
    private void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        float currentAcceleration;

        if (_isGrounded) currentAcceleration = _moveAcceleration;
        else currentAcceleration = _moveAcceleration / 2.5f;

        Vector3 direction = (transform.right * _moveInput.x + transform.forward * _moveInput.y).normalized;
        Vector3 targetVelocity = direction * _maxSpeed;
        
        Vector3 currentVelocity = _rigidbody.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
        
        Vector3 newHorizontalVelocity = Vector3.MoveTowards(horizontalVelocity, targetVelocity, currentAcceleration * Time.fixedDeltaTime);
        _rigidbody.linearVelocity = new Vector3(newHorizontalVelocity.x, currentVelocity.y, newHorizontalVelocity.z);
    }

    private void Update()
    {
        float yaw = _lookInput.x * _cameraspeed;
        transform.Rotate(Vector3.up * yaw);
        
        _pitch -= _lookInput.y * _cameraspeed;
        _pitch = Mathf.Clamp(_pitch, -_pitchClamp, _pitchClamp);
        _camera.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }
    
}
