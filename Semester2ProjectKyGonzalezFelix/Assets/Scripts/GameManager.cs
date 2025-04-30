using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject deathScreen;
    public GameObject winScreen;
    public GameObject dungeonDoor;
    public Image playerHealth;
    

    public GameObject player;
    public GameObject[] enemySpawners;
    public GameObject[] enemyTypes;

    public int maxEnemies = 1;
    public int currentEnemies;

    private PlayerDataStorage data;

    private void Awake()
    {
        data = GameObject.FindGameObjectWithTag("Data").GetComponent<PlayerDataStorage>();
        enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawn");
    }

    private void Start()
    {
        SpawnEnemies();
    }

    private void Update()
    {
        currentEnemies = GameObject.FindObjectsOfType<EnemyController>().Length;
        if (currentEnemies <= 0 && data.gameWave < 5)
        {
            NextWave();
        }

        if (data.gameWave == 5 && currentEnemies <= 0)
        {
            DungeonCompleted();
        }
    }

    public void UpdatePlayerUI()
    {
        playerHealth.fillAmount = data.health / 100;
    }

    public void NextWave()
    {
        data.gameWave += 1;
        maxEnemies = data.gameWave;
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i <= maxEnemies - 1; i++)
        {
            int randomEnemy = Random.Range(0, enemyTypes.Length);
            int randomSpawn = Random.Range(0, enemySpawners.Length);
            Instantiate(enemyTypes[randomEnemy], enemySpawners[randomSpawn].transform.position, Quaternion.identity);
        }
    }

    public void DungeonCompleted()
    {
        winScreen.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void InitiateDungeonOpen()
    {
        StartCoroutine(DungeonOpen());

        
    }

    IEnumerator DungeonOpen()
    {
        data.dungeonCam.gameObject.SetActive(true);
        dungeonDoor.GetComponent<Animator>().SetTrigger("Open");

        yield return new WaitForSeconds(6);

        data.dungeonCam.gameObject.SetActive(false);
    }

}
