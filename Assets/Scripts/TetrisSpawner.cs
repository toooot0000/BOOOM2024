using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class TetrisSpawner : MonoBehaviour{
    
    [SerializeField] private GameObject[] blockPrefabs;
    
    private TetrisGrid _tetrisManager;
    private int prevIndex = 0;

    private void Awake(){
        _tetrisManager = GetComponent<TetrisGrid>();
    }

    private void Update(){
        // TODO: DELETE THIS! TEST ONLY!
        if (Input.GetKeyDown(KeyCode.Space)){
            Spawn(prevIndex);
            prevIndex = 1 - prevIndex;
        }
    }

    public void Spawn(int playerIndex){
        var count = blockPrefabs.Length;
        var prefab = blockPrefabs[Random.Range(0, count)];
        var inst = Instantiate(prefab, _tetrisManager.transform).GetComponent<TetrisBlock>();
        var position = new Vector2Int(playerIndex == 0 ? 3 : 7, 1);// _tetrisManager.TotalSize.x / 2 - 1, 1);
        inst.GridPosition = position;
        foreach (var spr in inst.SingleBlocks.Select(b => b.Spr)){
            spr.color = playerIndex == 0 ? Color.red : Color.blue;
        }
        _tetrisManager.Manage(inst);
        TetrisController.SetControlledBlock(playerIndex, inst);
    }
}