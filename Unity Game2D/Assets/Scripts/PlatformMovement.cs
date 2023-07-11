using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float jumpMultiplier = 0.5f; // N�sobitel pro s�lu skoku p�i dr�en� mezern�ku
    public float jumpMultiplierDuration = 1f; // Doba trv�n� zv��en�ho skoku
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    private bool isGrounded;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private bool isJumping = false;
    private float jumpMultiplierTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Kontrola, zda je hr�� na povrchu
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float moveDirection = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if (moveDirection < 0f && isFacingRight)
        {
            // Oto�it postavu doleva
            Flip();
        }
        else if (moveDirection > 0f && !isFacingRight)
        {
            // Oto�it postavu doprava
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyUp(KeyCode.Space) && isJumping)
        {
            // Uvoln�n� kl�vesy mezern�ku zastav� vyskakov�n�
            ResetJumpMultiplier();
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            // Pokra�uj�c� podr�en� kl�vesy mezern�ku zvy�uje v��ku skoku
            if (jumpMultiplierTimer < jumpMultiplierDuration)
            {
                jumpMultiplierTimer += Time.deltaTime;
                rb.AddForce(new Vector2(0f, jumpForce * jumpMultiplier));
            }
            else
            {
                ResetJumpMultiplier();
            }
        }
    }

    private void Jump()
    {
        // P�id�n� s�ly pro skok
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        isJumping = true;
        jumpMultiplierTimer = 0f;
    }

    private void ResetJumpMultiplier()
    {
        // Resetov�n� jumpMultiplier po uplynut� doby trv�n� zv��en�ho skoku
        isJumping = false;
        jumpMultiplierTimer = 0f;
    }

    private void Flip()
    {
        // Oto�it sm�r, ve kter�m je postava oto�ena
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
