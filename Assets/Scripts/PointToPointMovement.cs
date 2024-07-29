using UnityEngine;

public class PointToPointMovement : MonoBehaviour
{
    // ������ �����
    public Vector3[] points;
    // �������� ��������
    public float speed = 1.0f;
    // ������ ������� �����
    private int currentIndex = 0;
    // ����������� ��������
    private bool forward = true;

    void Update()
    {
        // ���������, ���� �� �����
        if (points.Length == 0) return;

        // ������� ����
        Vector3 target = points[currentIndex];
        // ������� ������ � ����
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // ���� ������ ������ ����
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