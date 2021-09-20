using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowManager : MonoBehaviour
{
    Animator _animator;
    Collider _collider;
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _collider = gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        //not using bool on Animator?
        _animator.Play("Spin");
        Debug.Log(collision);
    }

    void OnTriggerEnter(Collider other)
    {
        _animator.Play("Spin");
        Debug.Log(other);
    }
}
