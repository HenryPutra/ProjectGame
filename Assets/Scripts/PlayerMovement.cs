using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private TextMeshProUGUI healthText;
    
    private int currentHealth;
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    private bool jumpPressed;
    private Knockback knockback;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void Update()
    {
        // Handle keyboard input
        HandleKeyboardInput();
        
        if (knockback != null && knockback.GettingKnockedBack) return;
        
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        
        // Handle player facing direction
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // Handle jumping
        if (jumpPressed)
        {
            Jump();
            jumpPressed = false;
        }

        // Update animations
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());
    }

    private void HandleKeyboardInput()
    {
        // Reset horizontal input hanya jika tidak ada input dari button atau keyboard
        bool hasKeyboardInput = false;
        
        // Check for movement input (A and D keys)
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1;
            hasKeyboardInput = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1;
            hasKeyboardInput = true;
        }
        
        // Jika tidak ada input keyboard, biarkan button input bekerja
        // (horizontalInput akan diatur oleh method MoveLeft/MoveRight/StopMove)
        if (!hasKeyboardInput && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            // horizontalInput akan tetap sesuai dengan button yang ditekan
            // Tidak di-reset ke 0 di sini
        }
        
        // Check for jump input (W key or Space)
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded())
        {
            jumpPressed = true;
        }
        
        // S key bisa digunakan untuk aksi tambahan jika diperlukan
        // Misalnya untuk crouch atau fast fall
        if (Input.GetKey(KeyCode.S))
        {
            // Tambahkan logika untuk S key jika diperlukan
            // Contoh: rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); // Fast fall
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, speed);
        anim.SetTrigger("jump");
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            Vector2.down,
            0.1f,
            groundLayer
        );
        return raycastHit.collider != null;
    }

    // Method untuk mobile input (tetap dipertahankan untuk kompatibilitas)
    public void MoveLeft() => horizontalInput = -1;
    public void MoveRight() => horizontalInput = 1;
    public void StopMove() => horizontalInput = 0;
    public void JumpButton()
    {
        if (isGrounded()) jumpPressed = true;
    }

    public void TakeDamage(int damage)
    {
        if (knockback != null && knockback.GettingKnockedBack) return;
        
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player Mati");
        }
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = "Health: " + currentHealth;
    }
}