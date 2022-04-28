using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    private LogicManager logicManager;
    Camera mainCam;
    int poolRingLayer;
    int poolWaterLayer;
    /*[SerializeField] */private Transform p1;
   /* [SerializeField] */private Transform p2;
    LineRenderer line;
     public LineState lineState;
    public event System.Action<Transform> MergeCompletedEvent;
    //public event System.Action MidPointReachedEvent;
    public event System.Action MergeStartedEvent;
    public GameObject mergedGhostPrefab;
    public Stack<MergeOp> undoStack;
    /*[HideInInspector] */public List<GameObject> activeRings;//
    GameManager gameManager;
    private float lineHeight = 0.01f;
    public GameObject p1Circle;
    public GameObject p2Circle;
    float ringNormalScaleFactor = 1f;
    float ringHighlightScaleFactor = 1.15f;
    int undoStopValue =0;
    [HideInInspector] public List<List<Transform>> mergedList;
    private PackPositionScript allPositionList;
    private void Awake()
    {
        allPositionList = FindObjectOfType<PackPositionScript>();
        activeRings = new List<GameObject>();
        undoStack = new Stack<MergeOp>();
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        int layerNumber = LayerMask.NameToLayer("Rocket Group");
        poolRingLayer = 1 << layerNumber;
        layerNumber = LayerMask.NameToLayer("Pool Water");
        poolWaterLayer = 1 << layerNumber;
        mainCam = Camera.main;
        logicManager = FindObjectOfType<LogicManager>();
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnEnable()
    {
        gameManager.LevelDataReadyEvent += OnLevelDataReady;
    }
    private void OnDisable()
    {
        gameManager.LevelDataReadyEvent -= OnLevelDataReady;
    }
    private void OnLevelDataReady(int levelNUmber)
    {
        activeRings.AddRange(GameObject.FindGameObjectsWithTag("Rocket Group"));
    }
    private void Update()
    {
        if (logicManager.inputState != InputState.Ready)
        {
            return;
        }
        if (lineState == LineState.Fixed)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (lineState == LineState.Free)
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray.origin, ray.direction, out hit, 100, poolRingLayer)) 
                {
                    RocketGroup ringScript = hit.collider.GetComponent<RocketGroup>();
                    if (ringScript != null)
                    {
                        if (ringScript.launchPad == null)
                        {
                            lineState = LineState.Variable;
                            p1 = hit.collider.transform;
                            p1Circle.SetActive(true);
                            p1Circle.transform.position = p1.position;
                            p1.localScale = Vector3.one * ringHighlightScaleFactor;
                            AudioManager.Instance.PlayWaterTouch();
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (lineState == LineState.Variable)
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, poolRingLayer))
                {
                    RocketGroup ringScript = hit.collider.GetComponent<RocketGroup>();
                    if (ringScript != null)
                    {
                        if (ringScript.launchPad == null)
                        {
                            if (hit.collider.transform != p1)
                            {
                                p2 = hit.collider.transform;
                                p2Circle.SetActive(true);
                                p2Circle.transform.position = p2.position;
                                p2.localScale = Vector3.one * ringHighlightScaleFactor;
                                StartCoroutine(Merge(p1, p2));
                            }
                            else
                            {
                                CancelLine();
                            }
                        }
                        else
                        {
                            CancelLine();
                        }
                    }
                }
                else
                {
                    CancelLine();
                }
            }
        }
        if (lineState == LineState.Variable)
        {
            if (!line.enabled)
            {
                line.enabled = true;
                Vector3 firstPosition = p1.position;
                firstPosition.y = lineHeight;
                line.SetPosition(0, firstPosition);
                //SetGhost(p1.gameObject);
            }
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, poolWaterLayer))
            {
                Vector3 position2 = hit.point;
                position2.y = lineHeight;
                line.SetPosition(1, position2);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
#if UNITY_EDITOR
            EditorApplication.isPaused = true;
#endif
        }
    }
    private void CancelLine()
    {
        p1.localScale = Vector3.one*ringNormalScaleFactor;
        p1 = null;
        p1Circle.SetActive(false);
        p2Circle.SetActive(false);
        lineState = LineState.Free;
        line.enabled = false;
    }
    private void SetGhost(GameObject prefab)
    {
        Vector3 position = prefab.transform.position;
        GameObject ghost = Instantiate(prefab, position, Quaternion.identity);
        ghost.GetComponentInChildren<Collider>().enabled = false;
        ghost.layer = 0;
        float tweenTime = 0.8f;
        ghost.transform.DOScale(2f, tweenTime).OnComplete(() => { Destroy(ghost); });
        SpriteRenderer sr = ghost.GetComponent<SpriteRenderer>();
        DOTween.ToAlpha(() => sr.color, (x) => sr.color = x, 0, tweenTime);
    }
    private IEnumerator Merge(Transform p1, Transform p2)
    {
        undoStopValue++;
        AudioManager.Instance.PlaySplashMergeStart();
        Collider col = p1.gameObject.GetComponentInChildren<Collider>();
        if(col!=null)
        col.enabled = false;
        lineState = LineState.Fixed;

        MergeStartedEvent?.Invoke();
        Vector3 linePosition0 = p1.transform.position;
        linePosition0.y = lineHeight;
        Vector3 linePosition1 = p2.transform.position;
        linePosition1.y = lineHeight;
        line.SetPosition(0, linePosition0);
        line.SetPosition(1, linePosition1);
        yield return new WaitForSeconds(0.25f);
        p1Circle.SetActive(false);
        p2Circle.SetActive(false);

        Vector3 midPoint = (p1.position + p2.position) / 2f;
        float time = 0.5f;
        p1.transform.DOMove(midPoint, time).SetEase(Ease.Linear);
        p2.transform.DOMove(midPoint, time).SetEase(Ease.Linear).OnComplete(OnMidPointReached);
        line.enabled = false;
        MergeOp currentMo = new MergeOp(p2.gameObject, midPoint, p1.gameObject, p1.position, p2.gameObject, p2.position);
        undoStack.Push(currentMo);
        RemoveRingFromActiveList(p1.gameObject);// Remove p1 merged
    }
    private void OnMidPointReached()
    {
        undoStopValue--;
            p1.localScale =  Vector3.one * ringNormalScaleFactor;
            p2.localScale = Vector3.one* ringNormalScaleFactor;
        AudioManager.Instance.PlaySplashMergeMeet();
        RocketGroup rg1 = p1.GetComponentInChildren<RocketGroup>();
        RocketGroup rg2 = p2.GetComponentInChildren<RocketGroup>();
        rg2.rockets.AddRange(rg1.rockets);

        int childCount = rg1.transform.childCount;

        for (int i = childCount-1; i >=0; i--)
        {
            Transform child = rg1.transform.GetChild(i);
            child.SetParent(rg2.transform);
        }
        StartCoroutine(PrintChild(rg2));

        rg1.enabled = false;
        Destroy(rg1);

        List<Vector3> positionList = allPositionList.GetPackPositions(rg2.transform.childCount);
        for (int i = 0; i < rg2.transform.childCount; i++)
        {
            Transform child = rg2.transform.GetChild(i);
            child.localPosition = positionList[i];
        }

        //p1.gameObject.SetActive(false);
        p2.GetComponentInChildren<Collider>().enabled = true;
        lineState = LineState.Free;
        MergeCompletedEvent?.Invoke(p2);
        p2.DORotate(Vector3.up * (p2.localEulerAngles.y + 179), 0.5f);
    }
    private IEnumerator PrintChild(RocketGroup rg2)
    {
        yield return null;
    }
    public void Demerge(MergeOp mo)
    {
        logicManager.inputState = InputState.Wait;
        mo.resultObject.SetActive(false);
        mo.parent1.SetActive(true);
        mo.parent2.SetActive(true);
        float time = 0.5f;
        mo.parent1.transform.DOMove(mo.parent1Pos, time).SetEase(Ease.Linear);
        mo.parent2.transform.DOMove(mo.parent2Pos, time).SetEase(Ease.Linear).OnComplete(OnDemergeComplete);
        mo.parent1.GetComponentInChildren<Collider>().enabled = true;
        mo.parent2.GetComponentInChildren<Collider>().enabled = true;
        if (!activeRings.Contains(mo.parent1))
        {
            activeRings.Add(mo.parent1);
        }
        if (!activeRings.Contains(mo.parent2))
        {
            activeRings.Add(mo.parent2);
        }
    }
    public void RemoveRingFromActiveList(GameObject ring)
    {
        if (activeRings.Contains(ring))
        {
            activeRings.Remove(ring);
        }
    }
    private void OnDemergeComplete()
    {
        logicManager.inputState = InputState.Ready;
    }
    public void Undo()
    {
        if(undoStopValue > 0)
        {
            return;
        }
        if(logicManager.inputState != InputState.Ready)
        {
            return;
        }
        if (undoStack.Count == 0)
            return;
        MergeOp undoMo = undoStack.Pop();
       
        Demerge(undoMo);
    }
    private bool IsInList(Transform checkTransform)
    {
        for (int i = 0; i < mergedList.Count; i++)
        {
            List<Transform> tempList = mergedList[i];
            if (tempList.Contains(checkTransform))
            {
                return true;
            }
        }
        return false;
    }
}
public enum LineState
{
    Free,
    Variable,
    Fixed
}
public class MergeOp
{
    public GameObject resultObject;
    public Vector3 resultPosition;
    public GameObject parent1;
    public Vector3 parent1Pos;
    public GameObject parent2;
    public Vector3 parent2Pos;
    public MergeOp(GameObject resultObject, Vector3 resultPosition,GameObject parent1, Vector3 parent1Pos, GameObject parent2, Vector3 parent2Pos)
    {
        this.resultObject = resultObject;
        this.resultPosition = resultPosition;
        this.parent1 = parent1;
        this.parent1Pos = parent1Pos;
        this.parent2 = parent2;
        this.parent2Pos = parent2Pos;   
    }   
   


}
