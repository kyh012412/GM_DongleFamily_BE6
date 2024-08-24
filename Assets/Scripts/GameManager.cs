using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Dongle lastDongle;
    public List<Dongle> donglePool;
    public List<ParticleSystem> effectPool;

    [Range(1,30)]
    public int poolSize;
    public int poolCursor;

    public int maxLevel;
    public int score;
    public bool isOver;

    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayer;
    int sfxCursor;
    public AudioClip[] sfxClips;
    public enum Sfx { LevelUp, Next=3, Attach, Button, Over};

    [Header("# Group")]
    public Transform dongleGroup;
    public Transform effectGroup;

    [Header("# Prefab")]
    public GameObject donglePrefab;
    public GameObject effectPrefab;

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
}
