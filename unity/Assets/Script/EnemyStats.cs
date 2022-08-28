using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class EnemyStats : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
    private float playerDetectTime;
    public float playerDetectRate;
    public float chaseRange;
    bool lookRight;
    protected int currentHealth;
    public int maxHealth;

    [Header("Attack")]
    public float attackRange;
    public float attackRate;
    protected float lastAttackTime;
    public int damage;

    [Header("Component")]
    protected Rigidbody2D rb;
    private PlayerController targetPlayer;
    protected Animator anim;
    public GameObject healthBar;
    public Image life;

    [Header("PathFinding")]
    public float nextWaypointDistance = 2f;
    protected Path path;
    int currentWayPoint = 0;
    bool reachEndPath = false;
    protected Seeker seeker;
    protected float dist;

    [Header("Loot")]
    public GameObject[] loot;
    public delegate void MyAttack();
    public MyAttack myMethode;

    public void InitializeCharacter(MyAttack _myMethode)
    {
        this.currentHealth = this.maxHealth;
        life.fillAmount = this.maxHealth;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        InvokeRepeating("UpdatePath", 0f, .5f);
        myMethode = _myMethode;
    }

    private void Update() 
    {
        Flip();
    }

    public void UpdateHealthBar(int value)
    {
        life.fillAmount = (float)value / maxHealth;
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    private void FixedUpdate()
    {
        ReachPlayer(myMethode);
    }

    public void ReachPlayer(MyAttack Attack)
    {
        if(targetPlayer != null)
        {
            //distance entre l'entité ayant ce sript et notrte target
            dist = Vector2.Distance(transform.position, targetPlayer.transform.position);

            if(dist < attackRange && Time.time - lastAttackTime >= attackRate)
            {
                //L'attaque de notre enemi
                Debug.Log("J'attaque le joueur");
                Attack();
            }
            else if(dist > attackRange)
            {
                if(path == null)
                    return; 

                if(currentWayPoint >= path.vectorPath.Count)
                {
                    reachEndPath = true;
                }
                else
                {
                    reachEndPath = false;
                }

                Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
                Vector2 force = direction * speed * Time.fixedDeltaTime;

                rb.velocity = force;

                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

                if(distance < nextWaypointDistance)
                {
                    currentWayPoint++;   
                }

            }
            else
            {
                rb.velocity = Vector2.zero;
            }

        }
        
        DetectPlayer();
    }

    void DetectPlayer()
    {
        if(Time.time - playerDetectTime > playerDetectRate)
        {
            playerDetectTime = Time.time;

            foreach (PlayerController player in FindObjectsOfType<PlayerController>())
            {
                if(player != null)
                {
                    float dist = Vector2.Distance(transform.position, player.transform.position);
                    
                    if(player == targetPlayer)
                    {
                        if(dist > chaseRange)
                        {
                            targetPlayer = null;
                            rb.velocity = Vector2.zero;
                            anim.SetBool("onMove", false);
                        }
                    }
                    else if(dist < chaseRange)
                    {
                        if(targetPlayer == null)
                            targetPlayer = player;
                        anim.SetBool("onMove", true);
                    }
                    
                }
            }
        }
    }

    void UpdatePath()
    {
        if(seeker.IsDone() && targetPlayer != null)
        {
            seeker.StartPath(rb.position, targetPlayer.transform.position, OnPathComplete);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Animator anim;
        anim = GetComponent<Animator>();
        anim.SetTrigger("Hit");
        UpdateHealthBar(currentHealth);
        healthBar.SetActive(true);
        if(currentHealth <= 0)
        {
            foreach(var item in QuestManager.instance.allQuest)
            {
                if(item.statut == QuestSO.Statut.accepter)
                {
                    if(gameObject.name == item.objectToFind)
                    {
                        item.actualAmount++;
                    }
                }
            }

            if(loot.Length > 0)
            {
                int x = Random.Range(0, 1);

                if(x == 1)
                {
                    int i = Random.Range(0, loot.Length);
                    GameObject obj1 = Instantiate(loot[i], transform.position, Quaternion.identity);
                    obj1.name = loot[i].name;

                }
            }

            Destroy(gameObject);
        }
    }

    void Flip()
    {
        if(rb.velocity.x > 0 && lookRight ||  rb.velocity.x < 0 && !lookRight)
        {
            lookRight = !lookRight;
            transform.Rotate(0, 180f, 0);
        }
    }

}
