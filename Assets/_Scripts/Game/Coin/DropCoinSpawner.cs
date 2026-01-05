using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCoinSpawner : MonoBehaviour    //드랍코인 생성 및 재활용을 관리하는 스크립트
{
    public List<GameObject> DropCoinList;//드랍코인 재활용을 위한 리스트
    public GameObject DropCoinPrefab;//드랍코인의 프리팹
    public GameObject CoinTank;//좀비에게서 드랍된 코인을 넣을 코인탱크
    public GameObject CoinHouse;//코인탱크에서 드랍된 코인을 넣을 코인하우스
    public void CoinDrop(Vector3 pos)//좀비에게서 코인을 드랍하는 함수 (pos = 죽은 좀비의 위치)
    {
        bool succes = false;//재활용 성공 상태
        for (int i = 0; i < DropCoinList.Count; i++)//재활용 리스트만큼 반복
        {
            if (!(DropCoinList[i].gameObject.activeSelf))//i번째 리스트의 오브젝트(드랍코인)이 비활성화 상태일때
            {
                DropCoinList[i].transform.position = pos;//i번째 리스트의 드랍코인 위치를 pos(죽은 좀비의 위치)로 이동
                DropCoinList[i].GetComponent<DropCoin>().Target = CoinTank;//드랍코인의 타겟을 코인탱크로 지정
                DropCoinList[i].SetActive(true);//드랍코인을 활성화
                succes = true;//재활용 성공
                break;//반복 종료
            }
        }
        if (!succes)//재활용 실패시
        {
            DropCoinList.Add(Instantiate(DropCoinPrefab, pos, Quaternion.identity));//드랍코인을 pos(죽은 좀비의 위치)에 소환
            DropCoinList[DropCoinList.Count - 1].GetComponent<DropCoin>().Target= CoinTank;//생성된 드랍코인의 타겟을 코인탱크로 지정
            DropCoinList[DropCoinList.Count - 1].transform.parent = transform;//드랍코인스포너를 생성된 드랍코인의 부모오브젝트로 지정
        }
    }
    public void CoinInput(Vector3 pos) //코인탱크에서 마을의 코인하우스 코인투입구로 코인을 전달하는 함수 (pos = 코인탱크 내부의 코인 위치)
    {
        bool succes = false;//재활용 성공 상태
        for (int i = 0; i < DropCoinList.Count; i++)//재활용 리스트만큼 반복
        {
            if (!(DropCoinList[i].gameObject.activeSelf))//i번째 리스트의 오브젝트(드랍코인)이 비활성화 상태일때
            {
                DropCoinList[i].transform.position = pos;//i번째 리스트의 드랍코인 위치를 pos(코인탱크 내부의 코인위치)로 이동
                DropCoinList[i].GetComponent<DropCoin>().Target = CoinHouse;//드랍코인의 타겟을 코인하우스로 지정
                DropCoinList[i].SetActive(true);//드랍코인을 활성화
                succes = true;//재활용 성공
                break;//반복 종료
            }
        }
        if (!succes)//재활용 실패시
        {
            DropCoinList.Add(Instantiate(DropCoinPrefab, pos, Quaternion.identity));//드랍코인을 pos(코인탱크 내부의 코인위치)에 소환
            DropCoinList[DropCoinList.Count - 1].GetComponent<DropCoin>().Target = CoinHouse;//생성된 드랍코인의 타겟을 코인하우스로 지정
            DropCoinList[DropCoinList.Count - 1].transform.parent = transform;//드랍코인스포너를 생성된 드랍코인의 부모오브젝트로 지정
        }
    }

    public void CoinInputWithTarget(Vector3 pos,GameObject target) //코인탱크에서 코인투입구(마을밖 다음 스테이지의 입구)로 코인을 전달하는 함수 (pos = 코인탱크 내부의 코인 위치, target = 코인을 전달할 오브젝트(예시 : 스테이지 입구의 벽))
    {
        bool succes = false;//재활용 성공 상태
        for (int i = 0; i < DropCoinList.Count; i++)//재활용 리스트만큼 반복
        {
            if (!(DropCoinList[i].gameObject.activeSelf))
            {
                DropCoinList[i].transform.position = pos;
                DropCoinList[i].GetComponent<DropCoin>().Target = target;
                DropCoinList[i].SetActive(true);
                succes = true;
                break;
            }
        }
        if (!succes)//재활용 실패시
        {
            DropCoinList.Add(Instantiate(DropCoinPrefab, pos, Quaternion.identity));//드랍코인을 pos(코인탱크 내부의 코인위치)에 소환
            DropCoinList[DropCoinList.Count - 1].GetComponent<DropCoin>().Target = target;
            DropCoinList[DropCoinList.Count - 1].transform.parent = transform;//드랍코인스포너를 생성된 드랍코인의 부모오브젝트로 지정
        }
    }

}
