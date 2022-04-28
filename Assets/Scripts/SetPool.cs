using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

public class SetPool : MonoBehaviour
{
    public float poolWidth;
    public float poolHeight;
    public float poolDepth;
    //public float bankWidth;

    private float bankExtend = 5;

    [Header("Pool Walls")]
    public Transform leftWall;
    public Transform rightWall;
    public Transform frontWall;
    public Transform backWall;
    [Header("Water Level")]
    public Transform waterLevel;
    public Transform poolBottom;

    [Header("Pool Banks")]
    public Transform leftPoolBank;
    public Transform rightPoolBank;
    public Transform frontPoolBank;
    public Transform backPoolBank;

    [Button("Adjust Size")]
    private void AdjustPoolSize()
    {
#if UNITY_EDITOR
        bankExtend = 5;
        float halfWidth = poolWidth / 2f;
        float halfHeight = poolHeight / 2f;
        leftWall.localScale = new Vector3(1, poolDepth, poolHeight);
        leftWall.position = new Vector3(-halfWidth, 0.1f, 0);
        leftPoolBank.position = leftWall.position;
        leftPoolBank.localScale = new Vector3(bankExtend, 1,poolHeight+10);

        rightWall.localScale = new Vector3(1, poolDepth, poolHeight);
        rightWall.position = new Vector3(halfWidth, 0.1f, 0);
        rightPoolBank.position = rightWall.position;
        rightPoolBank.localScale = new Vector3(bankExtend, 1, poolHeight+10);

        frontWall.localScale = new Vector3(1, poolDepth, poolWidth);
        frontWall.position = new Vector3(0,0.1f, halfHeight);
        frontPoolBank.position = frontWall.position;
        frontPoolBank.localScale = new Vector3(poolWidth, 1, bankExtend);

        backWall.localScale = new Vector3(1, poolDepth, poolWidth);
        backWall.position = new Vector3(0,0.1f, -halfHeight);
        backPoolBank.position = backWall.position;
        backPoolBank.localScale = new Vector3(poolWidth, 1, bankExtend);


        poolBottom.position = Vector3.up * -poolDepth;
        poolBottom.localScale = new Vector3(poolWidth, 1, poolHeight);
        waterLevel.localScale = new Vector3(poolWidth/10f, 1, poolHeight/10f);

        PrefabUtility.RecordPrefabInstancePropertyModifications(leftWall);
        PrefabUtility.RecordPrefabInstancePropertyModifications(rightWall);
        PrefabUtility.RecordPrefabInstancePropertyModifications(frontWall);
        PrefabUtility.RecordPrefabInstancePropertyModifications(backWall);
        PrefabUtility.RecordPrefabInstancePropertyModifications(waterLevel);
#endif
    }

}
