using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketGroup : MonoBehaviour
{
    public List<Transform> rockets;
    /*[HideInInspector] */
    public Transform launchPad;
    public RocketState state; 
    GameDataClass gdc;
    LogicManager logicManager;
    private void Awake()
    {
        gdc = FindObjectOfType<GameDataClass>();
        logicManager = FindObjectOfType<LogicManager>();
    }
    public IEnumerator Launch()
    {
        print("Launch coroutine");
        AudioManager.Instance.PlayLaunchSound();
        yield return new WaitForSeconds(1);//Engine start
        transform.DOMoveY(20, 1.5f).OnComplete(() =>
        {
        if (logicManager!=null)
                logicManager.RocketLaunched();
            state = RocketState.Launched;
        });
    }
    public IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.5f);
        transform.DOMoveY(0,0.1f).OnComplete(()=> {
            if (gdc.rocketExplosionPrefab != null)
            {
                GameObject obj = Instantiate(gdc.rocketExplosionPrefab, transform.position, Quaternion.identity);
                Destroy(obj, 3);
                Vector3 pos = transform.position;
                pos.y += 2;
                transform.position = pos;
                obj.transform.localScale = Vector3.one * 3;
                state = RocketState.Exploded;
            }
            gameObject.SetActive(false);
            AudioManager.Instance.PlayExplosionSound();

            logicManager.OnRocketExploded();
        });
    }
}
public enum RocketState
{
    NotLaunched,
    Ignition,
    Launched,
    Exploded
}
