using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour
{
    public int lifetime = 10;
    public float maxForce = 5f;

    public Vector3[] genes = new Vector3[10];

    void Start()
    {
        for (int i = 0; i < lifetime; i++) {
            Vector3 gene = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            // Debug.Log("gene" + gene);
            // gene *= Random.Range(0, maxForce);
            gene *= maxForce;
            // Debug.Log(maxForce);
            // Debug.Log("gene after" + gene);

            genes[i] = gene;
        }
    }

    public DNA Crossover(DNA partner) {
        DNA child = new DNA();
        int midpoint = (int)Mathf.Floor(Random.Range(0f, child.genes.Length));

        for (int i = 0; i < genes.Length; i++) {
            if (i > midpoint) {
                child.genes[i] = genes[i];
            } else {
                child.genes[i] = partner.genes[i];
            }
        }

        return child;
    }

    public void Mutate(float mutationRate) {
        for (int i = 0; i < genes.Length; i++) {
            if (Random.Range(0, 1) < mutationRate) {
                Vector3 newGene = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                newGene *= Random.Range(0, maxForce);
            }
        }
    }
}
