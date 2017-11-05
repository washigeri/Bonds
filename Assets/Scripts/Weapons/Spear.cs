using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : WeaponController
{

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isOnGlobalCoolDown && !isAttacking)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                StartCoroutine(WeakAttack());
            }
            
        }
    }

    protected override IEnumerator WeakAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
        //TODO: Faire une pause correspondant à la vitesse de l'arme
        isOnGlobalCoolDown = true;
        yield return new WaitForSeconds(globalCD);
        isOnGlobalCoolDown = false;
    }

    protected override IEnumerator StrongAttack()
    {
        yield return new WaitForSeconds(0f);
    }

    protected override IEnumerator Skill()
    {
        yield return new WaitForSeconds(0f);
    }
}
