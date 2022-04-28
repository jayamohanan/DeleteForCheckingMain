using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class RingsParent : MonoBehaviour
{
    public List<Transform> ringsList;//
    public GameObject targetObject;

    [Button("Set Target")]
    void SetTarget()
    {
        Vector3 targetPos = ringsList[0].position;
        for (int i = 1; i < ringsList.Count; i++)
        {
            targetPos = (targetPos + ringsList[i].position) / 2;
        }
        targetObject.transform.position = targetPos;
    }
}
