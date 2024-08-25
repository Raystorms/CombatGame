using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControlSingleton : MonoBehaviour
{
    private GameControlSingleton _instance;
    public GameControlSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject();
                _instance = obj.AddComponent<GameControlSingleton>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        _instance = this;
    }

    private void Update()
    {
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
