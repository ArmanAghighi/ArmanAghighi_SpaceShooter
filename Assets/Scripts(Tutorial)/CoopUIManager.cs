using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CoopUIManager : MonoBehaviour
{
    [SerializeField]
    private Image _livesImageBlue;
    [SerializeField]
    private Sprite[] _livesSpritesBlue;
    [SerializeField]
    private Image _livesImageRed;
    [SerializeField]
    private Sprite[] _livesSpritesRed;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager Is null");
        }
    }

    public void UpdateLivesBlue(int currentLives)
    {
        if (currentLives < 0)
            _livesImageBlue.sprite = _livesSpritesBlue[0];
        else
            _livesImageBlue.sprite = _livesSpritesBlue[currentLives];
        if (currentLives <= 0)
        {
            GameOverSequence();
        }
    }
    public void UpdateLivesRed(int currentLives)
    {
        if (currentLives < 0)
            _livesImageRed.sprite = _livesSpritesRed[0];
        else
            _livesImageRed.sprite = _livesSpritesRed[currentLives];
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
        if (Input.GetKeyDown(KeyCode.Escape))
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