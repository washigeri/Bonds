﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public BoxCollider2D levelEndBox;

    // Use this for initialization
    void Start()
    {
        levelEndBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            FinishLevel();
        }
    }

    private void FinishLevel()
    {
        int current_scene = SceneManager.GetActiveScene().buildIndex;
        switch (current_scene)
        {
            case 1:
                SceneManager.LoadScene(2);
                //SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));
                break;
            //case 2:
            //    SceneManager.LoadScene(3);
            //    SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(3));
            //    break;
            default:
                SceneManager.LoadScene(0);
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
                break;

        }

    }
}
