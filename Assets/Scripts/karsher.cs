using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class karsher : MonoBehaviour
{
    [SerializeField] private float _raycastLength = 5f;
    [SerializeField] private PlayerInput _playerInput;
    private InputAction _clickAction;

    private void OnClick(InputValue value)
    {
        _clickAction = _playerInput.actions["Click"];
    }

    private void FixedUpdate()
    {
        Debug.Log(_clickAction.IsPressed());
        if (_clickAction.IsPressed())
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward * _raycastLength, Color.red);

            if (Physics.Raycast(transform.position, transform.forward, out hit, _raycastLength))
            {
                Debug.Log(hit.transform.name);
            }
        }
    }

    private void OnUse()
    {
        Debug.Log("Use");
    }

    private void OnMove(InputValue value)
    {
        Debug.Log("OnMove");
    }
}
