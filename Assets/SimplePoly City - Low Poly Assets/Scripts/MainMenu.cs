using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject levelSelectPanel;
    public GameObject aboutPanel;

    [Header("Backgrounds")]
    public GameObject mainBackground;
    public GameObject levelBackground;
    public GameObject aboutBackground;

    [Header("Audio")]
    public AudioSource audioSource;     // AudioSource untuk SFX klik
    public AudioSource musicSource;     // AudioSource untuk BGM
    public AudioClip clickSound;        // Suara klik button
    public AudioClip bgMusic;           // Musik background

    void Start()
    {
        // Play background music (loop)
        if (musicSource != null && bgMusic != null)
        {
            musicSource.clip = bgMusic;
            musicSource.loop = true;
            musicSource.Play();
        }

        ShowMainMenu();
    }

    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }

    public void ShowMainMenu()
    {
        PlayClickSound();

        mainPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        if (aboutPanel != null) aboutPanel.SetActive(false);

        if (mainBackground != null) mainBackground.SetActive(true);
        if (levelBackground != null) levelBackground.SetActive(false);
        if (aboutBackground != null) aboutBackground.SetActive(false);
    }

    public void ShowLevelSelect()
    {
        PlayClickSound();

        mainPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        if (aboutPanel != null) aboutPanel.SetActive(false);

        if (mainBackground != null) mainBackground.SetActive(false);
        if (levelBackground != null) levelBackground.SetActive(true);
        if (aboutBackground != null) aboutBackground.SetActive(false);
    }

    public void ShowAboutMenu()
    {
        PlayClickSound();

        mainPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        if (aboutPanel != null) aboutPanel.SetActive(true);

        if (mainBackground != null) mainBackground.SetActive(false);
        if (levelBackground != null) levelBackground.SetActive(false);
        if (aboutBackground != null) aboutBackground.SetActive(true);
    }

    public void LoadLevel(string sceneName)
    {
        PlayClickSound();
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        PlayClickSound();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
