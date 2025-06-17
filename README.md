# 🌊 Waterdew Valley

**Waterdew Valley**는 Unity 3D URP 기반의 생존 시뮬레이션 게임입니다.  
플레이어는 바다 위에서 건축, 낚시, 채집, 조합 등 다양한 생존 활동을 하며 세계를 탐험하게 됩니다.

![image](https://github.com/user-attachments/assets/6ebc2a27-7dfd-4e11-994d-c17ac7d3f068)

---

## 🧩 주요 기능

### 🎮 플레이어 FSM 시스템
- Idle, Move, Run, Jump, Swim, ThrowHook, Fish 등 상태 기반 FSM 구성
- Animator 파라미터 연동 / 상태 간 전이 최적화
- SOLID 원칙 + GC 최소화 기반 구조 설계

### 🧠 설계 패턴
- Singleton Pattern  
- EventBus Pattern  
- FSM State Pattern  

### 🌊 수영 및 부력 시스템
- WaterSystem 기반 수면 Y 좌표 연동
- 부력 계산, 수직 이동, 수면 위/아래 효과 자동 처리

### 🎣 낚시 시스템
- FSM: ThrowHook → Fish 상태 구성
- 낚싯대 프리팹 손 위치 부착
- 애니메이션/연출/기능 전환 포함

### 🏗️ 건축 / 제작 시스템
- BuildManager, PlacementPreview 활용
- RecipeData 기반 조합 시스템

### 🧰 인벤토리 및 아이템
- 드래그 인터페이스, 툴팁, 사용/장착/소모 아이템 처리

---

## 🗂 프로젝트 폴더 구조 (요약)

\`\`\`
📁 Scripts
├── Character        // Player FSM 및 애니메이션
├── Manager          // 싱글톤 매니저
├── Item             // 아이템 ScriptableObject
├── Inventory        // 인벤토리 시스템
├── Building         // 건축 및 배치 시스템
├── Crafting         // 조합 / 낚시 / 요리
├── Object           // 상호작용 가능한 오브젝트
├── Environment      // 날씨, 시간, 레벨
├── UI               // HUD, Dialog, 퀘스트 UI
├── FSM              // IState, StateMachine, BaseState
\`\`\`

---

## 🧪 개발 환경

- Unity 2022.3.17f1 (URP)
- Git / GitHub
- C# (.NET 4.x)

---

## 👥 팀 구성

| 이름     | 역할                        |
|----------|-----------------------------|
| 김수민   | 기획, FSM, 연출             |
| 유도균   | 아이템, Enemy, 인벤토리     |
| 박준식   | 건축, 제작, 상호작용        |
| 이종민   | UI, 저장, 시스템            |
| 황연주   | 날씨, 퀘스트, 시각 효과 연출 |

---

## ✅ 필수 구현 기능 :first_place:

### :pushpin: 1. 주인공 캐릭터 이동 및 기본동작
- 키보드 또는 터치 입력 ✅
- 이동 및 점프 구현 ✅
- 애니메이션: 시점변환(Pause Key 활용) 방식으로 변경 ✅

### :pushpin: 2. 레벨 디자인 및 적절한 오브젝트 배치
- 2개 이상 레벨 디자인 (상어 + 새 + 난이도 차이) ✅
- 플랫폼, 장애물, 보상 아이템: 바다 플랫폼 활용 ✅

### :pushpin: 3. 충돌 처리 및 피해량 계산
- 환경 충돌 (벽, 나무 등) ✅
- 적과의 충돌 (상어 등) ✅
- 피해량 계산 오브젝트 기반 처리 ✅

### :pushpin: 4. UI / UX 요소
- 게임 시작/일시정지 메뉴 (TimeScale 활용) ✅
- 점수/체력 게이지 ✅
- 레벨 진행 UI 포함 ✅

### :pushpin: 5. 인트로 씬 구현
- StartScene = IntroScene ✅
- 배경 연출 + 카메라 상황 설명 포함 ✅
- Timeline, Cinemachine 사용 ✅

---

## 🏁 도전 기능 :second_place:

- 다양한 무기/아이템 (갈고리, 망치, 창 등) ✅  
- 섬 2개 + 바다 환경 구현 ✅  
- 사운드 / 음악 효과 ✅  
- AI 적 캐릭터 FSM (Enemy) ✅  
- 크래프팅 시스템 구현 ✅  
- 게임 패턴 분석 및 난이도 밸런싱 ✅  
- 생존거점(Raft) 강화 기능 ✅  
- 건축 시스템 ✅  
- 날씨와 환경 효과 ✅  

---

## 🚀 실행 방법

1. Unity 2022.3.17f1 설치  
2. 해당 리포지토리 클론  
3. `StartScene` 열기  
4. ▶️ 실행  

