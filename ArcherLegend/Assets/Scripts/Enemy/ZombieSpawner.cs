using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private Zombie[] prefabs;
    [SerializeField] private ZombieData[] datas;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private LayerMask playerLayer;

    private GameManager gm;
    private int wave = 1;
    private float nextSpawnTime;
    private List<Zombie> zombies = new List<Zombie>();

    private void Start()
    {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (gm.IsGameOver)
            return;

        if (Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnInterval;
            CreateZombie();
        }
    }

    private void CreateZombie()
    {
        int index = Random.Range(0, prefabs.Length);
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var zombie = Instantiate(prefabs[index], spawnPoint.position, spawnPoint.rotation);
        zombie.gameObject.SetActive(true);

        var agent = zombie.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = true;
            if (!agent.Warp(spawnPoint.position))
            {
                Destroy(zombie.gameObject);
                return;
            }
        }
        else
        {
            Destroy(zombie.gameObject);
            return;
        }

        var data = datas[index];
        zombie.maxHp = data.hp;
        zombie.damage = data.damage;
        agent.speed = data.speed;

        zombie.whatIsTarget = LayerMask.GetMask("Player");
        zombies.Add(zombie);
    }
}
