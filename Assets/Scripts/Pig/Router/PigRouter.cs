using UnityEngine;
using System.Collections.Generic;
using WebUtility;

public class PigRouter : IDIRouter
{
    [Inject] private DIContainer _container;

    public List<IPresenter> Init()
    {
        PigPresenter pigPresenter = _container.RegisterSingleton<PigPresenter>();
        PigCollection pigCollection = _container.RegisterSingleton<PigCollection>();

        return new List<IPresenter>()
        {
            pigPresenter
        };
    }
}