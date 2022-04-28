using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LogicManager : MonoBehaviour
{
    [HideInInspector] public InputState inputState;
    GameManager gameManager;
    ConnectionManager connectionManager;
    //private int totalMergeCount;
    private int mergedCount = 0;
    private Vector3 targetPosition;
    private LaunchPadScript[] players;
    public GameObject undoButton;
    CameraFollow cameraFollow;
    private void Awake()
    {
        players = FindObjectsOfType<LaunchPadScript>();
        gameManager = FindObjectOfType<GameManager>();
        connectionManager = FindObjectOfType<ConnectionManager>();
        cameraFollow = FindObjectOfType<CameraFollow>();
    }
    private void OnEnable()
    {
        gameManager.LevelStartedEvent += OnLevelStarted;
        gameManager.LevelDataReadyEvent += OnLevelDataReady;
        connectionManager.MergeStartedEvent += OnMergeStarted;
        connectionManager.MergeCompletedEvent += OnMergeCompleted;
    }
    private void OnDisable()
    {
        gameManager.LevelStartedEvent -= OnLevelStarted;
        gameManager.LevelDataReadyEvent -= OnLevelDataReady;
        connectionManager.MergeStartedEvent -= OnMergeStarted;
        connectionManager.MergeCompletedEvent -= OnMergeCompleted;
    }
    private void OnMergeStarted()
    {
        inputState = InputState.Wait;
    }
    private void OnLevelDataReady(int currentLevel)
    {
        DotsParent dotsParent = FindObjectOfType<DotsParent>();
        //totalMergeCount = dotsParent.dotsList.Count -1;
        GameObject target = GameObject.FindGameObjectWithTag("Target");
        targetPosition = target.transform.position;
    }
    private void OnLevelStarted()
    {
        inputState = InputState.Ready;
    }
    private IEnumerator GameWonDelay()
    {
        print("won");
        
        yield return new WaitForSeconds(1);

        FindObjectOfType<UIManager>().ShowGoodWordText();
        yield return new WaitForSeconds(1.5f);
        FindObjectOfType<UIManager>().HideGoodWordText();
        gameManager.GameWon();
        AudioManager.Instance.Vibrate(30);
    }
    private IEnumerator GameFailedDelay()
    {
        RocketGroup[] rgs = FindObjectsOfType<RocketGroup>();
        for (int i = 0; i < rgs.Length; i++)
        {
            if (rgs[i].state == RocketState.NotLaunched)
            {
                StartCoroutine(rgs[i].Explode());
            }
        }

        yield return new WaitForSeconds(1.7f);
       
    }
    public void OnRocketExploded()
    {
        gameManager.GameFailed("Outside LaunchPad");
        AudioManager.Instance.Vibrate(30);
    }
    public void UndoLastMove()
    {
    }
    private void OnMergeCompleted(Transform resultTransform)
    {
        if (gameManager.currentLevel == 1)
        {
            LevelManager lm = FindObjectOfType<LevelManager>();
            Canvas cn = lm.currentLevelObject.GetComponentInChildren<Canvas>();
            if (cn != null)
            {
                cn.gameObject.SetActive(false);
            }
        }
        //Let player script do the necessary change before logic manager checking statuses
        StartCoroutine(OnMergeCompletedCoroutine(resultTransform));
    }
    private IEnumerator OnMergeCompletedCoroutine(Transform resultTransform)//called immediately after midpoint is reached, not waiting for merge effect to complete
    {
        yield return null;//wait for layer toprocess OnMergeCompleted event and sets it status
        inputState = InputState.Ready;
        mergedCount++;
        if(gameManager.currentLevel == 1)
        {
            LevelManager lm = FindObjectOfType<LevelManager>();
            if (lm != null)
            {
                Transform textTransform = lm.currentLevelObject.transform.Find("Instruction Text");
                if (textTransform != null)
                {
                    textTransform.gameObject.SetActive(false);
                }
            }
        }
        if (connectionManager.activeRings.Count == 0)
        {
            //cameraFollow.ZoomCam(resultTransform);
            //StartCoroutine(GameWonDelay());
            inputState = InputState.Disabled;
            //StartCoroutine(GameFailedDelay());
        }
        //only one ring left means no more possibility
        else if (connectionManager.activeRings.Count == 1)
        {
            //inputState = InputState.Wait;
            //print("Fail1");
            //FindObjectOfType<RocketGroup>().gameObject.SetActive(false);
            StartCoroutine(GameFailedDelay());
        }
        else
        {
            inputState = InputState.Ready;
        }
    }

    public void RocketLaunched()
    {
        print("Launced");
        int remainingToLaunch = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].playerState != PlayerState.Launched)
            {
                remainingToLaunch++;
            }
        }
        if (remainingToLaunch == 0)
        {
            inputState = InputState.Disabled;
            StartCoroutine(GameWonDelay());
        }
    }
    public void AllFredYetToLaunch()// called by player script after merge, use this to check if 
        //it is the last step and if still drowning game failed
    {
        undoButton.SetActive(false);
    }
}
public enum InputState
{
    Wait,
    Ready,
    Disabled
}
public enum CurrentStatus
{
    Null,
    Partial,
    Complete
}
