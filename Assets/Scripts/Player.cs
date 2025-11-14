using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Assign in Editor")] 
    [SerializeField] private ParryManager parryManager;
    
    [Header("Set in Editor")]
    [SerializeField] private float force;
    [SerializeField] private float acceleration;
    [SerializeField] private float moveSpeed;
    
    private CharacterInput _characterInput;
    private Rigidbody2D _rb;

    private float _movement;
    
    
    private void Awake()
    {
        _characterInput = new CharacterInput();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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

        if (parryState == ParryState.Perfect || parryState == ParryState.Early)
        {
            Jump();
        }
    }

    private void JumpOnperformed(InputAction.CallbackContext obj)
    {
        Jump();
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
}
