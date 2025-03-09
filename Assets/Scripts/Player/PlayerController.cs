using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] public float moveSpeed;
    private Vector2 _curMoveInput;
    public float jumpPower;
    public LayerMask groundLayerMask;

    [Header("Look")] public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    public Action inventory;
    public Action<string> itemQuckSlot;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    void Move()
    {
        Vector3 dir = transform.forward * _curMoveInput.y + transform.right * _curMoveInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;

    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _curMoveInput = context.ReadValue<Vector2>();
            Debug.Log("_curMoveInput " + _curMoveInput);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _curMoveInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("OnJump");
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursur();
        }
    }

    public void SuperJump(float jumpPadPower)
    {
        _rigidbody.AddForce(Vector2.up * jumpPadPower, ForceMode.Impulse);
    }
    
    public void OnItemUse(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            itemQuckSlot?.Invoke(context.control.name);
        }
    }
    
    void ToggleCursur()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * -0.9f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * -0.9f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * -0.9f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * -0.9f), Vector3.down),
        };

        for (int i = 0; i < rays.Length; ++i)
        {
            Debug.DrawRay(rays[i].origin, rays[i].direction * 0.1f, Color.red);
            if (Physics.Raycast(rays[i], 0.3f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void ActiveBoost(ConsumableType type, float value)
    {
        StartCoroutine(CoroutineActiveBoost(type, value));
    }

    private IEnumerator CoroutineActiveBoost(ConsumableType type, float value)
    {
        if (type == ConsumableType.Speed)
        {
            moveSpeed += value;
        }
        else if (type == ConsumableType.Jump)
        {
            jumpPower += value;
        }
        
        yield return new WaitForSeconds(5.0f);
        
        if (type == ConsumableType.Speed)
        {
            moveSpeed -= value;
        }
        else if (type == ConsumableType.Jump)
        {
            jumpPower -= value;
        }
    }
    
    
}
