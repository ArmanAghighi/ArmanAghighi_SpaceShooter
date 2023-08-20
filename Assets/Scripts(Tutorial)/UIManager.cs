using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _bestScoreText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private GameManager _gameManager;
    private void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        if (!_gameManager.isCoopMode)
            _scoreText.text = "score :" + 0;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager Is null");
        }
        if(!_gameManager.isCoopMode)
            _bestScoreText.text = ("Best : " + PlayerPrefs.GetInt("BestScore").ToString());
    }
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score : " + playerScore.ToString();
    }
    public void UpdateBestScore(int playerBestScore)
    {
        _bestScoreText.text = "Score : " + playerBestScore.ToString();
    }
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
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "Game Over";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void ResumePlay()
    {
        GameManager gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        gameManager.ResumeGame();
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}