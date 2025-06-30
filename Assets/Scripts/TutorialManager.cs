using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // Buat referensi ke Panel Tutorial Anda dari Inspector
    public GameObject panelTutorial;

    // Fungsi ini akan dipanggil di awal permainan untuk menampilkan tutorial
    void Start()
    {
        // Pastikan panel tutorial aktif saat game dimulai
        if (panelTutorial != null)
        {
            TampilkanTutorial();
        }
    }

    public void TampilkanTutorial()
    {
        // Aktifkan panelnya
        panelTutorial.SetActive(true);
        // Hentikan waktu di dalam game
        Time.timeScale = 0f; 
    }

    public void SembunyikanTutorialDanLanjutkan()
    {
        // Sembunyikan panelnya
        panelTutorial.SetActive(false);
        // Kembalikan waktu ke normal
        Time.timeScale = 1f; 
    }
}