using UnityEngine;

public class RelayRaceMovement : MonoBehaviour
{
    // ������ ����������� �������
    public Transform[] runners;
    // �������� ��������
    public float speed = 1.0f;
    // ���������� �������� ��������
    public float passDistance = 1.0f;
    // ������ �������� ������
    private int currentRunner = 0;

    void Update()
    {
        // ���������, ���� �� ������
        if (runners.Length == 0) return;

        // ������� � ��������� �����
        Transform current = runners[currentRunner];
        Transform next = runners[(currentRunner + 1) % runners.Length];

        // ������� �������� ������ � ����������
        current.position = Vector3.MoveTowards(current.position, next.position, speed * Time.deltaTime);

        // ���� ���������� �� ���������� ������ ������ passDistance
        if (Vector3.Distance(current.position, next.position) < passDistance)
        {
            // ������������� �� ���������� ������
            currentRunner = (currentRunner + 1) % runners.Length;
        }
    }
}