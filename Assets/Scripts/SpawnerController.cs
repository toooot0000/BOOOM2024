using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnerController: MonoBehaviour{

    public TetrisSpawner spawner;
    
    public void StartSpawn(){
        TetrisController.Shared.ControlledBlockLocked += (i, block) => {
            spawner.Spawn(i);
        };
        spawner.Spawn(0);
        spawner.Spawn(1);
    }
}