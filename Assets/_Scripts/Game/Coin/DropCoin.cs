using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCoin : MonoBehaviour   //플레이어의 차 또는 좀비에게서 드랍된 코인을 작동시키는 스크립트
{

    public GameObject Target;   //드랍된 코인이 향할 타겟

    private Rigidbody _rigidbody;//드랍된 코인의 물리상태
    private PlayerController _targetPStat;//타겟이 플레이어일시 플레이어의 상태
    private bool _goCoinTank = false;//드랍된 코인의 코인탱크방향으로의 출발 상태
    private bool _goCoinInput = false;//드랍된 코인을 다른 코인 투입구로 향할때의 출발 상태
    private float _coinSpeed = 0;//드랍된코인의 이동속도
    private bool _triggerOn = true;//드랍된 코인의 트리거가 켜져있는지 상태
    private void OnEnable()//스크립트가 포함된 오브젝트가 활성화시 = 드랍코인 활성화시
    {
        _triggerOn = true;//트리거상태 true
        gameObject.GetComponent<BoxCollider>().isTrigger = _triggerOn;//트리거 상태 박스콜리더에 적용
        _rigidbody = gameObject.GetComponent<Rigidbody>();//리지드바디 가져오기
        
        //출발상태 초기화
        _goCoinInput = false;
        _goCoinTank = false;
        
        _coinSpeed = 0;//드랍코인 이동속도
        gameObject.transform.rotation = Quaternion.identity;//드랍코인의 방향토기화
        _rigidbody.velocity = Vector3.zero;//드랍코인의 가속도 초기화
        _rigidbody.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(10f, 15f), Random.Range(-1f, 1f)), ForceMode.Impulse);//활성화시 위쪽방향으로 튕기기
        StartCoroutine(GoTarget());//타겟으로의 출발 시작
    }


    IEnumerator GoTarget()//타겟으로의 출발 함수
    {
        yield return new WaitForSeconds(1f);//1초 대기
        if (Target.CompareTag("CoinTank"))//타겟이 코인탱크라면
        {
            _targetPStat = Target.GetComponent<PickUpCoinForCoinTank>().PC;//코인탱크에 지정된 플레이어의 상태 가져오기
            _goCoinTank = true;//코인탱크방향으로의 출발상태 true
        }
        else//타겟이 코인탱크가 아니라면
        {
            _goCoinInput=true;//코인투입구를 향해 출발상태 true
        }
        
    }

    IEnumerator UnActiveCoin()//드랍코인 활동 비활성화 함수
    {
        yield return new WaitForSeconds(5f);//5초 대기
        if (!_triggerOn)//트리거가 off상태일때
        {
            gameObject.SetActive(false);//드랍코인 비활성화
        }
    }


    private void FixedUpdate()
    {
        if (_goCoinTank)//코인탱크 방향 출발일때
        {
            if (_triggerOn)//드랍코인의 트리거가 ON상태일때
            {
                if ((_targetPStat.Coin < _targetPStat.MaxCoin) && _targetPStat.OnStage)//플레이어의 현재 코인 갯수가 최대코인갯수보다 적고, 플레이어가 마을 밖 스테이지에 있을때
                {
                    Vector3 targetPos = Target.transform.position;//타겟 위치 지정

                    //타겟 방향 지정
                    Vector3 dir = targetPos - transform.position;//타겟위치 - 현재 드랍코인 위치
                    dir.Normalize();//방향 노멀라이즈

                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), _coinSpeed);//드랍코인의 방향을 타겟 방향으로 보간을 통해 부드럽게 회전
                    
                    if(_coinSpeed<100)//드랍코인속도가 100보다 작을때
                    {
                        _coinSpeed += 100f * Time.deltaTime;//1초동안 100의 속도를 추가
                        if(100<_coinSpeed)//드랍코인속도가 100보다 클때
                        {
                            _coinSpeed = 100f;//드랍코인 속도는 100
                        }
                    }
                    _rigidbody.velocity = transform.forward * _coinSpeed;//드랍코인의 가속도 = 드랍코인의 정면쪽으로 드랍코인 속도만큼 
                }
                else//플레이어의 현재 코인 갯수가 최대코인 갯수보다 많거나 같을때
                {
                    _triggerOn = false;//드랍코인의 트리거상태 off
                    gameObject.GetComponent<BoxCollider>().isTrigger = _triggerOn;//드랍코인의 트리거상태 적용
                    _coinSpeed = 0;//드랍코인속도 0으로 초기화
                    _rigidbody.velocity =Vector3.zero;//드랍코인의 가속도 0으로 초기화
                    StartCoroutine(UnActiveCoin());//드랍코인의 비활성화 함수 작동
                }
            }
            else //드랍코인의 트리거상태가 OFF일때
            {
                if(_targetPStat.Coin < _targetPStat.MaxCoin && _targetPStat.OnStage)//플레이어의 현재 코인 갯수가 최대코인갯수보다 적고, 플레이어가 마을 밖 스테이지에 있을때
                {
                    _triggerOn = true;//드랍코인의 트리거상태 ON
                    gameObject.GetComponent<BoxCollider>().isTrigger = _triggerOn;//드랍코인의 트리거상태 적용
                    transform.LookAt(Target.transform.position);//드랍코인의 방향을 타겟쪽으로
                }
            }
        }
        if(_goCoinInput)//코인투입구 방향으로 출발일때
        {
            Vector3 targetPos = Target.transform.position;//타겟 위치

            //타겟 방향
            Vector3 dir = targetPos - transform.position;//타겟위지 - 드랍코인위치
            dir.Normalize();//방향 노멀라이즈

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 50 * Time.deltaTime);//드랍코인의 방향을 타겟 방향으로 보간을 통해 부드럽게 회전
            _coinSpeed += 100f * Time.deltaTime;//1초동안 100의 속도를 추가
            _rigidbody.velocity = transform.forward * _coinSpeed;//드랍코인의 가속도 = 드랍코인의 정면쪽으로 드랍코인 속도만큼 
        }
    }
}
