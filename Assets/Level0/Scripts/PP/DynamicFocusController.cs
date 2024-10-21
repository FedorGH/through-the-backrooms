using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DynamicFocusControllerWithRaycast : MonoBehaviour
{
    public Camera mainCamera;             // Камера, которая выполняет Raycast
    public float focusSpeed = 2f;         // Скорость изменения фокуса
    public float maxFocusDistance = 10f;  // Максимальная дистанция Raycast для фокуса
    public Volume globalVolume;           // Volume для работы с эффектами
    private DepthOfField dofSettings;     // Настройки Depth of Field

    void Start()
    {
        // Проверяем наличие Volume, если нет - пытаемся найти его
        if (globalVolume == null)
        {
            globalVolume = GetComponent<Volume>();
        }

        // Проверка наличия Volume
        if (globalVolume == null)
        {
            Debug.LogError("Volume не найден! Пожалуйста, добавьте Volume к этому объекту.");
            return;
        }

        // Пытаемся найти Depth of Field в профиле
        if (!globalVolume.profile.TryGet(out dofSettings))
        {
            Debug.LogWarning("Настройки Depth of Field не найдены в Volume Profile. Добавляем DOF вручную...");

            // Добавляем DOF, если его нет
            dofSettings = globalVolume.profile.Add<DepthOfField>(true);
        }

        // Настраиваем DOF на Manual Ranges и активируем его
        dofSettings.active = true;
        dofSettings.focusMode.value = DepthOfFieldMode.Manual;

        Debug.Log("DOF активирован и настроен на Manual Ranges.");
    }

    void Update()
    {
        if (dofSettings != null && dofSettings.focusMode.value == DepthOfFieldMode.Manual)
        {
            // Выполняем Raycast из камеры вперед
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hit;

            // Если Raycast попадает в объект в пределах максимального расстояния
            if (Physics.Raycast(ray, out hit, maxFocusDistance))
            {
                float distanceToFocus = hit.distance;  // Расстояние до объекта
                AdjustFocus(distanceToFocus);          // Изменяем фокусное расстояние

                // Логируем информацию об объекте
                Debug.Log($"Объект найден: {hit.collider.name} на расстоянии {distanceToFocus:F2} метров.");
            }
            else
            {
                // Если луч никуда не попал, выводим сообщение
                Debug.Log("Raycast не нашел объектов для фокусировки.");
            }
        }
        else
        {
            Debug.LogWarning("DOF не настроен или не активен.");
        }
    }

    // Метод для изменения фокуса
    private void AdjustFocus(float focusDistance)
    {
        // Устанавливаем параметры размытия для ближнего и дальнего фокуса
        dofSettings.nearFocusStart.value = Mathf.Lerp(dofSettings.nearFocusStart.value, focusDistance - 2f, focusSpeed * Time.deltaTime); // Начало ближнего фокуса
        dofSettings.nearFocusEnd.value = Mathf.Lerp(dofSettings.nearFocusEnd.value, focusDistance, focusSpeed * Time.deltaTime);           // Конец ближнего фокуса
        dofSettings.farFocusStart.value = Mathf.Lerp(dofSettings.farFocusStart.value, focusDistance + 2f, focusSpeed * Time.deltaTime);    // Начало дальнего фокуса
        dofSettings.farFocusEnd.value = Mathf.Lerp(dofSettings.farFocusEnd.value, focusDistance + 5f, focusSpeed * Time.deltaTime);        // Конец дальнего фокуса

        // Выводим лог для отладки
        Debug.Log($"Фокусное расстояние настроено на: {focusDistance:F2} метров.");
    }

    // Отображаем линию Raycast с помощью Gizmos
    private void OnDrawGizmosSelected()
    {
        if (mainCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * maxFocusDistance);
        }
    }
}
