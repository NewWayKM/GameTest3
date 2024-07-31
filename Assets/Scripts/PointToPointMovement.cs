using UnityEngine;

public class PointToPointMovement : MonoBehaviour
{
    public Vector3[] points; // Массив точек

    public float speed = 1.0f; // Скорость движения

    private int currentIndex = 0; // Индекс текущей точки

    private bool forward = true; // Направление движения

    void Update()
    {
        
        if (points.Length == 0) return; // Проверяем, есть ли точки

        Vector3 target = points[currentIndex]; // Текущая цель
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime); // Двигаем объект к цели

        if (Vector3.Distance(transform.position, target) < 0.1f) // Если объект достиг цели
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