using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DynamicFocusControllerWithRaycast : MonoBehaviour
{
    public Camera mainCamera;             // ������, ������� ��������� Raycast
    public float focusSpeed = 2f;         // �������� ��������� ������
    public float maxFocusDistance = 10f;  // ������������ ��������� Raycast ��� ������
    public Volume globalVolume;           // Volume ��� ������ � ���������
    private DepthOfField dofSettings;     // ��������� Depth of Field

    void Start()
    {
        // ��������� ������� Volume, ���� ��� - �������� ����� ���
        if (globalVolume == null)
        {
            globalVolume = GetComponent<Volume>();
        }

        // �������� ������� Volume
        if (globalVolume == null)
        {
            Debug.LogError("Volume �� ������! ����������, �������� Volume � ����� �������.");
            return;
        }

        // �������� ����� Depth of Field � �������
        if (!globalVolume.profile.TryGet(out dofSettings))
        {
            Debug.LogWarning("��������� Depth of Field �� ������� � Volume Profile. ��������� DOF �������...");

            // ��������� DOF, ���� ��� ���
            dofSettings = globalVolume.profile.Add<DepthOfField>(true);
        }

        // ����������� DOF �� Manual Ranges � ���������� ���
        dofSettings.active = true;
        dofSettings.focusMode.value = DepthOfFieldMode.Manual;

        Debug.Log("DOF ����������� � �������� �� Manual Ranges.");
    }

    void Update()
    {
        if (dofSettings != null && dofSettings.focusMode.value == DepthOfFieldMode.Manual)
        {
            // ��������� Raycast �� ������ ������
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hit;

            // ���� Raycast �������� � ������ � �������� ������������� ����������
            if (Physics.Raycast(ray, out hit, maxFocusDistance))
            {
                float distanceToFocus = hit.distance;  // ���������� �� �������
                AdjustFocus(distanceToFocus);          // �������� �������� ����������

                // �������� ���������� �� �������
                Debug.Log($"������ ������: {hit.collider.name} �� ���������� {distanceToFocus:F2} ������.");
            }
            else
            {
                // ���� ��� ������ �� �����, ������� ���������
                Debug.Log("Raycast �� ����� �������� ��� �����������.");
            }
        }
        else
        {
            Debug.LogWarning("DOF �� �������� ��� �� �������.");
        }
    }

    // ����� ��� ��������� ������
    private void AdjustFocus(float focusDistance)
    {
        // ������������� ��������� �������� ��� �������� � �������� ������
        dofSettings.nearFocusStart.value = Mathf.Lerp(dofSettings.nearFocusStart.value, focusDistance - 2f, focusSpeed * Time.deltaTime); // ������ �������� ������
        dofSettings.nearFocusEnd.value = Mathf.Lerp(dofSettings.nearFocusEnd.value, focusDistance, focusSpeed * Time.deltaTime);           // ����� �������� ������
        dofSettings.farFocusStart.value = Mathf.Lerp(dofSettings.farFocusStart.value, focusDistance + 2f, focusSpeed * Time.deltaTime);    // ������ �������� ������
        dofSettings.farFocusEnd.value = Mathf.Lerp(dofSettings.farFocusEnd.value, focusDistance + 5f, focusSpeed * Time.deltaTime);        // ����� �������� ������

        // ������� ��� ��� �������
        Debug.Log($"�������� ���������� ��������� ��: {focusDistance:F2} ������.");
    }

    // ���������� ����� Raycast � ������� Gizmos
    private void OnDrawGizmosSelected()
    {
        if (mainCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * maxFocusDistance);
        }
    }
}
