using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YAxisScaler : MonoBehaviour
{
    public Transform characterTransform; // 캐릭터의 Transform 컴포넌트
    public float minScale = 1f; // 최소 스케일
    public float maxScale = 2f; // 최대 스케일
    public float minYPosition = -20f; // Y 위치의 최소 값
    public float maxYPosition = 0f; // Y 위치의 최대 값

    private void Awake()
    {
        characterTransform = GetComponent<Transform>();
    }
    private void Update()
    {
        if (characterTransform != null)
        {
            // 현재 캐릭터의 Y 위치를 가져옵니다.
            float currentY = characterTransform.position.y;

            // Y 위치를 기반으로 스케일을 계산
            float t = Mathf.InverseLerp(minYPosition, maxYPosition, currentY);
            float newScale = Mathf.Lerp(maxScale, minScale, t);

            // 새로운 스케일을 적용
            characterTransform.localScale = new Vector3(newScale, newScale, characterTransform.localScale.z);
        }
    }
}
