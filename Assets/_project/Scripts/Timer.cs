using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    private float startTime;
    private float pauseTime;
    private float time;
    public bool TimerActive { get; private set; }
    public bool TimerFinished { get; private set; }
    public bool countDown;
    public int countdownStart = 3600;
    private int countdownOrig;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        InitTimer();
        GetTimeString();
    }

    private void Update()
    {
        if (TimerActive)
        {
            if (countDown)
                time = countdownStart - Time.time;
            else
                time = Time.time - startTime;
            
            GetTimeString();
            if (time <= 0)
                StopTimer();
        }
    }

    void InitTimer()
    {
        countdownOrig = countdownStart;
        if (countDown)
            time = countdownStart;
        else
            time = Time.time - startTime;
    }
    public void StartTimer()
    {
        countdownStart = countdownOrig;
        startTime = Time.time;
        TimerFinished = false;
        
        // Pretty inaccurate but works good enough for this purpose...
        if (countDown)
            countdownStart += (int)startTime;
        
        TimerActive = true;
    }
    public void StopTimer()
    {
        TimerFinished = true;
        TimerActive = false;
    }
    public void PauseTimer()
    {
        if (TimerFinished) return;
        TimerActive = !TimerActive;
        if (!countDown)
        {
            if (!TimerActive) 
                pauseTime = time;
            else 
                startTime = pauseTime;
        }
        if (countDown)
        {
            if (!TimerActive)
                pauseTime = Time.time;
            else
                countdownStart += (int)Time.time - (int)pauseTime;
        }
    }
    public string GetTimeString()
    {
        int seconds = (int)(time % 60);
        int minutes = (int)(time / 60);
        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
