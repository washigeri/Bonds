using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class WeaponController : MonoBehaviour {

    public int damage;
    public int range;
    public int speed;
    protected float globalCD = 0.25f; 
    protected bool isAttacking = false;
    protected bool isOnGlobalCoolDown = false;
    protected PolygonCollider2D pCollider2D;
    protected float weakCD;
    protected float strongCD;
    protected float skillCD;
    protected string enemyTag;
  
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isOnGlobalCoolDown)
        {
            if (isAttacking)
            {
                if (collision.gameObject.CompareTag(enemyTag))
                {
                    collision.gameObject.GetComponent<EnemyController>().RemoveHealth(damage);
                }
            }
        }
    }

    // Use this for initialization
    protected virtual void Awake () {
        pCollider2D = GetComponent<PolygonCollider2D>();
        enemyTag = transform.root.CompareTag("Player2") ? "Spirit" : "Enemy";
    }

    protected virtual void Update()
    {
        pCollider2D.enabled = !isOnGlobalCoolDown && isAttacking;
    }

    protected abstract IEnumerator WeakAttack();

    protected abstract IEnumerator StrongAttack();

    protected abstract IEnumerator Skill();
	
}
