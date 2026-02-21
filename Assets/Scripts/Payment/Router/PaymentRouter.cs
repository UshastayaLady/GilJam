using UnityEngine;
using System.Collections.Generic;
using WebUtility;

public class PaymentRouter : IDIRouter
{
    [Inject] private DIContainer _container;

    public List<IPresenter> Init()
    {
        PaymentPresenter paymentPresenter = _container.RegisterSingleton<PaymentPresenter>();
        _container.RegisterSingleton<MoneyModel>();
        _container.RegisterSingleton<PaymentHandler>();

        return new List<IPresenter>()
        {
            paymentPresenter
        };
    }
}