using System.Collections.Generic;
using BehaviourTree;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PigView : MonoBehaviour
{
        [SerializeField] private Transform _treasure;
        [SerializeField] private Transform _player;
        [SerializeField] private Transform[] _walls;
        [SerializeField] private Transform[] _seedbeds;

        public float rayDistance = 5f;
        public float offsetX = 1f;

        private BehaviorTree _behaviorTree;

        private void Awake()
        {
          
        }

      
}
