using UnityEngine;

public interface ITetrisSingleBlockParent{
    Vector3 PositionFromGridPosition(Vector2Int gridPosition);
}

public class TetrisSingleBlock{
    public bool IsLocked = false;
    public SpriteRenderer Spr;
    public ITetrisSingleBlockParent Parent;

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