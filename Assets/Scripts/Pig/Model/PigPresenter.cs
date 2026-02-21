using UnityEngine;
using WebUtility;

public class PigPresenter : IPresenter
{
    [Inject] private DIContainer _container;
    [Inject] private PigWindow _window;

    public void Init()
    {
        
    }

    public void Exit()
    {
        
    }
}