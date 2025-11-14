using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("UI References")]
    public TMP_Text timeText;
    public TMP_Text packageText;
    public TMP_Text starText;

    [Header("Game Settings")]
    public int totalPackages = 5;
    public float timer = 0f;
    public int collectedPackages = 0;

    private bool finished = false;
    private int stars = 3;

    void Awake() => Instance = this;

    void Update()
    {
        if (finished) return;

        timer += Time.deltaTime;
        UpdateTimeUI();
        UpdateStars();
        UpdatePackageUI();
    }

    void UpdateTimeUI()
    {
        int m = Mathf.FloorToInt(timer / 60);
        int s = Mathf.FloorToInt(timer % 60);
        timeText.text = $"TIME : {m:00}:{s:00}";
    }

    void UpdatePackageUI()
    {
        packageText.text = $"PACKAGES : {collectedPackages} / {totalPackages}";
    }

    void UpdateStars()
    {
        if (timer > 300) stars = 1;      // > 5 menit
        else if (timer > 180) stars = 2; // > 3 menit
        else stars = 3;

        string starStr = "";
        for (int i = 0; i < stars; i++) starStr += "★";
        for (int i = stars; i < 3; i++) starStr += "☆";
        starText.text = $"STARS : {starStr}";
    }

    public void AddPackage()
    {
        collectedPackages++;
        UpdatePackageUI();
        PopupManager.Instance.ShowPopup("📦 Package Collected!");

        if (collectedPackages >= totalPackages)
        {
            FinishGame();
        }
    }

    public void AddPenalty(float extraTime, string reason)
    {
        timer += extraTime;
        PopupManager.Instance.ShowPopup($"{reason} (+{extraTime}s)");
    }

    public void FinishGame()
    {
        finished = true;
        string starStr = new string('★', stars) + new string('☆', 3 - stars);
        PopupManager.Instance.ShowPopup($"🎉 FINISH! Stars: {starStr}");
    }
}
