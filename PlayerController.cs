using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Hareket hızı
    public float jumpForce = 10f; // Zıplama gücü
    private Rigidbody2D rb;
    private Animator animator;
    public LayerMask groundLayer; // Zemin katmanı
    public Transform groundCheck; // Zemin kontrol noktası
    private bool isGrounded;
    private int maxJumps = 2; // Maksimum zıplama sayısı
    private int currentJumps;

    private bool facingRight = true; // Karakterin yönünü takip etmek için

    public GameObject fireballPrefab; // Ateş topu prefab'ı
    public Transform firePoint; // Ateş topunun çıkacağı nokta

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentJumps = maxJumps;
    }

    void Update()
    {
        // Hareket kontrolü
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Karakterin yönünü kontrol et
        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }

        // Animasyonları kontrol et
        animator.SetFloat("Speed", Mathf.Abs(move));

        // Zemin kontrolü
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Zıplama kontrolü
        if (isGrounded)
        {
            currentJumps = maxJumps; // Zemine indiğinde zıplama haklarını yenile
            animator.SetBool("IsJumping", false); // Zıplama animasyonunu durdur
        }

        if (Input.GetButtonDown("Jump") && currentJumps > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            currentJumps--;
            animator.SetBool("IsJumping", true);
        }

        // Ateş topu ateşleme
        if (Input.GetButtonDown("Fire1"))
        {
            ShootFireball();
        }
    }

    void Flip()
    {
        // Karakterin yönünü değiştir
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void ShootFireball()
    {
        Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            currentJumps = maxJumps; // Zemine indiğinde zıplama haklarını yenile
            animator.SetBool("IsJumping", false); // Zıplama animasyonunu durdur
        }
    }
}
