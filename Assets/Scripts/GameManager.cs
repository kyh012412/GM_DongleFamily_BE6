using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Dongle lastDongle;
    public int maxLevel;
    public int score;
    public bool isOver;

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
        }
    }

    void Start()
    {
        NextDongle();
    }

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

    void NextDongle(){
        if(isOver) return;

        Dongle newDongle = GetDongle();
        lastDongle = newDongle;
        lastDongle.level = Random.Range(0,maxLevel);
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
}
