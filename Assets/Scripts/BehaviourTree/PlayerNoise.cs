// PlayerNoise.cs

using BehaviourTree;
using UnityEngine;

public class PlayerNoise : MonoBehaviour
{
    public float noiseInterval = 1f;
    private float _timer;
    
    void Update()
    {
        _timer += Time.deltaTime;
        
        // Каждую секунду создаем шум
        if (_timer >= noiseInterval)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                // Находим всех охранников и говорим им о шуме
                GuardAI[] guards = FindObjectsOfType<GuardAI>();
                foreach (GuardAI guard in guards)
                {
                    guard.MakeNoise(transform.position);
                    Debug.Log("Игрок создал шум!");
                }
                
            }
  
        }
    }
}