using UnityEngine;

public class FanEvents : MonoBehaviour
{
    public AudioSource AudioFan;
    public static float rotationSpeed = 400f; // Текущая скорость вращения
    private float targetSpeed = 400f;         // Целевая скорость вращения
    public float speedChangeDuration = 1f;    // Время для плавного изменения скорости
    private float elapsedTime = 0f;           // Таймер для интерполяции
    private int currentSpeedLevel = -1;       // Уровень текущей скорости

    private readonly float[] speeds = { 0f, 400f, 700f, 1000f }; // Скорости для разных уровней
    private readonly float[] pitches = { 0f, 1f, 1.2f, 1.5f };   // Питчи для звука вентилятора

    void Update()
    {
        // Плавно изменяем скорость, если текущая не равна целевой
        if (!Mathf.Approximately(rotationSpeed, targetSpeed))
        {
            elapsedTime += Time.deltaTime;
            rotationSpeed = Mathf.Lerp(rotationSpeed, targetSpeed, elapsedTime / speedChangeDuration);
        }

        // Вращаем объект по локальной оси X
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime, Space.Self);
    }

    // Изменяем скорость в зависимости от уровня
    public void SetSpeedLevel(int speedLevel)
    {
        if (speedLevel < 0 || speedLevel >= speeds.Length || currentSpeedLevel == speedLevel) return;

        currentSpeedLevel = speedLevel;
        SetTargetSpeed(speeds[speedLevel]);

        // Обновляем параметры звука
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

    // Метод для задания новой целевой скорости и сброса таймера
    private void SetTargetSpeed(float newSpeed)
    {
        targetSpeed = newSpeed;
        elapsedTime = 0f; // Сбрасываем таймер
    }

    // Методы для нажатий кнопок
    public void SpeedOne() => SetSpeedLevel(1);
    public void SpeedTwo() => SetSpeedLevel(2);
    public void SpeedThree() => SetSpeedLevel(3);
    public void SpeedZero() => SetSpeedLevel(0);
}
