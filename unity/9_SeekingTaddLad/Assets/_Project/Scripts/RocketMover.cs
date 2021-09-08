using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMover : MonoBehaviour
{
    public DNA dna;
    int geneCount = 0;
    public float fitness = 0;

    void Start()
    {
        // dna = new DNA();
        dna = gameObject.GetComponent<DNA>();

    }

    void Update()
    {
        
    }

    public Vector3 Run(){
        Vector3 force = dna.genes[geneCount];
        geneCount++;

        return force;
    }
}
