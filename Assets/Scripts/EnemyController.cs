using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {


    public int health = 10;
    public Text enemyHp;



    public void RemoveHealth(int loss)
    {
        this.health -= loss;
        UpdateText();

    }

	// Use this for initialization
	void Awake () {
        UpdateText();
	}

    private void UpdateText()
    {
        enemyHp.text = "Enemy HP : " + this.health;
    }
	
	// Update is called once per frame
	void Update () {

        if (this.health <= 0)
            Destroy(gameObject);
	}
}
