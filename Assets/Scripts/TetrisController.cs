﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TetrisController: MonoBehaviour{
    private static TetrisController _shared;
    public static TetrisController Shared => _shared;

    public TetrisBlock[] ControlledBlock{ get; } = new TetrisBlock[]{ null, null };

    public event Action<int, TetrisBlock> ControlledBlockLocked;

    public readonly Dictionary<TetrisBlock.Command, KeyCode>[] ControlMap =
        new Dictionary<TetrisBlock.Command, KeyCode>[]{
            new Dictionary<TetrisBlock.Command, KeyCode>(){
                { TetrisBlock.Command.Left, KeyCode.A },
                { TetrisBlock.Command.Right, KeyCode.D },
                { TetrisBlock.Command.Rotate, KeyCode.W },
                { TetrisBlock.Command.Down, KeyCode.S },
                { TetrisBlock.Command.ForceFall, KeyCode.F},
            },
            new Dictionary<TetrisBlock.Command, KeyCode>(){
                { TetrisBlock.Command.Left, KeyCode.LeftArrow },
                { TetrisBlock.Command.Right, KeyCode.RightArrow },
                { TetrisBlock.Command.Rotate, KeyCode.UpArrow },
                { TetrisBlock.Command.Down, KeyCode.DownArrow },
                { TetrisBlock.Command.ForceFall, KeyCode.RightControl},
            },
        };

    private void Awake(){
        if (_shared != null) Destroy(this);
        else _shared = this;
    }

    private void Update(){
        foreach (var (idx, map) in ControlMap.Enumerated()){
            foreach (var (key, value) in map){
                if (Input.GetKeyDown(value) && ControlledBlock[idx] != null){
                    ControlledBlock[idx].TakeCommand(key);
                }
            }
        }
    }

    public static void SetControlledBlock(int playerIndex, TetrisBlock block){
        Assert.IsTrue(playerIndex is 0 or 1, "playerIndex is not 0 or 1");
        Shared.ControlledBlock[playerIndex] = block;
        block.Locked += (b) => {
            Shared.ControlledBlock[b.PlayerIndex] = null;
            Shared.ControlledBlockLocked?.Invoke(b.PlayerIndex, b);
        };
    }
}