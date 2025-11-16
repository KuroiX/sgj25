using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsParryJumping = Animator.StringToHash("IsParryJumping");
    private static readonly int Kick = Animator.StringToHash("Kick");
    private static readonly int IsAggro = Animator.StringToHash("IsAggro");

    [Header("Assign in Editor")] 
    [SerializeField] private ParryManager parryManager;
    [SerializeField] private Health bossHealth;
    [SerializeField] private Health playerHealth;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    
    [Header("Set in Editor")]
    [SerializeField] private float force;
    [SerializeField] private float forceMultiplier;
    [SerializeField] private float acceleration;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float xMargin;
    [SerializeField] private float yMargin;
    [SerializeField] private LayerMask groundLayerMask;

    public event Action<bool> AggroChanged;

    public bool IsInAggroMode
    {
        get => _isInAggroMode;
        set
        {
            _isInAggroMode = value;
            animator.SetBool(IsAggro, _isInAggroMode);
            //animator.SetTrigger(Kick);
            AggroChanged?.Invoke(_isInAggroMode);
        }
    }

    private CharacterInput _characterInput;
    private Rigidbody2D _rb;
    private Collider2D _collider;

    private float _movement;

    private bool _isGrounded;
    private bool _isAllowedToJump;

    private bool _isInAggroMode;
    
    
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
        _characterInput.Player.ActivateAggro.performed += ActivateAggroOnperformed;
        
        playerHealth.OnHealthChange += PlayerHealthOnHealthChange;
    }

    private void PlayerHealthOnHealthChange(float currentHealth, float maxHealth)
    {
        if (IsInAggroMode && currentHealth <= 0)
        {
            IsInAggroMode = false;
        }
        
        if (!IsInAggroMode && currentHealth > maxHealth)
        {
            IsInAggroMode = true;
            playerHealth.SetAggroMode();
        }
    }

    private void ActivateAggroOnperformed(InputAction.CallbackContext obj)
    {
        IsInAggroMode = !IsInAggroMode;
    }

    private void ParryOnperformed(InputAction.CallbackContext obj)
    {
        if (IsInAggroMode) return;
        
        animator.SetTrigger(Kick);
        animator.SetBool(IsParryJumping, false);

        StopAllCoroutines();
        StartCoroutine(KickRoutine());
    }

    private IEnumerator KickRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        
        // ENUMERATOR
        ParryState parryState = parryManager.TriggerParry();

        if (parryState != ParryState.Perfect && parryState != ParryState.Early) yield break;

        DoParry();
    }

    private void JumpOnperformed(InputAction.CallbackContext obj)
    {
        if (!_isAllowedToJump || !_isGrounded) return;
        
        Jump();
        _isAllowedToJump = false;

        //StopAllCoroutines();
        //spriteRenderer.color = Color.blue;
        //StartCoroutine(RevertColor());
    }

    public void TriggerParry()
    {
        if (!_isInAggroMode) return;
        DoParry();
    }

    private void DoParry()
    {
        // TODO: aggro no jump?
        if (!_isGrounded)
        {
            ParryJump();
            animator.SetBool(IsParryJumping, true);
        }
        
        parryManager.DoParry();
        //bossHealth.HitParry();
        
        if (_isInAggroMode) return;
        
        playerHealth.HitParry();
    }
    
    private void MoveOnperformed(InputAction.CallbackContext obj)
    {
        _movement = obj.ReadValue<float>();
    }
    
    private void OnDisable()
    {
        _characterInput.Disable();
        
        _characterInput.Player.Move.performed -= MoveOnperformed;
        
        _characterInput.Player.Move.canceled -= MoveOnperformed;
        _characterInput.Player.Jump.performed -= JumpOnperformed;
        _characterInput.Player.Parry.performed -= ParryOnperformed;
        
        _characterInput.Player.ActivateAggro.performed -= ActivateAggroOnperformed;
    }
    
    private void FixedUpdate()
    {
        bool wasGrounded = _isGrounded;
        _isGrounded = CheckGrounded();
        
        if (!wasGrounded && _isGrounded)
        {
            Land();
        }
        
        DrawBoxDebug();
        
        Vector2 velocity = _rb.linearVelocity;
        
        float currentSpeed = velocity.x;
        float targetSpeed = _movement * moveSpeed;
        
        float newSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration);
        
        _rb.linearVelocity = new Vector2(newSpeed, _rb.linearVelocity.y);

        animator.SetBool(IsWalking, _isGrounded && _movement != 0);
    }

    private void Jump()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0);
        _rb.AddForce(Vector2.up * force,  ForceMode2D.Impulse);
        
        animator.SetBool(IsJumping, true);
    }

    private void ParryJump()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0);
        _rb.AddForce((Vector2.up*2 + Vector2.left).normalized * force,  ForceMode2D.Impulse);
        
        animator.SetBool(IsParryJumping, true);
    }

    private void Land()
    {
        _isAllowedToJump = true;
        
        animator.SetBool(IsJumping, false);
        animator.SetBool(IsParryJumping, false);
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
