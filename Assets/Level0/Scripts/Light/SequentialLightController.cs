using System.Collections;
using UnityEngine;

public class SequentialLightController : MonoBehaviour
{
    public Light[] lights; // Массив для хранения объектов света
    public float delay = 1.0f; // Задержка между включением светов

    public void LightsOn()
    {
        // Начинаем корутину для поочередного включения светов
        StartCoroutine(TurnOnLightsSequentially());
    }

    public IEnumerator TurnOnLightsSequentially()
    {
        foreach (Light light in lights)
        {
            light.enabled = true; // Включаем свет
            yield return new WaitForSeconds(delay); // Ждем заданный интервал
        }
    }
}

