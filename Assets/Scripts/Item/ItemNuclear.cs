using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNuclear : Item
{
    public GameObject nuclearBomb;
    //public Transform spawnPosition;

    protected  override  void Awake()
    {
      //  spawnPosition = FindObjectOfType<NuclearSpawnPosition>().transform;
        base.Awake();
    }

    public override void Use(VoxObject target)
    {
        var newBomb = Instantiate(nuclearBomb);
        newBomb.transform.position = transform.position;
        //newBomb.transform.position = spawnPosition.position;
        base.Use(target);
    }
}
