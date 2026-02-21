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

        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                Update();
            })
            .AddTo(_disposables);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

            foreach (var hit in hits)
            {
                if (hit.collider != null)
                {
                    // Получаем компонент с объекта
                    PickableMoney component = hit.collider.GetComponent<PickableMoney>();
                
                    if (component != null)
                    {
                        _moneyModel.IncreaseMoney(10);
                        
                        Object.Destroy(component.gameObject);
                    }
                }
            }
           
        }

    }

    private void Buy(int result, PigModel pigModel)
    {
        _moneyModel.SpendMoney(result);
    }

    public void Exit()
    {
        
    }
}