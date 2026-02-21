using UnityEngine;
using WebUtility;

public class WallPresenter : IPresenter
{
    [Inject] private DIContainer _container;
    [Inject] private WallWindow _window;

    public void Init()
    {
        
    }

    public void Exit()
    {
        
    }
}