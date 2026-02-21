// BehaviourTree/ Composites/ Sequence.cs
using System.Collections.Generic;
using System.Linq; // Для подсчета

namespace BehaviourTree
{
    public class Sequence : Node
    {
        // Конструктор просто передает детей базовому классу Node
        public Sequence(List<Node> children) : base(children) { }

        protected override ENodeState OnEvaluate()
        {
            bool anyChildRunning = false;

            // Проходим по всем детям
            foreach (Node child in Children)
            {
                switch (child.Evaluate())
                {
                    case ENodeState.Running:
                        // Если текущий ребенок еще работает, запоминаем это,
                        // но продолжаем проверку? НЕТ! В Sequence мы останавливаемся на первом Running.
                        // Иначе мы бы запустили следующее действие до завершения текущего.
                        State = ENodeState.Running;
                        return State;
                    
                    case ENodeState.Failure:
                        // Если ребенок провалился, весь Sequence провалился
                        State = ENodeState.Failure;
                        return State;
                    
                    case ENodeState.Success:
                        // Если успешно, переходим к следующему ребенку
                        continue;
                    
                    default:
                        State = ENodeState.Success;
                        return State;
                }
            }

            // Если мы прошли всех детей и никто не провалился и не работает, значит все успешно.
            State = ENodeState.Success;
            return State;
        }
    }
}

// BehaviourTree/ Composites/ Selector.cs

namespace BehaviourTree
{
    public class Selector : Node
    {
        public Selector(List<Node> children) : base(children) { }

        protected override ENodeState OnEvaluate()
        {
            foreach (Node child in Children)
            {
                switch (child.Evaluate())
                {
                    case ENodeState.Running:
                        // Если ребенок работает, мы тоже работаем и НЕ переходим к следующему
                        State = ENodeState.Running;
                        return State;
                    
                    case ENodeState.Success:
                        // Нашли успешного ребенка - ура, мы успешны!
                        State = ENodeState.Success;
                        return State;
                    
                    case ENodeState.Failure:
                        // Этот ребенок не подошел, пробуем следующего
                        continue;
                }
            }

            // Если перебрали всех, и никто не подошел (все Failure)
            State = ENodeState.Failure;
            return State;
        }
    }
}


namespace BehaviourTree
{
    public class Parallel : Node
    {
        // Сколько детей должно успешно завершиться, чтобы Parallel считался успешным?
        // Если 2, значит нужно минимум 2 успеха.
        private int _requiredSuccesses;
        
        // Сколько детей может провалиться, чтобы Parallel все еще считался рабочим?
        // Если 1, то при одном провале мы еще живы, при двух - провал.
        private int _requiredFailures; 

        public Parallel(List<Node> children, int requiredSuccesses, int requiredFailures) : base(children)
        {
            _requiredSuccesses = requiredSuccesses;
            _requiredFailures = requiredFailures;
        }

        protected override ENodeState OnEvaluate()
        {
            int successCount = 0;
            int failureCount = 0;
            int runningCount = 0;

            foreach (Node child in Children)
            {
                ENodeState childState = child.Evaluate();
                
                if (childState == ENodeState.Success) successCount++;
                else if (childState == ENodeState.Failure) failureCount++;
                else if (childState == ENodeState.Running) runningCount++;
            }

            // Проверяем условия выхода
            if (successCount >= _requiredSuccesses)
            {
                State = ENodeState.Success;
                return State;
            }
            
            if (failureCount >= _requiredFailures)
            {
                State = ENodeState.Failure;
                return State;
            }

            // Если еще не набрали нужное количество успехов/провалов и есть работающие дети
            State = runningCount > 0 ? ENodeState.Running : ENodeState.Failure;
            return State;
        }
    }
}