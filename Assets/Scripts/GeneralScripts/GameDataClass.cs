using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataClass : MonoBehaviour
{
    public static GameDataClass Instance;
    public float escapeSpeed = 0.2f;
    public float escapeDelta = 0.2f;
    public GameObject rocketExplosionPrefab;
    public GameObject rocketIgnitionPrefab;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

        }
    }
}
