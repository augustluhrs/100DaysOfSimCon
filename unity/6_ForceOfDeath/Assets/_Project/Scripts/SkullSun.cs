using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullSun : MonoBehaviour
{
    public Vector3 sunriseRate = new Vector3(0, 0.3f, 0);

    void FixedUpdate()
    {
        transform.position += sunriseRate;
    }
}
