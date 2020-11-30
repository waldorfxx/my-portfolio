using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public GameObject canvas;
    private int interval=1;
    private Text _scoretext;
    private Text _highscoretext;
    private GameController gameManager;
    private ScoreController scoreManager;
    private CoinsController coinsManager;
    private TimeController timeManager;
    private int timer;

    void Start()
    {
        canvas.gameObject.SetActive(false);
        _scoretext = canvas.transform.Find("Menu/_score").GetComponent<Text>();
        _highscoretext = canvas.transform.Find("Menu/_highscore").GetComponent<Text>();
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartTimer()
    {
        LogFileManager.countGame=1;
        canvas.gameObject.SetActive(false);
        StopAllCoroutines();
        StartCoroutine("ie_timer");
    }

    IEnumerator ie_timer()
    {
        yield return new WaitForSeconds(15);
        PlayerController.time += 15;
        
        yield return new WaitForSeconds(15);
        PlayerController.time += 15;

        yield return new WaitForSeconds(15);
        PlayerController.time += 15;

        yield return new WaitForSeconds(15);
        PlayerController.time += 15;

        yield return new WaitForSeconds(15);
        PlayerController.time += 15;

        yield return new WaitForSeconds(15);
        PlayerController.time += 15;

        yield return new WaitForSeconds(15);
        PlayerController.time += 15;

        yield return new WaitForSeconds(15);
        PlayerController.time += 15;

        yield return new WaitForSeconds(interval);

        gameManager = FindObjectOfType<GameController>();
        scoreManager = FindObjectOfType<ScoreController>();
        coinsManager = FindObjectOfType<CoinsController>();
        timeManager = FindObjectOfType<TimeController>();

        if (gameManager == null)
            yield break;

        timer = PlayerPrefs.GetInt("Timer");
        if (timer > 0)
        {
            timeManager.SetIsDead(true);
            timeManager.FixPreviousTime();
        }

        GameObject player = GameObject.Find("Player");
        if (player != null)
            player.gameObject.SetActive(false);
        DisplayScoring();
    }

    public void StopTimer()
    {
        canvas.gameObject.SetActive(false);
        StopAllCoroutines();
    }
        
    private void DisplayScoring()
    {
        canvas.gameObject.SetActive(true);
        int score = scoreManager.GetScore();
        LogFileManager.totalScore = score + coinsManager.CoinsScoring();

        if (PlayerPrefs.GetInt("BodyMode") == 1)
        {
        }
        else
        {
            _scoretext.text = System.String.Format("Distance : {0}m\nCoins : {1}\nMultiplier : {2}x\nTotal Score : {3}", score, coinsManager.GetCoinsNumber(), coinsManager.GetCoinValue(), LogFileManager.totalScore);
        }

        int highScore = scoreManager.GetHighScore();
        if (LogFileManager.totalScore > highScore)
        {
            UpdateHighScore(LogFileManager.totalScore);
        }

        Time.timeScale = 0;
    }

    private void SaveLog()
    {
        LogFileManager.LogFile();
        LogFileManager.ResetValue();
        LogFileManager.countGame++;
    }

    /** 
     *  Updathe the highScore value
     */
    private void UpdateHighScore(int newHighScore)
    {
        PlayerPrefs.SetInt("HighScore", newHighScore);
        _highscoretext.enabled = true;
    }

    /** 
     *  Restart the game by reset
     */
    public void Restart()
    {
        SaveLog();
        gameManager.Reset();
        StopTimer();
        Time.timeScale = 1.0f;
    }

    /** 
     *  Return to the main menu
     */
    public void BackToMainMenu()
    {
        SaveLog();
        gameManager.BackToMainMenu();
        StopTimer();
        Time.timeScale = 1.0f;
    }

    /** 
     *  Exit the game
     */
    public void Exit()
    {
        gameManager.ExitGame();
        StopTimer();
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        // print("ts: " + Time.timeScale);
    }
}
