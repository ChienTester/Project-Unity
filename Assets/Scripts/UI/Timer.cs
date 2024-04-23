using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI timeText;
    [SerializeField] GameObject GameOverWindow;
    [SerializeField] GameObject WinnerWindow;
    [SerializeField] GameObject GameWindowMap_1;
    [SerializeField] GameObject GameWindowMap_2;
    [SerializeField] GameObject PlayerHP;
    [SerializeField] int wait;
    int minute;
    int second;
    float startTime;
    bool isMap2Started = false;

    private Timer() { }

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        if (!isMap2Started)
            StartCoroutine(CountTime());
    }

    void Initialize()
    {
        timeText = GetComponent<TextMeshProUGUI>();
        minute = 0;
        second = 0;
    }

    IEnumerator CountTime()
    {

        while (true)
        {
            yield return new WaitForSeconds(1f);

            ++second;
            if (second >= 60)
            {
                ++minute;
                second = 0;

                if (minute != 0 && minute != 10 && minute % 2 == 0)
                    EnemySpawner.GetInstance().IncreaseStage();
            }

            if (second < 10 && timeText)
            {
                timeText.text = minute.ToString() + " : 0" + second.ToString();

            }
            else
                if (timeText)
            {

                timeText.text = minute.ToString() + " : " + second.ToString();
            }

            if (second >= 4)
            {
                startTime = Time.realtimeSinceStartup;
                StopCoroutine(CountTime());
                StartCoroutine(SwitchToMap2());
                break;
            }
        }
    }

    IEnumerator SwitchToMap2()
    {
        // sart cutscene
        Time.timeScale = 0f;
        PlayerHP.active = false;
        SceneManager.LoadScene("CutScene", LoadSceneMode.Additive);
        bool check = true;
        while (check)
        {
            yield return null;
            float elapsedTime = Time.realtimeSinceStartup - startTime;

            if (elapsedTime >= wait)
            {
                PlayerHP.active = true;
                SceneManager.UnloadSceneAsync("CutScene");
                Time.timeScale = 1f;
                check = false;
            }
        }


        // Reset thời gian và xóa quái vật trên map 1
        minute = 0;
        second = 0;

        isMap2Started = true;
        EnemySpawner.isMap2Started = true;
        StartCoroutine(CountTimeForMap2());
        EnemySpawner.GetInstance().StartSpawningMap2Enemies();
        EnemySpawner.GetInstance().EndSpawnEnemy();
    }

    IEnumerator CountTimeForMap2()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (isMap2Started)
            {
                ++second;
                if (second >= 60)
                {
                    ++minute;
                    second = 0;

                    // Tăng stage và sinh quái vật khi thời gian thích hợp
                    if (minute != 0 && minute != 10 && minute % 2 == 0)
                        EnemySpawner.GetInstance().IncreaseStage();
                }

                // Cập nhật hiển thị thời gian trên UI
                if (second < 10 && timeText)
                {
                    timeText.text = minute.ToString() + " : 0" + second.ToString();
                }
                else if (timeText)
                {
                    timeText.text = minute.ToString() + " : " + second.ToString();
                }
            }

            if (second >= 5)
            {
                WinnerWindow.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public int GetMinute()
    {
        return minute;
    }
}
