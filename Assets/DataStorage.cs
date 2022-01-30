using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Obsolete]
public class DataStorage : MonoBehaviour
{
    public Camera cam;
    public GameObject player;
    public SpawnEnemies spawner;
    public Shooting shooting;
    public UIManager UI;
    public static bool gameEnd;

    //public InputField condition;
    //public InputField participantID;
    public static int shots;
    public static int hits;
    private float accuracy = 0f;
    public float timeStamp;
    public float prevTimeStamp = 0;
    public float dt;

    public static Vector2 mousePosition;
    public static Vector2 oldMousePosition;
    public List<float> mouseVelocity = new List<float>();
    public List<float> mouseAcceleration = new List<float>();
    public static int clicks;
    public static int keystrokes;
    public static int nearHits;
    public static bool stressed;

    public int enemyCount;
    public string enemyType;
    private GameObject[] enemyProjectiles;
    public int enemyProjectileCount;
    public List<string> enemyPosition = new List<string>();
    public List<Vector2> enemyProjectilePosition = new List<Vector2>();
    public Vector2 playerPosition;
    public List<Vector2> playerProjectilePosition = new List<Vector2>();

    private int bufferClicks;
    private int bufferKeystrokes;
    private int bufferNearHits;
    private bool bufferStressed;

    /*
    private List<string> bufferEnemyPosition = new List<string>();
    private List<Vector2> bufferEnemyProjectilePosition = new List<Vector2>();
    private Vector2 bufferPlayerPosition;*/
    private List<string> bufferBulletListOnShot = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        oldMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseVelocity.Add(0);
        mouseAcceleration.Add(0);
        StartCoroutine(LoadCondition());
        StartCoroutine(DataManager());
        //InvokeRepeating("GetData", 0f, 0.5f);
    }

    /*public void SetParticipantID(int participantID)
    {
        this.participantID.text = participantID.ToString();
    }*/

    IEnumerator LoadCondition()
    {
        string url = "/fetch_condition";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string response = request.downloadHandler.text;
                request.Dispose();

                if (response.Length == 0)
                {
                    Debug.Log("Unable to load condition! Does the participant have a valid session?");
                }
                else
                {
                    Debug.Log(response);
                    //condition.text = response;
                }
            }
        }
    }

   public IEnumerator DataManager()
    {
        //yield return new WaitForSeconds(1f);
        while (!gameEnd)
        {
            GetData();
            PostData();
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void GetData()
    {
        //mouseVelocity = Vector2.Distance(mousePosition, oldMousePosition) / dt;
        //mouseAcceleration = (mouseVelocity - oldMouseVelocity) / dt;
        if (shots > 0)
        {
            accuracy = (float)hits / shots;
        }
        playerPosition = player.transform.position;

        enemyCount = spawner._enemyList.Count;

        
        foreach (GameObject enemy in spawner._enemyList)
        {
            if (enemy)
            {
                enemyPosition.Add(enemy.tag + ": " + enemy.transform.position.ToString());
            }
        }
        //Debug.Log(String.Join(", ", new List<string>(enemyPosition).ConvertAll(i => i.ToString()).ToArray()));

        enemyProjectiles = GameObject.FindGameObjectsWithTag("EnemyBullet");
        enemyProjectileCount = enemyProjectiles.Length;

        foreach (GameObject bullet in enemyProjectiles)
        {
            if (bullet)
            {
                enemyProjectilePosition.Add(bullet.transform.position);
            }
        }

        foreach (GameObject bullet in shooting._bulletList)
        {
            if (bullet) { 
                playerProjectilePosition.Add(bullet.transform.position);
            }
        }

        bufferClicks = clicks;
        bufferKeystrokes = keystrokes;
        bufferNearHits = nearHits;
        bufferStressed = stressed;

        bufferBulletListOnShot.Clear();
        //Debug.Log("BulletListOnShot: " + String.Join(", ", new List<string>(shooting._bulletListOnShot).ConvertAll(i => i.ToString()).ToArray()));
        foreach (string pos in shooting._bulletListOnShot)
        {
            bufferBulletListOnShot.Add(pos);
        }

        //--- Debug data ---//
        //Debug.Log("Near hits: " + nearHits);

        //Debug.Log("Data gathered");
        //PostData();
    }
    
    public void PostData()
    {
        WWWForm frm = new WWWForm();
        frm.AddField("timestamp", timeStamp.ToString());
        
        frm.AddField("shots", shots);
        frm.AddField("hits", hits);
        frm.AddField("accuracy", accuracy.ToString());
        frm.AddField("mean_mouse_acceleration", mouseAcceleration.Average().ToString());
        frm.AddField("max_mouse_acceleration", mouseAcceleration.Max().ToString());
        frm.AddField("min_mouse_acceleration", mouseAcceleration.Min().ToString());
        frm.AddField("std_mouse_acceleration", Std(mouseAcceleration).ToString());
        frm.AddField("mean_mouse_velocity", mouseVelocity.Average().ToString());
        frm.AddField("max_mouse_velocity", mouseVelocity.Max().ToString());
        frm.AddField("min_mouse_velocity", mouseVelocity.Min().ToString());
        frm.AddField("std_mouse_velocity", Std(mouseVelocity).ToString());
        frm.AddField("mouse_position", mousePosition.ToString());
        frm.AddField("clicks", bufferClicks);
        frm.AddField("keystrokes", bufferKeystrokes);
        frm.AddField("near_hits", bufferNearHits);
        frm.AddField("score", UIManager.score);
        frm.AddField("stressed", bufferStressed.ToString());
        frm.AddField("hp", UI.hp);

        frm.AddField("enemy_count", enemyCount);
        frm.AddField("enemy_position", String.Join(", ", new List<string>(enemyPosition).ConvertAll(i => i.ToString()).ToArray()));//
        frm.AddField("enemy_projectile_count", enemyProjectileCount);
        frm.AddField("enemy_projectile_position", String.Join(", ", new List<Vector2>(enemyProjectilePosition).ConvertAll(i => i.ToString()).ToArray()));//
        frm.AddField("player_position", playerPosition.ToString());
        frm.AddField("player_projectile_vector_and_mouse", String.Join(", ", new List<string>(bufferBulletListOnShot).ConvertAll(i => i.ToString()).ToArray()));
        frm.AddField("player_projectile_position", String.Join(", ", new List<Vector2>(playerProjectilePosition).ConvertAll(i => i.ToString()).ToArray()));//

        //Debug.Log("Shots: " + shots + " Clicks: " + clicks + "  time: " + timeStamp);

        try
        {
            var request = UnityWebRequest.Post("#", frm);
            request.SendWebRequest();
            Debug.Log("Data send");
            request.Dispose();

            var request2 = UnityWebRequest.Post("#", new WWWForm());
            request2.SendWebRequest();
            request2.Dispose();
        }
        catch (Exception ex)
        {
            Debug.Log("Error in PostData(): " + ex.Message);
        }

        //Debug.Log(String.Join(", ", new List<float>(mouseVelocity).ConvertAll(i => i.ToString()).ToArray()));
        mouseVelocity.RemoveRange(0, mouseVelocity.Count - 1);
        //Debug.Log("Max before: " + mouseAcceleration.Max());
        //Debug.Log(String.Join(", ", new List<float>(mouseVelocity).ConvertAll(i => i.ToString()).ToArray()));
        mouseAcceleration.RemoveRange(0, mouseAcceleration.Count - 1);
        //Debug.Log("Max after: " + mouseAcceleration.Max());

        clicks = 0;
        //Debug.Log("Clicks: " + clicks + " BufferClicks: " + bufferClicks);
        keystrokes = 0;
        nearHits = 0;
        stressed = false;

        shooting._bulletListOnShot.Clear();
        enemyPosition.Clear();
        enemyProjectilePosition.Clear();
        playerProjectilePosition.Clear();
    }

    [DllImport("__Internal")]
    public static extern void RedirectBOF();

    // Can't add RedirectBOF() to onClick directly.
    public void RedirectBOFClicked()
    {
        if (UIManager.test)
        {
            RedirectBOF();
        } else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    public void RedirectBOFDeprecated()
    {
        // Deprecated way of redirecting participants to the next page in the experiment
        Application.ExternalEval("window.location.href = \"/redirect_next_page\";");

        // Make the game simulation stop.
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeStamp = Time.realtimeSinceStartup;
        dt = timeStamp - prevTimeStamp;
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseVelocity.Add((Vector2.Distance(mousePosition, oldMousePosition))/dt);
        mouseAcceleration.Add((mouseVelocity[mouseVelocity.Count-1] - mouseVelocity[mouseVelocity.Count - 2]) / dt);

        oldMousePosition = mousePosition;
        prevTimeStamp = timeStamp;

        if (UIManager.pressed)
        {
            stressed = true;
        }

        if (Input.anyKeyDown)
        {
            keystrokes++;
        }
    }

    private double Std(IEnumerable<float> values)
    {
        double standardDeviation = 0;

        if (values.Any())
        {
            // Compute the average.     
            double avg = values.Average();

            // Perform the Sum of (value-avg)_2_2.      
            double sum = values.Sum(d => Math.Pow(d - avg, 2));

            // Put it all together.      
            standardDeviation = Math.Sqrt((sum) / (values.Count() - 1));
        }

        return standardDeviation;
    }
}
