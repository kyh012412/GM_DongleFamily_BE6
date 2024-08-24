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

### 물리 퍼즐게임 - 🌠멋진 이펙트 만들기 [B57]

#### 파티클 꾸미기

1. 하이라키에 particle system 추가 (Effect > particle system)(Effect)
   1. 단시간에 입자가 많이 나오는 형태를 사용하기위해서는
      1. Emission > Bursts 를 써야한다.
   2. Shape
      1. shape Circle
      2. radius 0.5
   3. Texture Sheet Animation
      1. Mode sprite
      2. sprite는 circle로 해준다.
   4. Start lifetime 0.5~1
   5. Start speed 10
   6. looping 체크해제
   7. Play on awake 꺼주기
   8. Limit velocity over lifetime
      1. drag 1
   9. size over lifetime
      1. 좌상단 우하단
   10. color over lifetime
       1. 중앙 하단 앵커 50% 하얀색
       2. 75 노란색
       3. 100 주황색
   11. Trails : 이밪에 꼬리 혹은 리본 효과 추가
       1. Renderer에 다시가보면 trail material이 생겨있음
          1. Default-line 사용
       2. Size affect lifetime 체크
       3. width over trail - curve선택
          1. 좌상단 우하향
   12. Renderer에서 Order in layer값 2로 설정
   13. Prefab화
       1. 0,0,0 포지션확인 (로테이션은 무관)

#### 이펙트 생성

1. Dongle.cs에서 particle system effect 만드렁준다.

#### 이펙트 실행

1. Dongle.cs 1. EffectPlay() 메서드 생성 & 연결
   GameManager.cs

```cs
    [Header("# Group")]
    public Transform dongleGroup;
    public Transform effectGroup;

    [Header("# Prefab")]
    public GameObject donglePrefab;
    public GameObject effectPrefab;

    Dongle GetDongle(){
        // 이펙트 생성
        GameObject instantEffectObj = Instantiate(effectPrefab, effectGroup);
        ParticleSystem instantEffect = instantEffectObj.GetComponent<ParticleSystem>();

        // 동글 생성
        // 두번째 매개변수로 parent.transform를주기
        GameObject instantDongleObj = Instantiate(donglePrefab, dongleGroup);
        Dongle instantDongle = instantDongleObj.GetComponent<Dongle>();
        instantDongle.effect = instantEffect;
        return instantDongle;
    }
```

Dongle.cs

```cs
	public ParticleSystem effect;

	IEnumerator LevelUpRoutine(){
		yield return new WaitForSeconds(0.2f);
		anim.SetInteger("Level",++level); // 실제 레벨 상승을 늦게 하는 이유는 애니메이션 시간 때문!
		EffectPlay();

		GameManager.instance.maxLevel = Mathf.Max(level,GameManager.instance.maxLevel);

		yield return new WaitForSeconds(0.35f);
		isMerge = false;
	}

	void EffectPlay(){
		effect.transform.position = transform.position;
		effect.transform.localScale = transform.localScale;
		effect.Play();
	}
```

### 물리 퍼즐게임 - 게임오버 구현하기 [B58]

#### 점수 추가

1. GameManager.cs에
2. score 변수를 public으로 두고
3. dongle을합치기전에 2의 level승만큼 점수를 부여해준다.

#### 경계선 이벤트

1. Line 객체의 태그를 Finish로 해준다.
2. Dongle에 잇는 Rigidbody 2d 컴포넌트 내에서 Sleeping Mode에서
   1. Sleeping Mode : 물리 연산을 멈취고 쉬는 상태 모드 설정
   2. Start Awake값을 never sleep으로 바꿔줘야한다.
      1. 버전차이인지 never sleep을 하지않아도 잘 연산되었다
      2. 그전의 문제는 onTriggerStay2d!를 쓰지않아서 동작하지 않았다.
      3. 다시 never sleep을 사용

#### 게임 오버

GameManager.cs

```cs
	public int score;

	public bool isOver;

	void NextDongle(){
		if(isOver) return;
		//...
	}

	public void GameOver(){
		if(isOver) return;

		isOver = true;
		StartCoroutine(GameOverRoutine());
	}

	IEnumerator GameOverRoutine(){
		// 1. 장면 안에 활성화 되어잇는 모든 동글 가져오기
		Dongle[] dongles = FindObjectsOfType<Dongle>();
		for(int index=0; index< dongles.Length;index++){
			dongles[index].rigid.simulated = false;
		}

		// 3. 1번의 목록을 하나씩 접근해서 지우기
		for(int index=0; index< dongles.Length;index++){
			dongles[index].Hide(Vector3.up*100);
			yield return new WaitForSeconds(0.1f);
		}
	}
```

