using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public float SpawnTime = 5f;
    public GameObject ZombiePrefabs;
    public List<GameObject> ZombieList;
    public GameObject Zombies;
    public GameObject Target;
    public int MaxZombie=100;
    public GameObject DropCoinSpawner;
    // Start is called before the first frame update
    /*
    void Start()
    {
        StartCoroutine(SpawnTimer());
    }
    */
    void OnEnable()
    {
        StartCoroutine(SpawnTimer());
    }

    IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(SpawnTime);
        ZombieSpawn();
        StartCoroutine(SpawnTimer());
    }

    public void UnActiveSelf()
    {
        for(int i = 0; i < ZombieList.Count; i++)
        {
            ZombieList[i].GetComponent<Zombie>().StopAllCoroutines();
            ZombieList[i].SetActive(false);
        }
        gameObject.SetActive(false);
    }
    void ZombieSpawn()
    {
        bool succes = false;
        for (int i = 0; i < ZombieList.Count; i++)
        {
            if (!(ZombieList[i].gameObject.activeSelf))
            {
                ZombieList[i].transform.position = transform.position;
                ZombieList[i].SetActive(true);
                //ZombieList[i].GetComponent<Zombie>().Spawn();
                succes = true;
                break;
            }
        }
        if (!succes && ZombieList.Count< MaxZombie)
        {
            ZombieList.Add(Instantiate(ZombiePrefabs, transform.position, transform.rotation));
            ZombieList[ZombieList.Count - 1].transform.parent = Zombies.transform;
            ZombieList[ZombieList.Count - 1].SetActive(true);
            ZombieList[ZombieList.Count - 1].GetComponent<Zombie>().FirstSpawn(Target, Random.Range(0, 21),DropCoinSpawner);
        }
    }

}
