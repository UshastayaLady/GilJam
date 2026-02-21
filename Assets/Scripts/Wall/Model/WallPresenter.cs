using UnityEngine;
using WebUtility;

public class WallPresenter : IPresenter
{
    [Inject] private DIContainer _container;
    [Inject] private WallWindow _window;
    [Inject] private WallsCollection _wallsCollection;

    public void Init()
    {
        _wallsCollection.AddWall(Object.FindObjectOfType<WallView>());
    }

    public void Exit()
    {
        
    }
}