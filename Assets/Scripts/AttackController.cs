using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour {

    [HideInInspector]
    public bool isAttacking = false;

    private WeaponController weapon;

	// Use this for initialization
	void Awake () {
        weapon = gameObject.transform.GetChild(0).gameObject.GetComponent<WeaponController>();
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
        yield return new WaitForSeconds((1f / weapon.speed) * 1);
        //TODO: Faire une pause correspondant à la vitesse de l'arme
        //yield return new WaitForSeconds(2);
        isAttacking = false;
    }
}
