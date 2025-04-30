using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float damage = 1f;
    public float health;
    private float initialHealth;

    public bool attackCooldown;
    public bool deathCutscene;
    public bool immunityCooldown;

    private Rigidbody rb;
    private Animator anim;
    private NavMeshAgent agent;
    public GameObject target;
    private PlayerController player;

    public AudioSource audioSource;
    public AudioClip attackSound;

    public Image healthBar;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player");
        player = target.GetComponent<PlayerController>();

        initialHealth = health;
        UpdateHealth(0f);
    }

    void Update()
    {
        if (deathCutscene == false)
        {
            Pursue();
        }

        if (agent.velocity.magnitude < 0.5f)
        {
            anim.SetFloat("speed", 0f);
        }

        if (IsInRange() && attackCooldown == false && deathCutscene == false)
        {
            StartCoroutine(Attack());
        }

        if (health <= 0f && deathCutscene == false)
        {
            StartCoroutine(Die());
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

    bool IsInRange()
    {
        if (Vector3.Distance(target.transform.position, this.transform.position) < 4)
        {
            return true;
        }

        return false;
    }

    IEnumerator Attack()
    {
        anim.SetTrigger("isAttacking");
        audioSource.PlayOneShot(attackSound);
        player.playerDataStorage.UpdatePlayerHealth(-damage);
        attackCooldown = true;
        yield return new WaitForSeconds(1);

        attackCooldown = false;
    }

    public void UpdateHealth(float addedHealth)
    {
        health += addedHealth;
        healthBar.fillAmount = health / initialHealth;
    }

    IEnumerator Hit()
    {

        yield return new WaitForSeconds(0.3f);

    }

    IEnumerator Die()
    {
        print("Die");
        anim.SetTrigger("isFall");
        deathCutscene = true;
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Weapon>(out Weapon component))
        {
            UpdateHealth(-player.playerDataStorage.damage);
            
        }
    }


}
