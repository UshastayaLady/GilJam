using UniRx;
using UnityEngine;

public class MoneyModel
{
    private readonly ReactiveProperty<int> _money = new ReactiveProperty<int>(100);

    public IReadOnlyReactiveProperty<int> CurrentMoney => _money;

    public void SpendMoney(int money)
    {
        _money.Value -= money;
    }
}
