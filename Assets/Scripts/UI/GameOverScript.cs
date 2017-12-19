using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        GameManager gm = GameManager.gameManager;
        Button mainMenu = transform.Find("MenuPrincipal").GetComponent<Button>();
        Button quit = transform.Find("Quitter").GetComponent<Button>();
        mainMenu.onClick.AddListener(delegate
        {
            gm.GoBackToMenu();
        });
        quit.onClick.AddListener(delegate
        {
            gm.Quit();
        });
    }
}