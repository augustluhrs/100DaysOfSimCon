using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lovers : MonoBehaviour
{
    List<GameObject> lovers = new List<GameObject>();
    
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            lovers.Add(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < lovers.Count; i++)
        {
            if (i == 0)
            {
                //bison, nothing
            }
            else if (i % 2 == 0)
            {
                //Debug.LogFormat("{0} \n {1}", i, i -1);
                lovers[i].GetComponent<AgentNav>().target = lovers[i - 1];
            }
            else
            {
                //Debug.LogFormat("{0} \n {1}", i, i + 1);
                lovers[i].GetComponent<AgentNav>().target = lovers[i + 1];

            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                lovers[0].GetComponentInChildren<NavMeshAgent>().destination = hit.point;
            }
        }

        
    }
}
