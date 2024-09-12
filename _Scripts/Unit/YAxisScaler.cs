using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YAxisScaler : MonoBehaviour
{
    public Transform characterTransform; // ĳ������ Transform ������Ʈ
    public float minScale = 1f; // �ּ� ������
    public float maxScale = 2f; // �ִ� ������
    public float minYPosition = -20f; // Y ��ġ�� �ּ� ��
    public float maxYPosition = 0f; // Y ��ġ�� �ִ� ��

    private void Awake()
    {
        characterTransform = GetComponent<Transform>();
    }
    private void Update()
    {
        if (characterTransform != null)
        {
            // ���� ĳ������ Y ��ġ�� �����ɴϴ�.
            float currentY = characterTransform.position.y;

            // Y ��ġ�� ������� �������� ���
            float t = Mathf.InverseLerp(minYPosition, maxYPosition, currentY);
            float newScale = Mathf.Lerp(maxScale, minScale, t);

            // ���ο� �������� ����
            characterTransform.localScale = new Vector3(newScale, newScale, characterTransform.localScale.z);
        }
    }
}
