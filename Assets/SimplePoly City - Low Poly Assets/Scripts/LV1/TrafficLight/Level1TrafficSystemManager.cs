using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1TrafficSystemManager : MonoBehaviour
{
    [Header("Group A (arah kanan)")]
    public List<TrafficLightController> groupA = new List<TrafficLightController>();

    [Header("Group B (arah kiri)")]
    public List<TrafficLightController> groupB = new List<TrafficLightController>();

    [Header("Timing (detik)")]
    public float greenTime = 6f;
    public float yellowTime = 2f;
    public float allRedTime = 1.5f;

    private Coroutine controlRoutine;

    void Start()
    {
        controlRoutine = StartCoroutine(ControlTraffic());
    }

    IEnumerator ControlTraffic()
    {
        while (true)
        {
            // === 1️⃣ Grup A Hijau ===
            SetGroup(groupA, false, false, true);
            SetGroup(groupB, true, false, false);
            yield return new WaitForSeconds(greenTime);

            // === 2️⃣ Grup A Kuning ===
            SetGroup(groupA, false, true, false);
            yield return new WaitForSeconds(yellowTime);

            // === 3️⃣ Semua Merah ===
            SetGroup(groupA, true, false, false);
            SetGroup(groupB, true, false, false);
            yield return new WaitForSeconds(allRedTime);

            // === 4️⃣ Grup B Hijau ===
            SetGroup(groupB, false, false, true);
            SetGroup(groupA, true, false, false);
            yield return new WaitForSeconds(greenTime);

            // === 5️⃣ Grup B Kuning ===
            SetGroup(groupB, false, true, false);
            yield return new WaitForSeconds(yellowTime);

            // === 6️⃣ Semua Merah Lagi ===
            SetGroup(groupA, true, false, false);
            SetGroup(groupB, true, false, false);
            yield return new WaitForSeconds(allRedTime);
        }
    }

    void SetGroup(List<TrafficLightController> group, bool red, bool yellow, bool green)
    {
        foreach (var light in group)
        {
            if (light != null)
            {
                light.SetLight(red, yellow, green);
            }
        }
    }
}
