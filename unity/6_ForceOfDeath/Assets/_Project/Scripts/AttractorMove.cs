using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorMove : MonoBehaviour
{
    // public Vector3 motion;
    public GameObject target;

    void FixedUpdate()
    {
        Vector3 motion = target.transform.position - transform.position;
        Vector3 normMotion = motion.normalized * .15f;
        transform.position += normMotion; 
        // transform.position += new Vector3 (Random.Range(-1, 1), Random.Range(-5, 5), Random.Range(-1,1));
    }
}
