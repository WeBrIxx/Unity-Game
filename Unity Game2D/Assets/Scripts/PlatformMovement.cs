using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float chargedJumpForce = 10f; // S�la nabit�ho skoku
    public float chargeDuration = 1f; // Doba nab�jen� skoku
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    private bool isGrounded;
    private bool isJumping = false;
    private bool isMovementAllowed = true; // Povolen� pohybu postavy
    private bool isChargingJump = false; // Indik�tor, zda se pr�v� nab�j� skok
    private float jumpChargeTimer = 0f; // �asov� odpo�et nab�jen� skoku
    private Rigidbody2D rb;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Kontrola, zda je hr�� na povrchu
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float moveDirection = Input.GetAxis("Horizontal");

        // Pohyb postavy pouze pokud je povolen a je na zemi
        if (isMovementAllowed && isGrounded)
        {
            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        }

        if (moveDirection < 0f && isFacingRight && isGrounded)
        {
            // Oto�it postavu doleva
            Flip();
        }
        else if (moveDirection > 0f && !isFacingRight && isGrounded)
        {
            // Oto�it postavu doprava
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
        // P�id�n� s�ly pro skok
        rb.AddForce(new Vector2(0f, force), ForceMode2D.Impulse);
        isJumping = true;

        // Zablokov�n� pohybu doleva a doprava p�i skoku
        isMovementAllowed = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Povolen� pohybu doleva a doprava po dopadu na zem
            isMovementAllowed = true;
        }
    }

    private void Flip()
    {
        if (isGrounded)
        {
            // Oto�it sm�r, ve kter�m je postava oto�ena
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}



