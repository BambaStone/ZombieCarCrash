using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WolrdCoin : MonoBehaviour //코인투입구로 저장한 월드 코인을 UI에 표시해주는 스크립트
{
    public TMP_Text WorldCoinText; // 월드코인 UI
    private int _wolrdCoin=0;      //월드코인의 변화를 감지하기위한 변수 
    void Start()//게임이 시작되면
    {
        WorldCoinText = gameObject.GetComponent<TMP_Text>();//해당 스크립트가 포함된 오브젝트에서 텍스트UI 받아오기
        _wolrdCoin = StatusManager.Instance.WorldCoin;      //스테이터스매니저에서 월드코인 갯수 받아오기
        WorldCoinText.text = _wolrdCoin + "";               //받아온 월드코인갯수를 UI에 표시하기
    }

    private void FixedUpdate()//일정 주기로 반복
    {
        if(_wolrdCoin != StatusManager.Instance.WorldCoin)//만약 스테이터스의 월드코인과 해당 스크립트에서 받아왔던 월드코인의 갯수가 다를경우
        {
            _wolrdCoin = StatusManager.Instance.WorldCoin;//해당 스크립트의 변수에 다시 스테이터스 매니저에서 월드코인 갯수 받아오기
            WorldCoinText.text = _wolrdCoin + "";         //다시 받아온 월드코인 갯수를 UI에 표시하기
        }
    }
}
