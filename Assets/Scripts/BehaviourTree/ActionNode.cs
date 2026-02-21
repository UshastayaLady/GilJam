// BehaviourTree/ Nodes/ ActionNode.cs
using UnityEngine;

// BehaviourTree/ Nodes/ ActionNode.cs
using System; // Для Func
using UnityEngine;

namespace BehaviourTree
{
    public class ActionNode : Node
    {
        // Func<Blackboard, ENodeState> - это метод, который принимает Blackboard и возвращает ENodeState
        // Это полный аналог нашего самописного делегата ActionDelegate
        private Func<Blackboard, ENodeState> _action;

        // Конструктор теперь принимает Func
        public ActionNode(Func<Blackboard, ENodeState> action)
        {
            _action = action;
        }

        protected override ENodeState OnEvaluate()
        {
            if (_action == null)
            {
                Debug.LogError("ActionNode не имеет назначенного действия!");
                return ENodeState.Failure;
            }

            State = _action(Blackboard);
            return State;
        }
        
        public Blackboard Blackboard { get; set; }
    }
}

// BehaviourTree/ Nodes/ ConditionNode.cs

// BehaviourTree/ Nodes/ ConditionNode.cs (версия с Func)

namespace BehaviourTree
{
    public class ConditionNode : Node
    {
        // Func<Blackboard, bool> - тоже самое что Predicate, но принято использовать Predicate для условий
        private Func<Blackboard, bool> _condition;

        public ConditionNode(Func<Blackboard, bool> condition)
        {
            _condition = condition;
        }

        protected override ENodeState OnEvaluate()
        {
            if (_condition == null)
                return ENodeState.Failure;

            bool result = _condition(Blackboard);
            State = result ? ENodeState.Success : ENodeState.Failure;
            return State;
        }

        public Blackboard Blackboard { get; set; }
    }
}