using System.Collections;
using UnityEngine;

public class SequentialLightController : MonoBehaviour
{
    public Light[] lights; // ������ ��� �������� �������� �����
    public float delay = 1.0f; // �������� ����� ���������� ������

    public void LightsOn()
    {
        // �������� �������� ��� ������������ ��������� ������
        StartCoroutine(TurnOnLightsSequentially());
    }

    public IEnumerator TurnOnLightsSequentially()
    {
        foreach (Light light in lights)
        {
            light.enabled = true; // �������� ����
            yield return new WaitForSeconds(delay); // ���� �������� ��������
        }
    }
}

