using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullMover : MonoBehaviour
{
    private GameObject _attractor;
    private Rigidbody _rb;
    private float _mass;
    private float _aMass;

    void Start()
    {
        _attractor = GameObject.FindWithTag("attractor");
        _rb = gameObject.GetComponent<Rigidbody>();
        _mass = _rb.mass;
        _aMass = _attractor.GetComponent<Rigidbody>().mass;
    }
    
    void FixedUpdate()
    {
        Vector3 force = _attractor.transform.position -  transform.position;
        // Vector3 force = transform.position - _attractor.transform.position;
        
        float distance = force.magnitude;
        // distance = Mathf.Clamp(distance, 1f, 50f); //tweak

        Vector3 normForce = force.normalized;
        
        float strength = (10 * _mass * _aMass) / (distance * distance);
        Vector3 finalForce = normForce * strength;

        // Debug.LogFormat("force Start:{0}\ndistance:{1}\nnormForce:{2}\nstrength:{3}\nforce after{4}",
        //     force,
        //     distance,
        //     normForce,
        //     strength,
        //     finalForce);


        _rb.AddForce(finalForce, ForceMode.Impulse);
    }
}
