using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    // TESTING ONLY
    public PathFollow follow;
    
    //public GameObject questionWindow;
    public LoginSystem loginSystem;
    public TMP_Text announcementText;
    public GameObject menuContainer;
    public GameObject notificationBox;
    public TMP_Text timerText;
    public Button logoutButton;
    
    [Header("Pass UI")]
    public GameObject passScreen;
    public TMP_InputField passField;
    public Button enterButton;
    [Header("Name UI")] 
    public GameObject loginScreen;
    public TMP_InputField firstNameField;
    public TMP_InputField lastNameField;
    public Button nameNextButton;
    [Header("End UI")] 
    public GameObject endScreen;
    public TMP_Text endMessage;
    public TMP_Text scoreMessage;
    public TMP_Text attemptMessage;
    public Button endNextButton;

    [Header("DEBUG ONLY")] 
    public GameObject devUI;
    public Button increaseScore;
    public Button decreaseScore;
    public Button restart;
    public Button fastForward;
    public Button resetSpeed;
    public Button testPost;
    public TMP_Text scoreText;


    void Start()
    {
        UIInit();
    }
    void Update()
    {
        TabSelect();
        timerText.text = Timer.Instance.GetTimeString();
    }

    private void UIInit()
    {
        devUI.SetActive(false);
        notificationBox.SetActive(false);
        timerText.gameObject.SetActive(false);
        
        // DEBUG
        enterButton.onClick.AddListener(OpenLoginScreen);
        nameNextButton.onClick.AddListener(StartGame);
        endNextButton.onClick.AddListener(CloseEndScreen);  
        logoutButton.onClick.AddListener(GameManager.Instance.Logout);
        #if UNITY_EDITOR
        SetDebugUI();
        #endif
        if (Debug.isDebugBuild)
            SetDebugUI();
    }
    public void StartGame()
    {
        if (!loginSystem.CheckNamesFilled()) return;
        CanvasGroup cg = menuContainer.GetComponent<CanvasGroup>();
        cg.DOFade(0f, 1f).OnComplete(() =>
        {
            loginScreen.SetActive(false);
            passScreen.SetActive(false);
            menuContainer.SetActive(false);
            timerText.gameObject.SetActive(true);
            cg.alpha = 1f;
            GameManager.Instance.StartGame();
        });
    }

    public void OpenLoginScreen()
    {
        if (loginSystem.IsPassValid())
        {
            menuContainer.SetActive(true);
            passScreen.SetActive(false);
            loginScreen.SetActive(true);
            return;
        }
        ShowNotification("Passcode invalid!");
    }
    public void OpenPassScreen()
    {
        passField.text = "";
        menuContainer.SetActive(true);
        passScreen.SetActive(true);
        loginScreen.SetActive(false);
    }
    public void OpenEndScreen()
    {
        endScreen.SetActive(true);
    }
    
    public void CloseEndScreen()
    {
        endScreen.SetActive(false);
        GameManager.Instance.RestartGame();
    }
    public void SetAnnouncementText(string text)
    {
        announcementText.text = text;
    }

    public void ShowNotification(string text)
    {
        if (notificationBox == null)
        {
            Debug.LogWarning("Notification box not set!");
            return;
        }

        notificationBox.SetActive(true);
        CanvasGroup cg = notificationBox.GetComponent<CanvasGroup>();
        TMP_Text textField = notificationBox.GetComponentInChildren<TMP_Text>();
        textField.text = text;
        IEnumerator coroutine = CloseNotification(3, cg);
        StopAllCoroutines();
        cg.DOKill();
        cg.alpha = 0;
        cg.DOFade(1f, 0.5f);
        StartCoroutine(coroutine);
    }
    IEnumerator CloseNotification(int dur, CanvasGroup cg)
    {
        yield return new WaitForSeconds(dur);
        cg.DOFade(0, 0.5f).OnComplete(() => notificationBox.SetActive(false));
    }
    public void SetEndScreen(string endMsg, string scoreMsg, string attemptMsg, bool allowRetry)
    {
        endMessage.text = endMsg;
        scoreMessage.text = scoreMsg;
        attemptMessage.text = attemptMsg;
        endNextButton.GetComponentInChildren<TMP_Text>().text = !allowRetry ? "Retry" : "Logout";
    }
    
    // Adds the ability to switch between UI elements using the tab key
    private void TabSelect()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject c = EventSystem.current.currentSelectedGameObject;
            if (c == null) { return; }
         
            Selectable s = c.GetComponent<Selectable>();
            if (s == null) { return; }

            Debug.Log(s.gameObject.name);
            Selectable jump = Input.GetKey(KeyCode.LeftShift)
                ? s.FindSelectableOnUp() : s.FindSelectableOnDown();
            if (jump != null) { jump.Select(); }
        }
    }
    private void SetDebugUI()
    {
        if (devUI != null) devUI.SetActive(true);
        if (increaseScore != null)
        {
            increaseScore.onClick.AddListener(() =>
            {
                GameManager.Instance.IncreaseScore();
                scoreText.text = "Score: " + GameManager.Instance.Score;
            });
        }
        if (decreaseScore != null)
        {
            decreaseScore.onClick.AddListener(() =>
            {
                GameManager.Instance.DecreaseScore();
                scoreText.text = "Score: " + GameManager.Instance.Score;
            });
        }
        else increaseScore.gameObject.SetActive(false);
        if (restart != null) restart.onClick.AddListener(GameManager.Instance.RestartGame);
        else restart.gameObject.SetActive(false);
        if (fastForward != null) fastForward.onClick.AddListener(() => Time.timeScale = 10f);
        else fastForward.gameObject.SetActive(false);
        if (resetSpeed != null) resetSpeed.onClick.AddListener(() => Time.timeScale = 1f);
        else resetSpeed.gameObject.SetActive(false);
    }
}
