using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameManager gameManager;
    private Transform target;
    public Vector3 offsetPos;
    [HideInInspector] public bool followTarget;
    Vector3 initialPosition;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    //private void OnEnable()
    //{
    //    gameManager.LevelStartedEvent += OnLevelLoaded;
    //    gameManager.LevelOverEvent += OnLevelOver;
    //}
    //private void OnDisable()
    //{
    //    gameManager.LevelStartedEvent -= OnLevelLoaded;
    //    gameManager.LevelOverEvent -= OnLevelOver;

    //}
    Vector3 targetPos;
    Vector3 speed = Vector3.zero;
    public void ZoomCam(Transform player)
    {

        target = player;
        Vector3 closePosition = player.position + offsetPos;
        transform.DOMove(closePosition, 1).OnComplete(StartFollow);
    }
    public void StartFollow()
    {
        followTarget = true;
        initialPosition = transform.position;
    }
    void LateUpdate()
    {
        if (followTarget)
        {
            targetPos = target.position + offsetPos;
            //targetRot = target.localEulerAngles - offsetPos;
            //if (freezeX)
            //    targetPos.x = initialPosition.x;
            //if (freezeY)
            targetPos.y = initialPosition.y;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref speed, 0.2f);
        }
    }
}
