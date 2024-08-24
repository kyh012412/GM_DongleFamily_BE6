using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

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

    public void GameOver(){
        if(isOver) return;

        
        isOver = true;
        StartCoroutine(GameOverRoutine());
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

    void LateUpdate()
    {
        scoreText.text = score.ToString();
    }

    public void Update(){
        if(Input.GetButtonDown("Cancel")){
            Application.Quit();
        }
    }
}
