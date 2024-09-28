using UnityEngine;

public class FanRotate : MonoBehaviour
{
    // �������� ��������
    public float rotationSpeed = 50f;

    void Update()
    {
        // ������� ������ �� ��� ��������� ��� X
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
