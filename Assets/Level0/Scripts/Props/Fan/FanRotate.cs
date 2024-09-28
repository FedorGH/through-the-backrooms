using UnityEngine;

public class FanEvents : MonoBehaviour
{
    public AudioSource AudioFan;
    public static float rotationSpeed = 400f; // ������� �������� ��������
    private float targetSpeed = 400f;         // ������� �������� ��������
    public float speedChangeDuration = 1f;    // ����� ��� �������� ��������� ��������
    private float elapsedTime = 0f;           // ������ ��� ������������
    private int currentSpeedLevel = -1;       // ������� ������� ��������

    private readonly float[] speeds = { 0f, 400f, 700f, 1000f }; // �������� ��� ������ �������
    private readonly float[] pitches = { 0f, 1f, 1.2f, 1.5f };   // ����� ��� ����� �����������

    void Update()
    {
        // ������ �������� ��������, ���� ������� �� ����� �������
        if (!Mathf.Approximately(rotationSpeed, targetSpeed))
        {
            elapsedTime += Time.deltaTime;
            rotationSpeed = Mathf.Lerp(rotationSpeed, targetSpeed, elapsedTime / speedChangeDuration);
        }

        // ������� ������ �� ��������� ��� X
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime, Space.Self);
    }

    // �������� �������� � ����������� �� ������
    public void SetSpeedLevel(int speedLevel)
    {
        if (speedLevel < 0 || speedLevel >= speeds.Length || currentSpeedLevel == speedLevel) return;

        currentSpeedLevel = speedLevel;
        SetTargetSpeed(speeds[speedLevel]);

        // ��������� ��������� �����
        if (speedLevel == 0)
        {
            AudioFan.Stop();
        }
        else
        {
            AudioFan.pitch = pitches[speedLevel];
            if (!AudioFan.isPlaying) AudioFan.Play();
        }
    }

    // ����� ��� ������� ����� ������� �������� � ������ �������
    private void SetTargetSpeed(float newSpeed)
    {
        targetSpeed = newSpeed;
        elapsedTime = 0f; // ���������� ������
    }

    // ������ ��� ������� ������
    public void SpeedOne() => SetSpeedLevel(1);
    public void SpeedTwo() => SetSpeedLevel(2);
    public void SpeedThree() => SetSpeedLevel(3);
    public void SpeedZero() => SetSpeedLevel(0);
}
