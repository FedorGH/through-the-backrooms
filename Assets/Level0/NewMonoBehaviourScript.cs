using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Скорость вращения
    public float rotationSpeed = 50f;

    void Update()
    {
        // Вращаем объект по его локальной оси X
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
