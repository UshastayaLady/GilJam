using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using WebUtility;

public class PaymentPresenter : IPresenter
{
    [Inject] private DIContainer _container;
    [Inject] private PaymentWindow _window;
    [Inject] private PaymentHandler _paymentHandler;
    [Inject] private MoneyModel _moneyModel;
    
    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    public void Init()
    {
        _paymentHandler.OnBought.Subscribe(bought => {
                Buy(bought.Item1, bought.Item2); 
            })
            .AddTo(_disposables); 

    }

    private void Buy(int result, PigModel pigModel)
    {
        _moneyModel.SpendMoney(result);
    }

    public void Exit()
    {
        
    }
}