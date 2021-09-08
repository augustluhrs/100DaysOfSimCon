using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTest : MonoBehaviour
{
    public enum ForceType
    {
        Force,
        Acceleration,
        Impulse,
        VelocityChange
    }

    public ForceType forceType;
    float speed = 10;
    ForceMode mode;
    Rigidbody _rb;
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();

        switch(forceType) //hmm gotta be a better way to do this
        {
            case ForceType.Force:
                mode = ForceMode.Force;
                break;
            case ForceType.Acceleration:
                mode = ForceMode.Acceleration;
                break;
            case ForceType.Impulse:
                mode = ForceMode.Impulse;
                break;
            case ForceType.VelocityChange:
                mode = ForceMode.VelocityChange;
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            _rb.AddForce(new Vector3(0, 0, speed * Time.deltaTime), mode);
            //Debug.Log(mode);
        }

        if (Input.GetKey("s"))
        {
            _rb.AddForce(new Vector3(0, 0, -speed * Time.deltaTime), mode);
        }

        if (Input.GetKey("d"))
        {
            _rb.AddForce(new Vector3(speed * Time.deltaTime, 0, 0), mode);
        }

        if (Input.GetKey("a"))
        {
            _rb.AddForce(new Vector3(-speed * Time.deltaTime, 0, 0), mode);
        }
    }
}
