using UnityEngine;
using UnityEngine.UI; // Penting untuk bekerja dengan UI Image
using System.Collections.Generic;

public class HealthUIController : MonoBehaviour
{
    [SerializeField] private List<Image> healthImages; // Daftar gambar hati (misalnya, 3 gambar hati untuk maxHealth = 3)

    // Metode ini dipanggil oleh PlayerMovement untuk memperbarui tampilan
    public void UpdateHealthDisplay(int currentHealth)
    {
        // Pastikan jumlah gambar hati sesuai dengan currentHealth
        // Jika currentHealth = 0, semua hati akan disembunyikan
        for (int i = 0; i < healthImages.Count; i++)
        {
            if (i < currentHealth)
            {
                healthImages[i].enabled = true; // Tampilkan hati jika index kurang dari currentHealth
            }
            else
            {
                healthImages[i].enabled = false; // Sembunyikan hati jika index lebih atau sama dengan currentHealth
            }
        }
    }
}