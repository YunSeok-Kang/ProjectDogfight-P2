using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : AIController
{
    public bool isCanThrust = true;
    public Car car = null;

    protected virtual void Awake()
    {
        if (car == null)
        {
            car = GetComponent<Car>();
        }
    }

    protected virtual void Update()
    {
        if (isCanThrust)
        {
            car.Thrust();
        }
    }
}

