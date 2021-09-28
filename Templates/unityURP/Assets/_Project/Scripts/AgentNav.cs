using UnityEngine;
using UnityEngine.AI;

//thanks Brackeys!
// also https://docs.unity3d.com/Manual/nav-CouplingAnimationAndNavigation.html
//[RequireComponent (typeof (NavMeshAgent))]
[RequireComponent (typeof (Animator))]
public class AgentNav : MonoBehaviour
{
    public GameObject target = null;
    Animator anim;
    public NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    void Start()
    {
        anim = GetComponent<Animator>();
        //agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }
        agent.destination = target.transform.position;

        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        //map worlddeltapos to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        //low pass filter the delta move
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        //update velocity if time advances
        if (Time.deltaTime > 1e-5f)
        {
            velocity = smoothDeltaPosition / Time.deltaTime;
        }

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

        //update animation params
        anim.SetBool("isMoving", shouldMove);
        anim.SetFloat("blend", velocity.y);
        //no y


        /* //only works for humanoid avatars?
        LookAt lookAt = GetComponent<LookAt>();
        if (lookAt)
        {
            lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;
        }
        */
    }

    void OnAnimatorMove()
    {
        //update position to agent position
        transform.position = agent.nextPosition;


        // https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html
        float singleStep = 4.0f * Time.deltaTime;
        Vector3 lookDir = Vector3.RotateTowards(transform.forward, agent.destination - transform.position, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(lookDir);
    }
}
