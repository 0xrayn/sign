using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Route
    {
        public WaypointManager waypointManager;
        public Transform spawnPoint;
    }

    [Header("Daftar Rute")]
    public Route[] routes;

    [Header("Mobil")]
    public GameObject[] carPrefabs;
    public float spawnInterval = 3f;
    public int maxCars = 20;

    private int carCount = 0;

    void Start()
    {
        // spawn mobil secara periodik
        InvokeRepeating(nameof(SpawnCar), 1f, spawnInterval);
    }

    void SpawnCar()
    {
        if (carCount >= maxCars) return;

        // Pilih rute acak
        int r = Random.Range(0, routes.Length);
        Route selectedRoute = routes[r];

        if (selectedRoute.waypointManager == null || selectedRoute.spawnPoint == null)
        {
            Debug.LogWarning("❌ Route belum lengkap di index: " + r);
            return;
        }

        // Pilih mobil acak
        int randCar = Random.Range(0, carPrefabs.Length);
        GameObject car = Instantiate(
            carPrefabs[randCar],
            selectedRoute.spawnPoint.position,
            selectedRoute.spawnPoint.rotation
        );

        // Assign WaypointManager ke mobil (DI SINI!)
        NPCCar npc = car.GetComponent<NPCCar>();
        npc.waypointManager = selectedRoute.waypointManager;

        carCount++;
        Destroy(car, 60f); // auto hapus setelah 60 detik
    }
}
