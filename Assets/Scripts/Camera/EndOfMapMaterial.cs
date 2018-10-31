using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfMapMaterial : MonoBehaviour
{
    public Transform airplane;
    public MeshRenderer matRender;

    private float positionOfEnd;
    private float positionOfStart;


    public float endDistance = 200f;
    public float startDistance = 400;

    private void Start()
    {
        if (transform.position.x > 0)
        {
            positionOfEnd = transform.position.x - endDistance;
            positionOfStart = transform.position.x - startDistance;
        }
        else
        {
            positionOfEnd = transform.position.x + endDistance;
            positionOfStart = transform.position.x + startDistance;
        }
    }


    private void Update()
    {
        GetBlah();
    }

    public void GetBlah()
    {
        float targetX = airplane.position.x;
        float f = (positionOfEnd - targetX) / (positionOfEnd - positionOfStart);
        float a = Mathf.Lerp(0.3f, 0, f);
        Debug.Log(a);
        
        if(a<=0)
        {
            matRender.material.color =
            new Color(0,0,0,0);


        }
        else
        {
            matRender.material.color =
            new Color(
            0,
            0.5f,
            1,
            a);
        }
    }
}
