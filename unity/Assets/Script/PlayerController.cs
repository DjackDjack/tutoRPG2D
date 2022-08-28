using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Permet de mieux différencier nos composants
    [Header("Component")]
    Rigidbody2D rb;
    Animator anim;
    AudioSource src;

    [Header("Stat")]
    //SerializeField -> les autres scripts ne peuvent pas accéder aux variables
    [SerializeField]
    float moveSpeed;
    public int currentHealth;
    public int maxHealth;
    public int money;

    [Header("Attack")]
    private float attackTime;
    [SerializeField] float timeBetweenAttack;
    public bool canMove = true, canAttack = true;
    [SerializeField] Transform checkEnemy;
    public LayerMask whatIsEnemy;
    public float rangeAttack;
    public int damage = 1;

    [Header("SFX")]
    public AudioClip swordSound;

    public static PlayerController instance;

    private void Awake() 
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    
    {
        currentHealth = maxHealth;
        rb= GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canAttack)
        Attack();
    }

    void Attack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Time.time >= attackTime)
            {
                PlaySwordSound();
                rb.velocity = Vector2.zero;
                anim.SetTrigger("attack");

                StartCoroutine(Delay());

                IEnumerator Delay()
                {
                    canMove = false;
                    yield return new WaitForSeconds(.5f);
                    canMove= true;
                }

                attackTime = Time.time + timeBetweenAttack;
            }
        } 
    }

    private void FixedUpdate() 
    {
        if(canMove)
        {
            Move();  
        }  
    }

    void Move()
    {
        if(Input.GetAxis("Horizontal") > 0.1)
        {
            anim.SetFloat("lastInputX", 1);
            anim.SetFloat("lastInputY", 0);
        }
        else if(Input.GetAxis("Horizontal") < -0.1)
        {
            anim.SetFloat("lastInputX", -1);
            anim.SetFloat("lastInputY", 0);
        }

        if(Input.GetAxis("Vertical") > 0.1)
        {
            anim.SetFloat("lastInputX", 0);
            anim.SetFloat("lastInputY", 1);
        }
        else if(Input.GetAxis("Vertical") < -0.1)
        {
            anim.SetFloat("lastInputX", 0);
            anim.SetFloat("lastInputY", -1);
        }

        if(Input.GetAxis("Horizontal") > 0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x + rangeAttack, transform.position.y, 0);
        }
        else if(Input.GetAxis("Horizontal") < -0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x - rangeAttack, transform.position.y, 0);
        }
        else if(Input.GetAxis("Vertical") > 0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x, transform.position.y + rangeAttack, 0);
        }
        else if(Input.GetAxis("Vertical") < -0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x, transform.position.y - rangeAttack, 0);
        }

        //variables locales
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        
        rb.velocity = new Vector2(x,y) * moveSpeed * Time.fixedDeltaTime;
        //Normalize -> le vector retournera toujours un (ex: gauche= -1 et droite=1)
        rb.velocity.Normalize();

        if(x != 0 || y != 0)
        {
            anim.SetFloat("inputX", x);
            anim.SetFloat("inputY", y);
        }
    }

    public void OnAttack()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(checkEnemy.position, 0.5f, whatIsEnemy);

        foreach (var enemy_ in enemy)
        {
            //Degat à l'enemi
            enemy_.GetComponent<EnemyFly>().TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public void PlaySwordSound()
    {
        src.clip = swordSound;
        src.Play();
    }
}
