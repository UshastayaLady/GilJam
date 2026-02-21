using System;
using UniRx;
using UnityEngine;

public class PigModel
{
    private readonly ReactiveProperty<int> _currentHealth;
    private readonly ReactiveProperty<int> _money;

    public PigModel(int health, int money)
    {
        _currentHealth = new(health);
        _money = new(money);
    }

    private bool IsAlive => _currentHealth.Value > 0;

    public IReadOnlyReactiveProperty<int> CurrentHealth => _currentHealth;

    public IReadOnlyReactiveProperty<int> CurrentPrice => _money;

    public IObservable<Unit> OnDeath => 
        _currentHealth.Where(h => h <= 0).Select(_ => Unit.Default);

    public void TakeDamage(int damage)
    {
        if (!IsAlive) return;
        
        var newHealth = Math.Max(0, _currentHealth.Value - damage);
        _currentHealth.Value = newHealth;
    }
}
