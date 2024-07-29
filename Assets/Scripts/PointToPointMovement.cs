using UnityEngine;

public class PointToPointMovement : MonoBehaviour
{
    // Массив точек
    public Vector3[] points;
    // Скорость движения
    public float speed = 1.0f;
    // Индекс текущей точки
    private int currentIndex = 0;
    // Направление движения
    private bool forward = true;

    void Update()
    {
        // Проверяем, есть ли точки
        if (points.Length == 0) return;

        // Текущая цель
        Vector3 target = points[currentIndex];
        // Двигаем объект к цели
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Если объект достиг цели
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            if (forward)
            {
                currentIndex++;
                if (currentIndex >= points.Length)
                {
                    currentIndex = points.Length - 1;
                    forward = false;
                }
            }
            else
            {
                currentIndex--;
                if (currentIndex < 0)
                {
                    currentIndex = 0;
                    forward = true;
                }
            }
        }
    }
}