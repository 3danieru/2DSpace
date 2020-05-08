using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaMissile : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;
    [SerializeField]
    private float increaseSizeSpeed = 0.01f;
    public int missileLives = 3;
    private Vector3 direction;
   
    void Start()
    {
        direction = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate((direction) * speed * Time.deltaTime);

        if (missileLives == 0)
        {
            Destroy(gameObject);
        }
        if (transform.position.y > 8)
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
