using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionWindow : MonoBehaviour
{
    //Question UI
    public CSVReader reader;
    [Header("UI Elements")] 
    [Header("Question Screen")] 
    public GameObject questionScreen;
    public TMP_Text questionTitle;
    public TMP_Text questionText;
    public Button falseButton;
    public Button trueButton;
    // Location UI
    [Header("Location Screen")] 
    public GameObject locationScreen;
    public Sprite noImagePhoto;
    public Sprite[] locationPhotos;
    [Header("UI Elements")] 
    public Image locationPhoto;
    public Button nextButton;
    public TMP_Text locationTitle;
    public TMP_Text locationText;
    //Question UI
    private string printTitle;
    private string printText;
    private int playerLocation;
    private int questionNumber;
    private int totalQuestionNumber;
    private int randNum;
    private int prevRandNum;

    private void Start()
    {
        trueButton.onClick.AddListener(() => CheckAnswer(1));
        falseButton.onClick.AddListener(() => CheckAnswer(2));
        nextButton.onClick.AddListener(DisplayQuestion);
        
        totalQuestionNumber = 1;
        playerLocation = Random.Range(1, 5);
    }
    [ContextMenu("Display question")]
    private void DisplayQuestion()
    {
        if(!Timer.Instance.TimerActive) Timer.Instance.PauseTimer();
        locationScreen.SetActive(false);
        questionScreen.SetActive(true);
        
        randNum = Random.Range(0, 4);
        while (randNum == prevRandNum)
            randNum = Random.Range(0, 4);
        if(randNum != prevRandNum)
            prevRandNum = randNum;

        questionTitle.text = "Question " + (totalQuestionNumber);
        questionText.text = reader.Questions[GetLocationIndex()][randNum].question;
        // Debug.Log("Current index: " + GetLocationIndex());
    }
    private int GetLocationIndex()
    {
        Waypoint currentLocation = GameManager.Instance.CurrentLocation;
        for (var i = 0; i < GameManager.Instance.keyPoints.Count; i++)
        {
            Transform location = GameManager.Instance.keyPoints[i];
            if (currentLocation.locationType == location.GetComponent<Waypoint>().locationType)
                return i;
        }
        return 0;
    }
    private void CheckAnswer(int answer)
    {
        if(answer == reader.Questions[GetLocationIndex()][randNum].right)
        {
            GameManager.Instance.IncreaseScore();
            // Debug.Log("You answered correctly!");
        }
        else
        {
            // Debug.Log("Wrong answer");
        }
        questionNumber++;
        totalQuestionNumber++;
        //ready = false;
        //Check if player has answered two questions
        if (questionNumber >= 2)
            CloseWindow();
        else
            DisplayQuestion();
    }

    private void DisplayLocation()
    {
        locationScreen.SetActive(true);
        if (locationPhotos.Length > GetLocationIndex())
        {
            if (locationPhotos[GetLocationIndex()] != null)
                locationPhoto.sprite = locationPhotos[GetLocationIndex()];
            else
                locationPhoto.sprite = noImagePhoto;
        }
        else
            locationPhoto.sprite = noImagePhoto;

        locationText.text = reader.Questions[GetLocationIndex()][randNum].saateteksti;
        locationTitle.text = GetLocationIndex()+1 + ". " + reader.Questions[GetLocationIndex()][randNum].location;
    }
    public void OpenWindow()
    {
        questionNumber = 0;
        DisplayLocation();
        
        falseButton.interactable = true;
        trueButton.interactable = true;
        CanvasGroup cgLoc = locationScreen.GetComponent<CanvasGroup>();
        cgLoc.alpha = 0f;
        cgLoc.DOFade(1f, 1f);
    }

    private void CloseWindow()
    {
        if(Timer.Instance.TimerActive) Timer.Instance.PauseTimer();
        falseButton.interactable = false;
        trueButton.interactable = false;
        CanvasGroup cg = questionScreen.GetComponent<CanvasGroup>();
        cg.DOFade(0f, 1f).OnComplete(() =>
        {
            locationScreen.SetActive(false);
            questionScreen.SetActive(false);
            cg.alpha = 1f;
            GameManager.Instance.ProgressToNext();
        });
    }

    public void ResetQuestions()
    {
        totalQuestionNumber = 0;
    }
}
