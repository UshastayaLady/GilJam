using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class PigBehaviour
{
    [SerializeField] private Transform[] _walls;
    [SerializeField] private Transform[] _seedbeds;
    
    private readonly BehaviorTree _behaviorTree;

    private readonly float _rayDistance;
    private readonly float _offsetX;

    public PigBehaviour(Transform selfTransform, float rayDistance = 5f, float offsetX =1f)
    {
        Sequence findWall = new Sequence(new List<Node>
        {
            new ConditionNode(IsFoundWall),
            new ActionNode(NoAction),
        });
                    
        Sequence findSeedbed = new Sequence(new List<Node>
        {
            new ConditionNode(IsFoundSeedbed),
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

        _rayDistance = rayDistance;
        _offsetX = offsetX;
        
        _behaviorTree.Blackboard.SetValue("selfTransform", selfTransform);   
    }
    
      private ENodeState AttackSeedbed(Blackboard arg)
        {
            Transform selfTransform = arg.GetValue<Transform>("selfTransform");

            Vector2 origin = new Vector2(selfTransform.position.x + _offsetX, selfTransform.position.y);
            Vector2 direction = Vector2.right;
        
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, _rayDistance);

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent(out SeedbedView seedbedView))
                {
                    Debug.LogError("ATTACK SEEDBED VIEW");
                }
            }
        
            Debug.DrawRay(origin, direction * _rayDistance, Color.green);

            return ENodeState.Running;
        }

        private bool IsFoundWall(Blackboard arg)
        {
            return false;

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
            return false;

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


        public void Update()
        {
            _behaviorTree.Tick();
        }
}
