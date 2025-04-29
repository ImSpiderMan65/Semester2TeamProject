using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerDataStorage : MonoBehaviour
{
    private static PlayerDataStorage Instance;
    private GameManager gameManager;
    public Camera dungeonCam;

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
        dungeonCam = GameObject.Find("DungeonCamera").GetComponent<Camera>();
    }

    public void UpdatePlayerHealth(float addedHealth) // Updates the health bar UI for the player.
    {
        health += addedHealth;
        gameManager.UpdatePlayerUI();
    }

    public IEnumerator DungeonCutscene()
    {
        dungeonCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        
    }
}
