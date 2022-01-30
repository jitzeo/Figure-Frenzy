using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using TMPro;
using UnityEngine.UI;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;
    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }

    //UI
    /*public TMP_Text scoreText;
    private int score;

    public TMP_Text waveText;
    private int waveNumber = 1;
    public Slider waveSlider;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    private int hp = 3;

    public GameObject StressBar;
    public TMP_Text stressText;
    private bool pressed = false;

    public GameObject gameOverMenu;
    public static bool gameOver = false;
    public TMP_Text gameOverText;*/


    //Sounds
    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}
