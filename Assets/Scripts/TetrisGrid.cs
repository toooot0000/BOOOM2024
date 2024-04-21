using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TetrisGrid : MonoBehaviour, ITetrisSingleBlockParent{
    [SerializeField] private Vector2Int totalSize = new(10, 20);
    [FormerlySerializedAs("_grid")] [SerializeField] private Grid grid;

    private readonly List<TetrisBlock> _managed = new();
    private TetrisSingleBlock[,] _occupied;

    public Vector2Int TotalSize => totalSize;

    private void Awake(){
        _occupied = new TetrisSingleBlock[totalSize.x, totalSize.y];
    }

    private void Reset(){
        grid = GetComponent<Grid>();
    }

    public void Manage(TetrisBlock inst){
        inst.Locked += b => {
            int to = 0, from = int.MaxValue;
            foreach (var objOccupiedBlock in b.OccupiedBlocks){
                var p = objOccupiedBlock + b.GridPosition;
                to = Math.Max(to, p.y);
                from = Math.Min(from, p.y);
            }
            CheckClearLines(from, to);
        };
        inst.PositionChangedFromTo += (b, o, n) => {
            ClearOccupied(b.OccupiedBlocks.Select(p => p + o));
            SetOccupied(b.OccupiedBlocks.Select(p => p + n), b.SingleBlocks);
        };
        inst.OccupiedBlockChangedFromTo += (b, o, n) => {
            ClearOccupied(o.Select(p => p + b.GridPosition));
            SetOccupied(n.Select(p => p + b.GridPosition), b.SingleBlocks);
        };
        _managed.Add(inst);
    }

    private void ClearOccupied(IEnumerable<Vector2Int> positions){
        foreach (var p in positions){
            _occupied[p.x, p.y] = null;
        }
    }

    private void SetOccupied(IEnumerable<Vector2Int> positions, IEnumerable<TetrisSingleBlock> blocks){
        foreach (var (pos, block) in positions.Zip(blocks, (i, singleBlock) => (i, singleBlock))){
            _occupied[pos.x, pos.y] = block;
        }
    }

    public bool CanFitIn(IEnumerable<Vector2Int> occupiedPosition, TetrisBlock[] except){
        var exceptedBlocks = except.SelectMany(b => b.SingleBlocks).ToArray();
        return occupiedPosition.All(p => !IsOccupied(p, exceptedBlocks));
    }

    public bool IsOccupied(Vector2Int position, TetrisSingleBlock[] except = null){
        except ??= Array.Empty<TetrisSingleBlock>();
        if (!IsInRange(position.x, position.y)) return true;
        var cur = _occupied[position.x, position.y];
        if (cur == null) return false;
        return !except.Contains(cur);
    }

    public TetrisSingleBlock IsOccupiedBy(Vector2Int position){
        return !IsInRange(position.x, position.y) ? null : _occupied[position.x, position.y];
    }

    public bool IsXInRange(int x) => x >= 0 && x < totalSize.x;
    public bool IsYInRange(int y) => y >= 0 && y < totalSize.y;
    public bool IsInRange(int x, int y) => IsXInRange(x) && IsYInRange(y);

    public Vector3 PositionFromGridPosition(Vector2Int gridPosition){
        return grid.GetCellCenterWorld(new Vector3Int(gridPosition.x, -gridPosition.y, 0));
    }

    public void CheckClearLines(int from, int to){
        foreach (var i in from.To(to)){
            if (!IsLineFull(i)) continue;
            ClearLine(i);
        }

        var j = to;
        while (j >= from){
            if (IsLineEmpty(j)){
                foreach (var b in BlocksFromLineAbove(j-1)){
                    var pos = b.GridPosition;
                    b.GridPosition += Vector2Int.up;
                    _occupied[pos.x, pos.y] = null;
                    _occupied[pos.x, pos.y + 1] = b;
                }
                from++;
            } else{
                j--;
            }
        }
    }

    public void ClearLine(int lineNum){
        foreach (var (i, j) in Line(lineNum)){
            var block = _occupied[i, j];
            _occupied[i, j] = null;
            // TODO: Use object pool!
            Destroy(block.Spr.gameObject);
        }
    }

    public bool IsLineFull(int num){
        foreach (var (i, j) in Line(num)){
            if (_occupied[i, j] == null) return false;
            if (!_occupied[i, j].IsLocked) return false;
        }
        return true;
    }
    
    public bool IsLineEmpty(int num){
        foreach (var (i, j) in Line(num)){
            if (_occupied[i, j] != null) return false;
        }
        return true;
    }

    private IEnumerable<(int, int)> Line(int num){
        for (var j = 0; j < totalSize.x; j++){
            yield return (j, num);
        }
    }

    private IEnumerable<TetrisSingleBlock> BlocksFromLineAbove(int lineNum){
        if (!IsYInRange(lineNum)) yield break;
        for (var y = lineNum; y >= 0; y--){
            for (var x = 0; x < TotalSize.x; x++){
                if (_occupied[x, y] != null) yield return _occupied[x, y];
            }   
        }
    }
}