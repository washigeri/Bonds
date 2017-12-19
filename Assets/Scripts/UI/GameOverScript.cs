using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    private void Start()
    {
        GameManager gm = GameManager.gameManager;
        GameObject.Find("Canvas").transform.Find("MenuPrincipal").GetComponent<Button>().onClick.AddListener(delegate { gm.GoBackToMenu(); });
        GameObject.Find("Canvas").transform.Find("Quitter").GetComponent<Button>().onClick.AddListener(delegate { gm.Quit(); });
    }
}