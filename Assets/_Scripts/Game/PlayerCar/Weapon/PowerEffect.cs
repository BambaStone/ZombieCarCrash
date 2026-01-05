using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerEffect : MonoBehaviour
{
    public Light PointLight;
    private int _weaponDamage = 0;
    private bool big = true;
    private void OnEnable()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        _weaponDamage = StatusManager.Instance.WeaponDamage;
        big = true;
        StartCoroutine(DoSmall());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Vector3 direction = new Vector3(collision.contacts[0].normal.x * -1, Mathf.Abs(collision.contacts[0].normal.y), collision.contacts[0].normal.z * -1);
            collision.gameObject.GetComponent<Zombie>().CrashPowerBumper(direction, 20 + _weaponDamage * 20);
        }
    }
    private void FixedUpdate()
    {
        if (big)
        {
            transform.localScale = transform.localScale + (Vector3.one * 25 * Time.deltaTime);
            
        }
        else
        {
            transform.localScale = transform.localScale - (Vector3.one * 25 * Time.deltaTime);
            
        }
        PointLight.range = transform.localScale.x*2;
        PointLight.intensity = transform.localScale.x+1;
    }
    IEnumerator DoSmall()
    {
        yield return new WaitForSeconds(0.2f);
        big = false;
        StartCoroutine(UnActive());
    }
    IEnumerator UnActive()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
}
