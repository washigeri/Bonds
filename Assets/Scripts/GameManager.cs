using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [HideInInspector] public GameObject player1;
    [HideInInspector] public GameObject player2;
    [Range(0, 5)]
    [HideInInspector]
    public int potionNumber;
    [HideInInspector] public int maxPotion;
    [HideInInspector] public float potionHeal;

    public bool isPaused;
    private GameObject pauseMenu;
    private GameObject inGamePanel;

    private int gameOverScene;
    private int bossSceneBuildIndex;
    private int currentScene;
    private bool sceneChanged;
    private bool isSceneLoaded;

    private bool isOnMenu;
    private bool isPlaying;
    private bool isGameReady;

    private List<GameObject> toBeCleanOnSceneChange;

    private void Awake()
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
        //SoundManager.instance.PlayMusic(SoundManager.instance.bossMusic, true, 1000f);
    }

    private void Start()
    {
        toBeCleanOnSceneChange = new List<GameObject>();
        isPaused = false;
        bossSceneBuildIndex = 4;
        gameOverScene = 6;
        currentScene = 0;
        sceneChanged = false;
        isOnMenu = true;
        isPlaying = false;
        isGameReady = false;
        isSceneLoaded = false;
        InitializeGameVariables();
        IgnoreCollision();
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void IgnoreCollision()
    {
        Physics2D.IgnoreLayerCollision(8, 10, true);
        Physics2D.IgnoreLayerCollision(8, 14, false);
        Physics2D.IgnoreLayerCollision(9, 14, false);
    }

    private void OnSceneUnloaded(Scene scene)
    {
        CleanSceneOnChange();
    }

    private void SetMusic()
    {
        SoundManager sm = SoundManager.instance;
        switch (currentScene)
        {
            case 0:
                if(sm.currentMusic != 0)
                {
                    sm.PlayMusic(sm.titleMusic, true, 4f);
                    sm.currentMusic = 0;
                }
                break;
            case 1:

                if (sm.currentMusic != 1)
                {
                    sm.PlayMusic(sm.theme1Music, true, 4f);
                    sm.currentMusic = 1;
                }
                break;
            case 2:

                if (sm.currentMusic != 1)
                {
                    sm.PlayMusic(sm.theme1Music, true, 4f);
                    sm.currentMusic = 1;
                }
                break;
            case 3:

                if (sm.currentMusic != 1)
                {
                    sm.PlayMusic(sm.theme1Music, true, 4f);
                    sm.currentMusic = 1;
                }
                break;
            case 4:

                if (sm.currentMusic != 2)
                {
                    sm.PlayMusic(sm.bossMusic, true, 4f);
                    sm.currentMusic = 2;
                }
                break;
            case 5:
                if (sm.currentMusic != 1)
                {
                    sm.PlayMusic(sm.theme1Music, true, 4f);
                    sm.currentMusic = 1;
                }
                break;
            case 6:
                if (sm.currentMusic != 0)
                {
                    sm.PlayMusic(sm.titleMusic, true, 4f);
                    sm.currentMusic = 0;
                }
                break;
            default:
                break;
        }
    }

    private void InitializeGameVariables()
    {
        maxPotion = 5;
        potionHeal = 0.5f;
    }

    private void CleanPlayers()
    {
        Destroy(inGamePanel);
        Destroy(player1);
        Destroy(player2);
    }

    private void ResetPlayers()
    {
        player1.transform.position = Vector3.zero;
        player2.transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (isPlaying)
        {
            if (isGameReady)
            {
                if (player1.GetComponent<PlayerController>().isDead || player2.GetComponent<PlayerController>().isDead)
                {
                    GameOver();
                }
                if (!isSceneLoaded)
                {
                    LoadByIndex(currentScene);
                    ResetPlayers();
                    Camera.main.GetComponent<CameraController>().SetupCamera(currentScene);
                }
                else
                {
                    if (Input.GetButtonDown("Cancel"))
                    {
                        TogglePause();
                    }
                }
            }
            else
            {
                InstantiatePlayersWithItems();
                isGameReady = true;
            }  
        }
        else
        {
            if (isGameReady)
            {
                if (!isSceneLoaded)
                {
                    LoadByIndex(0);
                }
            }
            else
            {
                Camera.main.GetComponent<CameraController>().SetupCamera(0);
                CleanPlayers();
                isGameReady = true;
            }
        }
        
    }

    public void LoadByIndex(int index)
    {
        SceneManager.LoadScene(index);
        currentScene = index;
        SetMusic();
        isSceneLoaded = true;
    }

    private void InstantiatePlayersWithItems()
    {
        if (loadedSavedGame)
        {
            player1 = Instantiate(Resources.Load("Prefabs/Players/Player1"), savedPosition, Quaternion.Euler(0, 0, 0)) as GameObject;
            GiveWeapon(p1Weapon, player1);
            GiveTrinket(-1, player1);
            player2 = Instantiate(Resources.Load("Prefabs/Players/Player2"), savedPosition, Quaternion.Euler(0, 0, 0)) as GameObject;
            GiveWeapon(p2Weapon, player2);
            GiveTrinket(-1, player2);
        }
        else
        {
            player1 = Instantiate(Resources.Load("Prefabs/Players/Player1"), Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;
            GiveWeapon(1, player1);
            GiveTrinket(-1, player1);
            player2 = Instantiate(Resources.Load("Prefabs/Players/Player2"), Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;
            GiveWeapon(3, player2);
            GiveTrinket(-1, player2);
        }
        DontDestroyOnLoad(player1);
        DontDestroyOnLoad(player2);
        this.inGamePanel = Instantiate(Resources.Load("Prefabs/UI/InGamePanel"), Camera.main.transform) as GameObject;
        this.inGamePanel.GetComponent<Canvas>().worldCamera = Camera.main;
        this.inGamePanel.GetComponent<Canvas>().sortingLayerName = "UI";
    }

    private void InstantiatePlayersWithItems(int p1WeaponID, int p1TrinketID, Vector3 p1Position, int p2WeaponID, int p2TrinketID, Vector3 p2Position)
    {
        player1 = Instantiate(Resources.Load("Prefabs/Players/Player1"), p1Position, Quaternion.Euler(0, 0, 0)) as GameObject;
        GiveWeapon(p1WeaponID, player1);
        GiveTrinket(p1TrinketID, player1);
        player2 = Instantiate(Resources.Load("Prefabs/Players/Player2"), p2Position, Quaternion.Euler(0, 0, 0)) as GameObject;
        GiveWeapon(p2WeaponID, player2);
        GiveTrinket(p2TrinketID, player2);
    }

    public void StartNewGame()
    {
        isPlaying = true;
        isGameReady = false;
        isSceneLoaded = false;
        currentScene = 1;
    }

    public void GoToNextLevel()
    {
        isSceneLoaded = false;
        currentScene++;
    }

    public void GoBackToMenu()
    {
        isPlaying = false;
        isGameReady = false;
        isSceneLoaded = false;
        currentScene = 0;
    }

    public void GameOver()
    {
        isPlaying = false;
        isSceneLoaded = false;
        isGameReady = false;
        currentScene = 6;
    }

    private void GiveWeapon(int weaponType, GameObject player)
    {
        if (player.GetComponent<PlayerController>().GetMyWeapon() == null)
        {
            string weaponName = null;
            switch (weaponType)
            {
                case 0:
                    weaponName = "Spear";
                    break;
                case 1:
                    weaponName = "Sword";
                    break;
                case 2:
                    weaponName = "Daggers";
                    break;
                case 3:
                    weaponName = "Bow";
                    break;
                default:
                    break;
            }
            if (weaponName != null)
            {
                GameObject weapon = Instantiate(Resources.Load("Prefabs/Weapons/" + weaponName)) as GameObject;
                weapon.transform.parent = player.transform.Find("Hand");
                WeaponController weaponScript = weapon.GetComponent<WeaponController>();
                weapon.transform.localPosition = weaponScript.GetDefaultLocalPosition();
                weapon.transform.localEulerAngles = weaponScript.GetDefaultLocalRotation();
                weaponScript.SetHasOwner(true);
                weaponScript.SetOwner(player.CompareTag("Player1") ? 1 : 2);
                weaponScript.SetPlayer(player.GetComponent<PlayerController>());
                weaponScript.SetWeaponInfo();
                player.GetComponent<PlayerController>().SetMyWeapon(weaponScript);
            }
        }
    }

    private void GiveTrinket(int trinketType, GameObject player)
    {
        
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            if (pauseMenu == null)
            {
                pauseMenu = inGamePanel.transform.Find("PauseMenu").gameObject;
                pauseMenu.transform.Find("Quitter").gameObject.GetComponent<Button>().onClick.AddListener(delegate { this.GoBackToMenu(); this.TogglePause(); });
                pauseMenu.transform.Find("Reprendre").gameObject.GetComponent<Button>().onClick.AddListener(delegate { this.TogglePause(); });
            }
            pauseMenu.SetActive(true);
        }

        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    private void CleanSceneOnChange()
    {
        foreach (GameObject go in toBeCleanOnSceneChange)
        {
            Destroy(go);
        }
    }

    public void AddObjectToBeCleaned(GameObject go)
    {
        toBeCleanOnSceneChange.Add(go);
    }

    public void RemoveObjectToBeCleaned(int goID)
    {
        bool isRemoved = false;
        int length = toBeCleanOnSceneChange.Count;
        int i = 0;
        while (i < length && !isRemoved)
        {
            if (toBeCleanOnSceneChange[i].GetInstanceID() == goID)
            {
                toBeCleanOnSceneChange.RemoveAt(i);
                isRemoved = true;
            }
            i++;
        }
    }

    public int GetBossSceneBuildIndex()
    {
        return bossSceneBuildIndex;
    }

    private void OnDestroy()
    {
        if (toBeCleanOnSceneChange != null)
        {
            CleanSceneOnChange();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////// SAUVEGARDE ///////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////
    private bool loadedSavedGame;

    private Vector2 savedPosition;
    private int p1Weapon;
    private int p2Weapon;
    private int savedScene;


    public void Save()
    {
        savedPosition = player1.transform.position;
        savedScene = currentScene;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "Save.dat");

        SavedData data = new SavedData
        {
            potionNumber = potionNumber,
            y = savedPosition[1],
            x = savedPosition[0],
            player1Weapon = player1.GetComponentInChildren<WeaponController>().GetWeaponID(),
            player2Weapon = player2.GetComponentInChildren<WeaponController>().GetWeaponID(),
            savedScene = savedScene
        };

        bf.Serialize(file, data);
        file.Close();
    }

    public void RetrieveSavedData()
    {
        if (File.Exists(Application.persistentDataPath + "Save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "Save.dat", FileMode.Open);

            SavedData data = (SavedData)bf.Deserialize(file);
            file.Close();

            potionNumber = data.potionNumber;
            savedPosition = new Vector3(data.x, data.y);
            savedScene = data.savedScene;
        }
    }

    public void LoadSavedGame()
    {
        RetrieveSavedData();
        LoadByIndex(savedScene);
        loadedSavedGame = true;
    }
    //////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////

}

[System.Serializable]
class SavedData
{
    [Range(0, 5)]
    public int potionNumber;
    public float x;
    public float y;
    public int player1Weapon;
    public int player2Weapon;
    public int player1Trinket;
    public int player2Trinket;
    public int savedScene;
}