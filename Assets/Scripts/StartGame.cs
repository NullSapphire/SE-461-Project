using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
   [SerializeField] private ReadWrite rw;
   [SerializeField] private TMP_InputField usernameInput;
   [SerializeField] private TMP_Text WarningMsg;
   [SerializeField] private Canvas levelsCanvas;

   void Start()
   {
      GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>().StopMusic();
      WarningMsg.enabled = false;
      levelsCanvas.enabled = false;
   }
   public void onStartClick()
   {
      EntryMode.levelTesting = false;
      if (usernameInput.text != "")
      {
         TimingData currRun = new TimingData();

         currRun.username = usernameInput.text;
         for (int i = 0; i < currRun.levelTimes.Length; i++)
         {
            currRun.levelTimes[i] = 0.0f;
         }

         currRun.totalTime = 0.0f;
         rw.SaveToJson(currRun, "CurrentRun");

         TimingData bestRun = ReadWrite.loadData<TimingData>(usernameInput.text + "BestRun");
         
         if (bestRun == default(TimingData))
         {
            Debug.Log(usernameInput.text + "BestRun.txt not found. Creating new save");
            bestRun = new TimingData();
            bestRun.username = usernameInput.text;
            for (int i = 0; i < bestRun.levelTimes.Length; i++)
            {
               bestRun.levelTimes[i] = 3599;
            }
            bestRun.totalTime = 3599;
            rw.SaveToJson(bestRun, usernameInput.text + "BestRun");
         }
         SceneManager.LoadScene("1-A");
      }
      else
      {
         WarningMsg.enabled = true;
         WarningMsg.text = "Please ensure you have entered a username";
      }
   }

   public void onLevelsClicked()
   {
      if (usernameInput.text != "")
      {
         TimingData bestRun = ReadWrite.loadData<TimingData>(usernameInput.text + "BestRun");
         if (bestRun != default(TimingData) && bestRun.totalTime != 3599)
         {
            EntryMode.levelTesting = true;
            EntryMode.userName = usernameInput.text;
            levelsCanvas.enabled = true;
            WarningMsg.enabled = false;
            gameObject.SetActive(false);
         }
         else
         {
            WarningMsg.enabled = true;
            WarningMsg.text = "You must beat the game first!";
         }
      }
      else
      {
         WarningMsg.enabled = true;
         WarningMsg.text = "Please ensure you have entered a username";
      }
   }

   public void onQuitClicked()
   {
      Application.Quit();
   }

}
