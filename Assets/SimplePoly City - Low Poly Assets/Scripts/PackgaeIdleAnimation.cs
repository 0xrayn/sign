using UnityEngine;

public class PackageIdleAnimator : MonoBehaviour
{
    [Header("Rotate")]
    public Vector3 rotateAxis = new Vector3(0f, 1f, 0f);
    public float rotateSpeed = 90f; // derajat/detik

    [Header("Bob (Up-Down)")]
    public float bobAmplitude = 0.15f; // meter
    public float bobFrequency = 1.2f;  // Hz

    [Header("Sway (Left-Right/Forward-Back)")]
    public Vector2 swayAmplitude = new Vector2(0.05f, 0.03f); // XZ (meter)
    public Vector2 swayFrequency = new Vector2(0.7f, 0.9f);   // Hz

    [Header("Extras")]
    public bool useLocal = true; // true: animasikan posisi lokal child
    public bool randomizePhase = true; // biar tiap paket tidak sinkron

    private Vector3 basePos;
    private float phaseBob, phaseSwayX, phaseSwayZ;

    void OnEnable()
    {
        basePos = useLocal ? transform.localPosition : transform.position;

        if (randomizePhase)
        {
            phaseBob   = Random.value * Mathf.PI * 2f;
            phaseSwayX = Random.value * Mathf.PI * 2f;
            phaseSwayZ = Random.value * Mathf.PI * 2f;
            // acak juga starting yaw sedikit agar unik
            transform.localRotation *= Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        }
    }

    void Update()
    {
        float t = Time.time;

        // Rotate (selalu)
        transform.Rotate(rotateAxis.normalized, rotateSpeed * Time.deltaTime, useLocal ? Space.Self : Space.World);

        // Bob + sway (posisi)
        float y  = bobAmplitude * Mathf.Sin((t + phaseBob)   * (Mathf.PI * 2f) * bobFrequency);
        float sx = swayAmplitude.x * Mathf.Sin((t + phaseSwayX) * (Mathf.PI * 2f) * swayFrequency.x);
        float sz = swayAmplitude.y * Mathf.Sin((t + phaseSwayZ) * (Mathf.PI * 2f) * swayFrequency.y);

        Vector3 offset = new Vector3(sx, y, sz);

        if (useLocal)
            transform.localPosition = basePos + offset;
        else
            transform.position = basePos + offset;
    }
}
