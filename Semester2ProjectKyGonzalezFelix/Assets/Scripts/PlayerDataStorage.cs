using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataStorage : MonoBehaviour
{
    private static PlayerDataStorage Instance;

    public static int playerLevel;

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
}
