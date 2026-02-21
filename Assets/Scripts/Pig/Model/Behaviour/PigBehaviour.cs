using System;
using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using UnityEngine;

public class PigBehaviour
{
     private Func<List<WallView>> _walls;
    private Func<List<SeedbedView>> _seedbeds;
    private Action<PigView, SeedbedView> _action;
    
    private readonly BehaviorTree _behaviorTree;

    private readonly float _rayDistance;
    private readonly float _offsetX;

    public PigBehaviour(Transform selfTransform, Func<List<WallView>> getWalls, Func<List<SeedbedView>> getSeedbeds, Action<PigView, SeedbedView> action, float rayDistance = 1.5f, float offsetX =1f)
    {
        // Sequence findWall = new Sequence(new List<Node>
        // {
        //     new ConditionNode(IsFoundWall),
        //     new ActionNode(NoAction),
        // });
                    
        Sequence findSeedbed = new Sequence(new List<Node>
        {
            new ConditionNode(IsFoundSeedbed),
            new ActionNode(AttackSeedbed)
        });
            
        Sequence moveForward = new Sequence(new List<Node>
        {
            new ActionNode(Move)
        });

        _walls = getWalls;
        _seedbeds = getSeedbeds;
        
        _behaviorTree = new BehaviorTree(
            new Selector(new List<Node>
            {
                //findWall,
                findSeedbed,
                moveForward
            })
        );

        _rayDistance = rayDistance;
        _offsetX = offsetX;
        _action = action;
        _behaviorTree.Blackboard.SetValue("selfTransform", selfTransform);   
    }
    [SerializeField] private float _boxWidth = 2f;  // Ширина прямоугольника
    [SerializeField] private float _boxHeight = 3f; // Высота прямоугольника

    private ENodeState AttackSeedbed(Blackboard arg)
    {
        Transform selfTransform = arg.GetValue<Transform>("selfTransform");

        // Параметры прямоугольника
        Vector2 origin = new Vector2(selfTransform.position.x - _offsetX, selfTransform.position.y);
        Vector2 direction = Vector2.left; // Направление влево
        Vector2 boxSize = new Vector2(_boxWidth, _boxHeight); // Добавьте переменные для ширины и высоты прямоугольника
        float angle = 0f; // Угол поворота прямоугольника
    
        RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, boxSize, angle, direction, _rayDistance);

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out SeedbedView seedbedView))
            {
                Debug.LogError("ATTACK SEEDBED VIEW");
                _action?.Invoke(selfTransform.GetComponent<PigView>(), seedbedView);
            }
        }
    
        // Визуализация прямоугольника (для отладки)
        Vector2 boxCenter = origin + direction * (_rayDistance * 0.5f);
        Debug.DrawRay(origin, direction * _rayDistance, Color.green);
        Debug.DrawLine(
            new Vector3(boxCenter.x - boxSize.x/2, boxCenter.y - boxSize.y/2, 0),
            new Vector3(boxCenter.x + boxSize.x/2, boxCenter.y + boxSize.y/2, 0),
            Color.yellow
        );

        return ENodeState.Running;
    }

        private bool IsFoundWall(Blackboard arg)
        {
            Transform selfTransform = arg.GetValue<Transform>("selfTransform");

            foreach (Transform wall in _walls().Select(x=>x.gameObject.transform))
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

            foreach (Transform seedbed in _seedbeds().Select(x=>x.gameObject.transform))
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
            return ENodeState.Running;
        }

        private bool _isOnCooldown = false;


        public void Update()
        {
            _behaviorTree.Tick();
        }
}
