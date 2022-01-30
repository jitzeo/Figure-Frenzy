using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public static float sliderOption;
    public Slider speedSlider;
    public TMP_Text speedSliderValue;
    private float[] speedValues = new float[] { 0.5f, 0.75f, 1.0f, 1.25f, 1.5f, 1.75f, 2f };

    public Slider spawnRateSlider;
    public TMP_Text spawnRateSliderValue;

    public Slider shotRateSlider;
    public TMP_Text shotRateSliderValue;

    public void SpeedSlider()
    {
        float v = speedSlider.value;
        sliderOption = speedValues[(int)v];
        speedSliderValue.text = sliderOption.ToString("0.00x");
        PlayerMovement.speedMultiplier = sliderOption;
    }

    public void SpawnRateSlider()
    {
        float v = spawnRateSlider.value;
        spawnRateSliderValue.text = v.ToString("0.00x");
        SpawnEnemies._enemyCountUpdate = (int) v;
    }

    public void ShotRateSlider()
    {
        float v = shotRateSlider.value;
        sliderOption = speedValues[(int)v];
        shotRateSliderValue.text = sliderOption.ToString("0.00x");
        SpawnEnemies.shotIntervalMultiplier = sliderOption;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
