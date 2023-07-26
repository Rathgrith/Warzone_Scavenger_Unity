using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Singleton instance
    public static MenuManager instance = null;

    // Name of the scene to load when starting the game
    public string gameSceneName = "GameScene";

    public GameObject help;
    public GameObject ack;

    void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;
        }
        // If instance already exists and it's not this:
        else if (instance != this)
        {
            // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
        help.gameObject.SetActive(false);
        ack.gameObject.SetActive(false);


    }

    // Method to start the game
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Method to quit the game
    public void QuitGame()
    {
        // If we are running in a standalone build of the game
#if UNITY_STANDALONE
        // Quit the application
        Application.Quit();
#endif

        // If we are running in the editor
#if UNITY_EDITOR
        // Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void onHelp()
    {
        // Toggle the active state of the help canvas
        help.gameObject.SetActive(!help.gameObject.activeInHierarchy);
    }
    public void onAck()
    {
        // Toggle the active state of the help canvas
        ack.gameObject.SetActive(!ack.gameObject.activeInHierarchy);
    }
}
