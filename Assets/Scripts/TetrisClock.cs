using System;
using UnityEngine;

public class TetrisClock: MonoBehaviour{
    private static TetrisClock _shared;
    public static TetrisClock Shared => _shared;

    public event Action BeforeMoveTick;
    public event Action MoveTick;
    public event Action AfterMoveTick;
    
    /// <summary>
    /// Control fall down interval.
    /// </summary>
    public float tickTime = .5f;

    private float _timer = 0;

    private void Awake(){
        if (_shared != null) Destroy(this);
        else _shared = this;
    }

    private void Update(){
        _timer += Time.deltaTime;
        if (!(_timer >= tickTime)) return;
        BeforeMoveTick?.Invoke();
        MoveTick?.Invoke();
        AfterMoveTick?.Invoke();
        _timer = 0;
    }
}