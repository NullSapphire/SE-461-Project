using System;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayFinalTime : MonoBehaviour
{
    [SerializeField] private TMP_Text currentTimesList;
    [SerializeField] private TMP_Text bestTimesList;
    [SerializeField] private TMP_Text currentTimeTotal;
    [SerializeField] private TMP_Text bestTimeTotal;
    [SerializeField] private TMP_Text pbDisplay;
    [SerializeField] private ReadWrite rw;
    
    void Start()
    {
        if (!EntryMode.levelTesting)
        {
            TimingData currentRun = ReadWrite.loadData<TimingData>("CurrentRun");
            TimingData bestRun = ReadWrite.loadData<TimingData>(currentRun.username + "BestRun");
            string currentTimesListString = "";
            string bestTimesListString = "";
            float chapterTime = 0.0f;
            float bestChapterTime = 0.0f;

            for (int i = 0; i < currentRun.levelTimes.Length; i++)
            {
                chapterTime += currentRun.levelTimes[i];
                string currentTimeInterval = TimeSpan.FromSeconds(currentRun.levelTimes[i]).ToString("mm\\:ss\\:fff");
                currentTimesListString += currentTimeInterval + "\n";
                
                bestChapterTime += bestRun.levelTimes[i];
                if (bestRun.levelTimes[i] != 3599)
                {
                    string bestTimeInterval = TimeSpan.FromSeconds(bestRun.levelTimes[i]).ToString("mm\\:ss\\:fff");
                    bestTimesListString += bestTimeInterval + "\n";
                }
                else
                    bestTimesListString += currentTimeInterval + "\n";

                if((i+1)%4 == 0)
                {
                    currentTimesListString += TimeSpan.FromSeconds(chapterTime).ToString("mm\\:ss\\:fff") + "\n\n";
                    if (bestRun.levelTimes[i] != 3599*4)
                        bestTimesListString += TimeSpan.FromSeconds(bestChapterTime).ToString("mm\\:ss\\:fff") + "\n\n";
                    else
                        bestTimesListString += TimeSpan.FromSeconds(chapterTime).ToString("mm\\:ss\\:fff") + "\n\n";
                }
            }

            currentTimesList.text = currentTimesListString;
            bestTimesList.text = bestTimesListString;
            currentTimeTotal.text = TimeSpan.FromSeconds(currentRun.totalTime).ToString("mm\\:ss\\:fff");
            if (bestRun.totalTime != 3599)
                bestTimeTotal.text = TimeSpan.FromSeconds(bestRun.totalTime).ToString("mm\\:ss\\:fff");
            else
                bestTimeTotal.text = TimeSpan.FromSeconds(currentRun.totalTime).ToString("mm\\:ss\\:fff");
            if (currentRun.totalTime < bestRun.totalTime)
            {
                pbDisplay.enabled = true;
                bestRun = currentRun;
                rw.SaveToJson<TimingData>(bestRun, currentRun.username + "BestRun");
            }
            else
            {
                pbDisplay.enabled = false;
            }
        }
        else
        {
            TimingData testRun = ReadWrite.loadData<TimingData>("TestRun");
            TimingData bestRun = ReadWrite.loadData<TimingData>(testRun.username + "BestRun");

            string testTimeInterval = TimeSpan.FromSeconds(testRun.levelTimes[EntryMode.lastSceneID]).ToString("mm\\:ss\\:fff");
            currentTimeTotal.text = testTimeInterval;

            string bestTimeInterval = TimeSpan.FromSeconds(bestRun.levelTimes[EntryMode.lastSceneID]).ToString("mm\\:ss\\:fff");
            bestTimeTotal.text = bestTimeInterval;

            if (testRun.levelTimes[EntryMode.lastSceneID] < bestRun.levelTimes[EntryMode.lastSceneID])
                pbDisplay.enabled = true;
            else
                pbDisplay.enabled = false;
        }
    }
}
