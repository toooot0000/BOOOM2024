using UnityEngine;

public interface ITetrisSingleBlockParent{
    Vector3 PositionFromGridPosition(Vector2Int gridPosition);
}

public class TetrisSingleBlock{

    private bool _isLocked = false;

    public bool IsLocked{
        get => _isLocked;
        set{
            if (!_isLocked){
                JustLocked = true;
            }
            _isLocked = value;
        }
    }
    
    public SpriteRenderer Spr;
    public ITetrisSingleBlockParent Parent;
    public int PlayerIndex = -1;
    public bool JustLocked = false;
    public int SpawnBatch = -1;

    private Vector2Int _gridPosition;
    public Vector2Int GridPosition{
        get => _gridPosition;
        set{
            _gridPosition = value;
            UpdateSprites();
        }
    }
    
    private void UpdateSprites(){
        if (Parent == null || Spr == null) return;
        var transform = Spr.transform;
        transform.position = Parent.PositionFromGridPosition(GridPosition);
    }
}