using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TripleShot : NetworkBehaviour
{
    void FixedUpdate()
    {
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
