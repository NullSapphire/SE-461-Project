using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private ReadWrite rw;
    public void OnClick()
    {
        TMP_Text levelText = GetComponentInChildren<TMP_Text>();

        TimingData currRun = new TimingData();

        currRun.username = EntryMode.userName;
        for (int i = 0; i < currRun.levelTimes.Length; i++)
        {
            currRun.levelTimes[i] = 0.0f;
        }

        currRun.totalTime = 0.0f;

        rw.SaveToJson<TimingData>(currRun, "TestRun");
        

        SceneManager.LoadScene(levelText.text);
    }
}
