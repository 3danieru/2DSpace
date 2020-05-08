using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float speed = 6f;
    private Vector3 direction;
    void Start()
    {
        direction = Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((direction) * speed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }

        }
    }

}
