using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;
    private Animator _anim;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _destroyAudioClip;
    private AudioSource _AudioSource;
    private float _fireRate = 3.0f;
    private float _canTime = -1f;
    private GameManager _gameManager;
    private void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (!_gameManager.isCoopMode)
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            if (_player == null)
            {
                Debug.LogError("Player is null");
            }
        }
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The Animator is null");
        }
        _AudioSource = GetComponent<AudioSource>();
        if (_AudioSource == null)
        {
            Debug.LogError("The Audio Source is null");
        }
        else
        {
            _AudioSource.clip = _destroyAudioClip;
        }
    }
    private void Update()
    {
        CalculateMovement();
        if (Time.time > _canTime)
        {
            _fireRate = Random.Range(3f,7f);
            _canTime = Time.time + _fireRate;
            GameObject enemyLaser =  Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5)
        {
            float randomX = Random.Range(-8, 8);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            
            if (player != null)
            {
                player.Damage(1);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            this.GetComponent<BoxCollider2D>().enabled = false;
            _AudioSource.Play();
            Destroy(this.gameObject,2.8f);
        }
        else if (other.tag == "Player2")
        {
            Player2 player2 = other.transform.GetComponent<Player2>();

            if (player2 != null)
            {
                player2.Damage(1);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            this.GetComponent<BoxCollider2D>().enabled = false;
            _AudioSource.Play();
            Destroy(this.gameObject, 2.8f);
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            this.GetComponent<BoxCollider2D>().enabled = false;
            _AudioSource.Play();
            Destroy(this.gameObject,2.8f);
        }
    }
}