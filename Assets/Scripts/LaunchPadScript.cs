using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPadScript : MonoBehaviour
{

    private GameManager gameManager;
    private ConnectionManager connectionManager;
    private LogicManager logicManager;
    [HideInInspector] public Transform myRing;
    private float nearDist = 0.2f;
    private int poolWallLayer;
    GameDataClass gdc;
    [HideInInspector] public PlayerState playerState;
    private GameObject swimSoundObject;
    private void Awake()
    {
        gdc = FindObjectOfType<GameDataClass>();
        poolWallLayer = LayerMask.NameToLayer("Pool Wall");
        poolWallLayer = 1 << poolWallLayer;
        gameManager = FindObjectOfType<GameManager>();
        connectionManager = FindObjectOfType<ConnectionManager>();
        logicManager = FindObjectOfType<LogicManager>();
    }
    private void OnEnable()
    {
        //gameManager.LevelStartedEvent += OnLevelStarted;
        //gameManager.LevelDataReadyEvent += OnLevelDataReady;
        //connectionManager.MergeStartedEvent += OnMergeStarted;
        connectionManager.MergeCompletedEvent += OnMergeCompleted;
    }
    private void OnDisable()
    {
        //gameManager.LevelStartedEvent -= OnLevelStarted;
        //gameManager.LevelDataReadyEvent -= OnLevelDataReady;
        //connectionManager.MergeStartedEvent -= OnMergeStarted;
        connectionManager.MergeCompletedEvent -= OnMergeCompleted;

    }
    private void OnMergeCompleted(Transform mergedRing)
    {
        float dist = Vector3.Distance(mergedRing.position, transform.position);
        if (dist <= nearDist)
        {
            RocketGroup rocketGroup = mergedRing.GetComponentInChildren<RocketGroup>();
            if (rocketGroup.launchPad == null)//Unused launchpad nearby
            {
                Vector3 pos = mergedRing.transform.position;
                pos.x = transform.position.x;
                pos.z = transform.position.z;
                mergedRing.position = pos;

                myRing = mergedRing;
                connectionManager.RemoveRingFromActiveList(mergedRing.gameObject);//when taken by person
                rocketGroup.launchPad = transform;
                //logicManager.RocketLaunched();
                //rocketGroup.transform.SetParent(transform);
                StartCoroutine(rocketGroup.Launch());
                StartCoroutine(DisableLaunchPad());

                //StartCoroutine(LaunchRocket(rocketGroup));

                LaunchPadScript[] playerScripts = FindObjectsOfType<LaunchPadScript>();
                bool allLaunched = true;
                for (int i = 0; i < playerScripts.Length; i++)
                {
                    if(playerScripts[i].playerState == PlayerState.launchPadUnused)
                    {
                        allLaunched = false;
                        break;
                    }
                }
                if (allLaunched)
                {
                    logicManager.AllFredYetToLaunch();
                }
            }
        }
    }
    private IEnumerator DisableLaunchPad()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
    //private IEnumerator LaunchRocket(RocketGroup rg)
    //{
    //    yield return new WaitForSeconds(1f);//Engine start
    //    rg.transform.DOMoveY(20,1.5f).OnComplete(()=> { 
    //    playerState = PlayerState.Launched;
    //    logicManager.RocketLaunched();
    //    });
    //}
    public IEnumerator DrownToDeath()
    {

        yield return new WaitForSeconds(0.5f);
        transform.DOMoveY(-1.12f, 0.5f).SetEase(Ease.OutSine).OnComplete(()=> {
            AudioManager.Instance.PlayLastBreathBubble();
        });
    }
}
public enum PlayerState
{
    launchPadUnused,
    Swimming,
    Launched
}
