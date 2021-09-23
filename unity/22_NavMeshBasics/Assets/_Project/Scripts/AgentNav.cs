using UnityEngine;
using UnityEngine.AI;

//thanks Brackeys!
public class AgentNav : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent agent;
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.transform.position;
    }
}
