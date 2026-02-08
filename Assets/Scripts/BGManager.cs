using UnityEngine;
using System.Collections.Generic;

public class BGManager : MonoBehaviour
{
    [Header("BG Settings")]
    [SerializeField] private GameObject _bgPrefab;
    [SerializeField] private Transform _player;
    [SerializeField] private int _poolSize = 5;
    [SerializeField] private float _scrollSpeed = 2f;

    private List<GameObject> _bgPool = new List<GameObject>();
    private float _bgLength = 38.4f;
    private float _nextSpawnY;
    private bool _isGameOver = false;

    void Start()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject bg = Instantiate(_bgPrefab, Vector3.zero, Quaternion.identity, transform);
            bg.SetActive(false);
            _bgPool.Add(bg);
        }

        _nextSpawnY = 0f;
        for (int i = 0; i < 3; i++)
        {
            SpawnBG();
        }
        GameManager.Instance.OnGameOver += SetGameOverValue; 
    }

    void Update()
    {
        if (_isGameOver || _player == null)
        return;
        
        foreach (GameObject bg in _bgPool)
        {
            if (bg.activeSelf)
            {
                bg.transform.Translate(Vector3.down * _scrollSpeed * Time.deltaTime);

                if (bg.transform.position.y < _player.position.y - _bgLength * 2)
                {
                    bg.SetActive(false);
                }
            }
        }
        if (_player.position.y >= _nextSpawnY - (_bgLength * 2))
        {
            SpawnBG();
        }
    }

    private void SpawnBG()
    {
        GameObject bg = GetInactiveBG();
        if (bg != null)
        {
            bg.transform.position = new Vector3(0, _nextSpawnY, 0);
            bg.SetActive(true);
            _nextSpawnY += _bgLength;
        }
    }

    private GameObject GetInactiveBG()
    {
        foreach (GameObject bg in _bgPool)
        {
            if (!bg.activeSelf)
                return bg;
        }
        return null;
    }

    private void SetGameOverValue(bool gameOver) => _isGameOver = gameOver;
}
