using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemySpeed = 4f;
    
    private Player player;
    private Collider2D enemyCollider;
    //Shooting
    private float secondsBetweenShots;
    private bool isDead;
    [SerializeField]
    private GameObject enemyLaser;
    //Animation
    private Animator animator;
    // Sound
    public AudioClip explosion;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        enemyCollider = gameObject.GetComponent<Collider2D>();
        animator = gameObject.GetComponent<Animator>();

        //Coroutine loops
        isDead = false;
        EnemyShoot();

    }
    void Update()
    {
        Movement();
    }
    void Movement()
    {
        transform.Translate(Vector3.down * enemySpeed * Time.deltaTime);

        if (transform.position.y <= -5.55f)
        {
            transform.position = new Vector3(Random.Range(-9.45f, 9.45f), 7.7f, 0);
        }
    }
    //Enemy Shooting
    public void EnemyShoot()
    {
        StartCoroutine(EnemyShootCoroutine());
    }

    // THIS CAN BE HEAVILY IMPROVED
    public IEnumerator EnemyShootCoroutine()
    {
        while (true)
        {
                secondsBetweenShots =Random.Range(2f,5f);
                yield return new WaitForSeconds(secondsBetweenShots);
                if (isDead ==  false)
                {
                    Instantiate(enemyLaser, transform.position + new Vector3(0, -1), Quaternion.identity);
                }
        }
    }


    //COLLISION
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            
            
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            isDead = true; //Marks death of enemy to stop shooting
            enemySpeed = 0.5f;
            enemyCollider.enabled = false;
            animator.SetTrigger("OnEnemyDeath");
            AudioManager.Instance.PlaySFXsound(explosion);
            Destroy(this.gameObject,5f);

        }

        if (other.gameObject.CompareTag("Projectile"))

        {
            if (player != null)
            {
                player.AddScore(Random.Range(5,11));
                
            }
            isDead = true; //Marks death of enemy to stop shooting
            enemySpeed = 0.5f;
            enemyCollider.enabled = false;
            animator.SetTrigger("OnEnemyDeath");
            AudioManager.Instance.PlaySFXsound(explosion);
            Destroy(other.gameObject);
            Destroy(this.gameObject,5f);
        }

        if (other.gameObject.CompareTag("MegaMissile"))

        {
            if (player != null)
            {
                player.AddScore(Random.Range(5, 11));

            }
            isDead = true; //Marks death of enemy to stop shooting
            enemySpeed = 0.5f;
            enemyCollider.enabled = false;
            animator.SetTrigger("OnEnemyDeath");
            AudioManager.Instance.PlaySFXsound(explosion);
            other.gameObject.GetComponent<MegaMissile>().missileLives--;
            Destroy(this.gameObject, 5f);
        }
    }
}
