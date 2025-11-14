using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Controller : MonoBehaviour
{
    public static Controller Instance;

    [Header("Movement")]
    public float speed = 5f;             // m/s normal
    public float sprintMultiplier = 1.5f; // multiplier shift
    public float rotationSpeed = 120f;   // deg/s

    [Header("Audio Clips")]
    public AudioClip engineClip;
    public AudioClip brakeClip;
    public AudioClip crashClip;

    [Header("Settings")]
    public float maxEnginePitch = 2f;
    public float minEnginePitch = 0.8f;

    private Rigidbody rb;
    private AudioSource engineSource;
    private AudioSource sfxSource;

    private float currentSpeed = 0f;

    void Awake() => Instance = this;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Engine
        engineSource = gameObject.AddComponent<AudioSource>();
        engineSource.clip = engineClip;
        engineSource.loop = true;
        engineSource.spatialBlend = 1f;
        engineSource.Play();

        // SFX
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.spatialBlend = 1f;
    }

    void FixedUpdate()
    {
        if (Keyboard.current == null) return;

        float moveVertical = 0f;
        float turn = 0f;

        // Input maju/mundur
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveVertical = 1f;
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveVertical = -1f;

        // Input belok
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) turn = -1f;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) turn = 1f;

        // Turbo / Shift
        float finalSpeed = speed;
        if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
            finalSpeed *= sprintMultiplier;

        // Brake / Space
        bool isBraking = Keyboard.current.spaceKey.isPressed;
        if (isBraking)
        {
            rb.linearVelocity *= 0.9f; // pelambatan
            currentSpeed = 0f;

            if (!sfxSource.isPlaying && brakeClip != null)
            {
                sfxSource.clip = brakeClip;
                sfxSource.Play();
            }
        }
        else
        {
            // Gerak
            currentSpeed = moveVertical * finalSpeed;
            Vector3 movement = transform.forward * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }

        // Rotasi
        float rotation = turn * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, rotation, 0f));

        // Pitch engine
        float speedPercent = Mathf.Abs(currentSpeed) / finalSpeed;
        engineSource.pitch = Mathf.Lerp(minEnginePitch, maxEnginePitch, speedPercent);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (sfxSource != null && crashClip != null)
        {
            sfxSource.clip = crashClip;
            sfxSource.Play();
        }
    }

    public float GetCurrentSpeed()
    {
        return Mathf.Abs(currentSpeed); // m/s
    }
}
