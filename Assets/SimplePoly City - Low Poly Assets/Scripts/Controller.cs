using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Controller : MonoBehaviour
{
    public static Controller Instance;

    [Header("Movement")]
    public float speed = 14f;             
    public float sprintMultiplier = 1.5f;
    public float rotationSpeed = 120f;

    [Header("Acceleration Settings")]
    public float acceleration = 4f;      
    public float deceleration = 6f;      

    [Header("Brake Settings")]
    public float brakeDeceleration = 12f;   // semakin besar, semakin pakem rem

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
    private float targetSpeed = 0f;      

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

        // Sprint
        float finalSpeed = speed;
        if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
            finalSpeed *= sprintMultiplier;

        // Brake
        bool isBraking = Keyboard.current.spaceKey.isPressed;

        if (isBraking)
        {
            // Smooth brake
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0f,
                brakeDeceleration * Time.fixedDeltaTime
            );

            // Play brake SFX
            if (!sfxSource.isPlaying && brakeClip != null)
            {
                sfxSource.clip = brakeClip;
                sfxSource.Play();
            }

            // Gerakan saat brake
            Vector3 movement = transform.forward * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
        else
        {
            // Tentukan kecepatan target dari input gas
            targetSpeed = moveVertical * finalSpeed;

            if (moveVertical != 0)
            {
                // akselerasi smooth
                currentSpeed = Mathf.MoveTowards(
                    currentSpeed,
                    targetSpeed,
                    acceleration * Time.fixedDeltaTime
                );
            }
            else
            {
                // decelerasi natural
                currentSpeed = Mathf.MoveTowards(
                    currentSpeed,
                    0f,
                    deceleration * Time.fixedDeltaTime
                );
            }

            // Gerakkan karakter
            Vector3 movement = transform.forward * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }

        // Rotasi
        float rotation = turn * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, rotation, 0f));

        // Pitch engine sound
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
        return Mathf.Abs(currentSpeed);
    }
}
