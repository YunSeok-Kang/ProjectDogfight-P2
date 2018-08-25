using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Vehicle
{
    public float speed;
    [SerializeField]
    private Vector3 towardVector;



    protected override bool Init()
    {
        base.Init();
        towardVector = transform.forward;
        return true;
    }

    private new void Update()
    {
        Thrust(towardVector, speed);
    }



    /// <summary>
    /// direction 향해 움직이는 함수
    /// </summary>
    /// <param name="direction"></param>
    public void Thrust(Vector3 direction, float speed)
    {
        transform.localPosition += direction * speed * Time.deltaTime;
    }

    /// <summary>
    /// 맵 끝에 닿았는지 검사하여 운동 방향 바꿈.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            towardVector = towardVector * -1;
        }
    }
}
