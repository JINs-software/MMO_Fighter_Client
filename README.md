### \[개요\]

유니티를 통한 클라이언트 개발을 통해 클라이언트 개발 분야에 대한 학습과
유니티 개발 실습을 수행.
기존 RPC 개념을 적용할 수 없었던 프로카데미 제공 클라이언트의 모작을 만들어 RPC 개념이 적용된 게임 서버와 클라이언트를 구현.

### \[RPC, Proxy, Stub 클래스\]

-   RPC 클래스: 전역 객체에 붙여지는 컴포넌트 클래스이며, 서버로의
    메시지 송신을 함수 호출로 추상화하는 Proxy객체를 public 정적 멤버로
    제공. 추상 스텁 클래스를 상속받아 컨텐츠 코드가 호출되도록 가상
    함수를 재정의한 스텁 구체 클래스의 객체가 부착되도록 AttachStub
    함수를 제공하며, 메시지 수신 시 메시지 종류에 따라 부착된 스텁의
    멤버 함수를 호출함.

-   Proxy 클래스: json 명세 또는 JPD 편집 자동화 툴에서 정의한
    Client-to-Server 메시지를 송신하는 기능을 함수로 구현. 컨텐츠
    코드에선 이 프록시를 통해 함수를 호출함으로써 메시지를 송신(추상화).

-   Stub 및 namespace 별 추상 Stub 클래스: 개발자는 메시지 종류를 나누는
    메시지 네임 스페이스 별로 정의된 추상 Stub 클래스 정의를 코드 자동화
    생성으로 제공 받음. 추상 함수를 재정의하는 방식으로 메시지 수신 시
    컨텐츠 코드가 호출되도록 하는 것이며, 구체 스텁 클래스는 필드 내
    적절한 게임 오브젝트에 컴포넌트로 부착. 컴포넌트 부착 시 RPC 코드를
    통해 자동으로 전역의 싱글톤 RPC 객체의 AttachStub이 호출되어 메시지
    수신 및 컨텐츠 코드 호출의 준비를 마침.

### \[씬 흐름\]

1. MMO_FIGTER의 게임과 서버-클라이언트 간 메시지 송수신 내용은 간단함. 클라이언트가 게임 서버에 연결 요청(connect) 하는 것으로 게임은 시작.
  'BattleField'라는게임 오브젝트에 RPC 관련 초기화 작업을 수행하는 컴포넌트를 부착한다.
  이 컴포넌트의 초기화 함수를 통해 RPC의 Initiate 함수를 통해 서버와의 연결 수립.

  ![1](https://github.com/user-attachments/assets/4c5ae66c-0435-427b-b5ae-0a107c24a9de)
  

2.  미리 제공된 Stub 추상 클래스를 상속받아 게임 로직에 필요한 컨텐츠
    코드를 삽입하여 오버라이딩 준비.

  ![2](https://github.com/user-attachments/assets/3856588a-8ed2-4cc6-a75b-da0588f1a6a4)


3.  구체 스텁 클래스들을 컴포넌트로 부착, RPC 코드를 통해 자동으로 전역
    싱글톤 객체인 RPC 오브젝트에 부착됨(AttachStub)

  ![3](https://github.com/user-attachments/assets/63608139-fb61-412e-8788-ef880e88bf53)

4.  컨텐츠 코드로 정의한 스텁 함수는 RPC 코드에 의해 자동으로 호출됨.
    MMO\_Fighter 게임에서 게임 서버에 연결 시 서버는 임의의 좌표와 함께
    플레이어의 Fighter 캐릭터를 생성 메시지로 전송함. 이 메시지 타임에
    따라 맵핑된 스텁 함수가 호출됨으로써 캐릭터 생성 로직이 수행됨.

  ![4](https://github.com/user-attachments/assets/be9ad13a-a16b-4613-8b65-ae76f669c422)

  
5.  플레이어의 키 입력을 통해 플레이어의 Fighter 캐릭터의 이동과 공격 시
    RPC의 프록시를 통해 메시지를 전송함

  ![5](https://github.com/user-attachments/assets/ef999ad7-be79-4f34-b79a-fbc294251a86)


6.  다른 플레이어들의 Fighter 상태와 행위에 대한 메시지들 또한 스텁
    함수로 처리하여 클라이언트에 반영한다.

  ![6](https://github.com/user-attachments/assets/36e52413-9e7c-4b94-9367-cd2a2dd7c542)


### \[시연 영상\]

1. 상/하/좌/우 방향키 입력 시 FighterController에서는 프록시 함수를 호출, 서버는 이동 패킷을 받고 주변의 다른 Fighter들의 클라이언트에 MOVE_START 패킷 전송. 속도를 기반으로 프레임마다 위치를 동기화함.
   ![move](https://github.com/user-attachments/assets/a12bf3ac-bde8-4457-bab3-ec247c90d305)

2. Z/X/C 키 입력을 통해 공격 수행, 마찬가지로 FighterController에서 프록시 함수 호출을 통해 공격 패킷 전송, 서버는 공격 패킷을 받은 뒤 주변의 다른 Fighter들의 클라이언트에 ATTACK1/2/3 패킷 전송. 또한 공격 범위의 Fighter들을 섹터 기반의 자료구조를 통해 획득하여 데미지 반영 및 데미지 패킷 브로드캐스팅.
   ![attack](https://github.com/user-attachments/assets/9b69dfb1-62a9-49e2-97da-02b16e6a6792)

3. 자체 유니티 클라이언트 프로그램 및 더미 클라이언트 프로그램(프로카데미 제공) 실행. 서버는 하나의 클라이언트에 필드 상 모든 Fighter 캐릭터들의 정보와 공격/이동 등의 정보를 전달할 필요가 없기에 단일 클라이언트의 관심 영역의 섹터들의 Fighter 캐릭터들의 정보만을 전송함. 따라서 클라이언트의 이동 시 새로운 캐릭터들의 생성과 삭제가 반복됨.
    ![dummy](https://github.com/user-attachments/assets/b018e481-a370-48ba-8767-94ec1e8b5f14)

