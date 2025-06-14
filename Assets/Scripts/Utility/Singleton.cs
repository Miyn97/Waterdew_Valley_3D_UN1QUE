using UnityEngine;

/// 이 클래스는 제네릭 타입 T를 사용, 제한을 두고 T는 반드시 MonoBehaviour여야만 함
/// 구조체(abstract)라서 직접 인스턴스화 할 수 없음 > 반드시 상속해서 사용해야 함
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    ///실제 싱글톤 인스턴스를 저장할 정적 필드
    private static T _instance;

    /// 외부에서 이 싱글톤 객체에 접근할 수 있도록 하는 정적 프로퍼티
    public static T Instance
    {
        get ///아직 인스턴스가 생성되지 않았다면 (최초 접근 시)
        {
            if (_instance == null)
            {
                ///씬에 이미 존재하는 T 타입의 오브젝트를 찾아서 할당 (예: 수동 배치한 SoundManager 오브젝트)
                _instance = (T)FindObjectOfType(typeof(T));

                ///그래도 없으면 > 직접 생성해서 인스턴스화
                if (_instance == null)
                {
                    ///빈 GameObject를 생성
                    GameObject singletonObject = new GameObject();

                    ///해당 GameObject에 T타입 컴포넌트를 추가 (즉, 싱글톤으로 지정된 컴포넌트 자동 생성)
                    _instance = singletonObject.AddComponent<T>();

                    ///GameObject의 이름을 "클래스명 (Singleton)" 형식으로 지정 > 디버깅 편의를 위해서
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";

                    ///씬 전환 시에도 파괴되지 않도록 설정 > 싱글톤 유지
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance; ///생성이 완료되었든, 이미 존재했든 간에 최종적으로 인스턴스를 반환
        }
    }
    protected virtual void Awake() ///상속받는 쪽에서 재정의(override) 가능하게 해줌
    {
        ///최초 초기화 시
        if (_instance == null)
        {
            ///현재 오브젝트(this)를 싱글톤 인스턴스로 지정
            _instance = this as T;

            ///이 오브젝트도 씬 전환 시 삭제되지 않도록 유지
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this) ///이미 다른 인스턴스가 존재한다면
        {
            ///현재 중복된 오브젝트는 제거 > 싱글톤 보장
            Destroy(gameObject);
        }
    }
}