Dongle.cs

```cs
	float deadTime;

	public Rigidbody2D rigid;
	SpriteRenderer spriteRenderer;

	void Awake()
	{
		//...
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void Hide(Vector3 targetPos){
		isMerge = true;
		rigid.simulated = false;
		circleCollider2D.enabled = false;

		if(targetPos == Vector3.up * 100){
			EffectPlay();
		}

		StartCoroutine(HideRoutine(targetPos));
	}

	IEnumerator HideRoutine(Vector3 targetPos){
		int frameCount =0;

		while(frameCount<20){
			frameCount++;
			if(targetPos != Vector3.up * 100){
				transform.position = Vector3.Lerp(transform.position,targetPos,0.5f);
			}else{
				transform.localScale = Vector3.Lerp(transform.localScale,Vector3.zero,0.2f);
			}

			yield return null;
		}

		GameManager.instance.score += (int)Mathf.Pow(2,level);

		isMerge = false;

		gameObject.SetActive(false);
	}

	void OnTriggerStay2D(Collider2D other){
		if(other.CompareTag("Finish")){
			deadTime += Time.deltaTime;

			if(deadTime >2){
				// float myConstant = 1f*deadTime/5;
				// spriteRenderer.color = new Color(myConstant,1-myConstant,1-myConstant);
				spriteRenderer.color = Color.red;
				Debug.Log("reded");
			}

			if(deadTime >5){
				GameManager.instance.GameOver();
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.CompareTag("Finish")){
			deadTime=0;
			spriteRenderer.color =Color.white;
		}
	}
```

### 물리 퍼즐게임 - 채널링이 포함된 🎵 사운드 시스템 [B59]

#### 배경음악

1. GameManager 이하의 객체에 오디오소스 컴포넌트를 추가해준다.(BGM Player)
2. GameManager 이하의 객체에 오디오소스 컴포넌트를 추가해준다.(SFX Player)
3. sfx player와 sfx clips 초기화
4. SfxPlay 메서드

#### 효과음 배치

GameManger.cs

```cs
	public AudioSource bgmPlayer;

	public AudioSource[] sfxPlayer;

	int sfxCursor;

	public AudioClip[] sfxClips;

	public enum Sfx { LevelUp, Next=3, Attach, Button, Over};

	void Start()
	{
		bgmPlayer.Play();
		NextDongle();
	}

	void NextDongle(){
		if(isOver) return;

		Dongle newDongle = GetDongle();
		lastDongle = newDongle;
		lastDongle.level = Random.Range(0,maxLevel);
		lastDongle.gameObject.SetActive(true);

		SfxPlay(Sfx.Next);
		StartCoroutine(WaitNext());
	}

	IEnumerator GameOverRoutine(){
		// 1. 장면 안에 활성화 되어잇는 모든 동글 가져오기
		Dongle[] dongles = FindObjectsOfType<Dongle>();

		// 2. 지우기 전에 모든 동글의 물리효과 비활성화
		for(int index=0; index< dongles.Length;index++){
			dongles[index].rigid.simulated = false;
		}

		// 3. 1번의 목록을 하나씩 접근해서 지우기
		for(int index=0; index< dongles.Length;index++){
			dongles[index].Hide(Vector3.up*100);
			yield return new WaitForSeconds(0.1f);
		}

		yield return new WaitForSeconds(1f);

		SfxPlay(Sfx.Over);
	}

	public void SfxPlay(Sfx type){
		switch(type){
			case Sfx.LevelUp:
			sfxPlayer[sfxCursor].clip = sfxClips[Random.Range(0,3)];
				break;
			case Sfx.Next:
			case Sfx.Attach: // 사물간에 부딪칠 때 나는 소리
			case Sfx.Button:
			case Sfx.Over:
				sfxPlayer[sfxCursor].clip = sfxClips[(int)type];
				break;
		}

		sfxPlayer[sfxCursor].Play();
		sfxCursor = (sfxCursor+1) % sfxPlayer.Length;
	}
```

Dongle.cs

```cs
	public bool isAttach;

	IEnumerator LevelUpRoutine(){
		yield return new WaitForSeconds(0.2f);
		anim.SetInteger("Level",++level); // 실제 레벨 상승을 늦게 하는 이유는 애니메이션 시간 때문!

		EffectPlay();
		GameManager.instance.SfxPlay(GameManager.Sfx.LevelUp);

		GameManager.instance.maxLevel = Mathf.Max(level,GameManager.instance.maxLevel);

		yield return new WaitForSeconds(0.35f);
		isMerge = false;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		StartCoroutine(AttachRoutine());
	}

	IEnumerator AttachRoutine(){
		if(isAttach) yield break;
		isAttach =true;
		GameManager.instance.SfxPlay(GameManager.Sfx.Attach);
		yield return new WaitForSeconds(0.2f);
		isAttach = false;
	}
```

