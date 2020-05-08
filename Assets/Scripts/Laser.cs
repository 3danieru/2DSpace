using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float speed = 8f;
    private Vector3 direction;
        void Start()
    {
        direction = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((direction) * speed * Time.deltaTime);

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
