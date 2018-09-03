using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Vehicle
{
    public float speed;
    [SerializeField]
    private float _direction = 1;


    protected override bool Init()
    {
        base.Init();
        return true;
    }

 
    /// <summary>
    /// direction 향해 움직이는 함수
    /// </summary>
    public void Thrust()
    {
        transform.position += transform.forward * _direction * speed * Time.deltaTime;
    }

    /// <summary>
    /// 맵 끝에 닿았는지 검사하여 운동 방향 바꿈.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall(Cars Only)"))
        {
            var thisRot = this.gameObject.transform.rotation;
            thisRot.y *= -1;
            this.gameObject.transform.rotation = thisRot;
           // _direction = _direction * -1;
        }
    }
}
