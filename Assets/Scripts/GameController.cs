using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public enum GameState{
    PreStart,
    Idle,
    End,
    Pause,
}

public enum SideEffect{
    CantSpin,
    OpponentDoublePoints,
    Length,
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

public delegate void BeforeGameStart();

public class GameController: MonoBehaviour{
    // Signleton
    public static GameController Shared;

    // Private
    private float _timer = 0;
    
    // Public
    public int curLevel = 1;
    public float totalTimeInSec = 300;
    public float comboIntervalInSec = 1;
    public TetrisGrid grid;    
    public TetrisSpawner spawner;

    public readonly Dictionary<int, int> PointOfClearLineNum = new(){
        { 1, 100 },
        { 2, 300 },
        { 3, 500 },
        { -1, 800 },
    };

    public readonly int[] PlayerPoints = {0, 0};
    public readonly DateTime?[] LastClearTime ={ null, null };
    public readonly int[] ComboNums ={ 0, 0 };
    public readonly HashSet<SideEffect>[] SideEffects = new HashSet<SideEffect>[2] { new(), new()};

    /// <summary>
    /// 游戏状态，初始为PreStart
    /// </summary>
    public  Bindable<GameState> State{ get; } = new(GameState.PreStart);
    
    // Events
    /// <summary>
    /// (index, newPoint, comboNum)
    /// </summary>
    public event Action<int, int, int> PlayerPointChanged;
    
    /// <summary>
    /// (playerIndices) 有可能有两个
    /// </summary>
    public event Action<int[]> PlayerOverflow;

    /// <summary>
    /// (playerIndex, NextBlockType) 
    /// </summary>
    public event Action<int, TetrisType> PlayerNextBlockUpdated;

    /// <summary>
    /// (playerIndex, (sideEffect, remainingTime))
    /// </summary>
    public event Action<int, (SideEffect, float)[]> PlayerSideEffectsUpdated;

    /// <summary>
    /// Time out
    /// </summary>
    public event Action TimerOut;

    private void Awake(){
        if (Shared != null) Destroy(this);
        else Shared = this;
    }

    private void Start(){
        grid.LineCleared += (tetrisGrid, results) => {
            foreach (var r in results){
                UpdateCombo(r);
                PlayerPoints[r.PlayerIndex] += PointFromClearedLineNum(r.NumOfClearedLine);
                PlayerPointChanged?.Invoke(r.PlayerIndex, PlayerPoints[r.PlayerIndex], ComboNums[r.PlayerIndex]);
            }
        };
        grid.BlockOverflowByPlayer += (tetrisGrid, ints) => {
            GameEnd(ints);
        };

        spawner.PlayerNextTetrisTypeUpdated += (i, type) => {
            PlayerNextBlockUpdated?.Invoke(i, type);
        };
        
        TetrisController.Shared.ControlledBlockLocked += (i, block) => {
            spawner.Spawn(i);
        };

        GameStart();
    }

    private void Update(){
        if (State.Value == GameState.Idle){
            _timer += Time.deltaTime;
            if (_timer >= totalTimeInSec){
                TimeOut();
            }
        }
    }

    private void TimeOut(){
        State.Value = GameState.End;
        TimerOut?.Invoke();
    }

    private void GameEnd(int[] overflowPlayerIndices){
        // Game End
        State.Value = GameState.End;
        PlayerOverflow?.Invoke(overflowPlayerIndices);
    }

    private void GameStart(){
        // Start the spawn timer
        State.Value = GameState.Idle;
        spawner.OnGameStart();
        
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

    private void UpdateCombo(TetrisGrid.LineClearResult r){
        var now = DateTime.Now;
        if (LastClearTime[r.PlayerIndex] is{ } last){
            if (now.Subtract(last).TotalSeconds < comboIntervalInSec){ // we have a combo
                ComboNums[r.PlayerIndex]++;
            } else{
                ComboNums[r.PlayerIndex] = 0;
            }
        }
        LastClearTime[r.PlayerIndex] = now;
    }
}