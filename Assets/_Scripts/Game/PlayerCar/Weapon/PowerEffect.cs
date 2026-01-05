using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerEffect : MonoBehaviour//파워범퍼의 이펙트를 작동시키는 스크립트
{
    public Light PointLight;//파워범퍼 이펙트 안의 라이트 컴포넌트
    private int _weaponDamage = 0;//이펙트에 추가될 무기 데미지
    private bool big = true;//이펙트 크기 변경 방향
    private void OnEnable()//파워 범퍼 이펙트가 활성화 되었을 때
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);//파워범퍼 이펙트의 크기 초기화
        _weaponDamage = StatusManager.Instance.WeaponDamage;//파워범퍼 이펙트의 무기데미지 스테이터스매니저에서 받아오기
        big = true;//파워범퍼 이펙트 크기변경은 커지는 방향으로
        StartCoroutine(DoSmall());//크기변경을 작은방향으로 바꾸는 함수를 지연호출
    }

    private void OnCollisionEnter(Collision collision)//파워이펙트의 콜리전에 충돌체크가 생겼을때
    {
        if (collision.gameObject.CompareTag("Zombie"))//충돌된 오브젝트의 태그가 "Zombie"라면
        {
            //충돌된 좀비가 튕겨져 나갈 방향 설정
            Vector3 direction = new Vector3(collision.contacts[0].normal.x * -1, Mathf.Abs(collision.contacts[0].normal.y), collision.contacts[0].normal.z * -1);
            //좀비의 <Zombie>컴포넌트에서 파워범퍼 충돌 함수에 튕겨져나갈 방향과 데미지 전달
            collision.gameObject.GetComponent<Zombie>().CrashPowerBumper(direction, 20 + _weaponDamage * 20);
        }
    }
    private void FixedUpdate()//오브젝트가 활성화 되어 있는 동안 일정 주기로 반복 호출
    {
        if (big)//크기변경 방향이 커지는방향일때 (big == true)
        {
            //파워이펙트의 크기를 초당 25의 속도로 키우기
            transform.localScale = transform.localScale + (Vector3.one * 25 * Time.deltaTime);
        }
        else//크기변경 방향이 작아지는 방향일때 (big==false)
        {
            //파워이펙트의 크기를 초당 25의 속도로 작게 만들기
            transform.localScale = transform.localScale - (Vector3.one * 25 * Time.deltaTime);
        }
        //크기에 맞춰서 파워이펙트 안의 조명도 변경
        PointLight.range = transform.localScale.x*2;
        PointLight.intensity = transform.localScale.x+1;
    }
    IEnumerator DoSmall()//파워이펙트의 크기변경 방향을 작은쪽으로 만드는 함수
    {
        yield return new WaitForSeconds(0.2f);//0.2초 지연
        big = false;//크기변경 방향 작은쪽으로
        StartCoroutine(UnActive());//비활성화 함수 지연호출
    }
    IEnumerator UnActive()//비활성화 함수
    {
        yield return new WaitForSeconds(0.2f);//0.2초지연
        gameObject.SetActive(false);//파워이펙트 비활성화
    }
}
