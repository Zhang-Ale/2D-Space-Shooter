using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Enemy_laser : NetworkBehaviour
{
    public float _speed = 16f;

    void FixedUpdate()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerArduino player1 = other.transform.GetComponent<PlayerArduino>();
            PlayerNormal player2 = other.transform.GetComponent<PlayerNormal>();
            //if player is no equal to null, then the player gets Damage.
            if (player1 != null)
            {
                player1.Damage();
            }

            if (player2 != null)
            {
                player2.Damage();
            }

            Destroy(this.gameObject);
        }

        if (other.tag == "Laser")
        {
            Destroy(this.gameObject);
        }
    }

}
