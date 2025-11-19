using UnityEngine;

public class WrongWayTriggerLv2 : MonoBehaviour
{
    [Header("Penalty Settings")]
    public float penaltyTime = 10f;

    [Header("Audio")]
    public AudioSource audioSource;     
    public AudioClip wrongWaySound;     

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HudManagerLv2.Instance.AddPenalty(penaltyTime, "Salah Jalur! ");

            if (audioSource != null && wrongWaySound != null)
                audioSource.PlayOneShot(wrongWaySound);
        }
    }
}
