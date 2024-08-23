using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Dongle lastDongle;
    public int maxLevel;

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
}
