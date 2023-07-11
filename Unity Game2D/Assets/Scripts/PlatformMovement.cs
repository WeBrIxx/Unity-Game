using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float chargedJumpForce = 10f; // Síla nabitého skoku
    public float chargeDuration = 1f; // Doba nabíjení skoku
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    private bool isGrounded;
    private bool isJumping = false;
    private bool isMovementAllowed = true; // Povolení pohybu postavy
    private bool isChargingJump = false; // Indikátor, zda se právì nabíjí skok
    private float jumpChargeTimer = 0f; // Èasový odpoèet nabíjení skoku
    private Rigidbody2D rb;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Kontrola, zda je hráè na povrchu
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float moveDirection = Input.GetAxis("Horizontal");

        // Pohyb postavy pouze pokud je povolen a je na zemi
        if (isMovementAllowed && isGrounded)
        {
            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        }

        if (moveDirection < 0f && isFacingRight && isGrounded)
        {
            // Otoèit postavu doleva
            Flip();
        }
        else if (moveDirection > 0f && !isFacingRight && isGrounded)
        {
            // Otoèit postavu doprava
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isChargingJump = true;
            jumpChargeTimer = 0f;
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded && isChargingJump)
        {
            jumpChargeTimer += Time.deltaTime;

            if (jumpChargeTimer >= chargeDuration)
            {
                isChargingJump = false;
                Jump(chargedJumpForce);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && isGrounded && isChargingJump)
        {
            if (jumpChargeTimer < 0.5f)
            {
                Jump(jumpForce);
            }

            isChargingJump = false;
        }
    }

    private void Jump(float force)
    {
        // Pøidání síly pro skok
        rb.AddForce(new Vector2(0f, force), ForceMode2D.Impulse);
        isJumping = true;

        // Zablokování pohybu doleva a doprava pøi skoku
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
            // Otoèit smìr, ve kterém je postava otoèena
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}



