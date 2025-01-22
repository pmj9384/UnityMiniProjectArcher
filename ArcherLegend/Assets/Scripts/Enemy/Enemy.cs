using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;


public class Enemy : LivingEntity
{
    public LayerMask whatIsTarget;  
    public float findTargetDistance = 10f;  
    private LivingEntity targetEntity;
    private NavMeshAgent agent;

    private Animator zombieAnimator;
    private AudioSource audioSource;

    private Coroutine coUpdatePath;

    public ParticleSystem hitEffect;
    public AudioClip hitSound;
    public AudioClip deathSound;

    public GameObject expPrefab; //exp
    public int experienceValue = 10; 
    public float damage = 20f;
    public float timeBetAttack = 0.5f;
    private float lastAttackTime;

    private GameManager gm; 

    private void Awake()
    {
        zombieAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>(); 
    
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        coUpdatePath = StartCoroutine(UpdatePath());
    }

    protected void OnDisable()
    {
        if (coUpdatePath != null)
        {
            StopCoroutine(coUpdatePath);
            coUpdatePath = null;
        }
    }

    private void Update()
    {
        zombieAnimator.SetBool("HasTarget", hasTarget);
        float speed = agent.velocity.magnitude;
        zombieAnimator.SetFloat("Speed", speed);
    }

    private bool hasTarget
    {
        get { return targetEntity != null && !targetEntity.IsDead; }
    }

    private IEnumerator UpdatePath()
    {
        while (true)
        {
            if (!hasTarget)
            {
                agent.isStopped = true;
                targetEntity = FindTarget();
            }

            if (hasTarget)
            {
                agent.isStopped = false;
                agent.SetDestination(targetEntity.transform.position); 
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    public LivingEntity FindTarget()
    {
        var cols = Physics.OverlapSphere(transform.position, findTargetDistance, whatIsTarget.value);
        foreach (var col in cols)
        {
            var livingEntity = col.GetComponent<LivingEntity>();
            if (livingEntity != null && !livingEntity.IsDead)
            {
                return livingEntity;  
            }
        }
        return null;  
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);
        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffect.Play();
        audioSource.PlayOneShot(hitSound);
    }

    protected override void Die()
    {
    base.Die();
    audioSource.PlayOneShot(deathSound);
    zombieAnimator.SetTrigger("Die");

    agent.isStopped = true;


    if (coUpdatePath != null)
    {
        StopCoroutine(coUpdatePath);
        coUpdatePath = null;
    }

    var cols = GetComponents<Collider>();
    foreach (var col in cols)
    {
        col.enabled = false;
    }

    if (expPrefab != null)
    {
        GameObject exp = Instantiate(expPrefab, transform.position, Quaternion.identity);
        exp.GetComponent<EnemyExp>().expValue = experienceValue;
        ExperienceManager.Instance.RegisterExpItem(exp);
    }
    GameManager.Instance.DecrementZombieCount();
    Destroy(gameObject);
    if (gm != null)
    {
        gm.AddScore(100);
    }
  
    StartCoroutine(DieRoutine());
}


    private IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<LivingEntity>();
            if (player != null && !player.IsDead)
            {
                AttackTarget(player);
            }
        }
    }
    public void AttackTarget(LivingEntity target)
    {
        if (Time.time >= lastAttackTime + timeBetAttack)
        {
            if (target != null && !target.IsDead)
            {
                target.OnDamage(damage, transform.position, Vector3.zero);
                lastAttackTime = Time.time;
            }
        }
    }
}
