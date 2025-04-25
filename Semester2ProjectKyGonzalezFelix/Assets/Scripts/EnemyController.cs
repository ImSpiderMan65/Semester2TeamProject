using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float damage;
    public float health;

    private Rigidbody rb;
    private Animator anim;
    private NavMeshAgent agent;
    public GameObject target;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player");
    }

    void Update()
    {
        if (CanSeeTarget() == true)
        {
            Pursue();
        }
        else
        {
            Ray ray = new Ray(transform.position, -Vector3.forward);
            
        }

        if (agent.velocity.magnitude < 0.5f)
        {
            anim.SetFloat("speed", 0f);
        }
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
        anim.SetFloat("speed", 1f);
    }

    Vector3 wanderTarget = Vector3.zero;
    void Wander()
    {
        float wanderRad = 0.5f;
        float wanderDis = 0.5f;
        float wanderJit = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJit, 0, Random.Range(-1.0f, 1.0f) * wanderJit);

        wanderTarget.Normalize();
        wanderTarget *= wanderRad;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDis);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    void Pursue()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

        if ((toTarget > 90 && relativeHeading > 20) || target.GetComponent<PlayerController>().speed < 0.001f)
        {
            Seek(target.transform.position);
            return;
        }

        float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent<PlayerController>().currentSpeed);
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    bool CanSeeTarget()
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - this.transform.position;
        float lookAngle = Vector3.Angle(this.transform.forward, rayToTarget);
        if (Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo) && lookAngle < 85)
        {
            if (raycastInfo.transform.gameObject.tag == "Player") { return true; }
        }

        return false;
    }
}
