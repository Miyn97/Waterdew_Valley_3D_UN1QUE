using UnityEngine;

// 수면 기준과 부력 계산을 전담하는 유틸리티 클래스
public static class WaterSystem
{
    private static float waterSurfaceY = 0.5f; // 수면 y 기준값 (게임 시작 시 설정되거나 고정값으로 사용)
    private static float buoyancyStrength = 3f; // 부력 강도

    // 수면 높이 설정 메서드
    public static void SetWaterSurfaceY(float surfaceY)
    {
        waterSurfaceY = surfaceY; // 외부에서 수면 기준값 갱신
    }

    // 부력 강도 설정 메서드 (선택적)
    public static void SetBuoyancyStrength(float strength)
    {
        buoyancyStrength = strength; // 부력 강도 조정
    }

    // 현재 위치 기준 부력 값 계산
    public static float CalculateBuoyancy(Vector3 position)
    {
        float depth = waterSurfaceY - position.y; // 수면보다 얼마나 아래 있는지 계산
        return depth > 0f ? buoyancyStrength * depth : 0f; // 수면 아래일 때만 부력 작용
    }

    // 현재 수면보다 아래에 있는지 여부 반환
    public static bool IsUnderwater(Vector3 position)
    {
        return position.y < waterSurfaceY; // y 위치가 수면보다 아래인지 확인
    }

    // 수면 높이 반환
    public static float GetSurfaceY()
    {
        return waterSurfaceY;
    }
}
