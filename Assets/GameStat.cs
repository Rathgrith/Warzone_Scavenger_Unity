using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
public class GameStat : MonoBehaviour
{
    // Singleton instance
    public static GameStat Instance { get; private set; }

    // Variables to store stats
    public int DestroyedLandCount { get; private set; }
    public int DestroyedSuppliesCount { get; private set; }
    // Variable for the text display
    public TextMeshProUGUI[] statList;
    public Canvas pause;
    public TextMeshProUGUI notification;
    public GameObject resume;
    public TextMeshProUGUI scoreText;
    private float startTime;
    public int winScore;
    bool pauseflag;
    public GameObject player;
    TimeSpan time;
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        pause.gameObject.SetActive(false);
        statList[1].SetText("Supply: " + "0/" + winScore);
        startTime = Time.time;
        pauseflag = false;
    }

    // Function to increase the count of destroyed land
    public void IncreaseDestroyedLandCount()
    {
        DestroyedLandCount++;
        statList[0].SetText("Land Destruction: " + DestroyedLandCount.ToString());

    }

    // Function to increase the count of destroyed supplies
    public void IncreaseDestroyedSuppliesCount()
    {
        DestroyedSuppliesCount += 5;
        statList[1].SetText("Supply: " + DestroyedSuppliesCount.ToString() + "/" + winScore);
        if (DestroyedSuppliesCount == winScore)
        {
            this.onPause(1);
        }
    }

    private void Update()
    {
        // Compute the elapsed time
        time = TimeSpan.FromSeconds(Time.time - startTime);

        // Update the time display
        statList[2].text = "Time: " + time.ToString(@"mm\:ss");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onPause(0);
        }
    }

    public void onPause(int pauseCode)
    {
        // Pause the game
       
        
        int score = Math.Max(2000 - DestroyedLandCount, 0);
        if (pauseCode == 0){
            
            pauseflag = !pauseflag;
            pause.gameObject.SetActive(pauseflag);
            if(pauseflag) Time.timeScale = 0;
            else Time.timeScale = 1;
            notification.text = "Pause";
            resume.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
            
        }
        else if (pauseCode == 1)
        {
            Time.timeScale = 0;
            pause.gameObject.SetActive(true);
            notification.text = "You secured enough supplies!";
            scoreText.text = "score:" + score;
            resume.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(true);

        }
        else if (pauseCode == 2)
        {
            Time.timeScale = 0;
            pause.gameObject.SetActive(true);
            notification.text = "You lost";
            resume.gameObject.SetActive(true);
        }
    }
    public void RestartScene()
    {
        // Reload the scene
        Time.timeScale = 1;
        //Destroy(GameManager.instance); 
        SceneManager.LoadScene("MapGenerateScene");

    }


    public void ReturnScene()
    {
        // Reload the scene
        Time.timeScale = 1;
        //Destroy(GameManager.instance); 
        SceneManager.LoadScene("MainMenu");

    }
    public void ResumeScene()
    {
        if (DestroyedSuppliesCount >= 10)
        {
            DestroyedSuppliesCount -= 10;
            Time.timeScale = 1;

            pause.gameObject.SetActive(false);
            statList[1].SetText("Supply: " + DestroyedSuppliesCount.ToString() + "/" + winScore);
            player.transform.position = new Vector3(45, 10, 21);
        }

    }


}
