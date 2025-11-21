using UnityEngine;

public class PackagePickupLv4 : MonoBehaviour
{
    public AudioClip pickupSound;      // suara yang mau diputar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            HudManagerLv4.Instance.AddPackage();
            Destroy(gameObject);
        }
    }
}
