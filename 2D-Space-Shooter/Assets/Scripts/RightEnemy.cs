using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class RightEnemy : NetworkBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    [SyncVar]
    public int _lives = 3;

    public GameObject _EnemyLaserPrefab;

    public float _Firerate = 0.15f;
    public float _Canfire;
    private Animator _anim;
    Score _score;
    GameTime gametime;
    private void Start()
    {
        _score = GameObject.Find("ScoreManager").GetComponent<Score>();
        _anim = GetComponent<Animator>();
        gametime = GameObject.Find("TimeManager").GetComponent<GameTime>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (_lives > 0 && Time.time > _Canfire)
        {
            StartCoroutine(Attack(3));
        }

        /*_Canfire -= Time.deltaTime;
        if (_lives > 0 && _Canfire <= 0f)
        {
            Instantiate(_EnemyLaserPrefab, transform.position + new Vector3(0, -2f, 0), Quaternion.identity);
            _Canfire = 2;
        }*/

        if (transform.position.y < -5.7f)
        {
            float rightRandomX = Random.Range(11.5f, 22f);
            transform.position = new Vector3(rightRandomX, 5.6f, 0);
        }
        if(gametime.gameOver == true)
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Attack(float _waitTime)
    {
        _Canfire = Time.time + _Firerate;
        Instantiate(_EnemyLaserPrefab, transform.position + new Vector3(0, -0.4f, 0), Quaternion.identity);
        yield return new WaitForSeconds(_waitTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerNormal player2 = other.transform.GetComponent<PlayerNormal>();
            
            //if player is no equal to null, then the player gets Damage.
            if (player2 != null)
            {
                player2.Damage();
            }

            //This is the same thing as: 'other.transform.GetComponent<Player>().Damage();'
            //The difference is that if there's no script component of Player.cs on the player, the damage can still be applied.
            //Whereas 'other.transform.GetComponent<Player>().Damage();' applies damage only if the Player.cs component exists.
            //In this case, 'other.'can only be added to 'transform', making it become 'other.transform'.
            //So everytime the trigger activates, the void Damage in the Player.cs will be activated.
            //I can also type 'other.transform.GetComponent<MeshRenderer>().material.color;'to get the color of 'other'.
            _anim.SetTrigger("OnExplode");
            _speed = 0;
            StopCoroutine(Attack(3));
            Destroy(this.gameObject, 2.5f);         
        }

        if (other.tag == "Laser")
        {
            _lives -= 1;
            Destroy(other.gameObject);

            if (_lives < 1)
            {
                _score.RightScored();               
                _anim.SetTrigger("OnExplode");
                _speed = 0;
                StopCoroutine(Attack(3));
                Destroy(this.gameObject, 2.5f);
            }
        }
        if (other.tag == "TripleShot")
        {
            _lives -= 3;
            Destroy(other.gameObject);

            if (_lives < 1)
            {
                _score.RightScored();                
                _anim.SetTrigger("OnExplode");
                _speed = 0;
                StopCoroutine(Attack(3));
                Destroy(this.gameObject, 2.5f);
            }
        }
    }
}
