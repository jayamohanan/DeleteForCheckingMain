using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedTexture : MonoBehaviour
{
    public float speedX;
    public float speedY;
    private float currX;
    private float currY;

    private Renderer r;
    private void Awake()
    {
        r = GetComponent<Renderer>();
    }
    void Start()
    {
        currX = r.material.mainTextureOffset.x;
        currY = r.material.mainTextureOffset.y;
    }

    // Update is called once per frame
    void Update()
    {
        currX += Time.deltaTime * speedX;
        currY += Time.deltaTime * speedY;
        r.material.SetTextureOffset("_MainTex", new Vector2(currX, currY));
    }
} 
