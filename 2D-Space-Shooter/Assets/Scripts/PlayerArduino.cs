using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports; //IO = inputs outputs
using System.Threading;
using Mirror;
using TMPro;

public class PlayerArduino : NetworkBehaviour
{
    public float _speed = 3.5f;
    public GameObject _laserPrefab;
    public GameObject _tripleShot;
    public float _fireRate = 0.5f;
    public float _canFire = -1f;

    [SyncVar]
    public int _maxLives = 50;
    public int currentLives;

    private SpawnManager _spawnManager;
    private GameTime _time;
    private Transform _leftMask;
    private TextMeshProUGUI leftHealthText;

    public bool _isTripleShotActive = false;
    public Transform shootPosition;
    public bool _isShieldActive = false;
    public GameObject shieldVis;
   
    SerialPort arduinoSerialPort;
    Thread arduinoThread;
    public float PotValue;
    public bool ButtonValue;
    string currentValueString = "";

    private Vector2 initialPosition;
    public GameObject _leftFire, _rightFire;
    GameTime gametime;
    void Start()
    {
        InitializeSerialPort();
        arduinoThread = new Thread(ArduinoSerialRead);
        arduinoThread.Start();
        currentLives = _maxLives;
        _leftMask = GameObject.Find("LeftMask").GetComponent<Transform>();
        _time = GameObject.Find("TimeManager").GetComponent<GameTime>();
        leftHealthText = GameObject.Find("LeftHPFloat").GetComponent<TextMeshProUGUI>();
        this.initialPosition = this.transform.position;
        //transform.position = new Vector3(5f, -4, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        gametime = GameObject.Find("TimeManager").GetComponent<GameTime>();
    }

    void FixedUpdate()
    {
        if (this.isLocalPlayer)
        {
            if (currentValueString.Contains("Data"))
            {
                string[] serialContent = currentValueString.Split('/');
                PotValue = float.Parse(serialContent[1]);
                ButtonValue = serialContent[2] == "1" ? true : false;

                currentValueString = "";
            }

            leftHealthText.text = "HP: " + currentLives.ToString() + "/" + _maxLives.ToString();
            
            CalculateMovement();

            if (/*Input.GetKey(KeyCode.Space)*/ ButtonValue && Time.time > _canFire)
            {
                CmdFireLaser();
            }
            if (gametime.gameOver == true)
            {
                Destroy(this.gameObject);
            }
        }     
    }

    void InitializeSerialPort()
    {
        arduinoSerialPort = new SerialPort("COM5", 9600);
        arduinoSerialPort.Open();
    }

    void ArduinoSerialRead()
    {
        while (true)
        {
            currentValueString = arduinoSerialPort.ReadLine();
        }
    }

    void CalculateMovement()
    {
        transform.position = new Vector3(PotValue/102, -4, 0);
    }

    //[Command]
    void CmdFireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            GameObject tripleBullet = Instantiate(_tripleShot, shootPosition.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            tripleBullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * 8f;            
            NetworkServer.Spawn(tripleBullet);
        }
        else
        {
            GameObject bullet = Instantiate(_laserPrefab, shootPosition.transform.position + new Vector3(0, 0.05f, 0), Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * 8f;
            NetworkServer.Spawn(bullet);
        }
    }

    public void Damage()
    {
        if (this.isServer)
        {
            if (_isShieldActive == true)
            {
                Shield();
                return;
            }

            //_lives--1;
            //_lives = _lives-1;
            currentLives -= 1;
            _leftMask.Translate(-0.029f, 0.0f, 0.0f, Space.World);

            if(currentLives <= 30)
            {
                _leftFire.SetActive(true);
            }    
            if(currentLives <= 10)
            {
                _rightFire.SetActive(true);
            }
            if (currentLives < 1)
            {
                Destroy(this.gameObject);
                LeftRpcRespawn();
            }

            if (_time.gameOver)
            {
                Destroy(this.gameObject);
                _spawnManager.OnLeftPlayerDeath();
                arduinoSerialPort.Close();
                arduinoThread.Abort();
            }
        }     
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }
    public void Shield()
    {
        _isShieldActive = true;
        shieldVis.SetActive(true);
        StartCoroutine(ShieldsActive());
    }
    IEnumerator ShieldsActive()
    {
        yield return new WaitForSeconds(5f);
        _isShieldActive = false;
        shieldVis.SetActive(false);
    }

    [ClientRpc]
    void LeftRpcRespawn()
    {
        this.transform.position = this.initialPosition;
    }

    private void OnDisable() //when the game is disabled or destroyed, when you exit the game, when I'm not in Play mode
    {
        if (arduinoSerialPort != null && arduinoSerialPort.IsOpen)
        {
            arduinoSerialPort.Close();
        }
        if (arduinoThread != null && arduinoThread.IsAlive)
        {
            arduinoThread.Abort();
        }
    }
}
