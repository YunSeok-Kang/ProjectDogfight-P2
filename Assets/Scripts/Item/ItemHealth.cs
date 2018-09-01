using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : Item {

    public float healthIncreasePoint;

    public override void Use(VoxObject target)
    {
        target.HP += healthIncreasePoint;
        base.Use(target);
    }
}
