//Completely independant, uses player position to calculate confetti positions that can be adjusted
//Script execution order after level setter
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using GameAnalyticsSDK;
//using Facebook.Unity;

public class GameManager : MonoBehaviour//
{
    public bool deleteAll;
    public bool setLevel;
    public int level;
    [HideInInspector]public int score;
    //private Transform collectables;
    //private int totalCollectables;
    //[HideInInspector] public Transform currentLevelTransform;
    [HideInInspector] public bool gameOver;
    [HideInInspector] public bool gameWon;
    [HideInInspector] public bool gameLost;
    [HideInInspector] public bool started;
    public event System.Action LevelStartedEvent;
    public event System.Action<int> LevelSetEvent;
    public event System.Action<bool, int> LevelOverEvent;
    public event System.Action<int> LevelDataReadyEvent;

    public event System.Action UserInputStarted;


    [HideInInspector] public int currentLevel;
    bool inputReceived;


    //[HideInInspector] public bool editor;
    //[HideInInspector] public bool mobile;
    //private float halfX;
    //private float halfY;

    //private string systemID;
    public bool quickNavigation;
    public bool quickNavigationMob;
    public bool enableLogs;
    Touch t;
    private void Awake()
    {
        //GameAnalytics.Initialize();
#if UNITY_EDITOR
        if (deleteAll)
            PlayerPrefs.DeleteAll();
#endif
        FetchCurrentLevel();
    }
    void Start()
    {
        StartCoroutine(OnLevelStarted());
    }
    private void FetchCurrentLevel()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        if (currentLevel == 0)
        {
            currentLevel = 1;
        }
#if UNITY_EDITOR
        if (setLevel)
            currentLevel = level;
#endif
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
    }
    private void Update()
    {
        if (!inputReceived && started)
        {
            if (CheckInput())
            {
                inputReceived = true;
                UserInputStarted?.Invoke();
            }
        }
    }
    //    private void Update()
    //    {
    //        if (quickNavigation)
    //        {
    //            if (true /*|| jayaMobile*/)
    //            {
    //#if (UNITY_EDITOR)
    //                if (Input.GetKeyDown(KeyCode.A))
    //                {
    //                    JumpPreviousLevel();
    //                }
    //                else if (Input.GetKeyDown(KeyCode.S))
    //                {
    //                    JumpNextLevel();
    //                }
    //#elif (UNITY_ANDROID || UNITY_IOS)
    //            if(quickNavigationMob)
    //            if (Input.touchCount > 0)
    //            {
    //                    t = Input.GetTouch(0);
    //                    if(t.phase == TouchPhase.Began)
    //                    {
    //                        if (t.position.y > halfY)
    //                        {
    //                            if (Input.GetTouch(0).position.x < halfX)
    //                            {
    //                                JumpPreviousLevel();
    //                            }
    //                            else
    //                            {
    //                                JumpNextLevel();
    //                            }
    //                        }
    //                    }
    //                }
    //#endif
    //            }
    //        }
    //    }
    private IEnumerator OnLevelStarted()
    {
        LevelSetEvent?.Invoke(currentLevel);

        yield return null;//this is called in start, level setter uses it's start to subscribe to events, so wait one frame b4 calling LevelSetEvent
        gameOver = false;
        gameWon = false;
        gameLost = false;
        LevelDataReadyEvent?.Invoke(currentLevel);
        yield return null;
        LevelStartedEvent?.Invoke();
        Invoke("StartGameDelay", 0.3f);
#if !UNITY_EDITOR
        //        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevel.ToString());
        TinySauce.OnGameStarted(currentLevel.ToString());
#endif

    }
    private void StartGameDelay()
    {
        started = true;
    }
    public void GameWon(int multiplier = 1)
    {
        LevelOverEvent?.Invoke(true, multiplier);
        started = false;
        gameOver = true;
        gameWon = true;

        //UIManager.Instance.SetWinScreen(true, multiplier);
#if !UNITY_EDITOR
        //        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevel.ToString());
        TinySauce.OnGameFinished(true, 0, currentLevel.ToString());
#endif

        currentLevel++;
    }
    public void GameFailed(string remark = "")
    {
        LevelOverEvent?.Invoke(false, -1);
        started = false;
        gameOver = true;
        gameLost = true;
        //UIManager.Instance.SetLoseScreen(true);
#if !UNITY_EDITOR
        //        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, currentLevel.ToString());
        TinySauce.OnGameFinished(false, 0, currentLevel.ToString());
#endif

    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("CurrentLevel", (currentLevel));
        ReloadScene();
    }
    public void NextLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel", (currentLevel));
        ReloadScene();
    }
    private void JumpPreviousLevel()
    {
        if (currentLevel > 1)
            currentLevel--;
        ReloadScene();
    }
    private void JumpNextLevel()
    {
        currentLevel++;
        ReloadScene();

    }
    private void OnApplicationPause(bool pause)
    {
#if (!UNITY_EDITOR)
        if(pause)
        {
            //if gamewon currentlevel will be incremented before this
            PlayerPrefs.SetInt("CurrentLevel", (currentLevel));
        }
        else
        {
           FetchCurrentLevel();
        }
#endif
        // Check the pauseStatus to see if we are in the foreground
        // or background
        //if (!pause)
        //{
        //    //app resume
        //    if (FB.IsInitialized)
        //    {
        //        FB.ActivateApp();
        //    }
        //    else
        //    {
        //        //Handle FB.Init
        //        FB.Init(() =>
        //        {
        //            FB.ActivateApp();
        //        });
        //    }
        //}
    }
    private void OnApplicationQuit()
    {
#if (UNITY_EDITOR)
        //if gamewon currentlevel will be incremented before this
        PlayerPrefs.SetInt("CurrentLevel", (currentLevel));
#endif
    }
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    TouchPhase touchBegan = TouchPhase.Began;

    private bool CheckInput()
    {
        return
            (Input.touchCount > 0 && Input.GetTouch(0).phase == touchBegan) ||
            Input.anyKey || Mathf.Abs(Input.GetAxis("Mouse X")) > 0 || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0;
    }

}