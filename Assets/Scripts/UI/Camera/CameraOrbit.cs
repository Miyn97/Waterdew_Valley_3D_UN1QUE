using UnityEngine;

// 1인칭 시점용 카메라 오비트 클래스
public class CameraOrbit : BaseCameraOrbit
{
    // 기본 회전 각도와 오프셋 설정 초기화
    protected override void Start()
    {
        base.Start();                    // 공통 커서 처리 등 실행
        offset = Vector3.zero;          // 1인칭이므로 타겟과 동일 위치
        pitch = 0f;                     // pitch 초기화 (정면)
        yaw = 0f;                       // yaw 초기화
    }
}
