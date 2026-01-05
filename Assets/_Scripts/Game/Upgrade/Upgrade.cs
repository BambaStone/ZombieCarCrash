using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Upgrade : MonoBehaviour
{
    public List<Image> Weapon;
    public TMP_Text Damage;
    public TMP_Text MaxSpeed;
    public TMP_Text MaxAccele;
    public TMP_Text MaxHP;
    public TMP_Text HPRegen;
    public TMP_Text CoinTank;
    public TMP_Text NextPrice;
    public GameObject Player;
    private PlayerController _PC;

    private void OnEnable()
    {
        _PC = Player.GetComponent<PlayerController>();
        for(int i=0;i<3;i++)
        {
            if(StatusManager.Instance.WeaponNum ==i)
            {
                Weapon[i].gameObject.SetActive(true);
            }
            else
            {
                Weapon[i].gameObject.SetActive(false);
            }
        }

        Damage.text = StatusManager.Instance.WeaponDamage+"";
        MaxSpeed.text = StatusManager.Instance.MaxSpeed+"";
        MaxAccele.text = StatusManager.Instance.MaxAccele + "";
        MaxHP.text = StatusManager.Instance.MaxHP + "";
        HPRegen.text = StatusManager.Instance.HPRegen + "";
        CoinTank.text = StatusManager.Instance.CoinTank + "";
        NextPrice.text = StatusManager.Instance.NextUpgradePrice + "";
    }

    public void WeaponButton()
    {
        switch(StatusManager.Instance.WeaponNum)
        {
            case 0:
                if (5 <= StatusManager.Instance.WeaponDamage)
                {
                    StatusManager.Instance.WeaponNum++;
                    _PC.WeaponChange();
                }
                break;
            case 1:
                if (10 <= StatusManager.Instance.WeaponDamage)
                {
                    StatusManager.Instance.WeaponNum++;
                    _PC.WeaponChange();
                }
                break;
            case 2:
                StatusManager.Instance.WeaponNum = 0;
                _PC.WeaponChange();
                break;
        }
        for (int i = 0; i < 3; i++)
        {
            if (StatusManager.Instance.WeaponNum == i)
            {
                Weapon[i].gameObject.SetActive(true);
            }
            else
            {
                Weapon[i].gameObject.SetActive(false);
            }
        }

    }
    public void DamageButton()
    {
        if (StatusManager.Instance.NextUpgradePrice <= StatusManager.Instance.WorldCoin)
        {
            StatusManager.Instance.WorldCoin = StatusManager.Instance.WorldCoin - StatusManager.Instance.NextUpgradePrice;
            StatusManager.Instance.WeaponDamage++;
            StatusManager.Instance.NextUpgradePrice++;
            Damage.text = StatusManager.Instance.WeaponDamage + "";
            NextPrice.text = StatusManager.Instance.NextUpgradePrice + "";
            _PC.PlusDamage();
        }
    }
    public void MaxSpeedButton()
    {
        if (StatusManager.Instance.NextUpgradePrice <= StatusManager.Instance.WorldCoin)
        {
            StatusManager.Instance.WorldCoin = StatusManager.Instance.WorldCoin - StatusManager.Instance.NextUpgradePrice;
            StatusManager.Instance.MaxSpeed++;
            StatusManager.Instance.NextUpgradePrice++;
            MaxSpeed.text = StatusManager.Instance.MaxSpeed + "";
            NextPrice.text = StatusManager.Instance.NextUpgradePrice + "";
            _PC.PluseMaxSpeed();
        }
    }
    public void MaxAcceleButton()
    {
        if (StatusManager.Instance.NextUpgradePrice <= StatusManager.Instance.WorldCoin)
        {
            StatusManager.Instance.WorldCoin = StatusManager.Instance.WorldCoin - StatusManager.Instance.NextUpgradePrice;
            StatusManager.Instance.MaxAccele++;
            StatusManager.Instance.NextUpgradePrice++;
            MaxAccele.text = StatusManager.Instance.MaxAccele + "";
            NextPrice.text = StatusManager.Instance.NextUpgradePrice + "";
            _PC.PlusMaxAccele();
        }
    }
    public void MaxHPButton()
    {
        if (StatusManager.Instance.NextUpgradePrice <= StatusManager.Instance.WorldCoin)
        {
            StatusManager.Instance.WorldCoin = StatusManager.Instance.WorldCoin - StatusManager.Instance.NextUpgradePrice;
            StatusManager.Instance.MaxHP++;
            StatusManager.Instance.NextUpgradePrice++;
            MaxHP.text = StatusManager.Instance.MaxHP + "";
            NextPrice.text = StatusManager.Instance.NextUpgradePrice + "";
            
            _PC.PlusMaxHP();
        }
    }
    public void HPRegenButton()
    {
        if (StatusManager.Instance.NextUpgradePrice <= StatusManager.Instance.WorldCoin)
        {
            StatusManager.Instance.WorldCoin = StatusManager.Instance.WorldCoin - StatusManager.Instance.NextUpgradePrice;
            StatusManager.Instance.HPRegen++;
            StatusManager.Instance.NextUpgradePrice++;
            HPRegen.text = StatusManager.Instance.HPRegen + "";
            NextPrice.text = StatusManager.Instance.NextUpgradePrice + "";
            _PC.PlusHPRegen();
        }
    }
    public void CoinTankButton()
    {
        if (StatusManager.Instance.NextUpgradePrice <= StatusManager.Instance.WorldCoin)
        {
            StatusManager.Instance.WorldCoin = StatusManager.Instance.WorldCoin - StatusManager.Instance.NextUpgradePrice;
            StatusManager.Instance.CoinTank++;
            StatusManager.Instance.NextUpgradePrice++;
            CoinTank.text = StatusManager.Instance.CoinTank + "";
            NextPrice.text = StatusManager.Instance.NextUpgradePrice + "";
            _PC.PlusMaxCoin();
        }
    }

    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
}
