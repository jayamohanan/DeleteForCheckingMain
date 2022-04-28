using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PackPositionScript : MonoBehaviour
{
    public List<Transform> packs;
    public List<PositionList> allPositionList = new List<PositionList>();

    [Button("Add Positions")]
    public void AddPositions()
    {
        allPositionList = new List<PositionList>();
        for (int i = 0; i < packs.Count; i++)
        {
            PositionList pl = new PositionList();
            pl.pList = new List<Vector3>();
            for (int j = 0; j < (i+2); j++)
            {
                Transform child = packs[i].transform.GetChild(j);
                pl.pList.Add(child.localPosition);
            }
            allPositionList.Add(pl);
        }
        print(allPositionList.Count);
    }
    //[Button("Get Count")]
    //private void GetCount()
    //{
    //    if (allPositionList == null)
    //        print("Null");
    //    print(allPositionList.Count);
    //}
    public List<Vector3> GetPackPositions(int packCount)
    {
        return allPositionList[packCount - 2].pList;
    }
}
[System.Serializable]
public class PositionList
{
    public List<Vector3> pList;
}
