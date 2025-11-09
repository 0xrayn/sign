using UnityEngine;

public class NPCCar : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float currentSpeed;
    public float safeDistance = 5f;
    [HideInInspector] public WaypointManager waypointManager;

    private int currentIndex = 0;

    void Start()
    {
        if (maxSpeed <= 0) maxSpeed = 8f;
        currentSpeed = maxSpeed;
    }

    void Update()
    {
        if (waypointManager == null || waypointManager.waypoints.Length == 0)
        {
            Debug.LogWarning($"{gameObject.name} tidak punya waypoint!");
            return;
        }

        Transform target = waypointManager.waypoints[currentIndex];
        Vector3 dir = (target.position - transform.position).normalized;

        // Deteksi mobil lain di depan
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out RaycastHit hit, safeDistance))
        {
            if (hit.collider.GetComponent<NPCCar>() != null)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * 3);
            }
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime);
        }

        // Gerak mobil
        transform.position += dir * currentSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 2f);

        // Cek waypoint berikut
        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            currentIndex++;
            if (currentIndex >= waypointManager.waypoints.Length)
            {
                Destroy(gameObject);
            }
        }
    }
}
