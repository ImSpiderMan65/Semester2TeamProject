using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject deathScreen;
    public GameObject winScreen;
    public Image playerHealth;

    public GameObject player;
    public GameObject[] enemySpawners;
    public GameObject[] enemyTypes;

    public int maxEnemies = 5;

    private PlayerDataStorage data;

    private void Awake()
    {
        data = GameObject.FindGameObjectWithTag("Data").GetComponent<PlayerDataStorage>();
        enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawn");
    }

    private void Start()
    {
        for (int i = 0; i <= maxEnemies + 1; i++)
        {
            int randomEnemy = Random.Range(0, enemyTypes.Length);
            int randomSpawn = Random.Range(0, enemySpawners.Length);
            Instantiate(enemyTypes[randomEnemy], enemySpawners[randomSpawn].transform.position, Quaternion.identity);
        }
    }

    public void UpdatePlayerUI()
    {
        playerHealth.fillAmount = data.health / 100;
    }
}
