using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerNormal : NetworkBehaviour
{
    public bool isThisSingle; 
    public float _speed = 3.5f;
    public GameObject _laserPrefab;
    public GameObject _tripleShot;
    public float _fireRate = 0.5f;
    public float _canFire = -1f;
    public bool _IsShieldActive = false;
    public GameObject ShieldVis;

    [SyncVar]
    public int _MaxLives = 50;
    public int CurrentLives;

    private SpawnManager _spawnManager;
    private GameTime _time;
    private Transform _rightMask;
    public TextMeshProUGUI rightHealthText;

    public bool _IsTripleShotActive = false;
    public Transform shootPosition;

    private Vector2 initialPosition;
    public GameObject _LeftFire, _RightFire;
    GameTime gametime;

    void Start()
    {
        CurrentLives = _MaxLives;
        _rightMask = GameObject.Find("RightMask").GetComponent<Transform>();
        _time = GameObject.Find("TimeManager").GetComponent<GameTime>();
        rightHealthText = GameObject.Find("RightHPFloat").GetComponent<TextMeshProUGUI>();
        this.initialPosition = this.transform.position;
        //transform.position = new Vector3(17, -4, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        gametime = GameObject.Find("TimeManager").GetComponent<GameTime>();
    }

    void Update()
    {
        rightHealthText.text = "HP: " + CurrentLives.ToString() + "/" + _MaxLives.ToString();

        if (isThisSingle)
        {
            CalculateMovement(); 
        }
        else
        {
            CalculateNormalMovement();
        }

        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            CmdFireLaser();
        }
        if (gametime.gameOver == true)
        {
            Destroy(this.gameObject);
        }
    }

    void CalculateMovement()
    {
        //transform.Translate(Vector3.right * 3 * Time.deltaTime); This will let the player go towards right at the start of game.

        float Inputhorizontal = Input.GetAxis("Horizontal");
        //float Inputvertical = Input.GetAxis("Vertical");

        //transform.Translate(Vector3.up * Inputvertical * _speed * Time.deltaTime);
        //transform.Translate(new Vector3(1, 0, 0) * Inputhorizontal * _speed * Time.deltaTime);

        //transform.Translate(new Vector3(0, 1, 0) * Inputvertical * _speed * Time.deltaTime);
        //transform.Translate(Vector3.right * Inputhorizontal * _speed * Time.deltaTime);

        //transform.Translate(new Vector3(Inputhorizontal, Inputvertical, 0) * _speed * Time.deltaTime);

        /*Vector3 direction = new Vector3(Inputhorizontal, Inputvertical, 0);
        transform.Translate(direction * _speed * Time.deltaTime);*/

        GetComponent<Rigidbody2D>().velocity = new Vector2(Inputhorizontal * _speed, 0.0f);

        /*This part:
        if (transform.position.y >= 5.9f)
        {
            transform.position = new Vector3(transform.position.x, 5.9f, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }
        is the same as: (the code below)*/

        if (transform.position.x >= 20.8f)
        {
            transform.position = new Vector3(1.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= 1.3f)
        {
            transform.position = new Vector3(20.8f, transform.position.y, 0);
        }
    }
    void CalculateNormalMovement()
    {
        float Inputhorizontal = Input.GetAxis("Horizontal");

        GetComponent<Rigidbody2D>().velocity = new Vector2(Inputhorizontal * _speed, 0.0f);

        // To connect between screen boundaries
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 11.6f, 19.9f), transform.position.y, 0);
    }

    void CmdFireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_IsTripleShotActive == true)
        {
            GameObject tripleBullet = Instantiate(_tripleShot, shootPosition.transform.position + new Vector3(0, 0.85f, 0), Quaternion.identity);
            tripleBullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * 8f;
            NetworkServer.Spawn(tripleBullet);
        }
        else
        {
            GameObject bullet = Instantiate(_laserPrefab, shootPosition.transform.position + new Vector3(0, 0.85f, 0), Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * 8f;
            NetworkServer.Spawn(bullet);
        }
    }
    public void Damage()
    {
        if (this.isServer)
        {
            if (_IsShieldActive == true)
            {
                shield();
                return;
            }
            //_lives--1;
            //_lives = _lives-1;
            CurrentLives -= 1;
            _rightMask.Translate(-0.029f, 0.0f, 0.0f, Space.World);
            if (CurrentLives <= 30)
            {
                _LeftFire.SetActive(true);
            }
            if (CurrentLives <= 10)
            {
                _RightFire.SetActive(true);
            }
            if (CurrentLives < 1)
            {
                Destroy(this.gameObject);
                RightRpcRespawn();
            }

            if (_time.gameOver)
            {
                Destroy(this.gameObject);
                _spawnManager.OnRightPlayerDeath();
            }
        }
            
    }

    public void tripleShotActive()
    {
        _IsTripleShotActive = true;
        Debug.Log("Aha");
        StartCoroutine(tripleShotPowerDownRoutine());
    }

    IEnumerator tripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _IsTripleShotActive = false;
    }
    public void shield()
    {
        _IsShieldActive = true;
        Debug.Log("Yes");
        ShieldVis.SetActive(true);
        StartCoroutine(shieldsActive());
    }
    IEnumerator shieldsActive()
    {
        yield return new WaitForSeconds(5f);
        _IsShieldActive = false;
        ShieldVis.SetActive(false);
    }

    //[ClientRpc]
    void RightRpcRespawn()
    {
        this.transform.position = this.initialPosition;
    }
}
