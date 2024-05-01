using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum TetrisType{
    I,
    J,
    L,
    O,
    S,
    T,
    Z
}

public class TetrisSpawner : MonoBehaviour{

    [Serializable]
    struct Pair{
        public TetrisType type;
        public GameObject blockPrefab;
    }
    
    [SerializeField] private GameObject[] blockPrefabs;
    [SerializeField] private Pair[] prefabs;
    
    private TetrisGrid _tetrisManager;
    private int _prevIndex = 0;
    public readonly int[] SpawnBatch = {0, 0};
    

    private void Awake(){
        _tetrisManager = GetComponent<TetrisGrid>();
    }

    public void Spawn(int playerIndex){
        var count = prefabs.Length;
        var prefab = prefabs[Random.Range(0, count)];
        Spawn(playerIndex, prefab.type);
    }

    public void Spawn(int playerIndex, TetrisType type){

        var prefabPair = prefabs.FirstOrDefault(p => p.type == type);
        if (prefabPair.blockPrefab == null){
            Debug.Log($"Can't find prefab of type {type}");
            return;
        }

        SpawnBatch[playerIndex]++;
        
        var inst = Instantiate(prefabPair.blockPrefab, _tetrisManager.transform).GetComponent<TetrisBlock>();
        var position = new Vector2Int(playerIndex == 0 ? 3 : 7, -1);// _tetrisManager.TotalSize.x / 2 - 1, 1);
        inst.GridPosition = position;
        inst.SpawnBatch = SpawnBatch[playerIndex];
        foreach (var spr in inst.SingleBlocks.Select(b => b.Spr)){
            spr.color = playerIndex == 0 ? Color.red : Color.blue;
        }

        inst.PlayerIndex = playerIndex;
        _tetrisManager.Manage(inst);
        TetrisController.SetControlledBlock(playerIndex, inst);
    }
}