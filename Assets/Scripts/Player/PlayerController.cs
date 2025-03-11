using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private float defalutMoveSpeed;
    private Vector2 _curMoveInput;
    public float jumpPower;
    public float dashPower;
    public float dashCooldown;
    public LayerMask groundLayerMask;
    private bool isDashing = false;
    private float dashDuration = 0.2f;

    [Header("Climb")]
    public float climbSpeed;
    public float maxClimbTime;
    public Transform orientation;
    public float wallDetectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    public LayerMask wallLayerMask;

    private bool isClimbing;
    private float climbTimer;
    private float wallLookAngle;
    private RaycastHit frontWallHit;
    private bool wallFront;
    
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
        defalutMoveSpeed = moveSpeed;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
        }
    }

    //------------------------------------------------------
    // Climb

    private void WallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, wallDetectionLength, wallLayerMask);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        if (IsGrounded())
        {
            climbTimer = maxClimbTime;
        }
    }
    
    private void StartClimbing()
    {
        isClimbing = true;
    }

    private void climbingMovement()
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, climbSpeed, _rigidbody.velocity.z );
    }

    private void StopClimbing()
    {
        isClimbing = false;
        wallFront = false;
    }
    
    //------------------------------------------------------

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
        
        if ((wallFront && _curMoveInput.y > 0) && wallLookAngle < maxWallLookAngle)
        {
            if (!isClimbing && climbTimer > 0)
                StartClimbing();
            if (isClimbing)
                climbingMovement();
            if (climbTimer > 0)
                climbTimer -= Time.deltaTime;
            if (climbTimer <= 0)
                StopClimbing();
        }
        else
        {
            if (isClimbing)
                StopClimbing();
            _rigidbody.velocity = dir;
        }
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
        WallCheck();
        if (context.phase == InputActionPhase.Started && (IsGrounded() || isClimbing))
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


    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && CharacterManager.Instance.Player.condition.UseStamina(50))
        {
            Vector3 dir = transform.forward * _curMoveInput.y + transform.right * _curMoveInput.x;
            _rigidbody.AddForce(dir * dashPower, ForceMode.Impulse);

            StartCoroutine(Dash());
        }
    }
    
    private IEnumerator Dash()
    {
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
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
    
    public void KnockBack(float knockbackForce, Vector3 knockbackDirection)
    {       
        if (_rigidbody != null)
        {
            knockbackDirection.y = 0.5f;
            _rigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }
    
}

