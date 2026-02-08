using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 4f;

    [Header("Shooting")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private float _fireRate = 0.15f;
    [SerializeField] private AudioClip _laserAudioClip;

    [Header("Player State")]
    [SerializeField] private int _lives = 3;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _rightEngine, _leftEngine;

    [Header("Touch Settings")]
    [SerializeField] private float _tapTimeThreshold = 0.2f;
    [SerializeField] private float _dragDistanceThreshold = 20f;

    private float _canFire = -1f;
    private float _speedMultiplier = 2f;

    private bool _isTripleShotActive;
    private bool _isSpeedBoostActive;
    private bool _isShieldActive;

    private int _score;
    private int _bestScore;

    private float _touchStartTime;
    private Vector2 _touchStartPos;
    private bool _isDragging;

    private UIManager _uiManager;
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;

    void Start()
    {
        _spawnManager = SpawnManager.Instance;
        _uiManager = UIManager.Instance;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _laserAudioClip;

        _bestScore = PlayerPrefs.GetInt("BestScore");
        transform.position = new Vector3(0, -4, 0);
    }

    void Update()
    {
        if (Input.touchCount > 0)
            HandleTouchInput();
        else
            HandleKeyboardInput();
    }

    #region TOUCH

    void HandleTouchInput()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                _touchStartTime = Time.time;
                _touchStartPos = touch.position;
                _isDragging = false;
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                if (Vector2.Distance(touch.position, _touchStartPos) > _dragDistanceThreshold)
                    _isDragging = true;

                if (_isDragging)
                    MoveTowardFinger(touch.position);
                break;

            case TouchPhase.Ended:
                float duration = Time.time - _touchStartTime;

                if (!_isDragging && duration <= _tapTimeThreshold && Time.time > _canFire)
                    FireLaser();

                break;
        }
    }

    void MoveTowardFinger(Vector2 screenPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, Mathf.Abs(Camera.main.transform.position.z))
        );

        Vector3 target = new Vector3(worldPos.x, transform.position.y, 0);

        float currentSpeed = _isSpeedBoostActive ? _speed * _speedMultiplier : _speed;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            currentSpeed * Time.deltaTime
        );

        ClampAndWrap();
    }

    #endregion

    #region KEYBOARD

    void HandleKeyboardInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, v, 0);
        float currentSpeed = _isSpeedBoostActive ? _speed * _speedMultiplier : _speed;

        transform.Translate(direction * currentSpeed * Time.deltaTime);
        ClampAndWrap();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            FireLaser();
    }

    #endregion

    void ClampAndWrap()
    {
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, -5f, 5f),
            0
        );

        if (transform.position.x >= 4f)
            transform.position = new Vector3(-4f, transform.position.y, 0);
        else if (transform.position.x <= -4f)
            transform.position = new Vector3(4f, transform.position.y, 0);
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        GameObject prefab = _isTripleShotActive ? _tripleShotPrefab : _laserPrefab;
        Instantiate(prefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);

        _audioSource.Play();
    }

    public void Damage(int amount)
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives -= amount;

        if (_lives == 2) _leftEngine.SetActive(true);
        else if (_lives == 1) _rightEngine.SetActive(true);

        _uiManager.UpdateLives(_lives);

        if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
        }
    }

    public void TripleAShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedBoostActive = false;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;

        if (_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("BestScore", _bestScore);
        }

        _uiManager.UpdateScore(_score);
        _uiManager.UpdateBestScore(_bestScore);
    }
}
