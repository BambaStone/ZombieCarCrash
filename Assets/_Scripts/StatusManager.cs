using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    #region Singleton
    private static StatusManager s_instance = null;
    public static StatusManager Instance
    {
        get
        {
            if (s_instance == null) return null;
            return s_instance;
        }
    }
    private void Awake()
    {
        if(s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public int WeaponNum=0;
    public int WeaponDamage=0;
    public int MaxSpeed = 0;
    public int MaxAccele = 0;
    public int MaxHP = 0;
    public int HPRegen = 0;
    public int CoinTank = 0;
    public int WorldCoin = 0;
    public int NextUpgradePrice = 10;
    public int Score=0;
    public int SaveNum = 0;
    public List<int> StageOpen;
    public int MaxStage = 2;

    private void Start()
    {

        StageOpen.Clear();
        for (int i = 0; i < MaxStage; i++)
        {
            StageOpen.Add(0);
        }
    }
    public void NewGame()
    {
        WeaponNum    =    0;
        WeaponDamage =    0;
        MaxSpeed     =    0;
        MaxAccele    =    0;
        MaxHP        =    0;
        HPRegen      =    0;
        CoinTank     =    0;
        WorldCoin    =    0;
        Score =           0;
        NextUpgradePrice = 10;
        for(int i=0;i<MaxStage; i++)
        {
            StageOpen[i] = 0;
        }
        SaveGame();
    }
    public void LoadGame()
    {
        WeaponNum    = PlayerPrefs.GetInt("WeaponNum" + SaveNum, 0);
        WeaponDamage = PlayerPrefs.GetInt("WeaponDamage" + SaveNum, 0);
        MaxSpeed     = PlayerPrefs.GetInt("MaxSpeed" + SaveNum, 0);
        MaxAccele    = PlayerPrefs.GetInt("MaxAccele" + SaveNum, 0);
        MaxHP        = PlayerPrefs.GetInt("MaxHP" + SaveNum, 0);
        HPRegen      = PlayerPrefs.GetInt("HPRegen" + SaveNum, 0);
        CoinTank     = PlayerPrefs.GetInt("CoinTank" + SaveNum, 0);
        WorldCoin    = PlayerPrefs.GetInt("WorldCoin" + SaveNum, 0);
        NextUpgradePrice = PlayerPrefs.GetInt("NextUpgradePrice" + SaveNum, 0);
        Score = PlayerPrefs.GetInt("Score" + SaveNum, 0);
        
        for (int i = 0; i < MaxStage; i++)
        {
            StageOpen[i]=PlayerPrefs.GetInt("StageOpen" + i + SaveNum, 0);
        }
        
    }
    public void SaveGame()
    {
        PlayerPrefs.SetInt("WeaponNum" + SaveNum, WeaponNum);
        PlayerPrefs.SetInt("WeaponDamage" + SaveNum, WeaponDamage);
        PlayerPrefs.SetInt("MaxSpeed" + SaveNum, MaxSpeed);
        PlayerPrefs.SetInt("MaxAccele" + SaveNum, MaxAccele);
        PlayerPrefs.SetInt("MaxHP" + SaveNum, MaxHP);
        PlayerPrefs.SetInt("HPRegen" + SaveNum, HPRegen);
        PlayerPrefs.SetInt("CoinTank" + SaveNum, CoinTank);
        PlayerPrefs.SetInt("WorldCoin" + SaveNum, WorldCoin);
        PlayerPrefs.SetInt("Score" + SaveNum, Score);
        PlayerPrefs.SetInt("NextUpgradePrice" + SaveNum, NextUpgradePrice);
        for (int i = 0; i < MaxStage; i++)
        {
            PlayerPrefs.SetInt("StageOpen"+i + SaveNum, StageOpen[i]);
        }
    }

}
