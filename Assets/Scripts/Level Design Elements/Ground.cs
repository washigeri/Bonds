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
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            if(isModified && !isMultiplierUpdated)
            {
                reverseDamageDone = 1f / damageDoneMultiplier;
                reverseDamageReceived = 1f / damageReceivedMultiplier;
                player1Controller.SetDamageDoneMultiplier(damageDoneMultiplier * player1Controller.GetDamageDoneMultiplier());
                player1Controller.SetDamageReceivedMultiplier(damageReceivedMultiplier * player1Controller.GetDamageReceivedMultiplier());
                isMultiplierUpdated = true;
            }
            else if(!isModified && isMultiplierUpdated)
            {
                player1Controller.SetDamageDoneMultiplier(player1Controller.GetDamageDoneMultiplier() * reverseDamageDone);
                player1Controller.SetDamageReceivedMultiplier(player1Controller.GetDamageReceivedMultiplier() * reverseDamageReceived);

                isMultiplierUpdated = false;

                ResetVariables();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            if (isMultiplierUpdated)
            {
                player1Controller.SetDamageDoneMultiplier(player1Controller.GetDamageDoneMultiplier() * reverseDamageDone);
                player1Controller.SetDamageReceivedMultiplier(player1Controller.GetDamageReceivedMultiplier() * reverseDamageReceived);
                isMultiplierUpdated = false;
                ResetVariables();
            }
        }
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
