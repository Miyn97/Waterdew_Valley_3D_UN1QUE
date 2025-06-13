using UnityEngine;

// 바다 물결 효과를 주기 위한 스크립트 (Mesh의 정점을 흔들어 시각 효과 부여)
[RequireComponent(typeof(MeshFilter))]
public class WaveMotion : MonoBehaviour
{
    private Mesh mesh; // 편집할 대상 메시 캐싱
    private Vector3[] originalVertices; // 초기 정점 배열 (수정 불가)
    private Vector3[] modifiedVertices; // 수정될 정점 배열

    [SerializeField] private float waveHeight = 0.5f; // 파도 높이
    [SerializeField] private float waveFrequency = 36f; // 파도 간격
    [SerializeField] private float waveSpeed = 1f; // 파도 속도

    private void Awake()
    {
        // 메시 가져오기 및 정점 배열 캐싱
        mesh = GetComponent<MeshFilter>().mesh; // 메쉬 필터에서 메시 추출
        originalVertices = mesh.vertices; // 원본 정점 정보 저장
        modifiedVertices = new Vector3[originalVertices.Length]; // 수정용 정점 배열 생성
        System.Array.Copy(originalVertices, modifiedVertices, originalVertices.Length); // GC 없는 복사
    }

    private void Update()
    {
        float time = Time.time * waveSpeed; // 시간 기반 파도 계산용 변수

        for (int i = 0; i < originalVertices.Length; i++) // 모든 정점 순회
        {
            Vector3 vertex = originalVertices[i]; // 원본 정점 기준
            vertex.y = Mathf.Sin(vertex.x * waveFrequency + vertex.z * waveFrequency + time) * waveHeight; // 파도 높이 적용
            modifiedVertices[i] = vertex; // 수정된 값 저장
        }

        mesh.vertices = modifiedVertices; // 메시의 정점 배열 업데이트
        mesh.RecalculateNormals(); // 노멀 다시 계산해 광원 반영
    }
}
