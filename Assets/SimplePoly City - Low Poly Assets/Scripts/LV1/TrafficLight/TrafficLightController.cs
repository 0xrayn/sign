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

    [Header("Audio Clips")]
    public AudioClip redSound;
    public AudioClip yellowSound;
    public AudioClip greenSound;

    private AudioSource redAudio;
    private AudioSource yellowAudio;
    private AudioSource greenAudio;

    public enum LightState { Red, Yellow, Green }
    [HideInInspector] public LightState currentState = LightState.Red;

    private void Awake()
    {
        redAudio = gameObject.AddComponent<AudioSource>();
        yellowAudio = gameObject.AddComponent<AudioSource>();
        greenAudio = gameObject.AddComponent<AudioSource>();

        redAudio.playOnAwake = false;
        yellowAudio.playOnAwake = false;
        greenAudio.playOnAwake = false;

        redAudio.volume = 0.05f;
        yellowAudio.volume = 0.05f;
        greenAudio.volume = 0.05f;

    }

    public void SetLight(bool red, bool yellow, bool green)
    {
        LightState prev = currentState;

        if (red) currentState = LightState.Red;
        else if (yellow) currentState = LightState.Yellow;
        else if (green) currentState = LightState.Green;

        // Set material
        if (redRenderer)
            redRenderer.material = red ? LightsOnMat : LightsOffMat;
        if (yellowRenderer)
            yellowRenderer.material = yellow ? LightsOnMat : LightsOffMat;
        if (greenRenderer)
            greenRenderer.material = green ? LightsOnMat : LightsOffMat;

        // Set halo
        if (redHalo) redHalo.SetActive(red);
        if (yellowHalo) yellowHalo.SetActive(yellow);
        if (greenHalo) greenHalo.SetActive(green);

        // Mainkan suara hanya jika state berubah
        if (prev != currentState)
            PlaySound(currentState);
    }

    private void PlaySound(LightState state)
    {
        switch (state)
        {
            case LightState.Red:
                if (redSound) redAudio.PlayOneShot(redSound);
                break;

            case LightState.Yellow:
                if (yellowSound) yellowAudio.PlayOneShot(yellowSound);
                break;

            case LightState.Green:
                if (greenSound) greenAudio.PlayOneShot(greenSound);
                break;
        }
    }

    public bool IsRed() => currentState == LightState.Red;
    public bool IsGreen() => currentState == LightState.Green;
    public bool IsYellow() => currentState == LightState.Yellow;
}
