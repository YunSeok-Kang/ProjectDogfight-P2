using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNuclear : Item
{
    public GameObject nuclearBomb;
    public Transform spawnPosition;

    public override void Use(VoxObject target)
    {
        var newBomb = Instantiate(nuclearBomb);
        newBomb.transform.position = spawnPosition.position;
        base.Use(target);
    }
}
