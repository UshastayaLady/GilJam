using System;
using UniRx;
using UnityEngine;
using WebUtility;

public class PaymentHandler
{
    [Inject] private MoneyModel _moneyModel;

    private int Money => _moneyModel.CurrentMoney.Value;
    
    private readonly Subject<(int, PigModel)> _onBought = new Subject<(int, PigModel)>();
    
    public IObservable<(int, PigModel)> OnBought => _onBought;

    public bool TryBuy(PigModel pigModel)
    {
        int result = Money - pigModel.CurrentPrice.Value;

        Debug.LogError("RESULT,,, " + result);
        if (result > 0)
        {
            _onBought.OnNext((pigModel.CurrentPrice.Value, pigModel));
            return true;
        }

        return false;
    }
}
