using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private AudioClip _destroyAudioClip;

    private Player _player;
    private Animator _anim;
    private AudioSource _AudioSource;
    private float _fireRate = 3.0f;
    private float _canTime = -1f;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
       
        _anim = GetComponent<Animator>();
        _AudioSource = GetComponent<AudioSource>();
       
        _AudioSource.clip = _destroyAudioClip; 
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
            GetComponent<BoxCollider2D>().enabled = false;
            _AudioSource.Play();
            Destroy(gameObject,2.8f);
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
            GetComponent<BoxCollider2D>().enabled = false;
            _AudioSource.Play();
            Destroy(gameObject,2.8f);
        }
    }
}