### 물리 퍼즐게임 - 쉽게 구현해보는 오브젝트풀링 [B60]

#### 오브젝트 풀링

1. Instantiate, Destory를 사용할수록 파편화된 메모리 누적
2. 오브젝트풀링 (ObjectPooling) : 미리 생성해둔 오브젝트 재활용

#### 풀 생성

1. List를 2개만들어준다.(동글,particle system)
2. 다음과 같은 코드로편리하게 사용 가능한 하나의 방법이 있다.

```cs
    [Range(1,30)]
    public int poolSize;
    // 위와 같이 할경우 inspector에서 slider가 생겨서 사이즈 조절이편해짐
```

3. 리스트를 awake 단계에서 만들어준다.

#### 풀사용

1. 모든 dongle을 사용중일때 makedongle을하여 pool을 키워주고 그 대상을 사용한다.

#### 재사용 로직

1. active를 false할때 재사용을 위한 준비 코드
   GameManager.cs

```cs
    public List<Dongle> donglePool;

    public List<ParticleSystem> effectPool;



    [Range(1,30)]

    public int poolSize;

    public int poolCursor;

    void Awake()

    {

        if(instance==null){

            instance = this;

            Application.targetFrameRate = 60;

            donglePool = new List<Dongle>();

            effectPool = new List<ParticleSystem>();



            for(int index=0;index<poolSize;index++){

                MakeDongle();

            }

        }

    }

    void Start()

    {

        bgmPlayer.Play();

        NextDongle();

    }



    Dongle MakeDongle(){

        // 이펙트 생성

        GameObject instantEffectObj = Instantiate(effectPrefab, effectGroup);

        instantEffectObj.name = "Effect " + effectPool.Count;

        ParticleSystem instantEffect = instantEffectObj.GetComponent<ParticleSystem>();

        effectPool.Add(instantEffect);



        // 동글 생성

        // 두번째 매개변수로 parent.transform를주기

        GameObject instantDongleObj = Instantiate(donglePrefab, dongleGroup);

        instantDongleObj.name = "Dongle " + donglePool.Count;

        Dongle instantDongle = instantDongleObj.GetComponent<Dongle>();

        instantDongle.effect = instantEffect;

        donglePool.Add(instantDongle);



        return instantDongle;

    }



    Dongle GetDongle(){

        for (int i = 0; i < donglePool.Count; i++)

        {  

            poolCursor = (poolCursor +1) % donglePool.Count;

            if(!donglePool[poolCursor].gameObject.activeSelf){

                return donglePool[poolCursor];

            }

        }

        return MakeDongle();

    }



    void NextDongle(){

        if(isOver) return;



        lastDongle = GetDongle();

        lastDongle.level = Random.Range(0,maxLevel);

        lastDongle.gameObject.SetActive(true);



        SfxPlay(Sfx.Next);

        StartCoroutine(WaitNext());

    }
```

Dongle.cs

```cs
    void OnDisable()
    {
        // 동글 속성 초기화
        level = 0;
        isDrag =false;
        isMerge =false;
        isAttach =false;

        // 동글 트랜스폼 초기화
        transform.localPosition = Vector3.zero;
        transform.localRotation = quaternion.identity;
        transform.localScale =Vector3.zero;
       
        // 동글 물리 초기화
        circleCollider2D.enabled =true;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0;
        rigid.simulated =false;
    }
```

### 물리 퍼즐게임 - 📱모바일 게임으로 완성하기 [BE6]

#### 변수 정리

1. Header를 사용하여 정리

#### 점수 시스템 완성

1. Canvas Scaler
2. UI Scale Mode - Scale with Screen Size
   1. 450 : 950
3. Canvas > Text (Score Text)
   1. 앵커 좌상단
   2. pos x 10 y -5
   3. 0 0
   4. overflow
   5. 폰트 주아체
   6. 라벨 9999
   7. GameManager에 연결
4. lateUpdate : Updated 종료 후 실행되는 생명주기 함수
5. 메서드 구현 후 테스트
6. Score Text 를 복사 (Max Score Text)
   1. 앵커 우상단
   2. color c68444
   3. GameManager 연결
7. GameOverRoutine에서 갱신
8. 테스트 / 정상

#### 게임오버 UI

1. Canvas > Image (End Group)
   1. 앵커 전체크기
   2. 알파 100
2. End Group 내에 Image
   1. 소스 End
   2. set native size
   3. pos y 70
3. End Group 내에 Button
   1. 가로 세로 180 130
   2. pos y -60
   3. 소스 이미지 panel
