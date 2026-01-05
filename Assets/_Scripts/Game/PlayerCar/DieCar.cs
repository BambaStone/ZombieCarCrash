using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCar : MonoBehaviour
{
    public GameObject Smoke;
    public GameObject Car;
    private void OnEnable()
    {
        StartCoroutine(ActiveSmoke());
        StartCoroutine(ReActiveCar());
    }

    IEnumerator ActiveSmoke()
    {
        yield return new WaitForSeconds(2f);
        Smoke.SetActive(true);
    }

    IEnumerator ReActiveCar()
    {
        yield return new WaitForSeconds(10f);
        Car.transform.position = Vector3.zero;
        Car.transform.rotation = Quaternion.identity;
        Car.GetComponent<PlayerController>().HP = Car.GetComponent<PlayerController>().MAXHP;
        StatusManager.Instance.WorldCoin = StatusManager.Instance.WorldCoin / 2;
        Car.SetActive(true);
        Smoke.SetActive(false);
        gameObject.SetActive(false);
    }
}
