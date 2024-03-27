using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float _speed = 3f;
    public int _buffID;
    GameTime gametime;
    private void Start()
    {
        gametime = GameObject.Find("TimeManager").GetComponent<GameTime>();
    }
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
        if (gametime.gameOver == true)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerArduino player1 = other.transform.GetComponent<PlayerArduino>();
            if (player1 != null)
            {
                switch (_buffID)
                {
                    case 0:
                        player1.TripleShotActive();
                        break;
                    case 1:
                        player1.Shield();
                        break;
                    case 2:
                        break;
                    default:
                        break;
                }
                Destroy(this.gameObject);
            }

            PlayerNormal player2 = other.transform.GetComponent<PlayerNormal>();
            if (player2 != null)
            {
                switch (_buffID)
                {
                    case 3:
                        player2.tripleShotActive();
                        break;
                    case 4:
                        player2.shield();
                        break;
                    case 5:
                        break;
                    default:
                        break;
                }

                Destroy(this.gameObject);
            }
        }
    }
}
