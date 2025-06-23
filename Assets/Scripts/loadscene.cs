// ----------------------------------------------------
// File: loadscene.cs (Updated)
// Pasang skrip ini pada GameObject kosong (misal: "SceneLoader")
// ----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadscene : MonoBehaviour
{
    // Fungsi ini tetap sama, untuk tombol yang spesifik
    public void ChangeScene(string sceneName)
    {
        // Pastikan game tidak dalam keadaan pause saat pindah scene
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    // Fungsi baru untuk memuat level selanjutnya secara otomatis
    public void LoadNextLevel()
    {
        // Mengambil index dari scene yang sedang aktif
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Cek apakah level selanjutnya ada di dalam build settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Muat level selanjutnya
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Jika ini adalah level terakhir, kembali ke Main Menu
            Debug.Log("Level Terakhir Selesai! Kembali ke Main Menu.");
            SceneManager.LoadScene("Main Menu"); // Pastikan scene "Main Menu" ada
        }
    }

    // Fungsi ini diubah untuk memuat Main Menu, sudah benar
    public void quitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void paused()
    {
        Time.timeScale = 0;
    }

    public void resume()
    {
        Time.timeScale = 1;
    }
}
