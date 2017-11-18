using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private bool isModified;

    private PlayerController player1Controller;

    private float player1Multiplier;

    private bool isMultiplierUpdated;

    private float damageMultiplier;

    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }

    private void Awake()
    {
        isModified = false;
        damageMultiplier = 1;
        player1Controller = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerController>();
        isMultiplierUpdated = false;
        player1Multiplier = 1f;
    }

    //private void Update()
    //{
    //    Debug.Log("isModified, damage multiplier = " + damageMultiplier + ".");
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            if (isModified && !isMultiplierUpdated)
            {
                player1Multiplier = player1Controller.GetDamageMultiplier();
                //Debug.Log("player 1 entered collision and set multiplier to " + damageMultiplier * player1Multiplier);
                player1Controller.SetDamageMultiplier(damageMultiplier * player1Multiplier);
                isMultiplierUpdated = true;
            }
            
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("enemy entered collision and set multiplier to " + damageMultiplier);
            collision.gameObject.GetComponent<EnemyController>().SetDamageMultiplier(damageMultiplier);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            if(isModified && !isMultiplierUpdated)
            {
                player1Multiplier = player1Controller.GetDamageMultiplier();
                //Debug.Log("player 1 stayed collision and set multiplier to " + damageMultiplier * player1Multiplier);
                player1Controller.SetDamageMultiplier(damageMultiplier * player1Multiplier);
                isMultiplierUpdated = true;
            }
            else if(!isModified && isMultiplierUpdated)
            {
                //Debug.Log("player 1 stayed collision and set multiplier to " + player1Multiplier);
                player1Controller.SetDamageMultiplier(player1Multiplier);
                isMultiplierUpdated = false;
            }
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("multipler set to 1.5 in collision stay for enemy");
            collision.gameObject.GetComponent<EnemyController>().SetDamageMultiplier(damageMultiplier);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            //Debug.Log("player 1 exited collision and set multiplier to " + player1Multiplier);
            player1Controller.SetDamageMultiplier(player1Multiplier);
            isMultiplierUpdated = false;
            player1Multiplier = 1f;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("enemy exited collision and set multiplier to 1");
            collision.gameObject.GetComponent<EnemyController>().SetDamageMultiplier(1f);
        }
    }

    public IEnumerator ModifyGround(float time, float damageMultiplier)
    {
        //Debug.Log("modifying ground");
        this.damageMultiplier = damageMultiplier;
        isModified = true;
        yield return new WaitForSeconds(time);
        this.damageMultiplier = 1f;
        isModified = false;
        //Debug.Log("reseting ground");
    }
}
