using UnityEngine;
using System.Collections.Generic;
using WebUtility;

public class InventoryRouter : IDIRouter
{
    [Inject] private DIContainer _container;

    public List<IPresenter> Init()
    {
        InventoryPresenter inventoryPresenter = _container.RegisterSingleton<InventoryPresenter>();

        return new List<IPresenter>()
        {
            inventoryPresenter
        };
    }
}