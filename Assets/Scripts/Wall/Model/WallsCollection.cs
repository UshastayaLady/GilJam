using System.Collections.Generic;
using UnityEngine;

public class WallsCollection
{
    private List<WallView> _walls = new();

    public IEnumerable<WallView> GetWalls() => _walls;
    
    public void AddWall(WallView wallView)
    {
        _walls.Add(wallView);
    }
    
}
