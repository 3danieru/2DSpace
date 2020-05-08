using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    //GamePlay--------------------------
    [SerializeField]
    private int lives = 3;
    private SpawnManager spawnManager;

    public int ammoCount = 15;
    //-- when shooting

    [SerializeField]
    public int scoreValue;

    private UIManager uiManager;
    
    //THRUSTERS
    [SerializeField]
    private GameObject[] thrusters;
    public int thrustersID;

    public float maxThrustersEnergy;
    public float currentThrustersEnergy;
    public bool hasOvercharged;

    // Sound---------------------------------

    [SerializeField]
    private AudioClip laserSound;
    private AudioSource audioSource;
    
    //MovementVariables------------------------
    public float speed;
    public float thrusterSpeed;
    float xAxis;
    float yAxis;
    public float xLimit = 11.27f;
    public float yLimit = 3.98f;


    //ShootingVariables--------------------------
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private float fireRate = 0.5f;
    private float canFire = -1f;
    private Vector3 laserOrigin;
    private float laserYposition;

    //PowerUp --------------------------------------------
    //--------Prefabs
    [SerializeField]
    private GameObject tripleLaserPrefab;
    [SerializeField]
    private GameObject shieldsPrefab;
    [SerializeField]
    private GameObject megaMissilePrefab;
    
    //--------General
    [SerializeField]
    private float powerUpTime = 5f;

    //--------SPEED MULTIPLIER
    [SerializeField]
    private float speedMultipier = 2f;

    //--------SHIELD
    [SerializeField]
    private int shieldLives = 0;



    public bool isTripleShotActive = false;
    public bool isSpeedPowerUpActive = false;
    public bool isShieldsPowerupActive = false;
    public bool isMegaShotActive = false;

    public Camera gameCamera;
    void Start()
    {
        currentThrustersEnergy = maxThrustersEnergy;
        scoreValue = 0;
        transform.position = new Vector3(0, 0, 0);
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Audio Source is NULL");
        }
        else
        {
            audioSource.clip = laserSound;
        }

        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if ( spawnManager== null)
        {
            Debug.LogError("Spawn manager is NULL");
        }

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

    }

    void Update()
    {
        Move();
        CheckLaserPosition();
        FireLaser();
        VisualManager();
        ThrustersManager();
    }

    void Move()
    {

        //Movement
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
            if (isSpeedPowerUpActive == true)
            {
                transform.Translate(new Vector3(xAxis, yAxis, 0) * speed * speedMultipier * Time.deltaTime);
            }
            else if (isSpeedPowerUpActive == false && Input.GetKey(KeyCode.LeftShift) == false)
            {
                transform.Translate(new Vector3(xAxis, yAxis, 0) * speed * Time.deltaTime);
            }
            else if (isSpeedPowerUpActive == false && Input.GetKey(KeyCode.LeftShift))
            {
                if (currentThrustersEnergy > 0)
                {
                    transform.Translate(new Vector3(xAxis, yAxis, 0) * thrusterSpeed * Time.deltaTime);
                }
            }

            


        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -yLimit, 0), 0);

        if (transform.position.x >= xLimit)
        {
            transform.position = new Vector3(-xLimit + 0.01f, transform.position.y, 0);
        }
        else if (transform.position.x <= -xLimit)
        {
            transform.position = new Vector3(xLimit - 0.01f, transform.position.y, 0);
        }
    }

    // THRUSTERS METHOD

    void ThrustersManager()
    {
    
       
        if (Input.GetKey(KeyCode.LeftShift) && hasOvercharged ==false)
        {
            currentThrustersEnergy -= Time.deltaTime;
            // when it reaches 0, cant use it again until it is full again
        }
        else
        {
            if (currentThrustersEnergy < maxThrustersEnergy)
            {
                currentThrustersEnergy = currentThrustersEnergy + (Time.deltaTime / 2);
            }
        }

        if( currentThrustersEnergy <= 0)
        {
            hasOvercharged = true;
        }
        else if ( currentThrustersEnergy >= maxThrustersEnergy)
        {
            hasOvercharged = false;
        }

    }

        //  SCORE METHODS

        public void AddScore(int points)
    {
        scoreValue += points;
        uiManager.UpdateScore(scoreValue);
    }
  

    // LIVES MANAGAMENT
    public void Damage()
    {
        gameCamera.GetComponent<Animator>().SetTrigger("camShakeTrigger");
        if ( isShieldsPowerupActive ==true)
        {
            shieldLives--;
            
            if (shieldLives <= 0)
            {
                isShieldsPowerupActive = false;
                shieldsPrefab.SetActive(false);
                return;
            }
        }
        else
        {
            lives--;
            uiManager.UpdateLives(lives);
            if( lives == 2)
            {
                thrustersID = Random.Range(0, thrusters.Length);
                thrusters[thrustersID].SetActive(true);
            }
            if (lives == 1)
            {
               if (thrustersID == 0)
                {
                    thrusters[1].SetActive(true);
                }
               else
                {
                    thrusters[0].SetActive(true);
                }
            }
            if (lives < 1)
            {
              
                spawnManager.OnPlayerDeath();
                uiManager.OnGameOver();
                Destroy(this.gameObject);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyProjectile"))
        {
            Damage();
            Destroy(collision.gameObject);
        }
    }

    //POWERUPS------------------------------------------------------------------------------------------------------
    //MEGASHOT
    public void MegaShotPowerup()
    {
        isTripleShotActive = false;
        isMegaShotActive = true;
        StartCoroutine(MegaShotPowerDownRoutine());
    }
    IEnumerator MegaShotPowerDownRoutine()
    {

        yield return new WaitForSeconds(5f);
        if (isMegaShotActive == true)
        {
            isMegaShotActive = false;
        }
        
    }


    // TRIPLE SHOT
    public void TripleShotPowerup()
    {
        isMegaShotActive = false;
        isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        
        yield return new WaitForSeconds(powerUpTime);
        if (isTripleShotActive == true)
        {
            isTripleShotActive = false;
        }
        
    }
    // SPEED POWERUP

       public  void SpeedPowerup()
    {
        Debug.Log("Speed Powerup Collected");
        isSpeedPowerUpActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {

        yield return new WaitForSeconds(powerUpTime);
        isSpeedPowerUpActive = false;
    }


    // SHIELDS POWERUP

    public void ShieldsPowerup()
    {
        isShieldsPowerupActive = true;
        shieldsPrefab.SetActive(true);
        shieldLives = 3;
        
        Debug.Log("Shields Powerup Collected");
    }

    public void AmmoPowerup()
    {
        ammoCount = 15;
    }

    // HEALTH POWERUP

        public void HealthPowerUp()
        {   
            if ( lives < 3)
            {
                lives++;
                uiManager.UpdateLives(lives);
                if (thrusters[1].activeSelf == true && thrusters[0].activeSelf == true)
                {
                    thrusters[1].SetActive(false);
                }
                else if (thrusters[1].activeSelf == true && thrusters[0].activeSelf == false)
                {
                    thrusters[1].SetActive(false);
                }
                else if (thrusters[1].activeSelf == false && thrusters[0].activeSelf == true)
                {
                    thrusters[0].SetActive(false);
                }
            } 
        }
    //GENERAL LASER LOGIC
    void CheckLaserPosition()
    {
        laserOrigin = new Vector3(transform.position.x, transform.position.y + 1.05f, 0);
    }
    void FireLaser()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > canFire && ammoCount > 0)
        {
            ammoCount--;
            if (isTripleShotActive == true )
            {
                canFire = Time.time + fireRate;
                Instantiate(tripleLaserPrefab, transform.position, Quaternion.identity);
            }
            else if ( isMegaShotActive== true)
            {
                canFire = Time.time + (2 * fireRate);
                Instantiate(megaMissilePrefab, transform.position, Quaternion.identity);
            }
            else
            {
                canFire = Time.time + fireRate;
                Instantiate(laserPrefab, laserOrigin, Quaternion.identity);
            }
            audioSource.Play();
        }

            
    }

    // VISUAL MANAGER

    void VisualManager()
    {
        if ( shieldLives == 1)
        {
            shieldsPrefab.GetComponent<SpriteRenderer>().color = Color.red ;
        }
        else if (shieldLives == 2)
        {
            shieldsPrefab.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (shieldLives == 3)
        {
            shieldsPrefab.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
            
    
   


}
