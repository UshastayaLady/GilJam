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
            Sequence findWall = new Sequence(new List<Node>
            {
                new ConditionNode(IsFoundWall),
                new ActionNode(NoAction),
            });
                    
            Sequence findSeedbed = new Sequence(new List<Node>
            {
                new ConditionNode(IsFoundSeedbed),
                // new ConditionNode(IsActivePlayer),
                // new ConditionNode(IsNearbyPlayer),
                // new ActionNode(MoveToPlayer),
                new ActionNode(AttackSeedbed)
            });
            
            Sequence moveForward = new Sequence(new List<Node>
            {
               new ActionNode(Move)
            });
                
            _behaviorTree = new BehaviorTree(
                new Selector(new List<Node>
                {
                    findWall,
                    findSeedbed,
                    moveForward
                })
            );
            
            _behaviorTree.Blackboard.SetValue("treasure", _treasure);
            _behaviorTree.Blackboard.SetValue("player", _player);
            _behaviorTree.Blackboard.SetValue("selfTransform", transform);
        }

        private ENodeState AttackSeedbed(Blackboard arg)
        {
            Transform selfTransform = arg.GetValue<Transform>("selfTransform");

            Vector2 origin = new Vector2(selfTransform.position.x + offsetX, selfTransform.position.y);
            Vector2 direction = Vector2.right;
        
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, rayDistance);

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent(out SeedbedView seedbedView))
                {
                    Debug.LogError("ATTACK SEEDBED VIEW");
                }
            }
        
            Debug.DrawRay(origin, direction * rayDistance, Color.green);

            return ENodeState.Running;
        }

        private bool IsFoundWall(Blackboard arg)
        {
            Transform selfTransform = arg.GetValue<Transform>("selfTransform");

            foreach (Transform wall in _walls)
            {
                if (wall == null) continue;
        
                if (Mathf.Abs(selfTransform.position.x - wall.position.x) < 1.5f)
                {
                    return true;
                }
            }
    
            return false;
        }
        
        private bool IsFoundSeedbed(Blackboard arg)
        {
            Transform selfTransform = arg.GetValue<Transform>("selfTransform");

            foreach (Transform seedbed in _seedbeds)
            {
                if (seedbed == null) continue;
        
                if (Mathf.Abs(selfTransform.position.x - seedbed.position.x) < 1.5f)
                {
                    return true;
                }
            }
    
            return false;
        }

        private ENodeState Move(Blackboard arg)
        {
            Transform selfTransform = arg.GetValue<Transform>("selfTransform");

            selfTransform.position += new Vector3(-1, 0, 0) * Time.deltaTime * 3;
            return ENodeState.Running;
        }

        private ENodeState NoAction(Blackboard arg)
        {
            return ENodeState.Failure;
        }

        private bool _isOnCooldown = false;


        private void Update()
        {
            _behaviorTree.Tick();
        }
}
