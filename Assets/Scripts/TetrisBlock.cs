using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class TetrisBlock: MonoBehaviour{

    public enum Command{
        Left,
        Right,
        Down,
        Rotate,
    }
    
    [FormerlySerializedAs("occupiedBlocks")] [SerializeField]
    private Vector2Int[] relativeOccupiedPositions = new[]{
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
    };
    
    private TetrisGrid _grid;
    private Vector2Int _gridPosition;
    private bool _isLocked = false;
    
    public bool IsLocked => _isLocked;
    public Vector2Int GridPosition{
        get => _gridPosition;
        set{
            _gridPosition = value;
            UpdateSingleBlocksGridPositions();
        }
    }
    public Vector2Int[] RelativeOccupiedPositions{
        get => relativeOccupiedPositions;
        private set{
            relativeOccupiedPositions = value;
            UpdateSingleBlocksGridPositions();
        }
    }

    public int PlayerIndex{
        set{
            foreach (var s in SingleBlocks){
                s.PlayerIndex = value;
            }
        }
        get => SingleBlocks[0].PlayerIndex;
    }

    public IEnumerable<Vector2Int> OccupiedGridPositions => RelativeOccupiedPositions.Offset(GridPosition);

    private TetrisSingleBlock[] _singleBlocks;
    public TetrisSingleBlock[] SingleBlocks{
        get{
            if (_singleBlocks != null) return _singleBlocks;
            var sprites = GetComponentsInChildren<SpriteRenderer>();
            Assert.AreEqual(sprites.Length, relativeOccupiedPositions.Length, "Sprites length should be equal to blocks length");
            var array = OccupiedGridPositions.ToArray();
            _singleBlocks = sprites.Select((spr, i) => new TetrisSingleBlock(){
                Spr = spr,
                GridPosition = array[i],
                Parent = _grid
            }).ToArray();
            foreach (var spr in sprites){
                spr.transform.SetParent(_grid.transform);
            }
            return _singleBlocks;
        }
    }
    
    public event Action<TetrisBlock> Locked;
    public event Action<TetrisBlock, Vector2Int, Vector2Int> PositionChangedFromTo;
    public event Action<TetrisBlock, Vector2Int[], Vector2Int[]> OccupiedBlockChangedFromTo;

    private void Awake(){
        _grid = GetComponentInParent<TetrisGrid>();
    }

    private void Start(){
        TetrisClock.Shared.MoveTick += Fall;
        UpdateSingleBlocksGridPositions();
    }

    public void Move(bool left){
        var newPosition = GridPosition + (left ? Vector2Int.left : Vector2Int.right);
        MoveTo(newPosition);
    }

    public void Fall(){
        var newPosition = GridPosition + Vector2Int.up;
        MoveTo(newPosition);
    }

    public void MoveTo(Vector2Int newPosition){
        if (_isLocked) return;
        if (_grid.CanFitIn(RelativeOccupiedPositions.Offset(newPosition), new [] { this })){
            var old = GridPosition;
            GridPosition = newPosition;
            PositionChangedFromTo?.Invoke(this, old, newPosition);
        }
        CheckLock();
    }

    public void Rotate(bool clockWise){
        if (_isLocked) return;
        var newOccupied = RelativeOccupiedPositions.Select(p => clockWise ? new Vector2Int(p.y, -p.x) : new Vector2Int(-p.y, p.x))
            .ToArray();
        if (_grid.CanFitIn(newOccupied.Offset(GridPosition), new [] { this })){
            var olc = RelativeOccupiedPositions;
            RelativeOccupiedPositions = newOccupied;
            OccupiedBlockChangedFromTo?.Invoke(this, olc, newOccupied);
        }
        CheckLock();
    }

    private void CheckLock(){

        IEnumerator Inner(){
            yield return new WaitForSeconds(.1f);
            foreach (var b in SingleBlocks){
                b.Spr.color = Color.green;
                b.IsLocked = true;
            }
            Locked?.Invoke(this);
            Destroy(this);
        }
        
        if (!IsReachingBottom()) return;
        _isLocked = true;
        StartCoroutine(Inner());
    }

    public bool IsReachingBottom(){
        return OccupiedGridPositions.Any(
            (b) => {
                if (!_grid.IsOccupied(b + Vector2Int.up, SingleBlocks)){
                    return false;
                }
                var block = _grid.IsOccupiedBy(b + Vector2Int.up);
                return block == null || block.IsLocked;
            });
    }

    private void UpdateSingleBlocksGridPositions(){
        foreach (var (single, pos) in SingleBlocks.Zip(OccupiedGridPositions, (block, pos) => (block, pos))){
            single.GridPosition = pos;
        }
    }


    public void TakeCommand(Command command){
        switch (command){
        case Command.Left:
            Move(true);
            break;
        case Command.Right:
            Move(false);
            break;
        case Command.Down:
            Fall();
            break;
        case Command.Rotate:
            Rotate(true);
            break;
        default:
            throw new ArgumentOutOfRangeException(nameof(command), command, null);
        }
    }
}