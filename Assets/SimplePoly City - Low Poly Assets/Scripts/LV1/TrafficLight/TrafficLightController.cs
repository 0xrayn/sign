using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    [Header("Lamp Objects (Renderer)")]
    public Renderer redRenderer;
    public Renderer yellowRenderer;
    public Renderer greenRenderer;

    [Header("Light Halos (Optional)")]
    public GameObject redHalo;
    public GameObject yellowHalo;
    public GameObject greenHalo;

    [Header("Materials")]
    public Material LightsOnMat;
    public Material LightsOffMat;

    [Header("Audio Clips (opsional)")]
    public AudioClip redSound;
    public AudioClip yellowSound;
    public AudioClip greenSound;

    [Header("Options")]
    public bool enableSounds = true;

    private AudioSource redAudio;
    private AudioSource yellowAudio;
    private AudioSource greenAudio;

    public enum LightState { Red, Yellow, Green }
    [HideInInspector] public LightState currentState = LightState.Red;

    private void Awake()
    {
        EnsureAudio(); // pastikan ada
        // volume & setting standar
        redAudio.playOnAwake = false;
        yellowAudio.playOnAwake = false;
        greenAudio.playOnAwake = false;
        redAudio.volume = 0.05f;
        yellowAudio.volume = 0.05f;
        greenAudio.volume = 0.05f;
    }

    private void EnsureAudio()
    {
        if (redAudio == null)   redAudio   = gameObject.AddComponent<AudioSource>();
        if (yellowAudio == null) yellowAudio = gameObject.AddComponent<AudioSource>();
        if (greenAudio == null)  greenAudio  = gameObject.AddComponent<AudioSource>();
    }

    public void SetLight(bool red, bool yellow, bool green)
    {
        var prev = currentState;

        if (red) currentState = LightState.Red;
        else if (yellow) currentState = LightState.Yellow;
        else if (green) currentState = LightState.Green;

        // Set material (cek null biar aman)
        if (redRenderer)    redRenderer.material    = red    ? LightsOnMat : LightsOffMat;
        if (yellowRenderer) yellowRenderer.material = yellow ? LightsOnMat : LightsOffMat;
        if (greenRenderer)  greenRenderer.material  = green  ? LightsOnMat : LightsOffMat;

        // Halo
        if (redHalo)    redHalo.SetActive(red);
        if (yellowHalo) yellowHalo.SetActive(yellow);
        if (greenHalo)  greenHalo.SetActive(green);

        // Mainkan suara hanya kalau state berubah
        if (prev != currentState)
            PlaySound(currentState);
    }

    private void PlaySound(LightState state)
    {
        if (!enableSounds) return;

        EnsureAudio(); // jaga-jaga kalau Awake belum sempat terpanggil

        switch (state)
        {
            case LightState.Red:
                if (redAudio != null && redSound != null) redAudio.PlayOneShot(redSound);
                break;
            case LightState.Yellow:
                if (yellowAudio != null && yellowSound != null) yellowAudio.PlayOneShot(yellowSound);
                break;
            case LightState.Green:
                if (greenAudio != null && greenSound != null) greenAudio.PlayOneShot(greenSound);
                break;
        }
    }

    public bool IsRed() => currentState == LightState.Red;
    public bool IsGreen() => currentState == LightState.Green;
    public bool IsYellow() => currentState == LightState.Yellow;
}
