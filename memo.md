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

### 물리 퍼즐게임 - 📦프리펩으로 다양한 동글 생성하기 [B55]

#### 게임 매니저

1. GameManager.cs
2. Touch Pad 태그에 있는 Event Trigger에 대상을 Dongle에서 GameManager로 변경

#### 동글 생성

1. PlayGround > Dongle Group 객체 추가
   1. pos 0 8 0
2. 기존 하이라키에 있는 Dongle 삭제

#### 다양한 동글

1. _코루틴(Coroutine) : 로직 제어를 유니티에게 맡기는 함수_

#### 프레임 설정과 물리 보정

1. 애니메이션 만들기
2. Dongle Open Prefab으로 간다.
   1. animator 컴포넌트를 추가한다.
      1. runtime animator controller는 AcDongle을 연결해준다.
   2. 애니메이터 내부에는 이미 state와 level (int) 파라미터가 있고 Condition으로 연결되어 있다.
   3. Create Animation을 해준다.(Level 0)
   4. 애니메이션 : 오브젝트의 각종 컴포넌트를 시간을 활용해서 변화
      1. 스프라이트 변화시키는법 animation 탭내부로 외부 스프라이트를 드래그드랍해준다.
      2. scale 0 > 1 20프레임에 걸쳐서 되게 만들어준다.
   5. 기본 프리펩 이미지 크기를 0 0 0으로맞춰준다.
   6. 기본 객체를 비활성화로 만들어준다.
3. OnEnable : 스크립트가활성화 될때 실행되는 이벤트 함수

#### 프레임 설정

1. _프레임 설정하는법_ : `Application.targetFrameRate = 60;`
2. Dongle Prefab내부로 가서 Rigidbody 2d에서 1. Interpolate 속성값 - None > Interpolate 1. 이전 프레임을 비교하여 움직임을 부드럽게 보정
   GameManager.cs

```cs
public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public Dongle lastDongle;
	public GameObject donglePrefab;
	public Transform dongleGroup;

	void Awake()
	{
		if(instance==null){
			instance = this;
			Application.targetFrameRate = 60;
		}
	}

	void Start()
	{
		NextDongle();
	}

	Dongle GetDongle(){
		// 두번째 매개변수로 parent.transform를주기
		GameObject instant = Instantiate(donglePrefab, dongleGroup);
		Dongle instantDongle = instant.GetComponent<Dongle>();
		return instantDongle;
	}

	void NextDongle(){
		Dongle newDongle = GetDongle();
		lastDongle = newDongle;
		lastDongle.level = Random.Range(0,8);
		lastDongle.gameObject.SetActive(true);
		StartCoroutine(WaitNext());
	}

	IEnumerator WaitNext(){
		while(lastDongle !=null){
			yield return null;
		}

		// yield return null; // 한 프레임을 쉬는 코드
		yield return new WaitForSeconds(2.5f);

		NextDongle();
	}

	public void TouchDown(){
		if(lastDongle == null)
			return;

		lastDongle.Drag();
	}

	public void TouchUp(){
		if(lastDongle == null)
			return;

		lastDongle.Drop();
		lastDongle = null;
	}
}
```

Dongle.cs

```cs
	public int level;
	Animator anim;

	void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	void OnEnable()
	{
		anim.SetInteger("Level",level);
	}
```

### 물리 퍼즐게임 - 물리이벤트로 동글 합치기 [B56]

#### 동글 충돌

1. 조건문으로 까다로운제어를해준다.
2. 레벨비교 최대레벨확인 상대적위치확인
3. 위쪽 동글 또는 동일 높이라면 우측동글이 사라지도록 효과를 준다.
4. 버그를 막기위해 bool 변수 isMerge도 추가해준다.

#### 동글 성장(레벨업)

1. Rigidbody2D.angularVelocity는 자료형이float 다.
   1. 양수음수로 시계방향 또는 반시계방향으로 구분

#### 물리 보정

1. edit > Project Settings > physics 2d
   1. Auto Sysnc Transforms 체크
      1. 트랜스폼과 물리의 차이를 보정

#### 최대 레벨

1. GameManager의 Inspecotr에서 MaxLevel을 2로 잡아준다.
2. Dongle에서 level을 증가시는 곳에서 조건부 갱신로직을추가해준다.
3. 테스트
   Dongle.cs

```cs
	public bool isMerge; // 합쳐지는 중인지 상태를 제어하는 변수

	CircleCollider2D circleCollider2D;

	void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		circleCollider2D = GetComponent<CircleCollider2D>();
	}

	void OnCollisionStay2D(Collision2D other)
	{
		//동글 합치기 로직
		if(other.gameObject.CompareTag("Dongle")){
			Dongle otherDongle = other.gameObject.GetComponent<Dongle>();
			if(level == otherDongle.level && !isMerge && !otherDongle.isMerge && level < 7){
				// 나와 상대편 위치 가져오기
				float myX = transform.position.x;
				float myY = transform.position.y;
				float otherX = other.transform.position.x;
				float otherY = other.transform.position.y;

				// 1. 내가 아래에 있을 때
				// 나를 키우고 상대를 숨김

				// 2. 동일한 높이 일때, 내가 오른쪽에 있을 때
				if(myY < otherY || (myY == otherY && myX>otherX) ){                   
					// 상대방은 숨기기
					otherDongle.Hide(transform.position);
					//나는 레벨업
					LevelUp();
				}
			}
		}
	}

	public void Hide(Vector3 targetPos){   
		isMerge = true;
		rigid.simulated = false;
		circleCollider2D.enabled = false;
		StartCoroutine(HideRoutine(targetPos));
	}

	IEnumerator HideRoutine(Vector3 targetPos){
		int frameCount =0;

		while(frameCount<20){
			frameCount++;
			transform.position = Vector3.Lerp(transform.position,targetPos,0.5f);
			yield return null;
		}

		isMerge = false;
		gameObject.SetActive(false);
	}

	void LevelUp(){
		isMerge = true;
		// rigid.velocity = Vector2.zero;
		// rigid.angularVelocity = 0;
		StartCoroutine(LevelUpRoutine());
	}

	IEnumerator LevelUpRoutine(){
		yield return new WaitForSeconds(0.2f);
		anim.SetInteger("Level",++level); // 실제 레벨 상승을 늦게 하는 이유는 애니메이션 시간 때문!

		GameManager.instance.maxLevel = Mathf.Max(level,GameManager.instance.maxLevel);

		yield return new WaitForSeconds(0.35f);
		isMerge = false;
	}
```

GameManager.cs

```cs
	public int maxLevel;

	void NextDongle(){
		Dongle newDongle = GetDongle();
		lastDongle = newDongle;
		lastDongle.level = Random.Range(0,maxLevel);
		lastDongle.gameObject.SetActive(true);
		StartCoroutine(WaitNext());
	}
```

###
