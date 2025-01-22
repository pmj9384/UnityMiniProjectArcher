using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Slider를 사용하기 위해 필요

public class Player : MonoBehaviour
{
    public static Player Instance;
    private int currentExperience;
    public Slider experienceSlider;  // UI 슬라이더를 연결
    public int maxExperience = 100;  // 최대 경험치 (예시 값)

    void Awake()
    {
        Instance = this;
    }

    public void AddExperience(int amount)
    {
        currentExperience += amount;
        if (currentExperience > maxExperience)  // 최대 경험치를 초과하지 않도록 설정
        {
            currentExperience = maxExperience;
        }

        Debug.Log("Gained " + amount + " experience!");

        // 경험치 바 업데이트
        UpdateExperienceSlider();
    }

    private void UpdateExperienceSlider()
    {
        // 슬라이더의 값은 경험치 / 최대 경험치로 비율을 설정
        experienceSlider.value = (float)currentExperience / maxExperience;
    }
}
