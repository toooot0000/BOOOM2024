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

public static class EnumExt{
    public static T Rand<T>() where T: Enum{
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(Random.Range(0, values.Length-1));
    }
}

public class TetrisSpawner : MonoBehaviour{

    [Serializable]
    struct Pair{
        public TetrisType type;
        public GameObject blockPrefab;
    }
    
    [SerializeField] private Pair[] prefabs;
    
    private TetrisGrid _tetrisManager;
    public readonly int[] SpawnBatch = {0, 0};
    [NonSerialized]
    public TetrisType[] Next ={ TetrisType.I, TetrisType.T };

    public event Action<int, TetrisType> PlayerNextTetrisTypeUpdated;

    private void Awake(){
        _tetrisManager = GetComponent<TetrisGrid>();
    }

    public void OnGameStart(){
        Next = new [] {RandType(), RandType()} ;
        PlayerNextTetrisTypeUpdated?.Invoke(0, Next[0]);
        PlayerNextTetrisTypeUpdated?.Invoke(1, Next[1]);
    }

    public void Spawn(int playerIndex){
        Spawn(playerIndex, Next[playerIndex]);
        Next[playerIndex] = RandType();
        PlayerNextTetrisTypeUpdated?.Invoke(playerIndex, Next[playerIndex]);
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

    private TetrisType RandType(){
        var count = prefabs.Length;
        return prefabs[Random.Range(0, count)].type;
    }
}