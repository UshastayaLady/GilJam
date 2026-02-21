// BehaviourTree/ GuardAI.cs
using UnityEngine;
using System.Collections.Generic; // Для списков

namespace BehaviourTree
{
    // Этот компонент будет висеть на охраннике
    public class GuardAI : MonoBehaviour
    {
        [Header("Настройки перемещения")]
        public float walkSpeed = 2f;           // Скорость ходьбы (патруль)
        public float runSpeed = 4f;            // Скорость бега (когда видит игрока)
        public float rotationSpeed = 3f;        // Скорость поворота
        public float stoppingDistance = 1.5f;   // На каком расстоянии останавливаться от цели

        [Header("Настройки патруля")]
        public Transform[] patrolPoints;         // Точки патруля (перетащите в инспекторе)
        public float waitTimeAtPoint = 2f;       // Сколько ждать на точке

        [Header("Настройки зрения")]
        public float sightRange = 10f;           // Дальность зрения
        public float attackRange = 2f;           // Дальность атаки
        public LayerMask obstacleMask;           // Что считать препятствием (стены)

        [Header("Настройки слуха")]
        public float hearingRange = 15f;         // Дальность слуха

        [Header("Цели")]
        public Transform player;                  // Ссылка на игрока (перетащите в инспекторе)

        // Компоненты (нам нужен только Transform)
        private Transform _transform;
        
        // Наше дерево поведения
        private BehaviorTree _behaviorTree;
        
        // Переменные для состояний (чтобы значения сохранялись между кадрами)
        private int _currentPatrolIndex = 0;           // Текущая точка патруля
        private float _waitTimer = 0f;                  // Таймер ожидания
        private float _attackTimer = 0f;                // Таймер перезарядки атаки
        private Vector3 _lastNoisePosition = Vector3.zero; // Последняя позиция шума

        void Start()
        {
            _transform = transform;

            // --- СТРОИМ ДЕРЕВО ПОВЕДЕНИЯ ---
            
            // Корень - это Selector. Он будет выбирать между разными режимами по приоритету.
            // Чем левее ветка в списке, тем выше её приоритет.
            _behaviorTree = new BehaviorTree(
                new Selector(new List<Node>
                {
                    // --- ВЕТКА 1: АТАКА (самый высокий приоритет) ---
                    new Sequence(new List<Node>
                    {
                        // Условие: Вижу ли я игрока?
                        new ConditionNode(CanSeePlayer),
                        
                        // Действие: Бежать к игроку
                        new ActionNode(RunToPlayer),
                        
                        // Действие: Атаковать игрока
                        new ActionNode(AttackPlayer)
                    }),
                    
                    // --- ВЕТКА 2: ПРОВЕРКА ШУМА (средний приоритет) ---
                    new Sequence(new List<Node>
                    {
                        // Условие: Слышу ли я что-то подозрительное?
                        new ConditionNode(HearNoise),
                        
                        // Действие: Идти на шум
                        new ActionNode(MoveToNoise),
                        
                        // Действие: Осмотреться (повторить 3 раза)
                        new Repeater(new ActionNode(LookAround), 3)
                    }),
                    
                    // --- ВЕТКА 3: ПАТРУЛЬ (самый низкий приоритет) ---
                    new Sequence(new List<Node>
                    {
                        // Действие: Патрулировать по точкам
                        new ActionNode(Patrol)
                    })
                })
            );
            
            // --- НАСТРАИВАЕМ BLACKBOARD (общая память) ---
            
            // Сохраняем ссылки на компоненты и настройки
            _behaviorTree.Blackboard.SetValue("Self", _transform);
            _behaviorTree.Blackboard.SetValue("Player", player);
            _behaviorTree.Blackboard.SetValue("PatrolPoints", patrolPoints);
            _behaviorTree.Blackboard.SetValue("ObstacleMask", obstacleMask);
            
            // Сохраняем настройки
            _behaviorTree.Blackboard.SetValue("WalkSpeed", walkSpeed);
            _behaviorTree.Blackboard.SetValue("RunSpeed", runSpeed);
            _behaviorTree.Blackboard.SetValue("RotationSpeed", rotationSpeed);
            _behaviorTree.Blackboard.SetValue("StoppingDistance", stoppingDistance);
            _behaviorTree.Blackboard.SetValue("WaitTimeAtPoint", waitTimeAtPoint);
            _behaviorTree.Blackboard.SetValue("SightRange", sightRange);
            _behaviorTree.Blackboard.SetValue("AttackRange", attackRange);
            _behaviorTree.Blackboard.SetValue("HearingRange", hearingRange);
            
            // Сохраняем переменные состояний (они будут меняться)
            _behaviorTree.Blackboard.SetValue("CurrentPatrolIndex", _currentPatrolIndex);
            _behaviorTree.Blackboard.SetValue("WaitTimer", _waitTimer);
            _behaviorTree.Blackboard.SetValue("AttackTimer", _attackTimer);
            _behaviorTree.Blackboard.SetValue("LastNoisePosition", _lastNoisePosition);
            
            // Здесь будем хранить цель для перемещения
            _behaviorTree.Blackboard.SetValue("TargetPosition", Vector3.zero);
            // Здесь будем хранить, бежать или идти
            _behaviorTree.Blackboard.SetValue("IsRunning", false);
        }

