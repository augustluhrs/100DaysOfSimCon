using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRise : MonoBehaviour
{
    public Vector3 riseRate = new Vector3(0, 0.02f, 0);
    public float yMax = 50f;
    private Vector3 maxRise;
    //public Vector3 rotationRate = new Vector3(.02f, 0, 0);
    //public GameObject lookTarget;
    public float intensityRate = 10f;

    void Start()
    {
        maxRise = gameObject.transform.position + new Vector3(0, yMax, 0);
    }
    void Update()
    {
        if (gameObject.transform.position.y <= maxRise.y)
        {
            gameObject.transform.position += riseRate;
        }

        gameObject.GetComponent<Light>().intensity += intensityRate;
    }
}
