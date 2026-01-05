using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerController : MonoBehaviour
{
    SimpleCarController _car;
    public TMP_Text CarHPText;
    public Image CarHPImage;
    public TMP_Text CoinText;
    public Image CoinImage;
    public float HP = 100;
    public float MAXHP = 100;
    public float ReHP = 1f;
    public List<GameObject> Weapons;
    public List<GameObject> WeaponsEffect;
    public List<GameObject> Effectrecycling;
    public int WeaponNums = 3;
    public int Coin = 0;
    public int MaxCoin = 100;
    public int Energy=0;
    public int WeaponDamage = 0;
    public GameObject WeaponEffects;
    public GameObject CoinPrefab;
    public GameObject CoinTank;
    public List<GameObject> CoinList;
    public GameObject DropCoinSpawner;
    public GameObject UpgradeMenu;
    public GameObject DieCar;
    public bool OnStage = false;

    public GameObject ThornBumperSound;
    public List<GameObject> ThornBumperSoundrecycling;


    private float _thornBumperSoundDelay = 0.5f;
    private int _zombieKill = 0;
    private bool InputGold = false;
    // Start is called before the first frame update
    void Start()
    {
        _car = GetComponent<SimpleCarController>();
        WeaponDamage = StatusManager.Instance.WeaponDamage;
        _zombieKill = StatusManager.Instance.Score;
        WeaponNums = StatusManager.Instance.WeaponNum;
        Weapons[WeaponNums].SetActive(true);
        _car.MaxSpeed =100+ StatusManager.Instance.MaxSpeed * 10;
        _car.MaxBackSpeed = _car.MaxSpeed / 2;
        _car.Acceleration = 4 + StatusManager.Instance.MaxAccele;
        MAXHP = 100+ StatusManager.Instance.MaxHP*10;
        HP = MAXHP;
        CarHPText.text = (int)HP + " / " + MAXHP;
        CarHPImage.fillAmount = HP / MAXHP;
        ReHP = 0.1f + StatusManager.Instance.HPRegen * 0.1f;
        MaxCoin = 100 + StatusManager.Instance.CoinTank * 10;
        CoinText.text = Coin + " / " + MaxCoin;
        CoinImage.fillAmount = (float)Coin / MaxCoin;
    }

    private void OnEnable()
    {
        Coin = 0;
        for (int i = 0; i < CoinList.Count; i++)
        {
            if (CoinList[i].gameObject.activeSelf)
            {
                CoinList[i].SetActive(false);
            }
        }
        CarHPText.text = (int)HP + " / " + MAXHP;
        CarHPImage.fillAmount = HP / MAXHP;
        CoinText.text = Coin + " / " + MaxCoin;
        CoinImage.fillAmount = (float)Coin / MaxCoin;
    }


    private void FixedUpdate()
    {
        if (HP < MAXHP)
        {
            HP = HP + ReHP * Time.deltaTime;
            if (MAXHP < HP)
            {
                HP = MAXHP;
            }
            CarHPText.text = (int)HP + " / " + MAXHP;
            CarHPImage.fillAmount = HP / MAXHP;
        }
    }
    public void WeaponChange()
    {
        Weapons[WeaponNums].SetActive(false);
        if (WeaponNums==0)
        {
            for (int i = 0; i < ThornBumperSoundrecycling.Count; i++)
            {
                Destroy(ThornBumperSoundrecycling[i]);
            }
            ThornBumperSoundrecycling.Clear();
        }
            
        WeaponNums = StatusManager.Instance.WeaponNum;
        Weapons[WeaponNums].SetActive(true);
        for(int i=0;i<Effectrecycling.Count;i++)
        {
            Destroy(Effectrecycling[i]);
        }
        Effectrecycling.Clear();
        

    }
    public void PlusDamage()
    {
        WeaponDamage = StatusManager.Instance.WeaponDamage;
    }
    public void PluseMaxSpeed()
    {
        _car.MaxSpeed = 100 + StatusManager.Instance.MaxSpeed * 10;
        _car.MaxBackSpeed = _car.MaxSpeed / 2;
    }
    public void PlusMaxAccele()
    {
        _car.Acceleration = 4 + StatusManager.Instance.MaxAccele;
    }

    public void PlusMaxHP()
    {
        MAXHP = StatusManager.Instance.MaxHP*10+100;
        HP = MAXHP;
        CarHPText.text = (int)HP + " / " + MAXHP;
        CarHPImage.fillAmount = HP / MAXHP;
    }

    public void PlusHPRegen()
    {
        ReHP = 1 + StatusManager.Instance.HPRegen * 0.1f;
    }

    public void PlusMaxCoin()
    {
        MaxCoin = 100 + StatusManager.Instance.CoinTank * 10;
        CoinText.text = Coin + " / " + MaxCoin;
        CoinImage.fillAmount = (float)Coin / MaxCoin;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (0 < Coin)
        {
            if (other.CompareTag("InputGold"))
            {
                if (!InputGold)
                {
                    InputGold = true;
                    StartCoroutine(InputGolds());
                }
            }
        }
        if(other.CompareTag("CarUpgrade"))
        {
            UpgradeMenu.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InputGold"))
        {
            InputGold = false;
        }
        if (other.CompareTag("CarUpgrade"))
        {
            UpgradeMenu.SetActive(false);
        }
    }
    IEnumerator InputGolds()
    {
        yield return new WaitForSeconds(0.01f);
        if (InputGold)
        {
            if (0 < Coin)
            {
                MinusCoin();
                StartCoroutine(InputGolds());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Zombie zom = collision.gameObject.GetComponent<Zombie>();
            if (collision.contacts[0].thisCollider.CompareTag("Car"))
            {
                if (_car.CarSpeed >= 10)
                {
                    zom.CrashCar(gameObject, _car.CarSpeed);
                }
            }
            if (collision.contacts[0].thisCollider.CompareTag("ThornBumper"))
            {
                OnThornBumperSound(collision.contacts[0].point);
            }
            if (collision.contacts[0].thisCollider.CompareTag("CirCularSaw"))
            {
                OnEffect(collision.contacts[0].point);
                Vector3 direction = new Vector3(collision.contacts[0].normal.x * -1, Mathf.Abs(collision.contacts[0].normal.y)*5f, collision.contacts[0].normal.z*-1) ;
                zom.CrashCirCularSaw(direction, 10+ WeaponDamage*10);
            }
            if(collision.contacts[0].thisCollider.CompareTag("PowerBumper"))
            {
                OnEffect(collision.contacts[0].point);
                Vector3 direction = new Vector3(collision.contacts[0].normal.x * -1, Mathf.Abs(collision.contacts[0].normal.y), collision.contacts[0].normal.z * -1);
                zom.CrashPowerBumper(direction, 20 + WeaponDamage*20);
            }
        }
    }

    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Zombie zom = collision.gameObject.GetComponent<Zombie>();
            
            if (collision.contacts[0].thisCollider.CompareTag("ThornBumper"))
            {
                OnEffect(collision.contacts[0].point);
                zom.CrashThornBumper(transform.forward,10+ WeaponDamage*10);
            }
            if (collision.contacts[0].thisCollider.CompareTag("Car"))
            {
                HP = HP - collision.gameObject.GetComponent<Zombie>().Damage * Time.deltaTime;
                CarHPText.text = (int)HP + " / " + MAXHP;
                CarHPImage.fillAmount = HP / MAXHP;
                zom.GetComponent<Rigidbody>().AddForce(zom.transform.forward * -1, ForceMode.Impulse);
                if(HP<=0)
                {
                    YouDie();
                }
            }
        }
    }


    void OnEffect(Vector3 pos)
    {
        bool succes = false;
        for (int i = 0; i < Effectrecycling.Count; i++)
        {
            if (!(Effectrecycling[i].gameObject.activeSelf))
            {
                Effectrecycling[i].transform.position = pos;
                Effectrecycling[i].SetActive(true);
                succes = true;
                break;
            }
        }
        if (!succes)
        {
            Effectrecycling.Add(Instantiate(WeaponsEffect[WeaponNums], pos, Quaternion.identity));
            Effectrecycling[Effectrecycling.Count - 1].transform.parent = WeaponEffects.transform;
        }
    }

    void OnThornBumperSound(Vector3 pos)
    {
        _thornBumperSoundDelay = _thornBumperSoundDelay + Time.deltaTime;
        if (0.2f<_thornBumperSoundDelay)
        {

            _thornBumperSoundDelay = 0;
            bool succes = false;
            for (int i = 0; i < ThornBumperSoundrecycling.Count; i++)
            {
                if (!(ThornBumperSoundrecycling[i].gameObject.activeSelf))
                {
                    ThornBumperSoundrecycling[i].transform.position = pos;
                    ThornBumperSoundrecycling[i].SetActive(true);
                    succes = true;
                    break;
                }
            }
            if (!succes)
            {
                ThornBumperSoundrecycling.Add(Instantiate(ThornBumperSound, pos, Quaternion.identity));
                ThornBumperSoundrecycling[ThornBumperSoundrecycling.Count - 1].transform.parent = WeaponEffects.transform;
            }
        }
    }

    public void PlusCoin(Vector3 pos)
    {
        bool succes = false;
        for (int i = 0; i < CoinList.Count; i++)
        {
            if (!(CoinList[i].gameObject.activeSelf))
            {
                CoinList[i].transform.position = pos;
                CoinList[i].SetActive(true);
                succes = true;
                break;
            }
        }
        if (!succes)
        {
            CoinList.Add(Instantiate(CoinPrefab, pos, Quaternion.identity));
            CoinList[CoinList.Count - 1].transform.parent = CoinTank.transform;
        }
        Coin++;
        CoinText.text = Coin + " / " + MaxCoin;
        CoinImage.fillAmount = (float)Coin / MaxCoin;
    }
    void MinusCoin()
    {
        for(int i=0;i<CoinList.Count;i++)
        {
            if(CoinList[i].gameObject.activeSelf)
            {
                DropCoinSpawner.GetComponent<DropCoinSpawner>().CoinInput(CoinList[i].transform.position);
                CoinList[i].SetActive(false);
                break;
            }
        }
        Coin--;
        CoinText.text = Coin + " / " + MaxCoin;
        CoinImage.fillAmount = (float)Coin / MaxCoin;
    }

    public void MinusCoinWithTarget(GameObject target)
    {
        for (int i = 0; i < CoinList.Count; i++)
        {
            if (CoinList[i].gameObject.activeSelf)
            {
                DropCoinSpawner.GetComponent<DropCoinSpawner>().CoinInputWithTarget(CoinList[i].transform.position,target);
                CoinList[i].SetActive(false);
                break;
            }
        }
        Coin--;
        CoinText.text = Coin + " / " + MaxCoin;
        CoinImage.fillAmount = (float)Coin / MaxCoin;
    }

    public void YouDie()
    {
        DieCar.transform.position = gameObject.transform.position;
        DieCar.transform.rotation = gameObject.transform.rotation;
        DieCar.SetActive(true);
        Coin = 0;
        for (int i = 0; i < CoinList.Count; i++)
        {
            if (CoinList[i].gameObject.activeSelf)
            {
                CoinList[i].SetActive(false);
            }
        }
        gameObject.SetActive(false);
    }

    
}
