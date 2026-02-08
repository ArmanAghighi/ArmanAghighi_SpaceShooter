using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _bestScoreText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private GameManager _gameManager;

    private WaitForSeconds _gameOverTimeCounter = new WaitForSeconds(0.5f);
    
    void Awake()
    {
        if(Instance == null) Instance = this;
        else {Destroy(this); Instance = this;}
    }

    private void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _scoreText.text = "score :" + 0;
        _bestScoreText.text = "Best : " + PlayerPrefs.GetInt("BestScore").ToString();
    }

    public void UpdateScore(int playerScore) => _scoreText.text = "Score : " + playerScore.ToString();
    public void UpdateBestScore(int playerBestScore) => _bestScoreText.text = "Score : " + playerBestScore.ToString();

    public void UpdateLives(int currentLives)
    {
        if(currentLives < 0)
            _livesImage.sprite = _livesSprites[0];
        else
        _livesImage.sprite = _livesSprites[currentLives];
        if (currentLives <= 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        GameManager.Instance.OnGameStart?.Invoke(false);
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "Game Over";
            yield return _gameOverTimeCounter;
            _gameOverText.text = "";
            yield return _gameOverTimeCounter;
        }
    }

    public void ResumePlay() => GameManager.Instance.OnGamePause?.Invoke(false);
    
    public void BackToMainMenu() => SceneManager.LoadScene(0);
    
}