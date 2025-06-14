using UnityEngine;

// 수면 기준과 부력 계산을 전담하는 유틸리티 클래스
public static class WaterSystem
{
    private static float waterSurfaceY = 0.5f; // 수면 y 기준값 (게임 시작 시 설정되거나 고정값으로 사용)
    private static float buoyancyStrength = 3f; // 부력 강도 (1m 잠기면 3의 힘을 위로 가함)

    // 외부에서 수면 높이를 동적으로 설정할 수 있도록 허용
    public static void SetWaterSurfaceY(float surfaceY)
    {
        waterSurfaceY = surfaceY; // 수면 기준값을 갱신
    }

    // 외부에서 부력 강도를 조절할 수 있도록 허용 (게임 난이도, 아이템 효과 등)
    public static void SetBuoyancyStrength(float strength)
    {
        buoyancyStrength = strength; // 부력 강도 설정
    }

    // 입력된 위치가 수면보다 얼마나 아래에 있는지를 계산하여 부력 값을 반환
    public static float CalculateBuoyancy(Vector3 position)
    {
        float depth = waterSurfaceY - position.y; // 수면보다 얼마나 깊이 잠겼는지 계산
        return depth > 0f ? buoyancyStrength * depth : 0f; // 잠긴 경우에만 부력을 적용 (음수 X)
    }

    // 수면보다 약간이라도 아래에 있는 경우를 수영 상태로 간주
    public static bool IsUnderwater(Vector3 position)
    {
        return position.y < waterSurfaceY - 0.2f; // 수면보다 0.2 이상 잠겨야 수영 상태로 간주
    }


    // 현재 설정된 수면 기준 y값을 외부에서 가져올 수 있도록 반환
    public static float GetSurfaceY()
    {
        return waterSurfaceY; // 현재 수면 높이 반환
    }

    // 수면보다 일정 이상 위에 있는 경우를 수면 위로 올라온 상태로 간주
    public static bool IsAboveWater(Vector3 position)
    {
        return position.y > waterSurfaceY + 0.2f; // 0.2f 이상 올라가야 수면 탈출로 판정
    }
}
