using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour {

    [HideInInspector]
    public bool isAttacking = false;

    private WeaponScript weapon;

	// Use this for initialization
	void Awake () {
        weapon = gameObject.transform.GetChild(0).gameObject.GetComponent<WeaponScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            
            StartCoroutine(Attack());
        }
	}

    private IEnumerator Attack()
    {
        isAttacking = true;
        Debug.Log("Start");
        yield return new WaitForSeconds((1f / weapon.speed) * 1);
        //TODO: Faire une pause correspondant à la vitesse de l'arme
        //yield return new WaitForSeconds(2);
        isAttacking = false;
        Debug.Log("End");
        
    }
}
