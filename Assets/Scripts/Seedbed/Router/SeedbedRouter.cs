using UnityEngine;
using System.Collections.Generic;
using WebUtility;

public class SeedbedRouter : IDIRouter
{
    [Inject] private DIContainer _container;

    public List<IPresenter> Init()
    {
        SeedbedPresenter seedbedPresenter = _container.RegisterSingleton<SeedbedPresenter>();

        return new List<IPresenter>()
        {
            seedbedPresenter
        };
    }
}