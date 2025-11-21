using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WrongWayLaneTriggerLv4 : MonoBehaviour
{
    [Header("Lane Direction")]
    [Tooltip("Arah lajur yang BENAR. Kosongkan untuk pakai transform.forward.")]
    public Transform forwardRef;

    [Header("Rules")]
    [Tooltip("Kecepatan minimum (m/s) supaya dicek wrong-way. Diam / sangat pelan diabaikan.")]
    public float minSpeed = 1.0f;

    [Tooltip("Batas toleransi arah (derajat). 0=harus persis berlawanan, 90=sangat longgar.")]
    public float oppositeThresholdDeg = 60f; // dot < -cos(60°) → wrong-way

    [Tooltip("Grace sesaat setelah masuk lajur (detik), supaya tidak kena saat belok di simpang).")]
    public float enterGrace = 0.35f;

    [Tooltip("Cooldown penalti per pelanggaran (detik) agar tidak spam).")]
    public float penaltyCooldown = 2.0f;

    [Header("Penalty")]
    public float penaltyTime = 6f;
    public AudioSource audioSource;
    public AudioClip sfxWrongWay;

    private float lastEnterTime = -999f;
    private float lastPenaltyTime = -999f;

    private Rigidbody playerRb;
    private ControllerLv4 ctrlLv4;     // atau pakai controller lain jika perlu
    private System.Func<float> getSpeed;

    void Reset()
    {
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        lastEnterTime = Time.time;

        // Ambil sumber kecepatan
        ctrlLv4 = other.GetComponent<ControllerLv4>();
        if (ctrlLv4 != null)
            getSpeed = () => ctrlLv4.GetCurrentSpeed();         // m/s
        else
        {
            playerRb = other.attachedRigidbody;
            getSpeed = playerRb ? (System.Func<float>)(() => playerRb.linearVelocity.magnitude) : (() => 0f);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") || getSpeed == null) return;

        // 1) Grace saat baru masuk (supaya aman ketika belok di simpang)
        if (Time.time - lastEnterTime < enterGrace) return;

        // 2) Cek speed
        float speed = getSpeed();
        if (speed < minSpeed) return;

        // 3) Bandingkan arah gerak vs arah lajur
        Vector3 laneFwd = (forwardRef ? forwardRef.forward : transform.forward).normalized;
        Vector3 v = Vector3.zero;

        if (playerRb) v = playerRb.linearVelocity;
        else if (ctrlLv4) v = ctrlLv4.transform.forward * (ctrlLv4.GetCurrentSpeed());

        if (v.sqrMagnitude < 0.001f) return;

        float dot = Vector3.Dot(v.normalized, laneFwd); // +1 searah, -1 berlawanan
        float threshold = -Mathf.Cos(oppositeThresholdDeg * Mathf.Deg2Rad);

        if (dot < threshold) // cukup berlawanan → wrong-way
        {
            if (Time.time - lastPenaltyTime >= penaltyCooldown)
            {
                lastPenaltyTime = Time.time;
                HudManagerLv4.Instance?.AddPenalty(penaltyTime, "Salah jalur (arah berlawanan)!");
                if (audioSource && sfxWrongWay) audioSource.PlayOneShot(sfxWrongWay);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // bersih2 state bila perlu
        ctrlLv4 = null; playerRb = null; getSpeed = null;
    }
}
