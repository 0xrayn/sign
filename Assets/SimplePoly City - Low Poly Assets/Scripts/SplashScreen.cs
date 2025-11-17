using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashSimple : MonoBehaviour
{
    public Image backgroundImage;
    public CanvasGroup logoGroup;
    public AudioSource audioSource;

    public float blackFadeDuration = 0.3f;
    public float logoFadeInDuration = 0.5f;
    public float logoFadeOutDuration = 0.5f;

    public float totalTime = 4.9f;
    public string nextScene = "MainMenu";

    private void Start()
    {
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        if (audioSource != null) audioSource.Play();

        // Start state
        backgroundImage.color = new Color(0, 0, 0, 0); // black but alpha 0
        logoGroup.alpha = 0;

        // 1. Fade-in BG hitam 0.3 detik
        yield return FadeImage(backgroundImage, 0, 1, blackFadeDuration);

        // 2. Langsung ubah BG jadi putih (TANPA fade-out)
        backgroundImage.color = Color.white;

        // 3. Bersamaan → Logo fade-in
        StartCoroutine(FadeCanvas(logoGroup, 0, 1, logoFadeInDuration));

        // Tunggu sampai waktu total tinggal fade-out
        float remaining = totalTime - (blackFadeDuration + logoFadeInDuration + logoFadeOutDuration);
        if (remaining > 0)
            yield return new WaitForSeconds(remaining);

        // 4. Logo fade-out sebelum selesai total time
        yield return FadeCanvas(logoGroup, 1, 0, logoFadeOutDuration);

        // 5. Pindah scene
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator FadeImage(Image img, float start, float end, float duration)
    {
        float t = 0;
        Color c = img.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(start, end, t / duration);
            img.color = c;
            yield return null;
        }
    }

    IEnumerator FadeCanvas(CanvasGroup cg, float start, float end, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, t / duration);
            yield return null;
        }
    }
}
