using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : WeaponController
{

    private BoxCollider2D bCollider2D;

    protected override void Awake()
    {
        bCollider2D = GetComponent<BoxCollider2D>();
        damage = 5;
        range = 10;
        speed = 5;
        weaponName = "Bow";
        base.Awake();
    }

    protected override void Update()
    {
        bCollider2D.enabled = !hasOwner;
        base.Update();
    }

    protected override IEnumerator WeakAttack()
    {
        Debug.Log("Weak attack");
        isAttacking = 0;
        Shoot();
        yield return new WaitForSeconds(0.25f);
        isAttacking = -1;
        isOnGlobalCoolDown = true;
        yield return new WaitForSeconds(globalCD);
        isOnGlobalCoolDown = false;
    }

    private void Shoot()
    {
        GameObject arrow = Instantiate(Resources.Load("Prefabs/Weapons/Arrow"), gameObject.transform.position, Quaternion.Euler(0,0,0)) as GameObject;
        arrow.GetComponent<Arrow>().SetParameters(attacksDamage[isAttacking], enemyTag, (gameObject.transform.root.GetComponent<PlayerController>().faceRight ? 1 : -1), gameObject.transform.position);
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
