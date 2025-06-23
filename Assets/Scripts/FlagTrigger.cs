using UnityEngine;
using TMPro; // <-- TAMBAHKAN INI di bagian paling atas
using UnityEngine.SceneManagement;

public class FlagTrigger : MonoBehaviour
{
    [SerializeField] private GameObject panelMenang;

    // Variabel untuk menampung referensi ke komponen teks
    [SerializeField] private TextMeshProUGUI teksMenangLabel;
    [SerializeField] private TextMeshProUGUI koinTerkumpulLabel;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Dapatkan komponen script PlayerMovement dari objek yang menyentuh flag
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            // Pastikan player ditemukan dan label teks sudah di-assign
            if (player != null && teksMenangLabel != null && koinTerkumpulLabel != null)
            {
                // Dapatkan jumlah koin dari script player
                int jumlahKoin = player.GetCoinCount();

                // Ubah teks sesuai permintaan
                teksMenangLabel.text = "Level Complete";
                koinTerkumpulLabel.text = "Coin Collected: " + jumlahKoin;

                // Baru munculkan panelnya setelah semua teks di-update
                panelMenang.SetActive(true);
                Time.timeScale = 0f; // Pause game jika ingin
            }
        }
    }
     public void LoadLevelDua()
    {
        Time.timeScale =1f;
        SceneManager.LoadScene("Level2"); // Memuat scene dengan nama "Level_Dua"
    }

}