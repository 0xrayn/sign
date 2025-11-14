using UnityEngine;

public class TrafficLightTrigger : MonoBehaviour
{
    public TrafficLightController trafficLight;

    public bool IsRed()
    {
        if (trafficLight == null)
            return false;

        return trafficLight.IsRed(); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Collider col = GetComponent<Collider>();
        if (col)
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }
}
