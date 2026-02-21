using UniRx;
using UnityEngine;
using WebUtility;

public class PigCreatorPresenter : IPresenter
{
    [Inject] private DIContainer _container;
    [Inject] private PigWindow _window;
    [Inject] private PaymentHandler _paymentHandler;
    [Inject] private PigCollection _pigCollection;
    
    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    public void Init()
    {
        Debug.LogError(">>F.af.ag.,fg");
        _paymentHandler.OnBought.Subscribe(bought => {
                Buy(bought.Item1, bought.Item2); 
            })
            .AddTo(_disposables); 

    }

    private void Buy(int result, PigModel pigModel)
    {
        Debug.LogError("CLICKEDQ!");

        Object.FindObjectOfType<SpawnPig>(true).OnClick(_pigCollection.GetPrefabBy(pigModel).GetComponent<SpriteRenderer>(), out SpriteRenderer spawnedPig);

        if (spawnedPig != null)
        {
            _pigCollection.UpdateSpawnedPig(spawnedPig.GetComponent<PigView>(), pigModel);
        }
    }

    public void Exit()
    {
        
    }
}
