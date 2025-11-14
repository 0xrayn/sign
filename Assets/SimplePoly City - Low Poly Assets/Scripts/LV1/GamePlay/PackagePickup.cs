using UnityEngine;

public class PackagePickup : MonoBehaviour
{
    public AudioClip pickupSound;      // suara yang mau diputar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            HUDManager.Instance.AddPackage();
            Destroy(gameObject);
        }
    }
}
