using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//héritage enfant : parents
public class EnemyFly : EnemyStats
{

    public Transform attackPoint;
    public LayerMask playerLayerMask;

    // Start is called before the first frame update
    void Start()
    {
       InitializeCharacter(OnAttack);
    }

    void Update()
    {

    }

    void OnAttack()
    {
        StartCoroutine(Delay());

        //attack
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 1f));

            if(dist < attackRange)
            {
                anim.SetTrigger("Attack");
                rb.velocity = Vector2.zero;
            }
        }
        
        lastAttackTime = Time.time;
    }

    void Attack()
    {
        Collider2D player = Physics2D.OverlapCircle(attackPoint.transform.position, 0.5f, playerLayerMask);

        if(player != null && player.tag == "Player")
        {
            player.GetComponent<PlayerController>().TakeDamage(damage);
            Debug.Log("appel take damage");
        }
    }

    /*void PassOverObstacle()
    {}*/

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollision Enter 2D");
    }

    void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log("OnCollision Exit 2D");
    }
}
