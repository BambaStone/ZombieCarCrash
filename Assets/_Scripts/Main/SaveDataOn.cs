using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SaveDataOn : MonoBehaviour
{
    public GameObject Weapon0;
    public GameObject Weapon1;
    public GameObject Weapon2;

    public TMP_Text Coin;
    public TMP_Text ZombieKill;
    public TMP_Text Damage;
    public TMP_Text MaxSpeed;
    public TMP_Text MaxAccele;
    public TMP_Text MaxHP;
    public TMP_Text HPRegen;
    public TMP_Text CoinTank;

    private void OnEnable()
    {
        switch(StatusManager.Instance.WeaponNum)
        {
            case 0:
                Weapon0.SetActive(true);
                break;
            case 1:
                Weapon1.SetActive(true);
                break;
            case 2:
                Weapon2.SetActive(true);
                break;
        }
        Coin.text = StatusManager.Instance.WorldCoin+"";
        ZombieKill.text = StatusManager.Instance.Score + "";
        Damage.text = StatusManager.Instance.WeaponDamage + "";
        MaxSpeed.text = StatusManager.Instance.MaxSpeed + "";
        MaxAccele.text = StatusManager.Instance.MaxAccele + "";
        MaxHP.text = StatusManager.Instance.MaxHP + "";
        HPRegen.text = StatusManager.Instance.HPRegen + "";
        CoinTank.text = StatusManager.Instance.CoinTank + "";
    }

    private void OnDisable()
    {
        switch (StatusManager.Instance.WeaponNum)
        {
            case 0:
                Weapon0.SetActive(false);
                break;
            case 1:
                Weapon1.SetActive(false);
                break;
            case 2:
                Weapon2.SetActive(false);
                break;
        }
    }
}


