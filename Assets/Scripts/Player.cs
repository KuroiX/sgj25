using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Assign in Editor")] 
    [SerializeField] private ParryManager parryManager;
    [SerializeField] private Boss boss;
    
    [Header("Set in Editor")]
    [SerializeField] private float force;
    [SerializeField] private float acceleration;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float xMargin;
    [SerializeField] private float yMargin;
    [SerializeField] private LayerMask groundLayerMask;
    
    private CharacterInput _characterInput;
    private Rigidbody2D _rb;
    private Collider2D _collider;

    private float _movement;

    private bool _isGrounded;

    private bool _isAllowedToJump;
    
    
    private void Awake()
    {
        _characterInput = new CharacterInput();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        _characterInput.Enable();
        
        _characterInput.Player.Move.performed += MoveOnperformed;
        _characterInput.Player.Move.canceled += MoveOnperformed;
        _characterInput.Player.Jump.performed += JumpOnperformed;
        _characterInput.Player.Parry.performed += ParryOnperformed;
        
    }

    private void ParryOnperformed(InputAction.CallbackContext obj)
    {
        ParryState parryState = parryManager.TriggerParry();
        
        Debug.Log(parryState);

        if (parryState != ParryState.Perfect && parryState != ParryState.Early) return;
        
        Jump();
        boss.Parry();
    }

    private void JumpOnperformed(InputAction.CallbackContext obj)
    {
        if (!_isAllowedToJump) return;
        
        Jump();
        _isAllowedToJump = false;
    }

    private void MoveOnperformed(InputAction.CallbackContext obj)
    {
        _movement = obj.ReadValue<float>();
    }

    private void OnDisable()
    {
        _characterInput.Disable();
        
        _characterInput.Player.Move.performed -= MoveOnperformed;
        _characterInput.Player.Jump.performed -= JumpOnperformed;
        _characterInput.Player.Parry.performed -= ParryOnperformed;
    }

    private void FixedUpdate()
    {
        bool wasGrounded = _isGrounded;
        _isGrounded = CheckGrounded();

        if (!wasGrounded && _isGrounded)
        {
            _isAllowedToJump = true;
        }

        DrawBoxDebug();
        
        Vector2 velocity = _rb.linearVelocity;

        float currentSpeed = velocity.x;
        float targetSpeed = _movement * moveSpeed;
        
        float newSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration);
        
        _rb.linearVelocity = new Vector2(newSpeed, _rb.linearVelocity.y);
    }

    private void Jump()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.y, 0);
        _rb.AddForce(Vector2.up * force,  ForceMode2D.Impulse);
    }
    
    private bool CheckGrounded()
    {
        Bounds bounds = _collider.bounds;

        Collider2D col = Physics2D.OverlapBox(
            (Vector2)bounds.center + Vector2.down * bounds.extents.y,
            new Vector2(bounds.extents.x - xMargin, yMargin) * 2,
            0f,
            groundLayerMask);

        //bool result = !ReferenceEquals(col, null); // col != null;
        bool result = col is not null; // col != null;

        return result;
    }
    
    private void DrawBoxDebug()
    {
        Color rayColor = _isGrounded ? Color.green : Color.red;

        Bounds bounds = _collider.bounds;

        // Top-left to right
        Debug.DrawRay(bounds.min + Vector3.up * yMargin + Vector3.right * xMargin,
            Vector2.right * ((bounds.extents.x - xMargin) * 2), rayColor);
        // Top-left to down
        Debug.DrawRay(bounds.min + Vector3.up * yMargin + Vector3.right * xMargin, Vector2.down * (yMargin * 2),
            rayColor);
        // Bottom-left to right
        Debug.DrawRay(bounds.min + Vector3.down * yMargin + Vector3.right * xMargin,
            Vector2.right * ((bounds.extents.x - xMargin) * 2), rayColor);
        // Top-right to down
        Debug.DrawRay(bounds.min + Vector3.up * yMargin + Vector3.right * ((bounds.extents.x - xMargin) * 2),
            Vector2.down * (yMargin * 2), rayColor);
    }
}
