using Unity.VisualScripting;
using UnityEngine;

public class RelayRaceMovement : MonoBehaviour
{
    public Transform[] runners; // Массив трансформов бегунов
    public float speed = 1.0f; // Скорость движения
    public float passDistance = 1.0f; // Расстояние передачи эстафеты

    private int currentRunner = 0; // Индекс текущего бегуна

    public GameObject baton; // Объект, который передается между бегунами
    public Vector3 batonOffset = new Vector3(0.5f, 2.0f, 0.5f); // офсет для смещения иконки эстафетной палочки
    private GameObject batonInstance; // Экземпляр объекта


    void Start()
    {
        batonInstance = InitializeBaton(baton, runners, batonOffset); // присвоение префаба эстафетной палочки к первому бегуну
    }
    void Update()
    {
        if (runners.Length == 0 || batonInstance == null) return;  // Проверяем, есть ли бегуны

        Transform current = runners[currentRunner]; // Текущий бегун
        Transform next = runners[(currentRunner + 1) % runners.Length]; // Следующий бегун

        current.LookAt(next); // Повернуть текущего бегуна в сторону следующего

        (Animator currentAnimator, Animator nextAnimator) = GetComponentAnimatorAndNull(current, next); // перевел участок кода в метод, для сокращения строчек в Update
        if (currentAnimator == null || nextAnimator == null) return;

        current.position = Vector3.MoveTowards(current.position, next.position, speed * Time.deltaTime); // Двигаем текущего бегуна к следующему

        float distance = Vector3.Distance(current.position, next.position);
        Debug.Log($"Расстояние до следующего бегуна: {distance}");

        RunLogicDistance(current, next, currentAnimator, nextAnimator, ref currentRunner, passDistance, distance); // перевел участок кода в метод, для сокращения строчек в Update
    }

    void TransferBaton(Transform fromRunner, Transform toRunner)
    /*/ Устанавливаем объект как дочерний к следующему бегуну и задаем смещение /*/
    {
        if (batonInstance != null) 
        {
            batonInstance.transform.SetParent(toRunner);
            batonInstance.transform.localPosition = batonOffset;
            Debug.Log("Передача объекта от " + fromRunner.name + " к " + toRunner.name);
        }
    }

    private void RunLogicDistance(Transform current, Transform next, Animator currentAnimator, Animator nextAnimator, ref int currentRunner, float passDistance, float distance)
    /*/ переключает анимации, переключает бегунов /*/
    {
        if (distance < passDistance)
        {
            Debug.Log("Смена бегуна");
            Debug.Log("Останавливаем текущего бегуна: " + current.name);
            currentAnimator.SetBool("isRunBool", false);

            Debug.Log("Запускаем следующего бегуна: " + next.name);
            nextAnimator.SetBool("isRunBool", true);

            currentRunner = (currentRunner + 1) % runners.Length; // Переключаемся на следующего бегуна

            TransferBaton(current, next); // Передаем объект следующему бегуну
        }
        else
        {
            currentAnimator.SetBool("isRunBool", true); // Если бегун всё ещё движется, продолжаем анимацию
            Debug.Log("Текущий бегун продолжает двигаться: " + current.name);
        }
    }

    private (Animator, Animator) GetComponentAnimatorAndNull(Transform current, Transform next)
    /*/ получение анимаций для бегунов /*/
    {
        Animator currentAnimator = current.GetComponent<Animator>();
        Animator nextAnimator = next.GetComponent<Animator>();

        if (currentAnimator == null)
        {
            Debug.LogError("Отсутствует Animator у текущего бегуна: " + current.name);
            return (null, null);
        }

        if (nextAnimator == null)
        {
            Debug.LogError("Отсутствует Animator у следующего бегуна: " + next.name);
            return (null, null);
        }

        return (currentAnimator, nextAnimator);
    }

    private GameObject InitializeBaton(GameObject batonPrefab, Transform[] runners, Vector3 batonOffset)
    /*/ присвоение префаба эстафетной палочки к первому бегуну /*/
    {
        GameObject batonInstance = null;

        if (batonPrefab != null) // Создаем экземпляр объекта из Prefab
        {
            batonInstance = Instantiate(batonPrefab);
        }
        else
        {
            Debug.LogError("Prefab объекта не установлен!");
        }

        if (batonInstance != null && runners.Length > 0) // Устанавливаем начальное положение и родителя для объекта
        {
            batonInstance.transform.SetParent(runners[0]);
            batonInstance.transform.localPosition = batonOffset;
        }

        return batonInstance;
    }
}