using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPropellantHoming : AmmoPropellant
{
    public GameObject target;
    //호밍할 때 각도를 트는 감도는 얼마나 되는가
    public float homingSensitivity;
    //얼마나 호밍 할 것인가
    public float homingTime;

    private float currentHomingTime;

    public override void Propel(Vector3 direction)
    {
        currentHomingTime = Time.time;
        StartCoroutine("Homing", target.transform);
    }

    private IEnumerator Homing(Transform targetTrans)
    {
        while (true)
        {
            if (target != null)
            {
                if (Time.time <= (currentHomingTime + homingTime))
                {
                    var relativePos = targetTrans.position - this.transform.position;
                    var newRot = Quaternion.LookRotation(relativePos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, newRot, homingSensitivity);
                }
            }
            transform.Translate(0, 0, force * Time.deltaTime, Space.Self);
            yield return new WaitForFixedUpdate();
        }
    }
}
