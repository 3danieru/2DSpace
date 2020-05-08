using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    //ID for Powerups
    //0 for TripleShot
    // 1 = speed
    // 2 = Shields
    // 3 = AmmoCount
    [SerializeField]
    private int powerUpID;
   
    [SerializeField]
    private float pwupSpeed = 3f;

    public AudioClip powerUpSound;
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    void Movement()
    {
        transform.Translate(Vector3.down * pwupSpeed * Time.deltaTime);

        if (transform.position.y <= -5.55f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX2sound(powerUpSound);
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {

                switch (powerUpID)
                {
                    case 0:
                        player.TripleShotPowerup();
                        break;
                    case 1:
                        player.SpeedPowerup();
                        break;
                    case 2:
                        player.ShieldsPowerup();
                        break;
                    case 3:
                        player.AmmoPowerup();
                        break;
                    case 4:
                        player.HealthPowerUp();
                        break;
                    case 5:
                        player.MegaShotPowerup();
                        break;


                    default:
                        Debug.Log("Default Value");
                        break;
                }

                Destroy(this.gameObject);

            }
        }
    }
}
