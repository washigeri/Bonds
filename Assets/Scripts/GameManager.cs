using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [HideInInspector] public static GameObject player1;
    [HideInInspector] public static GameObject player2;
    [Range(0, 5)]
    [HideInInspector]
    public static int potionNumber;
    [HideInInspector] public static int maxPotion;
    [HideInInspector] public static float potionHeal;

    public static GameManager gameManager;

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

    // Use this for initialization
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
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        potionNumber = 0;
        maxPotion = 5;
        potionHeal = 0.5f;
        Physics2D.IgnoreLayerCollision(8, 9, true);
        Physics2D.IgnoreLayerCollision(9, 10, true);
        Physics2D.IgnoreLayerCollision(4, 12, false);
        Physics2D.IgnoreLayerCollision(4, 10, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (player1.GetComponent<PlayerController>().GetHealth() <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (player2.GetComponent<PlayerController>().GetHealth() <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void PreSave()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        //potionNumber = 
        savedPosition = player1.transform.position;
        savedScene = SceneManager.GetActiveScene().buildIndex;
        //    strength =
        //    agility  =
        //    stamina =
        //    weaponTypeP1 =
        //    weaponTypeP2 =
        //    weaponTierP1 =
        //    weaponTierP2 =
        Save();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "Save.dat");

        SavedData data = new SavedData
        {
            potionNumber = potionNumber,
            y = savedPosition[1],
            x = savedPosition[0],
            savedScene = savedScene
            //strength = strength,
            //agility = agility,
            //stamina = stamina,
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
            //strength = data.strength;
            //agility = data.agility;
            //stamina = data.stamina;
            //weaponTypeP1 = data.weaponTypeP1;
            //weaponTypeP2 = data.weaponTypeP2;
            //weaponTierP1 = data.weaponTierP1;
            //weaponTierP2 = data.weaponTierP2;

            StartCoroutine(PostLoad());
        }
    }

    public IEnumerator PostLoad()
    {
        if (SceneManager.GetActiveScene().buildIndex != savedScene)
        {
            SceneManager.LoadScene(savedScene);
        }

        yield return new WaitForSeconds(0.25f);

        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        Camera.main.transform.position = new Vector3(savedPosition.x, savedPosition.y, Camera.main.transform.position.z);
        player1.transform.position = savedPosition;
        player2.transform.position = savedPosition;

        yield return null;

    }
}

//[Serializable]
//class SavedData
//{
//    [Range(0, 5)]
//    public int potionNumber;
//    public float x;
//    public float y;
//    public int savedScene;
//    public int strength;
//    public int agility;
//    public int stamina;
//    [Range(1, 4)]
//    public int weaponTypeP1;
//    [Range(1, 4)]
//    public int weaponTypeP2;
//    [Range(1, 4)]
//    public int weaponTierP1;
//    [Range(1, 4)]
//    public int weaponTierP2;
//}
