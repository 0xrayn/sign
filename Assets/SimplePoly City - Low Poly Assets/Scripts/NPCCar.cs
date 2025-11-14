using UnityEngine;

public class NPCCar : MonoBehaviour
{
    [Header("Kecepatan")]
    public float maxSpeed = 10f;
    public float currentSpeed;
    public float safeDistance = 10f;
    public float brakeStrength = 5f;

    [Header("AI & Jalur")]
    [HideInInspector] public WaypointManager waypointManager;
    [HideInInspector] public CarSpawner spawner;

    private int currentIndex = 0;
    private Rigidbody rb;
    private bool stopForRedLight = false;
    private bool stopForPlayer = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            return;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if (maxSpeed <= 0) maxSpeed = 8f;
        currentSpeed = maxSpeed;
    }

    void Update()
    {
        if (waypointManager == null || waypointManager.waypoints.Length == 0)
            return;

        Transform target = waypointManager.waypoints[currentIndex];
        Vector3 dir = (target.position - transform.position).normalized;

        float desiredSpeed = maxSpeed;
        stopForPlayer = false;

        Vector3 rayStart = transform.position + Vector3.up * 1f;
        if (Physics.Raycast(rayStart, transform.forward, out RaycastHit hit, safeDistance))
        {
            var player = hit.collider.GetComponent<Controller>();
            var otherNpc = hit.collider.GetComponent<NPCCar>();

            if (player != null)
            {
                stopForPlayer = true;
                desiredSpeed = 0f;
            }
            else if (otherNpc != null)
            {
                desiredSpeed = Mathf.Min(desiredSpeed, otherNpc.currentSpeed * 0.9f);
            }
        }

        if (stopForRedLight)
        {
            desiredSpeed = 0f;
        }

        if (stopForPlayer || stopForRedLight)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, brakeStrength * 2f * Time.deltaTime);
            if (currentSpeed < 0.05f)
            {
                currentSpeed = 0f;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, brakeStrength * Time.deltaTime);
        }

        if (currentSpeed > 0.01f)
        {
            Vector3 movement = dir * currentSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 3f));
        }

        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            currentIndex++;
            if (currentIndex >= waypointManager.waypoints.Length)
            {
                if (spawner != null)
                    spawner.NotifyCarDestroyed();
                Destroy(gameObject);
            }
        }

        Debug.DrawRay(rayStart, transform.forward * safeDistance, stopForPlayer ? Color.red : Color.green);
    }

    private void OnTriggerStay(Collider other)
    {
        var lightTrigger = other.GetComponent<TrafficLightTrigger>();
        if (lightTrigger != null)
        {
            stopForRedLight = lightTrigger.IsRed();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var lightTrigger = other.GetComponent<TrafficLightTrigger>();
        if (lightTrigger != null)
        {
            stopForRedLight = false;
        }
    }
}
