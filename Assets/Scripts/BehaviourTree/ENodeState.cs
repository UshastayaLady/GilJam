using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;

// BehaviourTree/ Core/ ENodeState.cs
namespace BehaviourTree
{
    // Три состояния, в которых может находиться любой узел.
    // Это основа всех проверок в дереве.
    public enum ENodeState
    {
        Running,    // Узел работает прямо сейчас (например, охранник идет)
        Success,    // Узел успешно завершил работу (дошел до точки)
        Failure     // Узел провалил задачу (путь заблокирован, игрок не найден)
    }
}

// BehaviourTree/ Core/ Node.cs

namespace BehaviourTree
{
    public abstract class Node
    {
        // --- Свойства (Характеристики) узла ---

        // Текущий статус. protected set - значит, менять его могут только сам узел и его наследники.
        public ENodeState State { get; protected set; }

        // Родительский узел. Нужен, чтобы узел мог "попросить" родителя перезапуститься или сообщить о себе.
        public Node Parent { get; private set; }

        // Дочерние узлы (если это композит или декоратор). Храним в списке для гибкости.
        public List<Node> Children = new List<Node>();

        // Словарь для хранения данных, специфичных для конкретного экземпляра узла.
        // Например, время начала действия. Это удобнее, чем плодить кучу полей в каждом наследнике.
        protected Dictionary<string, object> NodeLocalData = new Dictionary<string, object>();

        // --- Конструктор (Как создать узел) ---
        public Node(List<Node> children = null)
        {
            if (children != null)
            {
                foreach (var child in children)
                {
                    // При добавлении ребенка сразу говорим ребенку: "Я твой родитель"
                    AttachChild(child);
                }
            }
        }

        // --- Методы (Что узел умеет) ---

        // Приватный метод для привязки ребенка. Недоступен извне, чтобы не нарушить структуру.
        private void AttachChild(Node child)
        {
            child.Parent = this;       // Ребенок запоминает родителя
            Children.Add(child);       // Добавляем в список
        }

        // Это сердце узла. Его будет вызывать родитель или само дерево.
        public ENodeState Evaluate()
        {
            // Здесь может быть кастомная логика для всех узлов (например, лог в консоль)
            // Но основную работу делает метод, который мы переопределим в наследниках.
            return OnEvaluate();
        }

        // Абстрактный метод, который ОБЯЗАНЫ переопределить все наследники.
        // Именно в нем будет заключена логика Sequence, Selector, Действия и т.д.
        protected abstract ENodeState OnEvaluate();

        // Полезный метод для работы с данными узла.
        // Позволяет безопасно получить значение по ключу.
        protected T GetLocalData<T>(string key, T defaultValue = default)
        {
            if (NodeLocalData.TryGetValue(key, out object value))
            {
                return (T)value;
            }
            return defaultValue;
        }
    }
}


namespace BehaviourTree
{
    // Blackboard - это просто большой словарь, где ключ - строка, значение - любой объект.
    // Удобно то, что мы можем сделать его ScriptableObject и сохранять настройки ИИ как ассет.
    [System.Serializable]
    public class Blackboard
    {
        // Основное хранилище
        public Dictionary<string, object> data = new Dictionary<string,object>();
        
        // Удобные обертки для часто используемых типов Unity (чтобы не писать каждый раз Convert)
        public Transform SelfTransform { get; set; } // Трансформ самого ИИ
        public Transform TargetEnemy { get; set; }   // Текущий враг
        public Vector3 MoveToPosition { get; set; }  // Куда идти
        public float Timer { get; set; }             // Таймер для ожидания
        
        // Можно хранить и настройки
        public float SightRange { get; set; } = 15f;
        public float HearingRange { get; set; } = 20f;
        
        // --- Методы для удобной работы ---

        // Попытка получить значение. Если нет - возвращаем defaultValue.
        public T GetValue<T>(string key, T defaultValue = default)
        {
            if (data.TryGetValue(key, out object value))
            {
                return (T)value;
            }
            return defaultValue;
        }

        // Установить значение
        public void SetValue(string key, object value)
        {
            if (data.ContainsKey(key))
                data[key] = value;
            else
                data.Add(key, value);
        }

        // Очистить значение (удалить)
        public void ClearValue(string key)
        {
            if (data.ContainsKey(key))
                data.Remove(key);
        }
    }
}