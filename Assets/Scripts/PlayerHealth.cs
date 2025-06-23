using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Image healthFillImage; // drag & drop HealthBarFill ke sini

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        float fillAmount = (float)currentHealth / maxHealth;
        healthFillImage.fillAmount = fillAmount;
    }

    // Debug: pencet tombol untuk tes
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // tekan spasi untuk uji damage
        {
            TakeDamage(10);
        }
    }
}