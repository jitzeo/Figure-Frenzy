using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Obsolete]

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public static int score;

    public TMP_Text timerText;
    private static float timeValue = 300;

    public TMP_Text waveText;
    private int waveNumber = 1;
    public Slider waveSlider;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public int hp = 3;

    public GameObject StressBar;
    public TMP_Text stressText;
    public static bool pressed = false;

    public GameObject gameOverMenu;
    public static bool gameOver = false;
    public TMP_Text finalScoreText;

    public GameObject gameOverEndMenu;
    public TMP_Text finalScoreEndText;
    public static bool test = false;
    public TMP_Text endButtonText;

    public GameObject damageScreen;
    public static bool damage = false;
    public static float damageTime = 0;


    void Start()
    {
        scoreText.text = "Score:  " + 0;
        score = 0;

        waveSlider.value = 0f;
        waveSlider.maxValue = 1.0f;

        heart1.SetActive(true);
        heart2.SetActive(true);
        heart3.SetActive(true);
    }

    void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;
            GameOver();
        }
        DisplayTime(timeValue);

        stressBar();
        if (gameOver == false && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        if (damage && (Time.time - damageTime) >= 0.75f)
        {
           damageScreen.SetActive(false);
           damage = false;
        }
    }

    public void GameOver()
    {
        if (timeValue != 0)
        {
            gameOverMenu.SetActive(true);
            finalScoreText.text = "Final Score: " + score.ToString();
        } 
        else
        {
            gameOverMenu.SetActive(false);
            gameOverEndMenu.SetActive(true);
            finalScoreEndText.text = "Final Score: " + score.ToString();
            DataStorage.gameEnd = true;
            if (test)
            {
                endButtonText.text = "Continue questionnaire";
            } else
            {
                endButtonText.text = "Go back to main menu";
            }
}
        gameOver = true;
        //Time.timeScale = 0f;
    }

    public void StartOver()
    {
        gameOver = false;
        DataStorage.hits = 0;
        DataStorage.shots = 0;
        //Time.timeScale = 1f;
        Application.LoadLevel(Application.loadedLevel);
    }

    public void AddScore(int killScore)
    {
        score += killScore;
        scoreText.text = "Score:  " + score.ToString();
    }

    public void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void updateWave(int enemyCount, int waveSize)
    {
        float sliderValue = waveSlider.maxValue - waveSlider.maxValue * enemyCount / waveSize;
        waveSlider.value = sliderValue;
    }

    public void AddWave()
    {
        waveNumber++;
        waveText.text = "Wave " + waveNumber;

        waveSlider.value = 0f;
    }

    public void hitPoints(int h)
    {
        if (hp <= 3 && hp > 0)
        {
            hp += h;
            if (hp > 3)
            {
                hp = 3;
            }
        }

        switch (hp)
        {
            case 0:
                GameOver();
                heart1.SetActive(false);
                break;
            case 1:
                heart1.SetActive(true);
                heart2.SetActive(false);
                break;
            case 2:
                heart2.SetActive(true);
                heart3.SetActive(false);
                break;
            case 3:
                heart3.SetActive(true);
                break;

        }
    }

    public void stressBar()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StressBar.GetComponent<Image>().color = new Color(0, 0, 0, 0.6f);
            stressText.color = new Color(255, 255, 225, 0.6f);

            pressed = true;
        }
        else if (!Input.GetKey(KeyCode.Space) && pressed == true)
        {
            StressBar.GetComponent<Image>().color = new Color(0, 0, 0, 0.2f);
            stressText.color = new Color(255, 255, 225, 0.2f);

            pressed = false;
        }


    }
}
