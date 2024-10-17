# VR 컨텐츠 구현

## 중력장갑
- 알릭스의 중력 장갑의 상호작용을 구현
- **물체를 원거리에서 선택**
  - 물체는  XR Grab Interactable, RigidBody 컴포넌트를 가짐
  - XR Ray Interactor를 사용해 Raycast로 물체를 선택
    - 당길 수 있는 물체가 Raycast로 선택되면 짧은 오디오를 재생 (or 물체를 강조표시)
- **물체를 당겨옴**
  - 물체를 원거리에서 선택해서 당겨옴
  - 유저의 현재 위치와 물건의 위치를 비교해서 정규화된 방향 도출
  - 일정 힘 이상으로 당기면 물체가 플레이어 쪽으로 당겨져 눈앞까지 옴
  - 날아오는 도중 충돌체(trigger화?) 의 크기를 늘려 좀 더 쉽게 잡을 수 있게 함 
- **인벤토리**
  - 손목 부분에 공간을 두어 인벤토리를 구현
  - 

- 피칭 컨텐츠
- 정확히 던지면 점수?
- 특정 손가락으로 잡으면 커브, 슬라이더 등이 나가게? // 제외


## Asset
- 
- VR 손 모델 에셋 : [VR Hand Models Mega Pack](https://assetstore.unity.com/packages/3d/characters/humanoids/vr-hand-models-mega-pack-handy-hands-left-right-200607)
- 공 모델 에셋 : [Free Sport Balls]( https://assetstore.unity.com/packages/3d/props/free-sport-balls-293937)