using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderGenerator : MonoBehaviour
{
    public GameObject riderPrefab;
    public Vector3 launchSpeed = new Vector3(-50f, 0, 0);
    public float waitTimer = 0;
    public float launchTime = 4;
    public float interval = 0.5f;

    void Update()
    {
        if (waitTimer >= launchTime)
        {
            waitTimer -= interval;
            GameObject rider = Instantiate(riderPrefab, gameObject.transform.position, gameObject.transform.rotation);
            rider.GetComponent<Rigidbody>().AddForce(new Vector3(-100f, 0, 0), ForceMode.Impulse);
        }
        else
        {
            waitTimer += Time.deltaTime;
        }
    }
}
