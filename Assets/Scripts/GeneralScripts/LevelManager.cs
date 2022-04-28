using System.Collections;


using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

public class LevelManager : MonoBehaviour
{
    public bool loadLastLevel;
    GameManager gameManager;
    [HideInInspector] public int currentLevelDataIndex;
    private int firstTimeOnlyLevelCount= 1;
    [HideInInspector] public GameObject  currentLevelObject;
    public List<GameObject> allLevels;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnEnable()
    {
        gameManager.LevelSetEvent += OnLevelSet;
    }
    private void OnDisable()
    {
        gameManager.LevelSetEvent -= OnLevelSet;
    }
    public void OnLevelSet(int levelIndex)
    {
#if UNITY_EDITOR
        if (loadLastLevel)
        {
            firstTimeOnlyLevelCount = 0;
            GameObject obj = allLevels[allLevels.Count-1];
            allLevels.Clear();
            allLevels.Add(obj);
        }
#endif
        int levelsCount = allLevels.Count;
        int subIndex = levelIndex;
        if (subIndex > levelsCount)
            subIndex = (subIndex - firstTimeOnlyLevelCount - 1) % (levelsCount - firstTimeOnlyLevelCount) + firstTimeOnlyLevelCount;
        else
            subIndex--;
        currentLevelDataIndex = subIndex;
        currentLevelObject = Instantiate(allLevels[currentLevelDataIndex]);
    }
}
