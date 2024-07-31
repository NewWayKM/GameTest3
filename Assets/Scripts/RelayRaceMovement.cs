using Unity.VisualScripting;
using UnityEngine;

public class RelayRaceMovement : MonoBehaviour
{
    public Transform[] runners; // ������ ����������� �������
    public float speed = 1.0f; // �������� ��������
    public float passDistance = 1.0f; // ���������� �������� ��������

    private int currentRunner = 0; // ������ �������� ������

    public GameObject baton; // ������, ������� ���������� ����� ��������
    public Vector3 batonOffset = new Vector3(0.5f, 2.0f, 0.5f); // ����� ��� �������� ������ ���������� �������
    private GameObject batonInstance; // ��������� �������


    void Start()
    {
        batonInstance = InitializeBaton(baton, runners, batonOffset); // ���������� ������� ���������� ������� � ������� ������
    }
    void Update()
    {
        if (runners.Length == 0 || batonInstance == null) return;  // ���������, ���� �� ������

        Transform current = runners[currentRunner]; // ������� �����
        Transform next = runners[(currentRunner + 1) % runners.Length]; // ��������� �����

        current.LookAt(next); // ��������� �������� ������ � ������� ����������

        (Animator currentAnimator, Animator nextAnimator) = GetComponentAnimatorAndNull(current, next); // ������� ������� ���� � �����, ��� ���������� ������� � Update
        if (currentAnimator == null || nextAnimator == null) return;

        current.position = Vector3.MoveTowards(current.position, next.position, speed * Time.deltaTime); // ������� �������� ������ � ����������

        float distance = Vector3.Distance(current.position, next.position);
        Debug.Log($"���������� �� ���������� ������: {distance}");

        RunLogicDistance(current, next, currentAnimator, nextAnimator, ref currentRunner, passDistance, distance); // ������� ������� ���� � �����, ��� ���������� ������� � Update
    }

    void TransferBaton(Transform fromRunner, Transform toRunner)
    /*/ ������������� ������ ��� �������� � ���������� ������ � ������ �������� /*/
    {
        if (batonInstance != null) 
        {
            batonInstance.transform.SetParent(toRunner);
            batonInstance.transform.localPosition = batonOffset;
            Debug.Log("�������� ������� �� " + fromRunner.name + " � " + toRunner.name);
        }
    }

    private void RunLogicDistance(Transform current, Transform next, Animator currentAnimator, Animator nextAnimator, ref int currentRunner, float passDistance, float distance)
    /*/ ����������� ��������, ����������� ������� /*/
    {
        if (distance < passDistance)
        {
            Debug.Log("����� ������");
            Debug.Log("������������� �������� ������: " + current.name);
            currentAnimator.SetBool("isRunBool", false);

            Debug.Log("��������� ���������� ������: " + next.name);
            nextAnimator.SetBool("isRunBool", true);

            currentRunner = (currentRunner + 1) % runners.Length; // ������������� �� ���������� ������

            TransferBaton(current, next); // �������� ������ ���������� ������
        }
        else
        {
            currentAnimator.SetBool("isRunBool", true); // ���� ����� �� ��� ��������, ���������� ��������
            Debug.Log("������� ����� ���������� ���������: " + current.name);
        }
    }

    private (Animator, Animator) GetComponentAnimatorAndNull(Transform current, Transform next)
    /*/ ��������� �������� ��� ������� /*/
    {
        Animator currentAnimator = current.GetComponent<Animator>();
        Animator nextAnimator = next.GetComponent<Animator>();

        if (currentAnimator == null)
        {
            Debug.LogError("����������� Animator � �������� ������: " + current.name);
            return (null, null);
        }

        if (nextAnimator == null)
        {
            Debug.LogError("����������� Animator � ���������� ������: " + next.name);
            return (null, null);
        }

        return (currentAnimator, nextAnimator);
    }

    private GameObject InitializeBaton(GameObject batonPrefab, Transform[] runners, Vector3 batonOffset)
    /*/ ���������� ������� ���������� ������� � ������� ������ /*/
    {
        GameObject batonInstance = null;

        if (batonPrefab != null) // ������� ��������� ������� �� Prefab
        {
            batonInstance = Instantiate(batonPrefab);
        }
        else
        {
            Debug.LogError("Prefab ������� �� ����������!");
        }

        if (batonInstance != null && runners.Length > 0) // ������������� ��������� ��������� � �������� ��� �������
        {
            batonInstance.transform.SetParent(runners[0]);
            batonInstance.transform.localPosition = batonOffset;
        }

        return batonInstance;
    }
}