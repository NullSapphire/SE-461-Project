using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    void Start()
    {
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!EntryMode.levelTesting)
                SceneManager.LoadScene(sceneName);
            else
                SceneManager.LoadScene("LevelTestingResult");
        }
    }
}
