using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BehaviourTree
{
    public class EnemyAi : MonoBehaviour
    {
        [SerializeField] private Transform _treasure;
        [SerializeField] private Transform _player;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform[] _points;

        private BehaviorTree _behaviorTree;

        private void Awake()
        {
            
            Sequence findTreasure = new Sequence(new List<Node>
            {
                new ConditionNode(IsActiveTreasure),
                new ActionNode(MoveToTreasure),
            });
                    
            Sequence findPlayer = new Sequence(new List<Node>
            {
                new ConditionNode(IsActivePlayer),
                new ConditionNode(IsNearbyPlayer),
                new ActionNode(MoveToPlayer),
                new ActionNode(AttackTo)
            });
            
            Sequence patrol = new Sequence(new List<Node>
            {
               //new ActionNode()
            });
                
            _behaviorTree = new BehaviorTree(
                new Selector(new List<Node>
                {
                    findTreasure,
                    findPlayer,
                    patrol
                })
            );
            
            _behaviorTree.Blackboard.SetValue("treasure", _treasure);
            _behaviorTree.Blackboard.SetValue("player", _player);
            _behaviorTree.Blackboard.SetValue("selfTransform", transform);
        }

        private bool _isOnCooldown = false;

        private ENodeState AttackTo(Blackboard arg)
        {
            if (_isOnCooldown)
            {
                return ENodeState.Running;
            }
            
        //    Instantiate(_bullet)
    
            _isOnCooldown = true;
            WaitForCooldown().Forget();
            return ENodeState.Running;
        }

        private async UniTask WaitForCooldown()
        {
            await UniTask.WaitForSeconds(0.3f);
            _isOnCooldown = false;
        }
        private ENodeState MoveToTreasure(Blackboard arg)
        {
            Transform selfTransform = arg.GetValue<Transform>("selfTransform");
            Transform obj = arg.GetValue<Transform>("treasure");

            if (Vector3.Distance(obj.transform.position, selfTransform.position) < 1)
            {
                return ENodeState.Success;
            }

            selfTransform.position = Vector3.MoveTowards(selfTransform.transform.position, obj.transform.position, 5 * Time.deltaTime);

            return ENodeState.Running;

        }

        private ENodeState MoveToPlayer(Blackboard arg)
        {
            Transform selfTransform = arg.GetValue<Transform>("selfTransform");
            Transform obj = arg.GetValue<Transform>("player");

            if (Vector3.Distance(obj.transform.position, selfTransform.position) < 1)
            {
                return ENodeState.Success;
            }

            selfTransform.position = Vector3.MoveTowards(selfTransform.transform.position, obj.transform.position, 5 * Time.deltaTime);

            return ENodeState.Running;

        }

        private bool IsActivePlayer(Blackboard arg)
        {
            Transform obj = arg.GetValue<Transform>("player");
            
            return obj.gameObject.activeInHierarchy;
        }
        
        private bool IsActiveTreasure(Blackboard arg)
        {
            Transform obj = arg.GetValue<Transform>("treasure");
            
            return obj.gameObject.activeInHierarchy;
        }

        private bool IsNearbyPlayer(Blackboard arg)
        {
            Transform selfTransform = arg.GetValue<Transform>("selfTransform");
            Transform obj = arg.GetValue<Transform>("player");

            if (Vector3.Distance(obj.transform.position, selfTransform.position) < 10)
            {
                return true;
            }
            
            return false;
        }

        private bool IsNearbyTreasure(Blackboard arg)
        {
            return true;
        }

        private bool IsNearbyEntity(Blackboard arg)
        {
            return false;
        }


        private bool IsActiveEntity(Blackboard arg)
        {
            return false;
        }

        private ENodeState RunToPlayer(Blackboard arg)
        {
            Debug.LogError("RUN TO PLAYER");

            return ENodeState.Running;
        }

        private bool CanSeePlayer(Blackboard arg)
        {
            bool isPressed = arg.GetValue<bool>("IsDoneFirst");

            if (Input.GetKey(KeyCode.Space))
            {
                return true;

                // arg.SetValue("IsDoneFirst", true);
            }
            
            if (isPressed)
            {
                return true;
            }

            Debug.LogError("Do first....");
            return false;
        }

        private ENodeState Patrol(Blackboard arg)
        {
            Debug.LogError("Patrol...");
            return ENodeState.Running;
        }

        private ENodeState SecondDo(Blackboard arg)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                return ENodeState.Success;
            }
            
            Debug.LogError("Do second....");

            return ENodeState.Running;
        }

        private ENodeState FirstDo(Blackboard arg)
        {
            bool isPressed = arg.GetValue<bool>("IsDoneFirst");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                arg.SetValue("IsDoneFirst", true);
            }
            
            if (isPressed)
            {
                return ENodeState.Failure;
            }

            Debug.LogError("Do first....");
            return ENodeState.Running;
        }

        private void Update()
        {
            _behaviorTree.Tick();
        }
    }

}
