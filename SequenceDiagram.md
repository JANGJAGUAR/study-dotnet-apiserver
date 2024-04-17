sequenceDiagram
    autonumber
    actor A as client
    participant B as app
    participant C as server
    participant D as google
    participant E as database
    participant F as cloud server
    A->>B: 파일 업로드 버튼 클릭
    B->>A: 구글 로그인 popup 제공
    A->>D: 구글 로그인
    D->>B: ID 토큰 응답
    B->>C: ID 토큰과 업로드 파일 전달
    C->>D: ID 토큰 검증 요청
    D->>C: ID 토큰 검증 결과 응답
    alt is 검증 확인
        C->>E: 정보 저장
        E->>C: 정상 insert 확인 응답
        C--)F: 파일 업로드
        C->>B: 업로드 성공 응답
        activate B
        B->>B: 성공 UI 랜더링
        deactivate B
        B->>A: 성공 확인
    else is 검증 실패
