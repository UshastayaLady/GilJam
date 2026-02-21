using System.Collections.Generic;
using UnityEngine;

public class SeedbedsCollection
{
    private List<SeedbedView> _seedbeds = new List<SeedbedView>();
    
    public IEnumerable<SeedbedView> GetSeedbeds()
    {
        return Object.FindObjectsOfType<SeedbedView>();
    }
}
