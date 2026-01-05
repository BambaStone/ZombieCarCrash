using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public List<GameObject> Spanwer;    //현 스테이지의 좀비 스포너
    public GameObject ZombiePrepab;     //좀비 스포너에 넣을 좀비 프리팹
    public GameObject Zombies;          //현 스테이지의 좀비 오브젝트의 부모오브젝트 
    public float SpawnTime = 5;         //현 스테이지의 좀비 스폰 속도
    public int SpanwerMaxZombie = 100;  //현 스테이지의 한 스포너당 최대 소환 좀비 갯수
    public bool isActive = false;       //스테이지활성화상태

    bool _setSpanwer = false;            //좀비스포너 세팅이 되어 있는지 여부
    
    private void OnEnable() //활성화시
    {
        if(!_setSpanwer)     //좀비프리팹 세팅 되어있는가?
        {
            _setSpanwer = true;                  //세팅됨으로 변경
            ZombieSpawner tempSpawner;          //임시 스포너 설정
            for(int i=0;i<Spanwer.Count;i++)    //좀비 스포너 수 만큼
            {
                //임시 스포너설정에 i번째 스포너의 설정 지정
                tempSpawner = Spanwer[i].GetComponent<ZombieSpawner>();
                //스포너에 해당 스테이지에서 소환할 좀비 프리팹 넣기
                tempSpawner.ZombiePrefabs = ZombiePrepab;
                //스포너에 스폰 타임 지정
                tempSpawner.SpawnTime = SpawnTime;
                //스포너에 최대 좀비 소환 갯수 지정
                tempSpawner.MaxZombie = SpanwerMaxZombie;
            }
        }
        //스포너의 랜덤이동 시작
        
    }

    //스포너의 랜덤이동함수
    IEnumerator SpawnerMove()
    {
        //1초에 한번씩
        yield return new WaitForSeconds(0.2f);

        //스포너의 갯수 만큼
        for (int i = 0; i < Spanwer.Count; i++)
        {
            //스포너를 스테이지 크기 안에서 랜덤으로 이동
            Spanwer[i].transform.localPosition = new Vector3(Random.Range(-0.48f, 0.48f), 3, Random.Range(-0.48f, 0.48f));
        }
        //스포너의 랜덤이동 반복호출
        StartCoroutine(SpawnerMove());
    }

    

    public void ActiveSpanwer()//스포너 활성화
    {
        //좀비의 부모오브젝트 활성화
        Zombies.SetActive(true);
        //스포너의 수만큼 반복
        for (int i = 0; i < Spanwer.Count; i++)
        {
            //스포너 활성화
            Spanwer[i].SetActive(true);
        }
        isActive = true; // 활성화 상태 변경
        StartCoroutine(SpawnerMove());
    }
    public void UnActiveSpanwer()//스포너 비활성화
    {
        //스포너의 수만큼 반복
        for (int i = 0; i < Spanwer.Count; i++)
        {
            //스포너 비활성화
            Spanwer[i].GetComponent<ZombieSpawner>().UnActiveSelf();
        }
        //좀비의 부모오브젝트 비활성화
        Zombies.SetActive(false);
        isActive = false; // 활성화 상태 변경
        StopAllCoroutines();
    }


    private void OnTriggerExit(Collider other)
    {
        //좀비가 스테이지 밖으로 나갔을 때
        if(other.CompareTag("Zombie"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
