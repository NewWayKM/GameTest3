using UnityEngine;

public class RelayRaceMovement : MonoBehaviour
{
    // Массив трансформов бегунов
    public Transform[] runners;
    // Скорость движения
    public float speed = 1.0f;
    // Расстояние передачи эстафеты
    public float passDistance = 1.0f;
    // Индекс текущего бегуна
    private int currentRunner = 0;

    void Update()
    {
        // Проверяем, есть ли бегуны
        if (runners.Length == 0) return;

        // Текущий и следующий бегун
        Transform current = runners[currentRunner];
        Transform next = runners[(currentRunner + 1) % runners.Length];

        // Двигаем текущего бегуна к следующему
        current.position = Vector3.MoveTowards(current.position, next.position, speed * Time.deltaTime);

        // Если расстояние до следующего бегуна меньше passDistance
        if (Vector3.Distance(current.position, next.position) < passDistance)
        {
            // Переключаемся на следующего бегуна
            currentRunner = (currentRunner + 1) % runners.Length;
        }
    }
}