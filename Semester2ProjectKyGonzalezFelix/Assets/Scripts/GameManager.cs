using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject deathScreen;
    public GameObject winScreen;

    public GameObject player;
    public GameObject[] enemySpawners;
    public GameObject[] enemyTypes;

    private PlayerDataStorage data;

    private void Awake()
    {
        data = GameObject.FindGameObjectWithTag("Data").GetComponent<PlayerDataStorage>();
        enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawn");
    }

    private void Start()
    {
        foreach (GameObject enemySpawner in enemySpawners)
        {
            Instantiate(enemyTypes[0], enemySpawner.transform);
        }
    }
}
