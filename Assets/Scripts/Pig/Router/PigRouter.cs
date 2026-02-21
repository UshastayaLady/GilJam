using UnityEngine;
using System.Collections.Generic;
using WebUtility;

public class PigRouter : IDIRouter
{
    [Inject] private DIContainer _container;

    public List<IPresenter> Init()
    {
        PigPresenter pigPresenter = _container.RegisterSingleton<PigPresenter>();

        return new List<IPresenter>()
        {
            pigPresenter
        };
    }
}