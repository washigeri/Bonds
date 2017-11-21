using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private bool isModified;

    private PlayerController player1Controller;

    private bool isMultiplierUpdated;

    private float damageDoneMultiplier;
    private float reverseDamageDone;

    private float damageReceivedMultiplier;
    private float reverseDamageReceived;

    private void Awake()
    {
        isModified = false;
        damageDoneMultiplier = 1f;
        reverseDamageDone = 1f;
        damageReceivedMultiplier = 1f;
        reverseDamageReceived = 1f;
        player1Controller = null;
        isMultiplierUpdated = false;
    }

    private void Update()
    {
        Debug.Log("isModified = " + isModified + " , damage done multiplier = " + damageDoneMultiplier + " damage received multiplier = " + damageReceivedMultiplier);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            if(player1Controller == null)
            {
                player1Controller = collision.gameObject.GetComponent<PlayerController>();
            }
            if (isModified && !isMultiplierUpdated)
            {
                reverseDamageDone = 1f / damageDoneMultiplier;
                reverseDamageReceived = 1f / damageReceivedMultiplier;

                player1Controller.SetDamageDoneMultiplier(damageDoneMultiplier * player1Controller.GetDamageDoneMultiplier());
                player1Controller.SetDamageReceivedMultiplier(damageReceivedMultiplier * player1Controller.GetDamageReceivedMultiplier());

                isMultiplierUpdated = true;
            }
            
        }
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    //Debug.Log("enemy entered collision and set multiplier to " + damageMultiplier);
        //    collision.gameObject.GetComponent<EnemyController>().SetDamageMultiplier(damageDoneMultiplier);
        //}
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            if(isModified && !isMultiplierUpdated)
            {
                //Debug.Log("player 1 stayed collision and set multiplier to " + damageMultiplier * player1Multiplier);
                reverseDamageDone = 1f / damageDoneMultiplier;
                reverseDamageReceived = 1f / damageReceivedMultiplier;
                player1Controller.SetDamageDoneMultiplier(damageDoneMultiplier * player1Controller.GetDamageDoneMultiplier());
                player1Controller.SetDamageReceivedMultiplier(damageReceivedMultiplier * player1Controller.GetDamageReceivedMultiplier());
                isMultiplierUpdated = true;
            }
            else if(!isModified && isMultiplierUpdated)
            {
                //Debug.Log("player 1 stayed collision and set multiplier to " + player1Multiplier);
                player1Controller.SetDamageDoneMultiplier(player1Controller.GetDamageDoneMultiplier() * reverseDamageDone);
                player1Controller.SetDamageReceivedMultiplier(player1Controller.GetDamageReceivedMultiplier() * reverseDamageReceived);

                isMultiplierUpdated = false;

                ResetVariables();
            }
        }
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    //Debug.Log("multipler set to 1.5 in collision stay for enemy");
        //    collision.gameObject.GetComponent<EnemyController>().SetDamageMultiplier(damageDoneMultiplier);
        //}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            if (isMultiplierUpdated)
            {
                //Debug.Log("player 1 exited collision and set multiplier to " + player1Multiplier);
                player1Controller.SetDamageDoneMultiplier(player1Controller.GetDamageDoneMultiplier() * reverseDamageDone);
                player1Controller.SetDamageReceivedMultiplier(player1Controller.GetDamageReceivedMultiplier() * reverseDamageReceived);
                isMultiplierUpdated = false;
                ResetVariables();
            }
        }
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    //Debug.Log("enemy exited collision and set multiplier to 1");
        //    collision.gameObject.GetComponent<EnemyController>().SetDamageMultiplier(1f);
        //}
    }

    public IEnumerator ModifyGround(float time, float damageDoneMultiplier, float damageReceivedMultiplier)
    {
        this.damageDoneMultiplier = damageDoneMultiplier;
        this.damageReceivedMultiplier = damageReceivedMultiplier;
        isModified = true;
        yield return new WaitForSeconds(time);
        isModified = false;
        this.damageDoneMultiplier = 1f;
        this.damageReceivedMultiplier = 1f;
    }

    private void ResetVariables()
    {
        reverseDamageDone = 1f;
        reverseDamageReceived = 1f;
    }
}
