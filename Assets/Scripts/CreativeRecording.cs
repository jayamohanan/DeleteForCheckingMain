using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreativeRecording : MonoBehaviour
{
    public bool isRecording;
    public Canvas handCanvas;
    private Camera mainCamera;
    public GameObject levelNumberBlock;
    void Awake()
    {
        if (isRecording)
        {
            handCanvas.gameObject.SetActive(true);
        }
        else
        {
            handCanvas.gameObject.SetActive(false);
        }
#if !UNITY_EDITOR
 handCanvas.gameObject.SetActive(false);
            levelNumberBlock.SetActive(true);
#endif
    }
}
