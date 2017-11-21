using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{

    [HideInInspector] public GameObject player1;
    [HideInInspector] public GameObject player2;
    [Range(0, 5)]
    [HideInInspector]
    public int potionNumber;
    [HideInInspector] public int maxPotion;
    [HideInInspector] public float potionHeal;

    public static GameManager2 gameManager;

    public Vector2 savedPosition;
    public int savedScene;
    public int strength;
    public int agility;
    public int stamina;
    [Range(1, 4)]
    public int weaponTypeP1;
    [Range(1, 4)]
    public int weaponTypeP2;
    [Range(1, 4)]
    public int weaponTierP1;
    [Range(1, 4)]
    public int weaponTierP2;

    public bool isOnMenu;
    public int startedGame;
    public bool loadedSavedGame;
    public bool isGameInitialized;
    public bool isSceneLoaded;
    public bool isPaused;

    public int currentScene;

    private GameObject pauseMenu;

    void Awake()
    {
        if (gameManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }
    }

    private void IgnoreCollision()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        Physics2D.IgnoreLayerCollision(9, 10, true);
        Physics2D.IgnoreLayerCollision(4, 12, false);
        Physics2D.IgnoreLayerCollision(4, 10, true);
    }

    private void Start()
    {
        isOnMenu = true;
        currentScene = 0;
        startedGame = -1;
        isGameInitialized = false;
        isSceneLoaded = false;
        loadedSavedGame = false;
        isPaused = false;
        pauseMenu = Camera.main.transform.Find("InGamePanel").transform.Find("PauseMenu").gameObject;
        IgnoreCollision();
        InitializeGameVariables();
        Load();
        //Debug.Log(savedPosition);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (startedGame > -1)
        {
            if (!isSceneLoaded)
            {
                SceneManager.LoadScene(startedGame);
            }
            if (!isGameInitialized)
            {
                if (loadedSavedGame)
                {
                    InitializeSavedGame();
                }
                else
                {
                    InitializeNewGame();
                }
            }
            if (Input.GetButtonDown("Cancel"))
            {
                TogglePause();
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != currentScene)
        {
            isSceneLoaded = true;
            currentScene = scene.buildIndex;
            ResetPlayers();
        }
    }

    private void ResetPlayers()
    {
        player1.transform.position = Vector3.zero;
        player2.transform.position = Vector3.zero;
        float maxHp = player1.GetComponent<PlayerController>().maxHp;
        player1.GetComponent<PlayerController>().SetHealth(maxHp);
        player1.GetComponent<PlayerController>().SetHealth(maxHp);
        Camera.main.GetComponent<CameraController>().TargetPlayer1();
    }

    public void InitializeNewGame()
    {
        potionNumber = 0;
        player1 = Instantiate(Resources.Load("Prefabs/Players/Player1"), Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;
        //Debug.Log("Instantiated player1 " + player1.GetComponent<PlayerController>().GetDirH() + " " + player1.GetComponent<PlayerController>().GetHealth() + " " + player1.GetComponent<PlayerController>().GetMaxSpeed());
        player2 = Instantiate(Resources.Load("Prefabs/Players/Player2"), Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;
        //Debug.Log("Instantiated player2 " + player2.GetComponent<PlayerController>().GetDirH() + " " + player2.GetComponent<PlayerController>().GetHealth() + " " + player2.GetComponent<PlayerController>().GetMaxSpeed());
        DontDestroyOnLoad(player1);
        DontDestroyOnLoad(player2);
        CameraController mainCamera = Camera.main.GetComponent<CameraController>();
        mainCamera.SetCameraForGame();
        isGameInitialized = true;
    }

    public void InitializeSavedGame()
    {
        player1 = Instantiate(Resources.Load("Prefabs/Players/Player1"), savedPosition, Quaternion.Euler(0, 0, 0)) as GameObject;
        player2 = Instantiate(Resources.Load("Prefabs/Players/Player2"), savedPosition, Quaternion.Euler(0, 0, 0)) as GameObject;
        DontDestroyOnLoad(player1);
        DontDestroyOnLoad(player2);
        CameraController mainCamera = Camera.main.GetComponent<CameraController>();
        mainCamera.SetCameraForGame();
        isGameInitialized = true;
    }

    private void InitializeGameVariables()
    {
        maxPotion = 5;
        potionHeal = 0.5f;
    }

    public void LoadByIndex(int sceneIndex)
    {
        startedGame = sceneIndex;
    }

    public void LoadSavedGame()
    {
        startedGame = savedScene;
        loadedSavedGame = true;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

    public void Save()
    {
        savedPosition = player1.transform.position;
        savedScene = SceneManager.GetActiveScene().buildIndex;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "Save.dat");

        SavedData data = new SavedData
        {
            potionNumber = potionNumber,
            y = savedPosition[1],
            x = savedPosition[0],
            savedScene = savedScene
            //weaponTypeP1 = weaponTypeP1,
            //weaponTypeP2 = weaponTypeP2,
            //weaponTierP1 = weaponTierP1,
            //weaponTierP2 = weaponTierP2
        };

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "Save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "Save.dat", FileMode.Open);

            SavedData data = (SavedData)bf.Deserialize(file);
            file.Close();

            potionNumber = data.potionNumber;
            savedPosition = new Vector2(data.x, data.y);
            savedScene = data.savedScene;
            //weaponTypeP1 = data.weaponTypeP1;
            //weaponTypeP2 = data.weaponTypeP2;
            //weaponTierP1 = data.weaponTierP1;
            //weaponTierP2 = data.weaponTierP2;
            //Debug.Log("Before Coroutine");
            //PostLoad();
        }
    }

    public void PostLoad()
    {
        if (SceneManager.GetActiveScene().buildIndex != savedScene)
        {
            SceneManager.LoadScene(savedScene);
            Debug.Log("Scene loaded");
            //SetPlayers();
        }
        else
        {
            player1 = GameObject.FindGameObjectWithTag("Player1");
            player2 = GameObject.FindGameObjectWithTag("Player2");
            Camera.main.transform.position = new Vector3(savedPosition.x, savedPosition.y, Camera.main.transform.position.z);
            player1.transform.position = savedPosition;
            player2.transform.position = savedPosition;
            //Debug.Log(player1.transform.position.x);
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            if (pauseMenu == null)
            {
                pauseMenu = Camera.main.transform.Find("InGamePanel").transform.Find("PauseMenu").gameObject;
            }
            pauseMenu.SetActive(true);
        }

        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

}

[Serializable]
class SavedData
{
    [Range(0, 5)]
    public int potionNumber;
    public float x;
    public float y;
    public int savedScene;
    //public int strength;
    //public int agility;
    //public int stamina;
    //[Range(1, 4)]
    //public int weaponTypeP1;
    //[Range(1, 4)]
    //public int weaponTypeP2;
    //[Range(1, 4)]
    //public int weaponTierP1;
    //[Range(1, 4)]
    //public int weaponTierP2;
}
