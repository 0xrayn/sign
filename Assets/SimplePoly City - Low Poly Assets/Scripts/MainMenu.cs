using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;           // Panel berisi tombol Start, About, Exit
    public GameObject levelSelectPanel;    // Panel berisi tombol Level 1–4
    public GameObject aboutPanel;          // Panel berisi info tentang game / developer

    [Header("Backgrounds")]
    public GameObject mainBackground;      // Gambar background menu utama
    public GameObject levelBackground;     // Gambar background level select
    public GameObject aboutBackground;     // Gambar background about (opsional)

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        if (aboutPanel != null) aboutPanel.SetActive(false);

        if (mainBackground != null) mainBackground.SetActive(true);
        if (levelBackground != null) levelBackground.SetActive(false);
        if (aboutBackground != null) aboutBackground.SetActive(false);
    }

    public void ShowLevelSelect()
    {
        mainPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        if (aboutPanel != null) aboutPanel.SetActive(false);

        if (mainBackground != null) mainBackground.SetActive(false);
        if (levelBackground != null) levelBackground.SetActive(true);
        if (aboutBackground != null) aboutBackground.SetActive(false);
    }

    public void ShowAboutMenu()
    {
        mainPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        if (aboutPanel != null) aboutPanel.SetActive(true);

        if (mainBackground != null) mainBackground.SetActive(false);
        if (levelBackground != null) levelBackground.SetActive(false);
        if (aboutBackground != null) aboutBackground.SetActive(true);
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
