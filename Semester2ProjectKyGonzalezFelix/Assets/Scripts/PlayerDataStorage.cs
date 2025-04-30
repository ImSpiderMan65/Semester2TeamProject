using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class PlayerDataStorage : MonoBehaviour
{
    private static PlayerDataStorage Instance;
    private GameManager gameManager;
    public Camera dungeonCam;
    public Image sceneTransitionObject;
    public Image sceneTransitioner;

    public Canvas canvas;

    public static int playerLevel;
    public int gameWave = 1;
    public int dungeonLevel = 1;
    public float health = 100f;
    public float damage = 5f;

    private void Awake() // Makes sure that player data is saved in between scenes.
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        canvas = FindFirstObjectByType<Canvas>();
        if (sceneTransitioner == null)
        {
            sceneTransitioner = Instantiate(sceneTransitionObject, canvas.transform);
        }

        StartCoroutine(SceneTranstionerOut());
    }

    public void UpdatePlayerHealth(float addedHealth) // Updates the health bar UI for the player.
    {
        health += addedHealth;
        gameManager.UpdatePlayerUI();
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

}
