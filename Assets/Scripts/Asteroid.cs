using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float rotateSpeed= 5f;
    public GameObject explosion;
    public Collider2D collider2d;
    public SpawnManager spawnManager;

    // Sound
    [SerializeField]
    private AudioClip explosionSound;
    

    private void Start()
    {
        collider2d = GetComponent<CircleCollider2D>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotateSpeed) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("MegaMissile"))
        {
            AudioManager.Instance.PlaySFXsound(explosionSound);
            GameObject explosionMade = Instantiate(explosion, transform.position, Quaternion.identity);
            spawnManager.StartSpawning();
            Destroy(collision.gameObject);
            collider2d.enabled = false;
            Destroy(explosionMade, 3f);
            Destroy(this.gameObject, 1f);

        }
    }
   
}
