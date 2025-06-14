using System;
using System.Collections.Generic;

/// 정적 클래스 > 인스턴스를 만들지 않고 전역에서 접근 가능
/// 모든 이벤트는 구독(Subscribe), 발행(Publish), 해지(Unsubscribe) 중심
public static class EventBus
{
    // 타입 기반 이벤트 저장
    // 예시 : EventBus.Subscribe<int>(callback) > typeof(int)키로 저장
    private static Dictionary<Type, Delegate> typedEventTable = new();

    // 문자열 기반 이벤트 저장
    // 예시 : "OnItemCollected" 이벤트에 대해 콜백 연결
    public static Dictionary<string, Action<object>> stringEventTable = new();

    // void 이벤트 저장
    // 예시 : "OnGameStart" > Action만 저장 (파라미터 있음)
    private static Dictionary<string, Action> voidEventTable = new();

    // 타입 + 문자열 기반 이벤트 저장
    // 예시 : ("MyEvent", typeof(int)) 같은 이벤트 사용가능 (복잡한 경우 대응하기 위해)
    private static Dictionary<(Type, string), Delegate> typedNamedEventTable = new();

    // 타입 기반 구독
    /// T타입의 이벤트를 수신하기 위해 등록
    /// 델리게이트 체인으로 여러 개 등록 가능 (Delegate.Combine)
    public static void Subscribe<T>(Action<T> callback)
    {
        if (typedEventTable.TryGetValue(typeof(T), out var del))
            typedEventTable[typeof(T)] = Delegate.Combine(del, callback);
        else
            typedEventTable[typeof(T)] = callback;
    }

    // 타입 기반 구독 해제
    /// T 타입 이벤트 수신 해지
    /// 델리게이트에서 제거 후 남은 게 없다면 테이블에서도 삭제
    public static void Unsubscribe<T>(Action<T> callback)
    {
        if (typedEventTable.TryGetValue(typeof(T), out var del))
        {
            var currentDel = Delegate.Remove(del, callback);
            if (currentDel == null)
                typedEventTable.Remove(typeof(T));
            else
                typedEventTable[typeof(T)] = currentDel;
        }
    }

    // 타입 기반 발행
    /// T타입 이벤트를 발생시킴
    /// 등록된 Action<T> 델리게이트 모두 실행
    public static void Publish<T>(T evt)
    {
        if (typedEventTable.TryGetValue(typeof(T), out var del))
        {
            var callback = del as Action<T>;
            callback?.Invoke(evt);
        }
    }

    // 문자열 기반 구독
    /// "이벤트이름"에 대해 객체형 파라미터를 받아 처리하는 이벤트 등록
    public static void Subscribe(string eventName, Action<object> callback)
    {
        if (stringEventTable.TryGetValue(eventName, out var existing))
            stringEventTable[eventName] = existing + callback;
        else
            stringEventTable[eventName] = callback;
    }

    // 문자열 기반 구독 해제
    /// 문자열 이벤트 해지
    public static void Unsubscribe(string eventName, Action<object> callback)
    {
        if (stringEventTable.TryGetValue(eventName, out var existing))
        {
            existing -= callback;
            if (existing == null)
                stringEventTable.Remove(eventName);
            else
                stringEventTable[eventName] = existing;
        }
    }

    // 문자열 기반 발행
    /// 문자열 이벤트 발생
    /// 파라미터는 object 타입 (예 : EventBus.Publish("OnQuestComplete", questData) )
    public static void Publish(string eventName, object param = null)
    {
        if (stringEventTable.TryGetValue(eventName, out var callback))
        {
            callback?.Invoke(param);
        }
    }

    /// "이벤트이름" 에 대해 아무 파라미터 없이 실행되는 콜백 등록
    public static void SubscribeVoid(string eventName, Action callback)
    {
        if (voidEventTable.TryGetValue(eventName, out var existing))
            voidEventTable[eventName] = existing + callback;
        else
            voidEventTable[eventName] = callback;
    }

    /// void 이벤트 해지
    public static void UnsubscribeVoid(string eventName, Action callback)
    {
        if (voidEventTable.TryGetValue(eventName, out var existing))
        {
            existing -= callback;
            if (existing == null)
                voidEventTable.Remove(eventName);
            else
                voidEventTable[eventName] = existing;
        }
    }

    /// void 이벤트 실행
    /// 예시 : "OnPlayerDie" 처럼 파라미터 필요 없는 이벤트에 적합
    public static void PublishVoid(string eventName)
    {
        if (voidEventTable.TryGetValue(eventName, out var callback))
        {
            callback?.Invoke();
        }
    }

    /// (typeof(T), eventName) 쌍을 키로 해서 복잡한 이벤트 등록 가능
    public static void Subscribe<T>(string eventName, Action<T> callback)
    {
        var key = (typeof(T), eventName);
        if (typedNamedEventTable.TryGetValue(key, out var del))
            typedNamedEventTable[key] = Delegate.Combine(del, callback);
        else
            typedNamedEventTable[key] = callback;
    }

    /// 복합 키 기반 해지
    public static void Unsubscribe<T>(string eventName, Action<T> callback)
    {
        var key = (typeof(T), eventName);
        if (typedNamedEventTable.TryGetValue(key, out var del))
        {
            var currentDel = Delegate.Remove(del, callback);
            if (currentDel == null)
                typedNamedEventTable.Remove(key);
            else
                typedNamedEventTable[key] = currentDel;
        }
    }

    /// 복합 키 이벤트 발행
    public static void Publish<T>(string eventName, T evt)
    {
        var key = (typeof(T), eventName);
        if (typedNamedEventTable.TryGetValue(key, out var del))
        {
            var callback = del as Action<T>;
            callback?.Invoke(evt);
        }
    }

    /// 특정 타입과 이름으로 등록된 이벤트 초기화
    public static void Clear(Type type, string eventName)
    {
        var key = (type, eventName);
        if (typedNamedEventTable.ContainsKey(key))
        {
            typedNamedEventTable.Remove(key);
        }
    }

    /// 특정 타입과 이름으로 등록된 이벤트 초기화
    public static void Clear<T>(string eventName)
    {
        Clear(typeof(T), eventName);
    }

    //모든 구독 내역 초기화
    /// 모든 구독자 초기화 (주로 씬 전환 시 사용)
    public static void Clear()
    {
        typedEventTable = new();
        stringEventTable = new();
        voidEventTable = new();
        typedNamedEventTable = new();
    }

    /// 특정 이벤트 이름 기반의 문자열/void 이벤트 초기화
    public static void ClearEvent(string eventName)
    {
        stringEventTable.Remove(eventName);
        voidEventTable.Remove(eventName);
    }
}
