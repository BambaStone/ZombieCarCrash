using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CoinHouse : MonoBehaviour //마을의 코인을 넣는 코인하우스 스크립트
{
    

    public CoinSounds CS;//코인입력 사운드를 위한 코인사운드스크립트
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DropCoin"))//플레이어 차에서 드랍된 코인이 코인하우스에 트리거 될때
        {
            CS.OnCoinSound(other.transform.position);//코인 사운드 출력
            StatusManager.Instance.WorldCoin++;//월드코인 1추가
            other.gameObject.SetActive(false);//트리거된 드랍코인 비활성화
        }
    }
}
