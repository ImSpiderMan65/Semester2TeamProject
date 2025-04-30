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
        if (currentEnemies <= 0 && data.gameWave < 5)
        {
            NextWave();
        }

        if (data.gameWave == 5 && currentEnemies <= 0)
        {
            DungeonCompleted();
        }

        if (data.health <= 0)
        {
            Died();
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
        Cursor.visible = true;
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

}
