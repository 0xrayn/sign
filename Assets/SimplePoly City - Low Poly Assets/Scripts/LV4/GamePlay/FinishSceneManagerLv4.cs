using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class FinishSceneManagerLv4 : MonoBehaviour
{
    [Header("Story UI")]
    public CanvasGroup storyPanel;
    public TMP_Text storyText;

    [Header("Result UI")]
    public CanvasGroup resultPanel;
    public TMP_Text titleText;
    public TMP_Text timeText;
    public Image[] stars;
    public Sprite starOn;
    public Sprite starOff;

    [Header("Fade Overlay")]
    public Image fadeOverlay;

    [Header("Scenes")]
    public string menuSceneName = "MainMenu";

    [Header("Audio")]
    public AudioSource sfxSource;    // 1 AudioSource
    public AudioClip panelSound;     // ▶ Bunyi saat STORY tampil
    public AudioClip buttonSound;    // ▶ Bunyi saat klik button

    void Start()
    {
        storyPanel.alpha = 1;
        resultPanel.alpha = 0;
        fadeOverlay.color = new Color(0, 0, 0, 0);

        float finalTime = PlayerPrefs.GetFloat("FINAL_TIME", 0);
        int finalStars = PlayerPrefs.GetInt("FINAL_STARS", 0);

        int m = Mathf.FloorToInt(finalTime / 60);
        int s = Mathf.FloorToInt(finalTime % 60);
        timeText.text = $"Waktu: {m:00}:{s:00}";

        for (int i = 0; i < stars.Length; i++)
            stars[i].sprite = (i < finalStars) ? starOn : starOff;

        StartCoroutine(FinishSequence());
    }

    IEnumerator FinishSequence()
    {
        // --- STORY tampil ---
        storyText.text =
        "Anda berhasil mengantarkan paket penting ke rumah tukang kayu di tengah keramaian kota.\n" +
        "Setiap tikungan sempit dan penyeberang jalan Anda lewati dengan sabar dan penuh kehati-hatian.\n\n" +
        "Terima kasih atas kewaspadaan Anda — keselamatan warga dan paket tukang kayu kini terjaga.";

        // ▶ Play 1x saat story muncul
        if (panelSound != null) 
            sfxSource.PlayOneShot(panelSound);

        yield return new WaitForSeconds(3f);

        // Fade to black
        yield return StartCoroutine(FadeImage(fadeOverlay, 0, 1, 1f));

        // Hilangkan story
        storyPanel.alpha = 0;

        // Fade in result panel (TANPA SUARA)
        yield return StartCoroutine(FadeCanvas(resultPanel, 0, 1, 1.2f));

        // Fade out black
        yield return StartCoroutine(FadeImage(fadeOverlay, 1, 0, 1f));
    }

    IEnumerator FadeCanvas(CanvasGroup cg, float from, float to, float duration)
    {
        float t = 0;
        cg.alpha = from;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
    }

    IEnumerator FadeImage(Image img, float from, float to, float duration)
    {
        float t = 0;
        Color c = img.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, t / duration);
            img.color = c;
            yield return null;
        }
    }

    public void BackToMenu()
    {
        // ▶ Sound button
        if (buttonSound != null)
            sfxSource.PlayOneShot(buttonSound);

        SceneManager.LoadScene(menuSceneName);
    }
}
