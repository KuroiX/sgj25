using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
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
        Debug.Log("Parry");
    }

    private void JumpOnperformed(InputAction.CallbackContext obj)
    {
        _rb.AddForce(Vector2.up * force,  ForceMode2D.Impulse);
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
}
