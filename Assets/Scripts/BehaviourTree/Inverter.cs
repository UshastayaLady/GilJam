// BehaviourTree/ Decorators/ Inverter.cs

using System.Collections.Generic;

namespace BehaviourTree
{
    public class Inverter : Node
    {
        // В конструкторе мы ожидаем одного ребенка. Для удобства создадим отдельный конструктор.
        public Inverter(Node child) : base(new List<Node> { child }) { }

        protected override ENodeState OnEvaluate()
        {
            // У нас точно есть один ребенок, берем его под индексом 0
            Node child = Children[0];
            
            switch (child.Evaluate())
            {
                case ENodeState.Running:
                    State = ENodeState.Running;
                    break;
                case ENodeState.Success:
                    State = ENodeState.Failure; // Инвертируем
                    break;
                case ENodeState.Failure:
                    State = ENodeState.Success; // Инвертируем
                    break;
            }
            
            return State;
        }
    }
}

// BehaviourTree/ Decorators/ Repeater.cs
namespace BehaviourTree
{
    public class Repeater : Node
    {
        private int _repeatCount; // Сколько раз повторить. -1 = бесконечно.
        private int _currentCount;

        public Repeater(Node child, int repeatCount = -1) : base(new List<Node> { child })
        {
            _repeatCount = repeatCount;
            _currentCount = 0;
        }

        protected override ENodeState OnEvaluate()
        {
            Node child = Children[0];
            
            // Запускаем ребенка
            ENodeState childState = child.Evaluate();

            // Если ребенок завершился (успешно или провально)
            if (childState != ENodeState.Running)
            {
                _currentCount++;
                
                // Проверяем, нужно ли останавливаться
                if (_repeatCount != -1 && _currentCount >= _repeatCount)
                {
                    // Если достигли лимита, возвращаем результат последнего запуска
                    State = childState;
                    return State;
                }
                
                // Иначе сбрасываем состояние ребенка? Нет, мы просто запустим его заново в следующем тике.
                // Но нам нужно как-то сказать ребенку, что он должен начать заново.
                // Для этого нужен метод Reset(). Пока пропустим для простоты.
            }

            // Мы все еще работаем (повторяем)
            State = ENodeState.Running;
            return State;
        }
    }
}

// BehaviourTree/ Decorators/ UntilFail.cs
namespace BehaviourTree
{
    public class UntilFail : Node
    {
        public UntilFail(Node child) : base(new List<Node> { child }) { }

        protected override ENodeState OnEvaluate()
        {
            Node child = Children[0];
            ENodeState childState = child.Evaluate();

            if (childState == ENodeState.Failure)
            {
                // Если ребенок провалился, мы наконец-то завершаемся успешно (мы сделали свою работу)
                State = ENodeState.Success;
                return State;
            }

            // Если ребенок успешен или еще работает, мы продолжаем работать
            State = ENodeState.Running;
            return State;
        }
    }
}