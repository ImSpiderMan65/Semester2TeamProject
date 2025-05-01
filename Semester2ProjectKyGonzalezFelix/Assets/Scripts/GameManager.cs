using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
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
    public Image sceneTransitionObject;
    public Image sceneTransitioner;
    public Canvas canvas;

    public Camera dungeonCam;



    public GameObject player;
    public GameObject[] enemySpawners;
    public GameObject[] enemyTypes;

    public int maxEnemies = 1;
    public int currentEnemies;
    public int gameWave;

    public float health = 100f;

    private PlayerDataStorage data;

    private void Awake()
    {
        data = GameObject.FindGameObjectWithTag("Data").GetComponent<PlayerDataStorage>();
        enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawn");
    }

    private void Start()
    {
        canvas = FindFirstObjectByType<Canvas>();
        if (sceneTransitioner == null)
        {
            sceneTransitioner = Instantiate(sceneTransitionObject, canvas.transform);
        }

        

        StartCoroutine(SceneTranstionerOut());

        SpawnEnemies();
    }

    private void Update()
    {
        currentEnemies = GameObject.FindObjectsOfType<EnemyController>().Length;
        if (currentEnemies <= 0 && gameWave <= 4)
        {
            NextWave();
        }

        if (gameWave == 5 && currentEnemies <= 0 && winScreen != null)
        {
            DungeonCompleted();
        }

        if (health <= 0 && deathScreen != null)
        {
            Died();
        }
    }

    public void UpdatePlayerUI()
    {
        playerHealth.fillAmount = health / 100;
    }

    public void NextWave()
    {
        gameWave += 1;
        maxEnemies = gameWave;
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
        Cursor.visible = true;
    }

    public void InitiateDungeonOpen()
    {
        StartCoroutine(DungeonOpen());
    }

    public IEnumerator DungeonOpen()
    {
        dungeonCam.gameObject.SetActive(true);
        dungeonDoor.GetComponent<Animator>().Play("DoorOpen");

        yield return new WaitForSeconds(6);

        dungeonCam.gameObject.SetActive(false);
        dungeonDoor.gameObject.SetActive(false);
    }

    public void Died()
    {
        deathScreen.gameObject.SetActive(true);
    }

    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(SceneTransitionerIn(sceneName));
    }

    public IEnumerator SceneTransitionerIn(string sceneName)
    {
        sceneTransitioner.GetComponent<Animator>().Play("FadeIn");
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(sceneName);

    }

    public IEnumerator SceneTranstionerOut()
    {
        sceneTransitioner.GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdatePlayerHealth(float addedHealth) // Updates the health bar UI for the player.
    {
        health += addedHealth;
        UpdatePlayerUI();
    }

}
