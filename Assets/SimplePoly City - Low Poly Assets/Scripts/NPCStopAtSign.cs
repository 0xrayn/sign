using UnityEngine;
using System.Collections;

public class NPCStopAtSign : MonoBehaviour
{
    public float speed = 10f;
    private bool isStopped = false;

    void Update()
    {
        if (!isStopped)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StopSign") && !isStopped)
        {
            StartCoroutine(StopCar());
        }
    }

    IEnumerator StopCar()
    {
        isStopped = true;
        yield return new WaitForSeconds(3f); // berhenti 3 detik
        isStopped = false;
    }
}
