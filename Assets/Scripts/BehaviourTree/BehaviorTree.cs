// BehaviourTree/ BehaviorTree.cs
using UnityEngine;

namespace BehaviourTree
{
    public class BehaviorTree
    {
        private Node _rootNode; // Корневой узел (обычно Selector или Sequence)
        public Blackboard Blackboard { get; private set; }

        // Конструктор. Принимает корневой узел и создает Blackboard.
        public BehaviorTree(Node rootNode)
        {
            _rootNode = rootNode;
            Blackboard = new Blackboard();
            
            // Пробрасываем Blackboard во все Action и Condition узлы.
            // Это простой, но не самый эффективный способ. Для глубины >1 нужна рекурсия.
            InjectBlackboard(_rootNode);
        }

        private void InjectBlackboard(Node node)
        {
            // Если это ActionNode, даем ему Blackboard
            if (node is ActionNode actionNode)
                actionNode.Blackboard = Blackboard;
            
            // Если это ConditionNode, даем ему Blackboard
            if (node is ConditionNode conditionNode)
                conditionNode.Blackboard = Blackboard;
            
            // Рекурсивно проходим по детям
            foreach (var child in node.Children)
            {
                InjectBlackboard(child);
            }
        }

        // Этот метод нужно вызывать каждый кадр (например, в Update)
        public void Tick()
        {
            if (_rootNode != null)
            {
                _rootNode.Evaluate();
            }
        }
    }
}