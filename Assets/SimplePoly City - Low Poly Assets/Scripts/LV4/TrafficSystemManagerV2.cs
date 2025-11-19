using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSystemManagerV2 : MonoBehaviour
{
    [System.Serializable]
    public class LightGroup
    {
        public string name;
        public List<TrafficLightController> controllers = new List<TrafficLightController>();
    }

    [System.Serializable]
    public class Phase
    {
        [Tooltip("Index grup yang HIJAU pada fase ini (boleh >1 jika tidak konflik)")]
        public int[] greenGroups;
        public float greenTime = 6f;
        public float yellowTime = 2f;
        public float allRedTime = 1.5f;
    }

    [Header("Semua grup lampu di simpang ini (urutkan searah jarum jam, misalnya)")]
    public List<LightGroup> groups = new List<LightGroup>();

    [Header("Urutan fase yang diputar berulang")]
    public List<Phase> phases = new List<Phase>();

    Coroutine routine;

    void OnEnable()
    {
        routine = StartCoroutine(RunPhases());
    }

    void OnDisable()
    {
        if (routine != null) StopCoroutine(routine);
    }

    IEnumerator RunPhases()
    {
        while (true)
        {
            for (int i = 0; i < phases.Count; i++)
            {
                Phase p = phases[i];

                // Semuanya merah dulu (aman)
                SetAll(red:true, yellow:false, green:false);

                // Fase hijau
                foreach (int gi in p.greenGroups) SetGroup(gi, red:false, yellow:false, green:true);
                yield return new WaitForSeconds(p.greenTime);

                // Fase kuning untuk grup yang barusan hijau
                foreach (int gi in p.greenGroups) SetGroup(gi, red:false, yellow:true, green:false);
                yield return new WaitForSeconds(p.yellowTime);

                // Clearance: semua merah
                SetAll(red:true, yellow:false, green:false);
                yield return new WaitForSeconds(p.allRedTime);
            }
        }
    }

    void SetAll(bool red, bool yellow, bool green)
    {
        for (int gi = 0; gi < groups.Count; gi++) SetGroup(gi, red, yellow, green);
    }

    void SetGroup(int groupIndex, bool red, bool yellow, bool green)
    {
        if (groupIndex < 0 || groupIndex >= groups.Count) return;
        foreach (var c in groups[groupIndex].controllers)
            if (c) c.SetLight(red, yellow, green);
    }
}
