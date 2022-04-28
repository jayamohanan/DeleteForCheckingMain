using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

public class ddddelete : MonoBehaviour
{

    public GameObject prefab;
    public GameObject targetPrefab;
    [Button("Jaya")]
    private void Jaya()
    {
#if UNITY_EDITOR
        for (int i = 0; i < transform.childCount; i++)
        {
            print("i = "+i);
            Transform levelParent = transform.GetChild(i);
            Transform target = levelParent.Find("Target Outline");
            Transform dotsParent = levelParent.Find("Dots");
            List<Vector3> poss = new List<Vector3>();
            int count = dotsParent.childCount;
            for (int j  = 0; j < dotsParent.childCount; j++)
            {
                poss.Add(dotsParent.GetChild(j).position);
            }
            for (int j = 0; j < poss.Count; j++)
            {
              GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                obj.transform.position = poss[j];
                obj.transform.SetParent(dotsParent);
            }

            Vector3 targetPos = target.position;
            //DestroyImmediate(target.gameObject);
            GameObject newTarget = PrefabUtility.InstantiatePrefab(targetPrefab) as GameObject;
            newTarget.transform.position = targetPos;
            newTarget.transform.SetParent(levelParent);
            levelParent.gameObject.GetComponentInChildren<DotsParent>().targetObject = newTarget.gameObject;
        }
#endif
    }
    [Button("Rename")]
    private void Rename()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).name = "Level " + (i + 1).ToString();
        }
    }
    [Button("AddList")]
    private void AddList()
    {
        
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child =  transform.GetChild(i);
            DotsParent dotsParent = child.gameObject.GetComponentInChildren<DotsParent>();
            dotsParent.dotsList = new List<Transform>();

            for (int j = 0; j < dotsParent.transform.childCount; j++)
            {
                dotsParent.dotsList.Add(dotsParent.transform.GetChild(j));
            }
        }
    }
}
