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
    public Image sceneTransitionObject;
    public Image sceneTransitioner;

    public static int playerLevel;
    public int dungeonLevel = 1;
    public float maxHealth;
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

    }

    

    

}
