using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Laser : NetworkBehaviour
{
    [SerializeField]
    private float _velocity = 8f;

    void FixedUpdate()
    {        
        transform.Translate(Vector3.up * _velocity * Time.deltaTime);

        if (transform.position.y >= 7.5f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
