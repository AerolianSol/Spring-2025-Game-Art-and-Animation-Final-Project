using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;

    private float walkSpeed = 5f;
    private Vector2 moveInput;

    private float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                return walkSpeed;
            }
            else
            {
                return 0;
            }
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }

    [SerializeField]
    private bool _isFacingRight = false;
    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        }

    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool("canMove");
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocityY);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            //Face right
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            //Face left
            IsFacingRight = false;
        }
    }

    public void EnemyOnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    public void EnemyOnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger("attack");
        }
    }
}
