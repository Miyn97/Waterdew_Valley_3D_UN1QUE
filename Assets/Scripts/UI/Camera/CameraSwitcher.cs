using UnityEngine;

// V 키를 눌러 1인칭(MainCamera) ↔ 3인칭(SubCamera) 전환하는 스크립트
public class CameraSwitcher : MonoBehaviour
{
    [Header("카메라 설정")]
    [SerializeField] private Camera mainCamera;   // 1인칭 시점 카메라 (Main)
    [SerializeField] private Camera subCamera;    // 3인칭 시점 카메라 (Sub)

    private AudioListener mainListener;           // 1인칭 카메라 오디오 리스너
    private AudioListener subListener;            // 3인칭 카메라 오디오 리스너
    private bool isMainCameraActive = true;       // 현재 1인칭 카메라가 활성 상태인지 여부

    private void Awake()
    {
        mainListener = mainCamera.GetComponent<AudioListener>(); // 1인칭 오디오 리스너 캐싱
        subListener = subCamera.GetComponent<AudioListener>();   // 3인칭 오디오 리스너 캐싱
    }

    private void Start()
    {
        mainCamera.enabled = true;   // 시작 시 1인칭 카메라 활성화
        subCamera.enabled = false;   // 3인칭 카메라는 비활성화

        if (mainListener != null) mainListener.enabled = true;   // 1인칭 오디오 리스너 활성화
        if (subListener != null) subListener.enabled = false;    // 3인칭 오디오 리스너 비활성화
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Pause)) // Pause 키 입력 시 카메라 전환
        {
            isMainCameraActive = !isMainCameraActive;

            mainCamera.enabled = isMainCameraActive;
            subCamera.enabled = !isMainCameraActive;

            if (mainListener != null) mainListener.enabled = isMainCameraActive;
            if (subListener != null) subListener.enabled = !isMainCameraActive;

            //카메라 전환 후 커서 상태 갱신
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
