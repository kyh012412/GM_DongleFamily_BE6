using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class Dongle : MonoBehaviour
{
    public ParticleSystem effect;
    public int level;
    public bool isDrag; //기본값 false drag를 통해 true를 거쳐서 drop 후 다시 false
    public bool isMerge; // 합쳐지는 중인지 상태를 제어하는 변수

    float deadTime;

    public Rigidbody2D rigid;
    Animator anim;
    CircleCollider2D circleCollider2D;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        anim.SetInteger("Level",level);
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

    void LevelUp(){
        isMerge = true;
        // rigid.velocity = Vector2.zero;
        // rigid.angularVelocity = 0;

        StartCoroutine(LevelUpRoutine());
    }

    IEnumerator LevelUpRoutine(){
        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("Level",++level); // 실제 레벨 상승을 늦게 하는 이유는 애니메이션 시간 때문!
        
        EffectPlay();

        GameManager.instance.maxLevel = Mathf.Max(level,GameManager.instance.maxLevel);

        yield return new WaitForSeconds(0.35f);
        isMerge = false;
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

    void EffectPlay(){
        effect.transform.position = transform.position;
        effect.transform.localScale = transform.localScale;
        effect.Play();
    }
}
