using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Action<bool> OnGameStart;
    public Action<bool> OnGamePause;
    public Action<bool> OnGameOver;
    public bool _isGameOver = false;
    [SerializeField] private GameObject _pauseMenuPanel;
    
    private Animator _pauseAnimator;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else { Destroy(this); Instance = this;}

        OnGameStart += HandleGameStart;
        OnGamePause += OnGamePauseSequence;
    }

    private void Start()
    {
        _pauseAnimator = _pauseMenuPanel.GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void HandleGameStart(bool isOnGameOver) => StartCoroutine(OnGameOverSequence(isOnGameOver));

    private IEnumerator OnGameOverSequence(bool isOnGameOver)
    {
        if (!isOnGameOver)
        {
            OnGameOver?.Invoke(isOnGameOver);
            yield return new WaitUntil(() =>
                Input.touchCount > 0 || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.R)
            );

            SceneManager.LoadScene(1);
        }
    }

    public void PauseGame() => OnGamePause?.Invoke(true);

    private void OnGamePauseSequence(bool isOnGamePause)
    {
        if(isOnGamePause)
        {
            _pauseMenuPanel.SetActive(true);
            _pauseAnimator.SetBool("isPaused", true);
            Time.timeScale = 0;
        }
        else
        {
            _pauseMenuPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}