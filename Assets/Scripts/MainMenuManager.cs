using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Dropdown themeDropdown;
    public Dropdown playerCountDropdown;
    public static string selectedTheme;
    public static int playerCount;
    public Button exitButton; // Referensi ke tombol keluar
    public Button infoButton; // Referensi ke tombol info
    public GameObject infoPanel; // Referensi ke panel info
    public ScrollRect infoScrollRect; // Referensi ke komponen ScrollRect
    public GameObject infoContent; // Referensi ke GameObject Content

    private void Start()
    {
        // Menghubungkan tombol keluar ke metode ExitGame
        exitButton.onClick.AddListener(ExitGame);
        
        // Menghubungkan tombol info ke metode ToggleInfoPanel
        infoButton.onClick.AddListener(ToggleInfoPanel);
        
        // Memastikan panel info awalnya tersembunyi
        infoPanel.SetActive(false);
    }

    public void StartGame()
    {
        selectedTheme = themeDropdown.options[themeDropdown.value].text;

        // Mendapatkan jumlah pemain dari dropdown dan mengubahnya menjadi integer
        string playerCountText = playerCountDropdown.options[playerCountDropdown.value].text;
        if (int.TryParse(playerCountText, out int parsedPlayerCount))
        {
            playerCount = parsedPlayerCount;
        }
        else
        {
            // Penanganan kesalahan jika konversi gagal
            Debug.LogError("Gagal mengonversi jumlah pemain: " + playerCountText);
            return;
        }

        // Ganti "GameScene" dengan nama scene game Anda
        SceneManager.LoadScene("GameScene");
    }

    private void ExitGame()
    {
        Debug.Log("Tombol keluar diklik. Keluar dari game.");

        // Menutup aplikasi
        Application.Quit();

        // Jika berjalan di editor Unity, berhenti bermain
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void ToggleInfoPanel()
    {
        // Mengubah status aktif dari panel info
        bool isActive = !infoPanel.activeSelf;
        infoPanel.SetActive(isActive);

        // Mengatur ulang posisi ScrollRect ke atas saat panel diaktifkan
        if (isActive)
        {
            infoScrollRect.verticalNormalizedPosition = 1;
        }
    }
}