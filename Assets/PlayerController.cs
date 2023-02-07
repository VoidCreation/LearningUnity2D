using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    public float moveSpeed = 1f;
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;
    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    public SwordAttack swordAttack;

    bool canMove = true;

    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer =  GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(canMove)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);
    
                if(!success)
                {
                    success = TryMove(new Vector2(movementInput.x , 0));
                    
                }
                if(!success)
                {
                    success = TryMove( new Vector2(0 , movementInput.y));
                }
               
                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
    
            //Set direction of sprite to movement direction
            if(movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if(movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        
        }
        
        
    }

    private bool TryMove(Vector2 direction)
    {

        if(direction != Vector2.zero)
        {
            int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if(count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }

        else
        {
            return false;
        }
        
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
        Debug.Log(movementInput);
    }

    void OnFire()
    {
        animator.SetTrigger("sword_basic_attack");
    }

    public void SwordAttack()
    {
        LockMovement();

        if(spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
        
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

}