        void Update()
        {
            // Каждый кадр обновляем дерево
            if (_behaviorTree != null)
            {
                _behaviorTree.Tick();
                
                // Обновляем значения в Blackboard из локальных переменных (чтобы сохранять состояние)
                // Это нужно, потому что Blackboard хранит копии значений
                _behaviorTree.Blackboard.SetValue("CurrentPatrolIndex", _currentPatrolIndex);
                _behaviorTree.Blackboard.SetValue("WaitTimer", _waitTimer);
                _behaviorTree.Blackboard.SetValue("AttackTimer", _attackTimer);
                _behaviorTree.Blackboard.SetValue("LastNoisePosition", _lastNoisePosition);
            }
        }

        // --- ВСПОМОГАТЕЛЬНЫЙ МЕТОД ДЛЯ ПЕРЕМЕЩЕНИЯ ---
        
        // Универсальный метод для движения к цели
        private void MoveToTarget(Transform self, Vector3 target, float speed)
        {
            // Направление к цели
            Vector3 direction = (target - self.position).normalized;
            
            // Если направление не нулевое (цель не прямо на нас)
            if (direction != Vector3.zero)
            {
                // Плавно поворачиваемся в сторону движения
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                self.rotation = Quaternion.Slerp(self.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            
            // Двигаемся вперед (в сторону, куда смотрим)
            // Важно: используем self.forward, а не direction, чтобы охранник шел туда, куда повернут
            Vector3 move = self.forward * speed * Time.deltaTime;
            self.position += move;
        }

        // --- РЕАЛИЗАЦИЯ УСЛОВИЙ (ConditionNode) ---
        // Эти методы возвращают bool: true = условие выполнено, false = нет
        
        // Проверка: Видит ли охранник игрока?
        private bool CanSeePlayer(Blackboard bb)
        {
            Transform self = bb.GetValue<Transform>("Self");
            Transform target = bb.GetValue<Transform>("Player");
            float range = bb.GetValue<float>("SightRange");
            LayerMask mask = bb.GetValue<LayerMask>("ObstacleMask");

            if (target == null || self == null) 
                return false;

            // Вектор от охранника к игроку
            Vector3 directionToPlayer = (target.position - self.position).normalized;
            float distanceToPlayer = Vector3.Distance(self.position, target.position);

            // Проверка 1: Игрок дальше радиуса видимости?
            if (distanceToPlayer > range) 
                return false;

            // Проверка 2: Висит луч (Raycast), чтобы проверить, не за стеной ли игрок
            // Это называется Line of Sight (LOS) проверка
            RaycastHit hit;
            if (Physics.Raycast(self.position, directionToPlayer, out hit, range, mask))
            {
                // Если луч попал в игрока (проверяем по тегу "Player")
                if (hit.transform.CompareTag("Player"))
                {
                    // Сохраняем позицию игрока, чтобы другие узлы знали, куда идти
                    bb.SetValue("TargetPosition", target.position);
                    // Говорим, что нужно бежать (run speed)
                    bb.SetValue("IsRunning", true);
                    return true;
                }
            }
            
            return false;
        }

        // Проверка: Слышит ли охранник шум?
        private bool HearNoise(Blackboard bb)
        {
            Transform self = bb.GetValue<Transform>("Self");
            float range = bb.GetValue<float>("HearingRange");
            
            // Получаем позицию последнего шума из Blackboard
            Vector3 noisePos = bb.GetValue<Vector3>("LastNoisePosition", Vector3.zero);
            
            // Если шума не было (позиция нулевая) - не слышим
            if (noisePos == Vector3.zero) 
                return false;

            // Дистанция до источника шума
            float distance = Vector3.Distance(self.position, noisePos);
            
            // Если шум в пределах слышимости
            if (distance <= range)
            {
                // Запоминаем, куда идти
                bb.SetValue("TargetPosition", noisePos);
                // На шум идем шагом (не бежим)
                bb.SetValue("IsRunning", false);
                
                // Очищаем шум, чтобы не бежать на один и тот же дважды
                bb.SetValue("LastNoisePosition", Vector3.zero);
                _lastNoisePosition = Vector3.zero; // Обновляем и локальную переменную
                
                return true;
            }
            
            return false;
        }

        // --- РЕАЛИЗАЦИЯ ДЕЙСТВИЙ (ActionNode) ---
        // Эти методы возвращают ENodeState:
        // Running - действие выполняется (еще не закончило)
        // Success - действие успешно завершено
        // Failure - действие провалилось

        // Действие: Бежать к игроку
        private ENodeState RunToPlayer(Blackboard bb)
        {
            Transform self = bb.GetValue<Transform>("Self");
            Vector3 targetPos = bb.GetValue<Vector3>("TargetPosition");
            float runSpeed = bb.GetValue<float>("RunSpeed");
            float stopDist = bb.GetValue<float>("StoppingDistance");
            float attackRange = bb.GetValue<float>("AttackRange");

            if (self == null) 
                return ENodeState.Failure;

            // Дистанция до цели
            float distance = Vector3.Distance(self.position, targetPos);
            
            // Если мы уже достаточно близко, чтобы атаковать
            if (distance <= attackRange)
            {
                return ENodeState.Success; // Переходим к атаке
            }

            // Иначе двигаемся к цели
            MoveToTarget(self, targetPos, runSpeed);
            
            // Мы все еще в процессе движения
            return ENodeState.Running;
        }

        // Действие: Атаковать игрока
        private ENodeState AttackPlayer(Blackboard bb)
        {
            Transform self = bb.GetValue<Transform>("Self");
            Transform target = bb.GetValue<Transform>("Player");
            float attackRange = bb.GetValue<float>("AttackRange");
            
            // Получаем таймер атаки из Blackboard (или создаем новый)
            float attackTimer = bb.GetValue<float>("AttackTimer", 0f);

            if (self == null || target == null) 
                return ENodeState.Failure;

            // Дистанция до игрока
            float distance = Vector3.Distance(self.position, target.position);
            
            // Если игрок убежал дальше дистанции атаки
            if (distance > attackRange)
            {
                return ENodeState.Failure; // Провал - нужно снова бежать
            }

            // Поворачиваемся лицом к игроку (медленно)
            Vector3 direction = (target.position - self.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                self.rotation = Quaternion.Slerp(self.rotation, lookRotation, rotationSpeed * 0.5f * Time.deltaTime);
            }

            // Логика атаки с перезарядкой
            attackTimer += Time.deltaTime;

            float attackCooldown = 1;
            
            // Если пришло время атаковать
            if (attackTimer >= attackCooldown)
            {
                // "Наносим урон" (в консоль)
                
                // Сбрасываем таймер
                attackTimer = 0f;
                
                // Здесь можно добавить настоящий урон, если у игрока есть компонент Health
                // player.GetComponent<Health>().TakeDamage(attackDamage);
            }
            
            // Сохраняем обновленный таймер
            bb.SetValue("AttackTimer", attackTimer);
            _attackTimer = attackTimer; // Обновляем локальную переменную

            // Атака - это процесс, который может длиться бесконечно
            // Возвращаем Running, пока игрок в пределах досягаемости
            return ENodeState.Running;
        }

        // Действие: Идти на источник шума
        private ENodeState MoveToNoise(Blackboard bb)
        {
            Transform self = bb.GetValue<Transform>("Self");
            Vector3 targetPos = bb.GetValue<Vector3>("TargetPosition");
            float walkSpeed = bb.GetValue<float>("WalkSpeed");
            float stopDist = bb.GetValue<float>("StoppingDistance");

            if (self == null) 
                return ENodeState.Failure;

            // Дистанция до точки шума
            float distance = Vector3.Distance(self.position, targetPos);
            
            // Если дошли до точки
            if (distance <= stopDist)
            {
                // Очищаем целевую позицию
                bb.SetValue("TargetPosition", Vector3.zero);
                return ENodeState.Success; // Успешно дошли
            }

            // Идем к точке
            MoveToTarget(self, targetPos, walkSpeed);
            return ENodeState.Running;
        }

        // Действие: Осмотреться (повертеться по сторонам)
        private ENodeState LookAround(Blackboard bb)
        {
            Transform self = bb.GetValue<Transform>("Self");
            
            // Получаем данные из Blackboard (или создаем новые)
            // Используем локальные переменные узла через Blackboard с уникальными ключами
            float lookTimer = bb.GetValue<float>("LookTimer", 0f);
            float lookDirection = bb.GetValue<float>("LookDirection", 1f); // 1 = вправо, -1 = влево

            // Увеличиваем таймер
            lookTimer += Time.deltaTime;

            // Поворачиваемся
            float rotateAmount = 90f * lookDirection * Time.deltaTime; // Поворачиваем на 90 градусов в секунду
            self.Rotate(0, rotateAmount, 0);

            // Меняем направление каждые 2 секунды
            if (lookTimer >= 2f)
            {
                lookDirection *= -1; // Меняем направление
                lookTimer = 0f; // Сбрасываем таймер
                
                Debug.Log($"{self.name} осматривается: поворот {lookDirection}");
            }

            // Сохраняем обновленные значения
            bb.SetValue("LookTimer", lookTimer);
            bb.SetValue("LookDirection", lookDirection);

            // Это действие всегда Running, пока его не прервет родитель (Repeater)
            return ENodeState.Running;
        }

        // Действие: Патрулирование
        private ENodeState Patrol(Blackboard bb)
        {
            Transform self = bb.GetValue<Transform>("Self");
            Transform[] points = bb.GetValue<Transform[]>("PatrolPoints");
            float walkSpeed = bb.GetValue<float>("WalkSpeed");
            float stopDist = bb.GetValue<float>("StoppingDistance");
            float waitTime = bb.GetValue<float>("WaitTimeAtPoint");
            
            // Получаем индекс текущей точки
            int pointIndex = bb.GetValue<int>("CurrentPatrolIndex", 0);
            float waitTimer = bb.GetValue<float>("WaitTimer", 0f);

            if (self == null || points == null || points.Length == 0)
                return ENodeState.Failure;

            // Текущая целевая точка
            Transform targetPoint = points[pointIndex];
            
            // Если точки нет (удалили), пропускаем
            if (targetPoint == null)
            {
                // Переходим к следующей
                pointIndex = (pointIndex + 1) % points.Length;
                bb.SetValue("CurrentPatrolIndex", pointIndex);
                _currentPatrolIndex = pointIndex;
                return ENodeState.Running;
            }

            // Дистанция до целевой точки
            float distance = Vector3.Distance(self.position, targetPoint.position);

            // Если мы достаточно близко к точке
            if (distance <= stopDist)
            {
                // Увеличиваем таймер ожидания
                waitTimer += Time.deltaTime;
                
                // Если подождали нужное время
                if (waitTimer >= waitTime)
                {
                    // Переключаемся на следующую точку
                    pointIndex = (pointIndex + 1) % points.Length;
                    waitTimer = 0f;
                    
                    Debug.Log($"{self.name} идет к следующей точке {pointIndex}");
                    
                    // Сохраняем обновленные значения
                    bb.SetValue("CurrentPatrolIndex", pointIndex);
                    bb.SetValue("WaitTimer", waitTimer);
                    _currentPatrolIndex = pointIndex;
                    _waitTimer = waitTimer;
                    
                    // Успешно закончили ожидание на точке
                    return ENodeState.Success;
                }
                
                // Если еще ждем - ничего не делаем, просто стоим
                // Сохраняем таймер
                bb.SetValue("WaitTimer", waitTimer);
                _waitTimer = waitTimer;
                
                return ENodeState.Running;
            }
            
            // Идем к точке
            MoveToTarget(self, targetPoint.position, walkSpeed);
            
            // Обнуляем таймер ожидания (мы же идем, а не ждем)
            bb.SetValue("WaitTimer", 0f);
            _waitTimer = 0f;
            
            return ENodeState.Running;
        }

        // --- ПУБЛИЧНЫЙ МЕТОД ДЛЯ СОЗДАНИЯ ШУМА ---
        // Этот метод можно вызвать из другого скрипта (например, когда игрок бежит)
        public void MakeNoise(Vector3 noisePosition)
        {
            _lastNoisePosition = noisePosition;
            Debug.Log($"{gameObject.name} услышал шум в позиции {noisePosition}");
        }
        
        // Визуализация для отладки (рисует линии в редакторе)
        private void OnDrawGizmosSelected()
        {
            // Рисуем радиус зрения
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);
            
            // Рисуем радиус слуха
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, hearingRange);
            
            // Рисуем радиус атаки
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            
            // Соединяем точки патруля линиями
            if (patrolPoints != null && patrolPoints.Length > 0)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < patrolPoints.Length; i++)
                {
                    if (patrolPoints[i] != null)
                    {
                        // Рисуем сферу в точке патруля
                        Gizmos.DrawSphere(patrolPoints[i].position, 0.3f);
                        
                        // Рисуем линию до следующей точки
                        int next = (i + 1) % patrolPoints.Length;
                        if (patrolPoints[next] != null)
                        {
                            Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[next].position);
                        }
                    }
                }
            }
        }
    }
}