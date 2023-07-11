using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float jumpMultiplier = 0.5f; // Násobitel pro sílu skoku pøi držení mezerníku
    public float jumpMultiplierDuration = 1f; // Doba trvání zvýšeného skoku
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
        // Kontrola, zda je hráè na povrchu
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float moveDirection = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if (moveDirection < 0f && isFacingRight)
        {
            // Otoèit postavu doleva
            Flip();
        }
        else if (moveDirection > 0f && !isFacingRight)
        {
            // Otoèit postavu doprava
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyUp(KeyCode.Space) && isJumping)
        {
            // Uvolnìní klávesy mezerníku zastaví vyskakování
            ResetJumpMultiplier();
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            // Pokraèující podržení klávesy mezerníku zvyšuje výšku skoku
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
        // Pøidání síly pro skok
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        isJumping = true;
        jumpMultiplierTimer = 0f;
    }

    private void ResetJumpMultiplier()
    {
        // Resetování jumpMultiplier po uplynutí doby trvání zvýšeného skoku
        isJumping = false;
        jumpMultiplierTimer = 0f;
    }

    private void Flip()
    {
        // Otoèit smìr, ve kterém je postava otoèena
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
