using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SimpleCarController : MonoBehaviour
{

    public int MaxSpeed = 100;           //전진 최고속도//
    public int MaxBackSpeed = 45;       //후진 최고속도
    public int Acceleration = 4;              //가속도 
    public float MaxSteering = 45f;    //최대 휠 방향 회전각도
    public float SteeringSpeed = 0.5f; //휠 방향 회전속도
    public float BreakPower = 350;      //브레이크 파워
    public int Deceleration = 2;        //전진하지 않을때 감속도
    public Vector3 MassCenter;          //자동차의 중심

    public int DriftMultiplier = 5;         //드리프트시 접지력 감소정도

    public GameObject WheelFLMesh;            //왼쪽 앞 휠 메쉬
    public WheelCollider WheelFLCollider;     //왼쪽 앞 휠 콜라이더
    public GameObject WheelBLMesh;            //왼쪽 뒤 휠 메쉬
    public WheelCollider WheelBLCollider;     //왼쪽 뒤 휠 콜라이더
    public GameObject WheelFRMesh;            //오른쪽 앞 휠 메쉬
    public WheelCollider WheelFRCollider;     //오른쪽 앞 휠 콜라이더
    public GameObject WheelBRMesh;            //오른쪽 뒤 휠 메쉬
    public WheelCollider WheelBRCollider;     //오른쪽 뒤 휠 콜라이더

    public bool EffectsUse = false;       //이펙트 사용여부
    public ParticleSystem WheelFLMist;    //왼쪽 앞 휠 연기 이펙트
    public ParticleSystem WheelFRMist;    //오른쪽 앞 휠 연기 이펙트
    public TrailRenderer WheelFLSkid;     //왼쪽 앞 휠 스키드마크
    public TrailRenderer WheelFRSkid;     //오른쪽 앞 휠 스키드마크
    public ParticleSystem WheelBLMist;    //왼쪽 뒤 휠 연기 이펙트
    public ParticleSystem WheelBRMist;    //오른쪽 뒤 휠 연기 이펙트
    public TrailRenderer WheelBLSkid;     //왼쪽 뒤 휠 스키드마크
    public TrailRenderer WheelBRSkid;     //오른쪽 뒤 휠 스키드마크

    public bool SoundsUse = false;          //사운드 사용 여부
    public AudioSource CarEngineSound;      //차의 엔진 소리
    public AudioSource TireScreechSound;    //바퀴 끌리는 소리
    private float _initialCarEngineSoundPitch;  //자동차 엔진 소리의 초기 피치

    [HideInInspector]
    public float CarSpeed;                  //현재 자동차의 속도
    [HideInInspector]
    public bool UseAccele;                  //현재 차량의 가속사용 여부
    [HideInInspector]
    public bool UseBackAccele;              //현재 차량의 후진가속사용 여부
    [HideInInspector]
    public bool UseBrake;                   //현재 브레이크 사용중
    [HideInInspector]
    public bool UseHandBrake;               //현재 핸드브레이크 사용중
    [HideInInspector]
    public bool Drifting;                   //현재 차량의 드리프트 여부


    private Rigidbody _carRigidbody;        //차의 리지드 바디
    private float _steeringAxis;            //스티어링 정도 -1 ~ 1까지의 수
    private float _throttleAxis;            //차의 스로틀 상태이다.
    private float _driftingAxis;            //현재 드리프트 정도 0~1까지
    private float _localVelocityZ;          //현재 리지드바디의 Z로컬 속도 
    private float _localVelocityX;          //현재 리지드바디의 X로컬 속도 
    private float _localVelocity;           //현재 리지드바디의 로컬속도
    private bool _decelerating;             //감속중인지 상태
    private bool _right;                    //오른쪽 턴 상태
    private bool _left;                     //왼쪽 턴 상태

    //휠 각각의 측면 마찰력 정보
    private WheelFrictionCurve _wheelFLFriction;
    private float _wheelFLExtremumSlip;
    private WheelFrictionCurve _wheelFRFriction;
    private float _wheelFRExtremumSlip;
    private WheelFrictionCurve _wheelBLFriction;
    private float _wheelBLExtremumSlip;
    private WheelFrictionCurve _wheelBRFriction;
    private float _wheelBRExtremumSlip;

    // Start is called before the first frame update
    void Start()
    {
        _carRigidbody = gameObject.GetComponent<Rigidbody>();   //리지드 바디 변수 초기화
        _carRigidbody.centerOfMass = MassCenter;                //리지드 바디의 중심점 갱신

        //뒷바퀴의 마찰력 정보 사용하기 쉽게 저장
        _wheelBLFriction = new WheelFrictionCurve();
        _wheelBLFriction.extremumSlip = WheelBLCollider.sidewaysFriction.extremumSlip;
        _wheelBLExtremumSlip = WheelBLCollider.sidewaysFriction.extremumSlip;
        _wheelBLFriction.extremumValue = WheelBLCollider.sidewaysFriction.extremumValue;
        _wheelBLFriction.asymptoteSlip = WheelBLCollider.sidewaysFriction.asymptoteSlip;
        _wheelBLFriction.asymptoteValue = WheelBLCollider.sidewaysFriction.asymptoteValue;
        _wheelBLFriction.stiffness = WheelBLCollider.sidewaysFriction.stiffness;
              
        _wheelBRFriction = new WheelFrictionCurve();
        _wheelBRFriction.extremumSlip = WheelBRCollider.sidewaysFriction.extremumSlip;
        _wheelBRExtremumSlip = WheelBRCollider.sidewaysFriction.extremumSlip;
        _wheelBRFriction.extremumValue = WheelBRCollider.sidewaysFriction.extremumValue;
        _wheelBRFriction.asymptoteSlip = WheelBRCollider.sidewaysFriction.asymptoteSlip;
        _wheelBRFriction.asymptoteValue = WheelBRCollider.sidewaysFriction.asymptoteValue;
        _wheelBRFriction.stiffness = WheelBRCollider.sidewaysFriction.stiffness;


        if (CarEngineSound != null)//자동차 엔진 사운드가 비어있지 않을때
        {
            _initialCarEngineSoundPitch = CarEngineSound.pitch;//자동차 엔진소리의 초기 피치를 저장
        }

        if (SoundsUse)   //사운드를 사용한다고 체크했을시
        {
            InvokeRepeating("CarSounds", 0f, 0.1f);//차의사운드를담당하는 함수를 0.1f 마다 반복 호출
        }
        else if (!SoundsUse)//사운드를 사용하지 않을때
        {
            if (CarEngineSound != null) // 차엔진사운드가 비어있지 않을때
            {
                CarEngineSound.Stop();//차엔진 사운드 정지
            }
            if (TireScreechSound != null)// 바퀴 끌리는 소리가 비어있지 않을때
            {
                TireScreechSound.Stop();//바퀴 끌리는 소리 정지
            }
        }

        if (!EffectsUse) //이펙트를 사용 안한다고 했을때
        {
            if (WheelFLMist != null)//왼쪽 앞 휠에 연기 이펙트가 들어있을시
            {        
                WheelFLMist.Stop(); // 자동차의 왼쪽 앞 안개 파티클을 멈춤
            }        
            if (WheelFRMist != null)//오른쪽 앞 휠에 연기 이펙트가 들어있을시
            {        
                WheelFRMist.Stop(); // 자동차의 왼쪽 앞 안개 파티클 멈춤
            }        
            if (WheelFLSkid != null)//왼쪽 앞 휠에 스키드마크 이펙트가 들어있을시
            {        
                WheelFLSkid.emitting = false;//왼쪽 앞 휠의 스키드마크를 비활성화
            }        
            if (WheelFRSkid != null)//오른쪽 앞 휠에 스키드 마크 이펙트가 들어있을시
            {        
                WheelFRSkid.emitting = false;//오른쪽 앞 휠의 스키드마크를 비활성화
            }
            if (WheelBLMist != null)//왼쪽 뒷 휠에 연기 이펙트가 들어있을시
            {
                WheelBLMist.Stop(); // 자동차의 왼쪽 뒤 안개 파티클을 멈춤
            }
            if (WheelBRMist != null)//오른쪽 뒷 휠에 연기 이펙트가 들어있을시
            {
                WheelBRMist.Stop(); // 자동차의 왼쪽 뒤 안개 파티클을 멈춤
            }
            if (WheelBLSkid != null)//왼쪽 뒷 휠에 스키드마크 이펙트가 들어있을시
            {
                WheelBLSkid.emitting = false;//왼쪽 뒷 휠의 스키드마크를 비활성화
            }
            if (WheelBRSkid != null)//오른쪽 뒷 휠에 스키드 마크 이펙트가 들어있을시
            {
                WheelBRSkid.emitting = false;//오른쪽 뒷 휠의 스키드마크를 비활성화
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //현재속도 저장 ( 1 m/s = 3.6 km/h )
        _localVelocity = _carRigidbody.velocity.magnitude;
        CarSpeed = _localVelocity * 3.6f;
        
        //현재 X의 로컬 속도를 저장 -> 현재 드리프트 중인지 확인하는데 사용
        _localVelocityX = transform.InverseTransformDirection(_carRigidbody.velocity).x;
        //현재 Z의로컬 속도를 저장 -> 현재 자동차가 전진중인지 후진중인지 확인하는데 사용
        _localVelocityZ = transform.InverseTransformDirection(_carRigidbody.velocity).z;


        /*
         전진 가속 활성화 조건
            브레이크 상태가 아닐것(핸드브레이크는 괜찮음)
            후진가속상태가 아닐것 ( 만약 후진가속상태일 경우 브레이크 후에 가속상태로 변경 )
         후진 가속 활성화 조건
            브레이크 상태가 아닐것(핸드브레이크는 괜찮음)
            전진가속상태가 아닐것 ( 만약 전진가속상태일 경우 브레이크 후에 가속상태로 변경 )
         감속 활성화 조건
            브레이크,핸드브레이크,가속,후진상태가 아닐때 속도가 0이 아니라면
         브레이크 활성화 비활성화 조건
            전진중 후진 활성화,후진중 전진 활성화
            바퀴의 RPM이 0이되면 비활성화
         */
        if(WheelFLCollider.rpm==0)  //바퀴의 RPM이 0일때 (바퀴가멈춘상태)
        {
            UseBrake = false;       //브레이크상태 해제   
            UnBrake();              //브레이크토크 원상복귀
        }
        if(Input.GetKey(KeyCode.W))//W키를 누르고 있을때
        {
            if (UseBackAccele)          //후진이 작동중이면
            {
                UseBackAccele = false;  //후진해제
            }
            if (WheelFLCollider.rpm < 0) //RPM이 0보다 작으면(바퀴가 뒤로돌때)
            {
                UseBrake = true;        //브레이크 작동
            }
            if(!UseBrake && !UseAccele && !UseBackAccele) //브레이크,가속,후진 전부 작동중이 아닐때
            {
                UseAccele = true;                   //가속작동
            }
        }
        if(!Input.GetKey(KeyCode.W))            //W키를 안누르고있을때
        {
            UseAccele = false;                  //가속해제
        }

        if (Input.GetKey(KeyCode.S))//S키를 누르고 있을때
        {
            if (UseAccele)                  //가속이 작동중이면
            {
                UseAccele = false;          //가속해제
            }
            if (_localVelocityZ > 0)        //RPM이 0보다 크면(바퀴가 앞으로돌때)
            {
                UseBrake = true;            //브레이크 작동
            }
            if (!UseBrake && !UseAccele && !UseBackAccele) //브레이크,가속,후진 전부 작동중이 아닐때
            {
                UseBackAccele = true;                       //후진작동
            }
        }
        if (!Input.GetKey(KeyCode.S))              //S키를 땟을때
        {
            UseBackAccele = false;                      //후진해제
        }


        if(Input.GetKey(KeyCode.A)) //A키 눌렀을때
        {
            _left = true;           //좌회전 활성화
            _right = false;         //우회전 비활성화
        }
        if(Input.GetKey(KeyCode.D)) //D키 눌렀을때
        {
            _right = true;          //우회전 활성화
            _left = false;          //좌회전 비활성화
        }
        if(!(Input.GetKey(KeyCode.A)) && !(Input.GetKey(KeyCode.D)))
        {                       //A키,D키 둘다 안눌렀을때
            _right = false;     //우회전 비활성화
            _left = false;      //좌회전 비활성화
        }                       

        if(Input.GetKeyDown(KeyCode.Space))//스페이스 누를시
        {
            UseHandBrake = true;    //핸드브레이크 작동
        }
        if(Input.GetKeyUp(KeyCode.Space))//스페이스 땟을시
        {
            UseHandBrake = false;   //핸드브레이크 해제
            UnHandBrake();
        }


        if(_right)          //우회전
        {
            TurnRight();    //우회전 함수
        }
        if(_left)           //좌회전
        {
            TurnLeft();     //좌회전 함수
        }
        if((!_left)&&(!_right))     //좌,우 둘다 아닐때
        {
            ResetSteeringAngle();   //스티어링 복구 함수
        }

        if(UseAccele)   //가속작동중일때
        {
            Accele();   //가속함수
        }
        if(UseBackAccele)//후진작동중일때
        {
            BackAccele();//후진함수
        }
        //후진도,전진도,브레이크도,핸드브레이크도 아니면서 차의 속력이 0이 아닐때
        if(!UseAccele && !UseBackAccele && 0 < Mathf.Abs(_localVelocityZ))
        {
            Decele();   //감속실행
        }

        if(UseBrake)//브레이크 작동중일때
        {
            Brake();//브레이크함수
        }
        else if(UseHandBrake) //아니면 핸드브레이크가 작동중일때
        {
            HandBrake();//핸드브레이크함수
        }



        //현재 X축 로컬 속도가 2.5f보다 높거나, 브레이크또는 핸드브레이크 상태면서 차의 속력이 0이 아닐때 또는 전진상태인데 Z축 로컬속도가 음수이거나 후진상태일때 로컬속도가 양수일때
        
        if (Mathf.Abs(_localVelocityX) > 2.5f || ((UseBrake || UseHandBrake) && 1<Mathf.Abs(_localVelocity)) || (UseAccele && _localVelocityZ<0) || (UseBackAccele && 0 < _localVelocityZ)) 
        {
            Drifting = true;                //드리프트 상태 라고 판단
            Drift();                        //드리프트 상호작용 함수 실행
        }
        else                                //그렇지 않을 시
        {
            Drifting = false;               //드리프트 상태가 아니라고 판단
            Drift();                        //드리프트 상호작용 함수 실행
        }


        AnimateWheelMeshes();//바퀴 매쉬 애니메이션 함수
    }

    void AnimateWheelMeshes()//바퀴 매쉬 애니메이션 함수
    {
        // 각 바퀴의 휠콜라이더에서 바퀴의 위치값과 회전값을 받아서
        // 바퀴의 매쉬에 적용
        Quaternion FLWRotation;
        Vector3 FLWPosition;
        WheelFLCollider.GetWorldPose(out FLWPosition, out FLWRotation);
        WheelFLMesh.transform.position = FLWPosition;
        WheelFLMesh.transform.rotation = FLWRotation;

        Quaternion FRWRotation;
        Vector3 FRWPosition;
        WheelFRCollider.GetWorldPose(out FRWPosition, out FRWRotation);
        WheelFRMesh.transform.position = FRWPosition;
        WheelFRMesh.transform.rotation = FRWRotation;

        Quaternion RLWRotation;
        Vector3 RLWPosition;
        WheelBLCollider.GetWorldPose(out RLWPosition, out RLWRotation);
        WheelBLMesh.transform.position = RLWPosition;
        WheelBLMesh.transform.rotation = RLWRotation;

        Quaternion RRWRotation;
        Vector3 RRWPosition;
        WheelBRCollider.GetWorldPose(out RRWPosition, out RRWRotation);
        WheelBRMesh.transform.position = RRWPosition;
        WheelBRMesh.transform.rotation = RRWRotation;
    }
    public void Drift()                     //드리프트 상호작용 함수
    {
        if (EffectsUse)                      //이펙트를 사용하기로 했을 때
        {
            if (Drifting)
            {
                //현재 X축 로컬 속도가 2.5f보다 높거나, 브레이크상태일때
                if (Mathf.Abs(_localVelocityX) > 2.5f || UseBrake)
                {
                    if (WheelFLMist != null && WheelFRMist != null) //오른쪽과 왼쪽 앞바퀴 모두 연기 이펙트가 있을시
                    {
                        WheelFLMist.Play(); //연기 이펙트 실행
                        WheelFRMist.Play();
                    }
                    if (WheelFLSkid != null && WheelFRSkid != null) //오른쪽과 왼쪽 앞바퀴 모두 스키드 마크 이펙트가 있을때
                    {
                        WheelFRSkid.emitting = true;    //스키드마크 그리기
                        WheelFLSkid.emitting = true;
                    }
                }
                if (WheelBLMist != null && WheelBRMist != null) //오른쪽과 왼쪽 뒷바퀴 모두 연기 이펙트가 있을시
                {
                    WheelBLMist.Play(); //연기 이펙트 실행
                    WheelBRMist.Play();
                }
                if (WheelBLSkid != null && WheelBRSkid != null) //오른쪽과 왼쪽 뒷바퀴 모두 스키드 마크 이펙트가 있을때
                {
                    WheelBRSkid.emitting = true;    //스키드마크 그리기
                    WheelBLSkid.emitting = true;
                }
            }
            else
            {
                if (WheelFLMist != null && WheelFRMist != null)//오른쪽과 왼쪽 앞바퀴 모두 연기 이펙트가 있을시
                {
                    WheelFLMist.Stop(); //연기 이펙트 멈춤
                    WheelFRMist.Stop();
                }
                if (WheelFLSkid != null && WheelFRSkid != null) //오른쪽과 왼쪽 앞바퀴 모두 스키드 마크 이펙트가 있을때
                {
                    WheelFRSkid.emitting = false;    //스키드마크 그리지않기
                    WheelFLSkid.emitting = false;
                }
                if (WheelBLMist != null && WheelBRMist != null) //오른쪽과 왼쪽 뒷바퀴 모두 연기 이펙트가 있을시
                {
                    WheelBLMist.Stop(); //연기 이펙트 멈춤
                    WheelBRMist.Stop();
                }
                if (WheelBLSkid != null && WheelBRSkid != null) //오른쪽과 왼쪽 뒷바퀴 모두 스키드 마크 이펙트가 있을때
                {
                    WheelBRSkid.emitting = false;    //스키드마크 그리지않기
                    WheelBLSkid.emitting = false;
                }
            }
        }
    }

    /*
     휠콜라이더의 motorTorque는 바퀴의 RPM에 양수면 앞으로 음수면 뒤로 회전하게함
                    BrakeTorque는 바퀴의 RPM을 수치만큼 현재방향의 반대방향으로 적용시킴
     */


    public void Accele()
    {
        _throttleAxis = _throttleAxis + (Time.deltaTime * 3f);  //부드럽게 스로틀 올리기
        if(_throttleAxis > 1f)  //스로틀이 1보다 커지면
        {
            _throttleAxis = 1f; //1로고정
        }
        if (Mathf.RoundToInt(CarSpeed) < MaxSpeed)  //소수점을 반올림한 현재 차의 속도가 최고속도에 도달하지 못했을때
        {
            //모든 바퀴의 모터 토크에 차의 가속도와 스로틀상태에 따른 양의 수를 적용한다.
            WheelFLCollider.motorTorque = (Acceleration * 50f) * _throttleAxis;
            WheelFRCollider.motorTorque = (Acceleration * 50f) * _throttleAxis;
            if (!UseHandBrake)//핸드브레이크 일때는 뒷바퀴는 제외
            {
                WheelBLCollider.motorTorque = (Acceleration * 50f) * _throttleAxis;
                WheelBRCollider.motorTorque = (Acceleration * 50f) * _throttleAxis;
            }
        }
        else    //소수점을 반올림한 현재 차의 속도가 최고속도에 다다르면
        {
            //모든 바퀴의 모터토크에 0을 적용한다.
            WheelFLCollider.motorTorque = 0;
            WheelFRCollider.motorTorque = 0;
            WheelBLCollider.motorTorque = 0;
            WheelBRCollider.motorTorque = 0;
        }
    }

    public void Decele()    //감속함수
    {
        //스로틀을 자연스럽게 0으로 내리기
        if (_throttleAxis != 0f)                                        //스로틀이 0이 아닐때
        {
            if (_throttleAxis > 0f)                                     //스로들이 0보다 크면
            {
                _throttleAxis = _throttleAxis - (Time.deltaTime * 10f); //스로틀을 부드럽게 0을 향해 내리기
            }
            else if (_throttleAxis < 0f)                                //스로틀이 0보다 작으면
            {
                _throttleAxis = _throttleAxis + (Time.deltaTime * 10f); //스로틀을 부드럽게 0을 향해 올리기
            }
            if (Mathf.Abs(_throttleAxis) < 0.15f)                       //스로틀의 절댓값이 0.15f보다 작아졌을때
            {
                _throttleAxis = 0f;                                     //스로틀에 0을 적용
            }
        }
        //차의 속도를 천천히 감속
        _carRigidbody.velocity = _carRigidbody.velocity * (1f / (1f + (0.001f * Deceleration)));
        //모든 바퀴의 모터 토크를 0으로 적용
        WheelFLCollider.motorTorque = 0;
        WheelFRCollider.motorTorque = 0;
        WheelBLCollider.motorTorque = 0;
        WheelBRCollider.motorTorque = 0;

        if (_carRigidbody.velocity.magnitude < 0.25f)   //자동차의 속도가 0.25f보다 느리면
        {
            _carRigidbody.velocity = Vector3.zero;      //자동차를 완전히 멈춤
        }
    }

    public void BackAccele() //후진함수
    {
        _throttleAxis = _throttleAxis - (Time.deltaTime * 3f);  //부드럽게 스로틀 내리기
        if (_throttleAxis < -1f)    //스로틀이 -1보다 작아지면
        {
            _throttleAxis = -1f;    //-1로 고정
        }
        if (Mathf.Abs(Mathf.RoundToInt(CarSpeed)) < MaxBackSpeed)   //소수점을 반올림한 현재 차의 후진속도가 후진최고속도에 도달하지 못했을때
        {
            //모든 바퀴의 모터 토크에 차의 가속도와 스로틀상태에 따른 음의 수를 적용한다.
            WheelFLCollider.motorTorque = (Acceleration * 50f) * _throttleAxis;
            WheelFRCollider.motorTorque = (Acceleration * 50f) * _throttleAxis;
            if (!UseHandBrake)//핸드브레이크 일때는 뒷바퀴는 제외
            {
                WheelBLCollider.motorTorque = (Acceleration * 50f) * _throttleAxis;
                WheelBRCollider.motorTorque = (Acceleration * 50f) * _throttleAxis;
            }
        }
        else    //소수점을 반올림한 현재 차의 후진속도가 후진최고속도에 다다르면
        {
            //모든 바퀴의 모터토크에 0을 적용한다.
            WheelFLCollider.motorTorque = 0;
            WheelFRCollider.motorTorque = 0;
            WheelBLCollider.motorTorque = 0;
            WheelBRCollider.motorTorque = 0;
        }
    }

    public void TurnLeft()      //좌회전 함수
    {
        //스티어링 상태를 스티어링 속도에 따라 부드럽게 내리기
        _steeringAxis = _steeringAxis - (Time.deltaTime * 10f * SteeringSpeed);
        if (_steeringAxis < -1f)//스티어링 상태가 -1보다 작아지면
        {
            _steeringAxis = -1f;//스티어링 상태를 -1로 고정
        }
        //스티어링 상태 * 최대 스티어링 각도 = 목표 스티어링 각도
        float steering = _steeringAxis * MaxSteering;
        //현재 각 앞바퀴의 스티어링 각도를 목표 스티어링 각도를 향해 스티어링 속도에 따라 선형 보간을 통해 부드럽게 회전
        WheelFLCollider.steerAngle = Mathf.Lerp(WheelFLCollider.steerAngle, steering, SteeringSpeed);
        WheelFRCollider.steerAngle = Mathf.Lerp(WheelFLCollider.steerAngle, steering, SteeringSpeed);
    }

    public void TurnRight()      //우회전 함수
    {
        //스티어링 상태를 스티어링 속도에 따라 부드럽게 올리기
        _steeringAxis = _steeringAxis + (Time.deltaTime * 10f * SteeringSpeed);
        if (_steeringAxis > 1f)//스티어링 상태가 1보다 커지면
        {
            _steeringAxis = 1f;//스티어링 상태를 1로 고정
        }
        //스티어링 상태 * 최대 스티어링 각도 = 목표 스티어링 각도
        float steering = _steeringAxis * MaxSteering;
        //현재 각 앞바퀴의 스티어링 각도를 목표 스티어링 각도를 향해 스티어링 속도에 따라 선형 보간을 통해 부드럽게 회전
        WheelFLCollider.steerAngle = Mathf.Lerp(WheelFLCollider.steerAngle, steering, SteeringSpeed);
        WheelFRCollider.steerAngle = Mathf.Lerp(WheelFLCollider.steerAngle, steering, SteeringSpeed);
    }

    public void ResetSteeringAngle()    //스티어링 복구
    {
        if (_steeringAxis < 0f) //스티어링 상태가 0보다 작을때
        {
            _steeringAxis = _steeringAxis + (Time.deltaTime * 10f * SteeringSpeed);//스티어링상태를 부드럽게 올리기
        }
        else if (_steeringAxis > 0f)//스티어링 상태가 0보다 클때
        {
            _steeringAxis = _steeringAxis - (Time.deltaTime * 10f * SteeringSpeed);//스티어링상태를 부드럽게 내리기
        }
        if (Mathf.Abs(WheelFLCollider.steerAngle) < 1f)//바퀴의 스티어링 각도가 1f보다 작을때
        {
            _steeringAxis = 0f;//스티어링 상태 0적용
        }
        //목표 스티어링 각도 = 스티어링 상태 * 최대 스티어링 각도
        float steeringAngle = _steeringAxis * MaxSteering; 
        //현재 각 앞바퀴의 스티어링 각도를 목표 스티어링 각도를 향해 스티어링 속도에 따라 선형 보간을 통해 부드럽게 회전
        WheelFLCollider.steerAngle = Mathf.Lerp(WheelFLCollider.steerAngle, steeringAngle, SteeringSpeed);
        WheelFRCollider.steerAngle = Mathf.Lerp(WheelFRCollider.steerAngle, steeringAngle, SteeringSpeed);
    }
    public void HandBrake()//핸드브레이크함수
    {
        //모든 뒷바퀴의 모터토크에 0을 적용한다.
        WheelBLCollider.motorTorque = 0;
        WheelBRCollider.motorTorque = 0;
        //모든 뒷바퀴의 브레이크토크에 브레이크파워를 적용
        WheelBLCollider.brakeTorque = BreakPower;
        WheelBRCollider.brakeTorque = BreakPower;


        //뒷바퀴의 접지력 약화
        _wheelBLFriction.extremumSlip = _wheelBLExtremumSlip*5f;
        WheelBLCollider.sidewaysFriction = _wheelBLFriction;
        _wheelBRFriction.extremumSlip = _wheelBRExtremumSlip*5f;
        WheelBRCollider.sidewaysFriction = _wheelBRFriction;
    }
    public void UnHandBrake()//핸드브레이크함수
    {

        //모든 뒷바퀴의 브레이크토크에 0 적용
        WheelBLCollider.brakeTorque = 0;
        WheelBRCollider.brakeTorque = 0;


        //뒷바퀴의 접지 복구
        _wheelBLFriction.extremumSlip = _wheelBLExtremumSlip;
        WheelBLCollider.sidewaysFriction = _wheelBLFriction;
        _wheelBRFriction.extremumSlip = _wheelBRExtremumSlip;
        WheelBRCollider.sidewaysFriction = _wheelBRFriction;
    }
    public void Brake() //브레이크 함수
    {
        //모든 바퀴의 모터토크에 0을 적용한다.
        WheelFLCollider.motorTorque = 0;
        WheelFRCollider.motorTorque = 0;
        WheelBLCollider.motorTorque = 0;
        WheelBRCollider.motorTorque = 0;
        //모든 바퀴의 브레이크토크에 브레이크파워를 적용
        WheelFLCollider.brakeTorque = BreakPower;
        WheelFRCollider.brakeTorque = BreakPower;
        WheelBLCollider.brakeTorque = BreakPower;
        WheelBRCollider.brakeTorque = BreakPower;
    }
    public void UnBrake() //브레이크 함수
    {
        //모든 바퀴의 브레이크토크에 0을 적용
        WheelFLCollider.brakeTorque = 0;
        WheelFRCollider.brakeTorque = 0;
        WheelBLCollider.brakeTorque = 0;
        WheelBRCollider.brakeTorque = 0;
    }

    public void CarSounds()// 자동차 소리 함수
    {
        if (SoundsUse) //소리를 사용할때
        {
            if (CarEngineSound != null) //차의 엔진사운드가 null이 아닐때
            {
                float engineSoundPitch = _initialCarEngineSoundPitch + (Mathf.Abs(_carRigidbody.velocity.magnitude) / 25f); //엔진 사운드의 초기피치에 차의 속도만큼 값을 더한 값 = 현재 엔진의 사운드 피치
                CarEngineSound.pitch = engineSoundPitch; // 현재 엔진의 사운드 피치값 적용
            }
            if (TireScreechSound != null)//타이어 쓸리는 소리가 null이 아닐때
            {
                if (Drifting) //현재 드리프트 중일때
                {
                    if (!TireScreechSound.isPlaying)//타이어 쓸리는 소리가 플레이중이 아닐때
                    {
                        TireScreechSound.Play(); // 타이어 쓸리는 소리 플레이
                    }
                }
                else if (!Drifting) //현재 드리프트 중이 안닐때
                {
                    TireScreechSound.Stop(); // 타이어 쓸리는 소리 멈춤
                }
            }
        }
        else if (!SoundsUse) // 소리 사용 안할때
        {
            if (CarEngineSound != null && CarEngineSound.isPlaying) //엔진사운드가 null이 아니면서 플레이중일때
            {
                CarEngineSound.Stop(); //엔진소리 멈춤
            }
            if (TireScreechSound != null && TireScreechSound.isPlaying) //타이어 쓸리는 소리가 null이 아니면서 플레이중일때
            {
                TireScreechSound.Stop();//타이어 쓸리는 소리 멈춤
            }
        }
    }


}
