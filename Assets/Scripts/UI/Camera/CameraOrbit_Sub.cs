using UnityEngine;

// 3인칭 시점용 카메라 오비트 클래스
public class CameraOrbit_Sub : BaseCameraOrbit
{
    // 오프셋 값과 회전 초기화
    protected override void Start()
    {
        base.Start();                                // 공통 커서 처리 등 실행
        offset = new Vector3(0f, -4.4f, -2.65f);     // 3인칭 기준 오프셋 설정
        pitch = 20f;                                 // 3인칭 기본 pitch
        yaw = 0f;                                     // yaw 초기화

        //pitch 범위 확장 (위 아래로 충분히 회전 가능하게)
        minY = -60f;  // 아래로 더 많이 회전 가능
        maxY = 90f;   // 위로 거의 정면~하늘까지 회전 가능
    }
}
