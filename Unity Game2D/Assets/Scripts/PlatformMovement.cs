using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    public float maxJumpForce = 10f; // Maximální síla skoku
    public float jumpForceHoldDuration = 1f; // Doba držení tlačítka skoku pro maximální sílu
    public float initialJumpForce = 8f; // Začáteční síla skoku

    private bool isGrounded;
    private bool isJumping = false;
    private bool isMovementAllowed = true; // Povolený pohyb postavy
    private float jumpForceTimer = 0f; // Časový odpočet držení tlačítka skoku
    private Rigidbody2D rb;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Kontrola, zda je hráč na povrchu
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float moveDirection = Input.GetAxis("Horizontal");

        // Pohyb postavy pouze pokud je povolen a je na zemi
        if (isMovementAllowed && isGrounded)
        {
            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        }

        if (moveDirection < 0f && isFacingRight && isGrounded)
        {
            // Otočit postavu doleva
            Flip();
        }
        else if (moveDirection > 0f && !isFacingRight && isGrounded)
        {
            // Otočit postavu doprava
            Flip();
        }

        // Nárazový skok
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                jumpForceTimer = 0f;
            }

            if (isJumping)
            {
                jumpForceTimer += Time.deltaTime;

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    float normalizedJumpForce = Mathf.Clamp01(jumpForceTimer / jumpForceHoldDuration);
                    float jumpForce = initialJumpForce + (maxJumpForce - initialJumpForce) * normalizedJumpForce;
                    Jump(jumpForce);
                    isJumping = false;
                }
            }
        }
    }

    private void Jump(float force)
    {
        // Přidání síly pro nárazový skok
        rb.velocity = new Vector2(rb.velocity.x, force);

        // Zablokování pohybu doleva a doprava při skoku
        isMovementAllowed = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Povolení pohybu doleva a doprava po dopadu na zem
            isMovementAllowed = true;
        }
    }

    private void Flip()
    {
        if (isGrounded)
        {
            // Otočit směr, ve kterém je postava otočena
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
