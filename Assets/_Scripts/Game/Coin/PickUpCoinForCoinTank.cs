using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCoinForCoinTank : MonoBehaviour //차의 짐칸에 위치한 코인탱크에 드랍코인의 충돌을 체크하는 스크립트
{
    public GameObject Player;//플레이어 게임 오브젝트
    public PlayerController PC;//플레이어의 상태를 관리하는 스크립트
    public CoinSounds CS;//코인사운드 관리하는 스크립트
    private int Coin;//현재 코인갯수
    private int MaxCoin;//코인탱크가 가질수 있는 최대 코인 갯수

    private void Start()
    {
        PC = Player.GetComponent<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DropCoin"))//좀비로부터 드랍된 코인의 코인탱크 트리거시
        {

            Coin = PC.Coin;//플레이어 차의 상태에서 현재 코인 갯수를 받아옴
            MaxCoin = PC.MaxCoin;//플레이어 차의 상태에서 최대코인 갯수를 받아옴
            if (Coin < MaxCoin)//최대 코인 갯수보다 현재 코인 갯수가 적을때
            {
                if(other.GetComponent<DropCoin>().Target.CompareTag("CoinTank"))//드랍코인의 목표가 코인탱크일때
                {
                    CS.OnCoinSound(other.transform.position);//코인사운드 출력
                    other.gameObject.SetActive(false);//드랍코인비활성화
                    PC.PlusCoin(other.transform.position);//플레이어상태에서 코인추가,짐칸에 코인소환또는 활성화
                }
            }
        }
    }
}
