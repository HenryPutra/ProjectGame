using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float cooldownTime = 1f; // Cooldown untuk mencegah spam damage
    
    private bool canDamage = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trap triggered by: " + collision.name); // Debug untuk melihat apa yang menyentuh trap
        
        // Cek apakah yang menyentuh adalah Player
        if (collision.CompareTag("Player") && canDamage)
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            Knockback knockback = collision.GetComponent<Knockback>();

            if (player != null)
            {
                Debug.Log("Player found, applying damage: " + damage); // Debug damage
                player.TakeDamage(damage);
                
                if (knockback != null)
                {
                    Debug.Log("Knockback found, applying force: " + knockbackForce); // Debug knockback
                    knockback.GetKnockedBack(transform, knockbackForce);
                }
                else
                {
                    Debug.LogWarning("Knockback component not found on player!");
                }
                
                // Start cooldown
                StartCoroutine(DamageCooldown());
            }
            else
            {
                Debug.LogWarning("PlayerMovement component not found on colliding object!");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Optional: Jika ingin damage berkelanjutan saat player masih di dalam trap
        // Uncomment code di bawah jika diperlukan
        /*
        if (collision.CompareTag("Player") && canDamage)
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(damage);
                StartCoroutine(DamageCooldown());
            }
        }
        */
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(cooldownTime);
        canDamage = true;
    }

    // Method untuk testing di editor
    private void OnDrawGizmos()
    {
        // Menggambar area trigger trap untuk debugging
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, col.bounds.size);
        }
    }
}