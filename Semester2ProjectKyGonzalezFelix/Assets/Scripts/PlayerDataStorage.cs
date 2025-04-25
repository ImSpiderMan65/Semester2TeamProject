using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataStorage : MonoBehaviour
{
    private static PlayerDataStorage Instance;
    private GameManager gameManager;

    public static int playerLevel;
    public float health = 100f;

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
    }

    public void UpdatePlayerHealth(float addedHealth)
    {
        health += addedHealth;
        gameManager.UpdatePlayerUI();
    }
}
