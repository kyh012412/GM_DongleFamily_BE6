using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

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
