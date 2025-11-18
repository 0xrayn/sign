using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [Header("UI")]
    public CanvasGroup popupCanvas;
    public TextMeshProUGUI popupText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        popupCanvas.alpha = 0;
    }

    // POPUP NORMAL
    public void ShowPopup(string message, float duration = 1.2f)
    {
        StopAllCoroutines();
        StartCoroutine(PopupRoutine(message, duration));
    }

    IEnumerator PopupRoutine(string msg, float duration)
    {
        popupText.text = msg;
        popupText.color = Color.white;

        popupCanvas.alpha = 1;
        yield return new WaitForSeconds(duration);

        popupCanvas.alpha = 0;
    }

    // POPUP WARNING MERAH BERKEDIP
    public void ShowWarning(string message)
    {
        StopAllCoroutines();
        StartCoroutine(WarningRoutine(message));
    }

    IEnumerator WarningRoutine(string msg)
    {
        popupText.text = msg;
        popupText.color = Color.red;

        popupCanvas.gameObject.SetActive(true);

        // Kedip merah 3x
        for (int i = 0; i < 3; i++)
        {
            popupCanvas.alpha = 1;
            yield return new WaitForSeconds(0.15f);

            popupCanvas.alpha = 0;
            yield return new WaitForSeconds(0.15f);
        }

        // Stabil sebentar
        popupCanvas.alpha = 1;
        yield return new WaitForSeconds(0.8f);

        // Reset
        popupCanvas.alpha = 0;
        popupText.color = Color.white;
    }
}
