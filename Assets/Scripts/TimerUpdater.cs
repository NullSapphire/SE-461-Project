using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerUpdater : MonoBehaviour
{

    [SerializeField] private TMP_Text totalTimerText;
    private GameObject totalTimeLabelSetup;
    [SerializeField] private TMP_Text currentLevelTimerText;
    [SerializeField] private ReadWrite rw;

    private float totalTime;
    private float currentLevelTime;
    private TimingData prevData;

    void Awake()
    {
        totalTimeLabelSetup = GameObject.Find("TotalTimerLabel");
        TMP_Text totalTimeLabel = totalTimeLabelSetup.GetComponent<TMP_Text>();
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>().PlayMusic();
        if (!EntryMode.levelTesting)
        {
            prevData = ReadWrite.loadData<TimingData>("CurrentRun");
            totalTime = prevData.totalTime;
        }
        else
        {
            prevData = ReadWrite.loadData<TimingData>(ReadWrite.loadData<TimingData>("TestRun").username + "BestRun");
            totalTime = prevData.levelTimes[SceneManager.GetActiveScene().buildIndex - 1];
            string totalTimeInterval = TimeSpan.FromSeconds(totalTime).ToString("mm\\:ss\\:fff");
            totalTimerText.SetText(totalTimeInterval);
            totalTimeLabel.text = "Best Run Time";
            totalTimeLabel.color = new Color(255, 138, 236);
            totalTimerText.color = new Color(255, 138, 236);
            EntryMode.lastSceneID = SceneManager.GetActiveScene().buildIndex - 1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentLevelTime += Time.deltaTime;
        string currentLevelTimeInterval = TimeSpan.FromSeconds(currentLevelTime).ToString("mm\\:ss\\:fff");
        currentLevelTimerText.SetText(currentLevelTimeInterval);

        if (!EntryMode.levelTesting)
        {
            totalTime += Time.deltaTime;
            string totalTimeInterval = TimeSpan.FromSeconds(totalTime).ToString("mm\\:ss\\:fff");
            totalTimerText.SetText(totalTimeInterval);
        }
    }

    void OnDestroy()
    {
        prevData.levelTimes[SceneManager.GetActiveScene().buildIndex - 1] = currentLevelTime;
        if (!EntryMode.levelTesting)
        {
            prevData.totalTime = totalTime;
            rw.SaveToJson<TimingData>(prevData, "CurrentRun");
        }
        else
            rw.SaveToJson<TimingData>(prevData, "TestRun");
    }
}
