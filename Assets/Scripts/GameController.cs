using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState{
    PreStart,
    Idle,
    End,
    Pause,
}

public class Bindable<T>{
    private T _value;

    public event Action<T, T> ValueChangedFromTo; 

    public T Value{
        set{
            var o = _value;
            _value = value;
            ValueChangedFromTo?.Invoke(o, _value);
        }
        get => _value;
    }

    public Bindable(T init){
        _value = init;
    }
}

public class GameController: MonoBehaviour{
    // Signleton
    public static GameController Shared;

    // Private
    private float _timer = 0;
    
    // Public
    public int curLevel = 1;
    public TetrisGrid grid;    
    public TetrisSpawner spawner;

    public readonly Dictionary<int, float> LevelToFallTick = new(){
        { 1, 0.8f },
        { 2, 0.7f },
        { 3, 0.6f },
        { 4, 0.5f },
        { 5, 0.4f },
        { 6, 0.34f },
    };

    public readonly Dictionary<int, int> PointOfClearLineNum = new(){
        { 1, 100 },
        { 2, 300 },
        { 3, 500 },
        { -1, 800 },
    };

    public readonly int[] PlayerPoints = {0, 0};
    public readonly DateTime?[] LastClearTime ={ null, null };
    public readonly int[] ClearCombo ={ 0, 0 };
    public  Bindable<GameState> State{ get; } = new(GameState.PreStart);
    
    // Events
    /// <summary>
    /// (index, newPoint)
    /// </summary>
    public event Action<int, int> PlayerPointChanged;

    private void Awake(){
        if (Shared != null) Destroy(this);
        else Shared = this;
    }

    private void Start(){
        grid.LineCleared += (tetrisGrid, results) => {
            foreach (var r in results){
                PlayerPoints[r.PlayerIndex] += PointFromClearedLineNum(r.NumOfClearedLine);
                PlayerPointChanged?.Invoke(r.PlayerIndex, PlayerPoints[r.PlayerIndex]);
            }
        };
        grid.BlockOverflowByPlayer += (tetrisGrid, ints) => {
            GameEnd(ints);
        };

        GameStart();
    }

    private void Update(){
        if (State.Value == GameState.Idle){
            _timer += Time.deltaTime;
            if (_timer >= 300){
                TimeOut();
            }
        }
    }

    private void TimeOut(){
        State.Value = GameState.End;
    }

    private void GameEnd(int[] overflowPlayerIndices){
        // Game End
        State.Value = GameState.End;
    }

    private void GameStart(){
        // Start the spawn timer
        State.Value = GameState.Idle;
        
        TetrisController.Shared.ControlledBlockLocked += (i, block) => {
            spawner.Spawn(i);
        };
        spawner.Spawn(0);
        spawner.Spawn(1);

    }

    public int PointFromClearedLineNum(int num){
        return num switch{
            >= 1 and < 4 => PointOfClearLineNum[num],
            >= 4 => 800,
            _ => 0
        };
    }
}