4. End Group 내에 Button > Text
   1. 라벨 다시 하기
   2. 폰트 주아체
   3. 크기 30
   4. bottom 30
5. End Group 내에 Button > Text 를 복사 (SubScore Text)
   1. bottom 0 top 30
   2. 라벨 점수 : 9999
   3. 크기 20
6. End Group 비활성화 하기
7. GameManager.Reset() 메서드만들어주기
8. End Group > Button OnClick에 연결해준기
   1. navigation none;
9. GameOver시 bgmPlayer.Stop()

#### 게임 시작

1. End Group을 복사 (Start Group)
2. Start Group > Image
   1. 이미지 소스 Title
   2. Start Group > Button
      1. pos y -60
      2. 가로 세로 180 90
   3. Start Group > button > Score Text 삭제
   4. Start Group > button > Start Text
      1. 라벨 게임 시작
      2. bottom 15
3. Score Text와 Max Score Text 는 비활성화
4. PlayGround
   1. Line 비활성화
   2. Bottom 비활성화
5. GameManager에서 초기화
6. 게임시작 시 GameStart() 메서드가 실행되도록 연결

#### 모바일 빌드

1. Company Name, Product Name 설정
2. Default Icon 설정
3. Portrait 세팅
4. _other settings_ 1. configuration 1. scripting background - IL2CPP 2. ARM64체크
   GameManager.cs

```cs
public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	[Header("----------[ Core ]")]
	public bool isOver;
	public int score;
	public int maxLevel;

	[Header("----------[ Object Pooling ]")]
	public GameObject donglePrefab;
	public Transform dongleGroup;
	public List<Dongle> donglePool;
	public GameObject effectPrefab;
	public Transform effectGroup;
	public List<ParticleSystem> effectPool;

	[Range(1,30)]
	public int poolSize;
	public int poolCursor;
	public Dongle lastDongle;

	[Header("----------[ Audio ]")]
	public AudioSource bgmPlayer;
	public AudioSource[] sfxPlayer;
	public AudioClip[] sfxClips;
	public enum Sfx { LevelUp, Next=3, Attach, Button, Over};
	int sfxCursor;

	[Header("----------[ UI ]")]
	public GameObject startGroup;
	public GameObject endGroup;
	public Text scoreText;
	public Text maxScoreText;
	public Text subScoreText;

	[Header("----------[ ETC ]")]
	public GameObject line;
	public GameObject bottom;

	void Awake()
	{
		if(instance==null){
			instance = this;
			Application.targetFrameRate = 60;
			donglePool = new List<Dongle>();
			effectPool = new List<ParticleSystem>();

			for(int index=0;index<poolSize;index++){
				MakeDongle();
			}

			maxScoreText.text = PlayerPrefs.GetInt("MaxScore",0).ToString();
		}
	}

	public void GameStart()
	{
		// 오브젝트 활성화
		line.SetActive(true);
		bottom.SetActive(true);
		scoreText.gameObject.SetActive(true);
		maxScoreText.gameObject.SetActive(true);
		startGroup.SetActive(false);

		bgmPlayer.Play();
		SfxPlay(Sfx.Button);
		Invoke("NextDongle",1.5f);
	}

	IEnumerator GameOverRoutine(){
		// 1. 장면 안에 활성화 되어잇는 모든 동글 가져오기
		Dongle[] dongles = FindObjectsOfType<Dongle>();

		// 2. 지우기 전에 모든 동글의 물리효과 비활성화
		for(int index=0; index< dongles.Length;index++){
			dongles[index].rigid.simulated = false;
		}

		// 3. 1번의 목록을 하나씩 접근해서 지우기
		for(int index=0; index< dongles.Length;index++){
			dongles[index].Hide(Vector3.up*100);
			yield return new WaitForSeconds(0.1f);
		}      

		yield return new WaitForSeconds(1f);

		// 최고 점수 갱신
		int maxScore = Mathf.Max(score,PlayerPrefs.GetInt("MaxScore",0));
		PlayerPrefs.SetInt("MaxScore",maxScore);

		// 게임오버 UI 표시
		subScoreText.text = "점수 : "+scoreText.text;
		endGroup.SetActive(true);

		// 음향
		bgmPlayer.Stop();
		SfxPlay(Sfx.Over);
	}

	public void Reset(){
		SfxPlay(Sfx.Button);
		StartCoroutine(ResetCoroutine());
	}

	IEnumerator ResetCoroutine(){
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(0);
	}

	void LateUpdate()
	{
		scoreText.text = score.ToString();
	}

	public void Update(){
		if(Input.GetButtonDown("Cancel")){
			Application.Quit();
		}
	}
```

###
