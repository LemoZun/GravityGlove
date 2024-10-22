# VR 컨텐츠 구현

## 중력장갑
- 알릭스의 중력 장갑의 상호작용을 구현

### 동작 개요
1. 그랩 가능한 물체를 원거리에서 Ray 가르킴
2. Ray로 가르킨 물체를 Grab 버튼을 눌러 Select
3. Grab 버튼을 누른 Select를 유지한 상태로 손목을 위로 튕겨 당겨오듯이 손을 움직임
4. 물체가 끌려오는 도중 Grab 버튼 누르기를 해제 시 물체는 끌려오는 것을 중단함
5. 물체가 플레이어의 앞까지 끌려왔을 때 Grab 버튼을 떼고 다시 Grab버튼을 누르면 물체를 Direct로 Grab한다

### 추가 동작
- 손전등 기능
  - Right Controller의 Primary Button (A버튼) 으로 손전등을 켜고 끌 수 있음
- 텔레포트
  - 기존 ToolKit에 포함된 텔레포트 기능을 사용

### 동작 설명

- **물체를 원거리에서 선택**
  - 물체는  XR Grab Interactable, RigidBody 컴포넌트를 가짐
  - XR Ray Interactor를 사용해 Raycast로 물체를 선택
    - 당길 수 있는 물체가 Raycast로 선택되면 짧은 오디오를 재생 (or 물체를 강조표시)

- **물체를 당겨옴**
  - 물체를 원거리에서 선택해서 당겨옴
  - 유저의 현재 위치와 물건의 위치를 비교해서 정규화된 방향 도출
  - 일정 각 속도 이상으로 당기면 물체가 플레이어 쪽으로 당겨져 눈앞까지 옴

- **선택시작**
  - 현재 로컬 z 앵글 저장,isSelecting = True
  - 손 튕김 감지 코루틴 시작

- **코루틴 루프 시작**
  - isSelecting이 True고 isPulled가 false일때 (물체를 선택 했고 아직 안당겼을때)루프 시작
  - 현재 로컬 z앵글을 계산, 기존 앵글과 차이 비교 한 델타 앵글 생성
  - 델타 앵글을 통한 각속도 계산
  - 각속도가 일정속도를 넘기면 pullObject 수행
    - 루프 break
    - 코루틴 종료
  - 각속도가 기준을 넘기지 못했다면 현재 앵글(curZAngle)을 originalAngle에 대입, delay 후 다시 루프 시작

- **PullObject 시작**
  - 물체가 힘을 받을 방향 계산
    - (플레이어의 위치 - 물체의 위치) 정규화
  - 포물선으로 날아오기 위해 y축 방향 추가
  - Lerp하게 물체가 날아와야함
  - 선택된 물체에 pullForce만큼의 힘을 앞서 계산한 방향대로 줌
  - 물체가 당겨졌으니 isPulled = true

### 미구현
- **인벤토리**
  - 손목 부분에 공간을 두어 인벤토리를 구현
- **중력장갑 획득**
  - 기존 컨트롤러에서 중력장갑을 Direct로 Grab 시 중력장갑을 획득해 컨트롤러의 모델을 중력장갑으로 대체



## Asset
- VR 손 모델 에셋 : [VR Hand Models Mega Pack](https://assetstore.unity.com/packages/3d/characters/humanoids/vr-hand-models-mega-pack-handy-hands-left-right-200607)
- 공 모델 에셋 : [Free Sport Balls]( https://assetstore.unity.com/packages/3d/props/free-sport-balls-293937)

<!--
## memo
애니메이트 모델로 넣어줌
ray로 잡았을때는 안움직이고 Direct Interator로 잡았을 때에만 움직이도록 해야함

focus - 조준당했을때
hover - 상호작용 범위에 들어왔을때
select - grab 같은 컨트롤러로 상호작용을 시작할 때 
Track Position과 Track Rotation, Throw on Detach를 꺼주면 grab 했을때 움직이지 않도록 해 줄 수 있음

물체를 당기는 트리거
ray 로 선택
throw 참조 어려움
grab을 한 ray 위치와 그랩
거 속 시
그랩버튼을 누르고 있어도 당기는 동작은 시행됨
중력장갑이 다른 객체로 있다가 direct로 집으면 컨트롤러의 모델을 장갑으로 바꾼다?
본인 좌표 기준으로 z축 각속도를 계산하면 됨
-->

