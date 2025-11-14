using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (HUDManager.Instance.allPackagesCollected)
            {
                HUDManager.Instance.FinishGame();
            }
            else
            {
                PopupManager.Instance.ShowPopup("❌ You must collect all packages first!");
            }
        }
    }
}
