using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Tetap pakai jika masih ada coinText

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int maxHealth = 5; // Ubah ini menjadi jumlah hati/darah yang ingin Anda tampilkan
    // [SerializeField] private TextMeshProUGUI healthText; // Hapus ini
    [SerializeField] private TextMeshProUGUI coinText;

    // Tambahkan referensi ke HealthUIController
    [SerializeField] private HealthUIController healthUIController; 

    private int coinCount = 0;
    private int currentHealth; // Ini akan menjadi jumlah hati/darah saat ini
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

        // Pastikan healthUIController diinisialisasi
        if (healthUIController == null)
        {
            Debug.LogError("HealthUIController not assigned in PlayerMovement. Please assign it in the Inspector.");
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        // Panggil UpdateHealthUI() dari healthUIController
        if (healthUIController != null)
        {
            healthUIController.UpdateHealthDisplay(currentHealth);
        }
        UpdateCoinUI();
    }

    private void Update()
    {
        HandleKeyboardInput();
        UpdateAnimationAndDirection();

        if (jumpPressed)
        {
            Jump();
            jumpPressed = false;
        }
    }

    private void FixedUpdate()
    {
        if (knockback != null && knockback.GettingKnockedBack)
        {
            return;
        }
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }

    private void HandleKeyboardInput()
    {
        float keyboardHorizontal = Input.GetAxis("Horizontal");

        if (keyboardHorizontal != 0)
        {
            horizontalInput = keyboardHorizontal;
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded())
        {
            jumpPressed = true;
        }
    }

    private void UpdateAnimationAndDirection()
    {
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateCoinUI();
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = coinCount.ToString(); // Cukup menampilkan angka
    }

    public void TakeDamage(int damage)
    {
        if (knockback != null && knockback.GettingKnockedBack) return;

        currentHealth -= damage; // Asumsi setiap damage mengurangi 1 "hati"
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player Mati");
            // Add logic for player death here (e.g., restart level, show game over screen)
        }
        // Panggil UpdateHealthDisplay dari healthUIController
        if (healthUIController != null)
        {
            healthUIController.UpdateHealthDisplay(currentHealth);
        }
    }

    // Hapus method UpdateHealthUI() ini, karena sekarang di handle oleh HealthUIController
    // private void UpdateHealthUI()
    // {
    //     if (healthText != null)
    //         healthText.text = "Health: " + currentHealth;
    // }

    public void MoveLeft() => horizontalInput = -1;
    public void MoveRight() => horizontalInput = 1;
    public void StopMove() => horizontalInput = 0;
    public void JumpButton()
    {
        if (isGrounded())
        {
            jumpPressed = true;
        }
    }
}