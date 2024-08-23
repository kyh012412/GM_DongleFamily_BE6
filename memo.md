[전체링크](https://www.youtube.com/playlist?list=PLO-mt5Iu5TeYI4dbYwWP8JqZMC9iuUIW2)
[be6 링크](https://www.youtube.com/watch?v=eQPp0QTz4JM&list=PLO-mt5Iu5TeajtA5UQT7_2UjB7_dkGagU)
[에셋 다운 링크](https://www.youtube.com/redirect?event=video_description&redir_token=QUFFLUhqa2FIY2toNm1VdG1tVlpmM0ZpVjkxWEJvMGFnUXxBQ3Jtc0tsbVNrY2dmTF9fLTRuZzA5REQzLVVCQmpHMnBOb3ZNWms5bXFMZzUwc3NmQXc3WW8tYkxoNUFlbmEtbDgzMkxqRFI0UW5PLXlQM1hJSUdWbFQ1YkViLUhaR3g0UlA3d2NtbDZtZmF2ZnZRTXNaYkdfNA&q=https%3A%2F%2Fwww.goldmetal.co.kr%2Funity%2Fpackages%2FDongleFamily_Assets_Pack.unitypackage&v=eQPp0QTz4JM)

### 물리 퍼즐게임 - 🧱물리기반 퍼즐 밑바탕 만들기 [B54 + Asset]

#### _모바일 환경 설정_

1. Edit > Preference > External Tools 들어가기
   1. Android 항목에 4개가 잘 들어잇는지 확인
   2. 노란색 느낌표가 없어야함
2. 플랫폼을 안드로이드로 변경

#### 에셋 불러오기

#### 배경 만들기

1. Camera 세팅
   1. camera background color 흰색
   2. Projection Orthographic
   3. size 9
2. 받은에셋 > sprites > Background를 하이라키에 드래그 드랍
3. game tab 화면비율 9:19
4. BackGround order -1
5. 하이라키에 2d Object Square를 만들어준다.(Bottom)
   1. Scale 10 5 1
   2. pos y -11
   3. color 312524
   4. rigidbody 2d
      1. body type static
   5. box collider 2d
6. Bottom 복사 (Wall)
   1. scale 1 20 1
   2. pos -4.7 0 0
7. Wall 복사
   1. 대칭이 되게 배치
8. 빈객체생성 PlayGorund
   1. 위치 0 0 0
   2. 배경객체 4개를 넣어주기
9. 하이라키에 2d object > circle을 넣어주기(Dongle)
   1. 위치 초기화
   2. rigidbody 2d
      1. body type dynamic
   3. circle collider 2d
10. 테스트 용으로 circle을 pos y 8두고 실행해본다.

#### 기본 동글 로직

1. Dongle.cs
2. sprite renderer 의 sprite를 dongle 0으로바꿔주고
3. circle collider 2d 의반지름을 0.6으로 올려준다.
4. rigidbody 2d에 simulated를 꺼준다.
5. 테스트
   1. dongle의 위치가 마우스 좌표처럼 잘 따라가는데
   2. 카메라의 z위치가 -10이라서 dongle의 좌표도 -10되어서 game tab내에서 확인이 불가능함
6. PlayGround 내로 sprites 내에 있는 line을 넣어준다.
   1. pos y 6
   2. color ff902a
   3. 알파값 100
   4. box collider 2d
      1. is trigger 체크
7. _반지름의 크기를 구할수 있는 방법_ : `transform.localScale.x /2f;`

#### 끌어다가 놓기

1. isDrag와 Drag, Drop 메서드 구현
2. Canvas > Image 생성(Touch Pad)
   1. 앵커 전체
   2. 알파 0
   3. event trigger 컴포넌트 추가
      1. pointer down, pointer up에 메서드 연결
3. Dongle에 태그 Dongle추가
4. Assets/Prefabs 폴더를 만들어 준다.
5. Prefab화
   Dongle.cs

```cs
public class Dongle : MonoBehaviour
{
	public bool isDrag; //기본값 false drag를 통해 true를 거쳐서 drop 후 다시 false

	Rigidbody2D rigid;

	void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if(isDrag){
			// Vector3 mousePos = Input.mousePosition; // 스크린 좌표계
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			// x축 경계 설정
			float border = 4.2f - transform.localScale.x /2f;
			mousePos.x = Mathf.Clamp(mousePos.x,-border,border);

			mousePos.y = 8;
			mousePos.z = 0;

			// transform.position = mousePos; // 너무 달라 붙으므로 lerp를 사용
			//현재위치, 목표위치, 따라가는 강도(0~1)
			transform.position = Vector3.Lerp(transform.position,mousePos,0.2f);
		}
	}

	public void Drag(){
		isDrag = true;
	}

	public void Drop(){
		isDrag = false;
		rigid.simulated = true;
	}
}
```

###
