using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameUIManager uiManager;
    public QuestionWindow questionWindow;
    public PathFollow pathFollow;
    public LoginSystem loginSystem;
    public Minimap minimapSystem;
    public EmailUtility emailUtility;
    public APIUtility apiUtility;
    public GameObject minimap;
    public int requiredScore = 26;
    public bool autorun = true;
    [Header("Vehicles")]
    public GameObject kayak;
    public GameObject boat;
    public GameObject bus;
    public GameObject bicycle;
    public GameObject train;

    public List<Transform> keyPoints = new List<Transform>();
    public Waypoint CurrentLocation { get; private set; }

    public int Score { get; private set; }

    private Vehicle previousType;
    
    private static readonly HttpClient client = new HttpClient();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        Timer.Instance.StartTimer();
        Timer.Instance.PauseTimer();
    }

    public void StartGame()
    {
        if (minimap != null)
            minimap.SetActive(true);
        
        loginSystem.ReduceAttempt();
        ProgressToNext();
    }
    [ContextMenu("Restart game")]
    public void RestartGame()
    {
        ResetValues();
        if (loginSystem.currentPlayer.attempts <= 0)
        {
            Logout(); 
            return;
        }
        StartGame();
    }

    void ResetValues()
    {
        Score = 0;
        questionWindow.ResetQuestions();
        pathFollow.PlaceAtStart();
        minimapSystem.ClearPoints();
        DOTween.KillAll();
        Timer.Instance.StartTimer();
        Timer.Instance.PauseTimer();
    }
    public async Task EndGame()
    {
        bool allowRetry = loginSystem.currentPlayer.attempts <= 0;
        if (Timer.Instance.TimerFinished)
        {
            uiManager.SetEndScreen("Sorry! You have run out of time!", 
                "Score: " + Score + " / 40", 
                "You have " + loginSystem.currentPlayer.attempts + " attempts left.", 
                allowRetry);
        }
        else if (Score < requiredScore)
        {
            uiManager.SetEndScreen("You don't have enough points!", 
                "Score: " + Score + " / 40", 
                "You have " + loginSystem.currentPlayer.attempts + " attempts left.", 
                allowRetry);
        }
        else
        {
            string fullName = loginSystem.currentPlayer.firstName + " " + loginSystem.currentPlayer.lastName;
            uiManager.SetEndScreen("Congratulations\n " + fullName + "!", 
                "Score: " + Score + " / 40", 
                "You have passed the test!", 
                allowRetry);
            Score newScore = new Score(fullName, Score);
            
            if (apiUtility)
            {
                apiUtility.PostScoresUnity(newScore);
                apiUtility.PostMailUnity(newScore);
                uiManager.ShowNotification("Result submitted!");
            }
        }
        uiManager.OpenEndScreen();
    }
    
    public void SetCurrentLocation(Waypoint dest)
    {
        CurrentLocation = dest;
    }

    public void ProgressToNext()
    {
        if (pathFollow == null || !autorun)
            return;
        
        pathFollow.MoveNextLocation();
    }
    public void MovingToLocation(Waypoint location)
    {
        if (uiManager != null)
            uiManager.SetAnnouncementText("Next stop: " + location.GetName());
        if (minimapSystem != null)
            minimapSystem.HighlightPoint(location);
    }

    public void ReachedLocation(Waypoint location)
    {
        if (uiManager != null)
            uiManager.SetAnnouncementText("Arrived at " + location.GetName());
        CurrentLocation = location;
        if (location.locationType == SpecialLocation.End)
        {
            EndGame();
            Timer.Instance.StopTimer();
            return;
        }
        questionWindow.OpenWindow();
    }

    public void Logout()
    {
        //ResetValues();
        //uiManager.OpenPassScreen();
        //uiManager.logoutButton.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
    public void Login()
    {
        uiManager.logoutButton.gameObject.SetActive(true);
    }
    public void IncreaseScore()
    {
        Score++;
    }
    public void DecreaseScore()
    {
        Score--;
    }
    public void ChangeVehicle(Vehicle type)
    {
        if (type == previousType) return;
        
        kayak.SetActive(false);
        boat.SetActive(false);
        bus.SetActive(false);
        bicycle.SetActive(false);
        train.SetActive(false);

        switch (type)
        {
            case Vehicle.Bicycle:
                bicycle.SetActive(true);
                break;
            case Vehicle.Boat:
                boat.SetActive(true);
                break;
            case Vehicle.Bus:
                bus.SetActive(true);
                break;
            case Vehicle.Kayak:
                kayak.SetActive(true);
                break;
            case Vehicle.Train:
                train.SetActive(true);
                break;
        }

        previousType = type;
    }
}