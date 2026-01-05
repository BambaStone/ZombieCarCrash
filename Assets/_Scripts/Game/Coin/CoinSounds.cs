using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSounds : MonoBehaviour  //코인사운드를 관리하는 스크립트
{
    
    public GameObject CoinSound; //코인사운드 프리팹
    public List<GameObject> Soundrecycling; //코인사운드 재활용을 위한 리스트

    public void OnCoinSound(Vector3 pos) //코인사운드 실행
    {
        bool succes = false;//재활용 성공여부
        for (int i = 0; i < Soundrecycling.Count; i++)//재활용리스트 갯수만큼 반봅
        {
            if (!(Soundrecycling[i].gameObject.activeSelf))//i번째 리스트의 사운드 프리팹이 비활성화 상태일때
            {
                Soundrecycling[i].transform.position = pos;//pos위치로 사운드이동
                Soundrecycling[i].SetActive(true);//사운드 활성화 (활성화시 플레이 사운드)
                succes = true;//재활용 성공
                break;//반복종료
            }
        }
        if (!succes)//재활용 실패시
        {
            Soundrecycling.Add(Instantiate(CoinSound, pos, Quaternion.identity));// pos위치에 사운드 소환
            Soundrecycling[Soundrecycling.Count - 1].transform.parent = gameObject.transform;//사운드관리스크립드가 있는 오브젝트를 사운드의 부모오브젝트로 지정
        }
    }
}
