using UnityEngine;
using Cinemachine;

public class AutoOrbitCamera : MonoBehaviour
{
    [Header("기본 참조")]
    [SerializeField] private CinemachineFreeLook freeLookCam; // 회전용 FreeLook 카메라

    [Header("X축 회전 설정")]
    [SerializeField] private float rotationSpeed = 10f; // 자동 회전 속도

    [Header("Y축 흔들림 설정")]
    [SerializeField] private float yAmplitude = 0.3f; // 흔들림 진폭 (얼마나 위아래로)
    [SerializeField] private float yFrequency = 0.5f; // 흔들림 주기 (속도)

    [Header("Noise 설정 (Rig별)")]
    [SerializeField] private float noiseAmplitude = 0.5f; // 흔들림 강도
    [SerializeField] private float noiseFrequency = 0.8f; // 흔들림 속도

    private float baseY; // Y축 기본값 (흔들림 기준)
    private CinemachineBasicMultiChannelPerlin[] noiseModules; // 리그별 노이즈 모듈 캐싱

    private void Start()
    {
        baseY = freeLookCam.m_YAxis.Value;

        // 입력 이름 제거 → 자동 회전만 하도록 만듦
        freeLookCam.m_XAxis.m_InputAxisName = "";
        freeLookCam.m_YAxis.m_InputAxisName = "";

        noiseModules = new CinemachineBasicMultiChannelPerlin[3];
        noiseModules[0] = freeLookCam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noiseModules[1] = freeLookCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noiseModules[2] = freeLookCam.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        foreach (var noise in noiseModules)
        {
            if (noise != null)
            {
                noise.m_AmplitudeGain = noiseAmplitude;
                noise.m_FrequencyGain = noiseFrequency;
            }
        }
    }


    private void Update()
    {
        // 1. X축 회전 자동 증가
        freeLookCam.m_XAxis.Value += rotationSpeed * Time.deltaTime;

        // 2. Y축 사인파 흔들림
        float yOffset = Mathf.Sin(Time.time * yFrequency) * yAmplitude;
        freeLookCam.m_YAxis.Value = Mathf.Clamp01(baseY + yOffset);
    }
}
