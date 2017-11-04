using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponScript : MonoBehaviour {

    public int damage;
    public int range;
    public int speed;

    public Text textInfo;

    private string defaultText;

    private AttackController handScript;

  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision !");
        if(collision.gameObject.CompareTag("RealEnemy"))
        {
            if (handScript.isAttacking)
            {
                collision.gameObject.GetComponent<EnemyController>().RemoveHealth(this.damage);
                StartCoroutine(changeText());
            }
           
        }
        return;
    }

    // Use this for initialization
    void Awake () {
        this.handScript = this.transform.parent.GetComponent<AttackController>();
        this.defaultText = this.textInfo.text;
	}

    private IEnumerator changeText()
    {
        this.textInfo.text = "Hit !";
        this.textInfo.color = new Color(1, 0, 0);
        yield return new WaitForSeconds(1);
        this.textInfo.text = this.defaultText;
        this.textInfo.color = new Color(1, 1, 1);
        yield break;
    }
	
}
