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
        // Keyboard input akan mengoverride button input
        bool keyboardInputActive = false;
        
        // Check for movement input (A and D keys)
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1;
            keyboardInputActive = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1;
            keyboardInputActive = true;
        }
        else
        {
            // Jika tidak ada keyboard input, reset ke 0
            // Button input akan mengatur ulang nilai ini jika diperlukan
            if (!keyboardInputActive)
            {
                horizontalInput = 0;
            }
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

    // Method untuk mobile input (keyboard akan mengoverride ini)
    public void MoveLeft() 
    {
        // Hanya set jika tidak ada keyboard input yang aktif
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            horizontalInput = -1;
    }
    
    public void MoveRight() 
    {
        // Hanya set jika tidak ada keyboard input yang aktif
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            horizontalInput = 1;
    }
    
    public void StopMove() 
    {
        // Hanya set jika tidak ada keyboard input yang aktif
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            horizontalInput = 0;
    }
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