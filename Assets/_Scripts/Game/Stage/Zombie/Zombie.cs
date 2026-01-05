using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public List<GameObject> Body;
    public GameObject Target;
    public float HP = 100f;
    public float Damage = 1f;
    public float Speed = 2f;
    public int DropCoinNum = 1;
    public GameObject DropCoinSpawner;
    public List<GameObject> ZombieSound;

    private bool _onGround=false;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private int _state = 0; // ป๓ลย  0:Idle  1:Run  2:FallDown 3:Death
    private bool _firstSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
        FirstSpawn(Target,0,DropCoinSpawner);
    }

    

    private void OnEnable()
    {
        if (!_firstSpawn)
        {
            Spawn();
        }
    }
    public void FirstSpawn(GameObject target, int num,GameObject dropCoinSpawner)
    {
        GetComponent<CapsuleCollider>().isTrigger = false;
        GameObject temp = Body[0];
        Body[0] = Body[num];
        Body[num] = temp;
        Body[0].SetActive(true);
        Target = target;
        DropCoinSpawner = dropCoinSpawner;
        _state = 0;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _firstSpawn = false;
        StartCoroutine(Run(1f));
        for (; 1 < Body.Count;)
        {
            Destroy(Body[1]);
            Body.RemoveAt(1);
        }
    }
    public void Spawn()
    {
        ChangeLayerDefault();
        GetComponent<CapsuleCollider>().isTrigger = false;
        _animator.SetBool("FallDown", true);
        _animator.SetBool("Run", false);
        _onGround = false;
        _rigidbody.velocity = new Vector3(0, 0, 0);
        _state = 2;
        HP = 100;
        StartCoroutine(LongFallDown());
        StartCoroutine(ZombieSoundOn());
    }
    private void FixedUpdate()
    {
        if (0 < HP)
        {
            if (_state == 1 && _onGround)
            {
                if (_animator.GetBool("FallDown"))
                    _animator.SetBool("FallDown", false);
                if (Target != null)
                {
                    Vector3 targetPos = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
                    transform.LookAt(targetPos);
                }
                _rigidbody.velocity = transform.forward * Speed;
            }
            if (_state == 2)
            {
                if (_onGround)
                {
                    StartCoroutine(StandUp(1f));
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            _onGround = false;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void CrashCirCularSaw(Vector3 dir, int damage)
    {
        if (0 < HP)
        {
            _state = 2;
            _onGround = false;
            HP = HP - damage;
            _animator.SetBool("FallDown", true);
            _animator.SetBool("Run", false);
            _rigidbody.AddForce(Vector3.up * 4f, ForceMode.Impulse);

            StartCoroutine(LongFallDown());
        }
        if (HP <= 0)
            if (_state != 3)
            {
                ZombieDead();
            }
    }

    public void CrashPowerBumper(Vector3 dir, int damage)
    {
        if (0 < HP)
        {
            _state = 2;
            _onGround = false;
            HP = HP - damage;
            _animator.SetBool("FallDown", true);
            _animator.SetBool("Run", false);
            _rigidbody.AddForce(Vector3.up * 3f, ForceMode.Impulse);
            _rigidbody.AddForce(dir * 5f, ForceMode.Impulse);
            StartCoroutine(LongFallDown());
        }
        if (HP <= 0)
            if (_state != 3)
            {
                ZombieDead();
            }
    }
    public void CrashThornBumper(Vector3 dir,int damage)
    {
        

        if (0 < HP)
        {
            _state = 0;
            _onGround = false;
            HP = HP - damage * (Time.deltaTime);
            _animator.SetBool("Run", false);
            _rigidbody.velocity = new Vector3(0, 0, 0);
            if (gameObject.activeSelf)
            {
                StartCoroutine(ShotStunDown());
            }
        }
        else if (HP <= 0)
            if (_state != 3)
            {
                _animator.SetBool("FallDown", true);
                ZombieDead();
            }
    }

    void ZombieDead()
    {
        _state = 3;
        ChangeLayerDeadZombie();
        StartCoroutine(UnActive());
    }
    public void CrashCar(GameObject Car, float CarSpeed)
    {
        HP = HP - CarSpeed * 0.1f;
        Vector3 bounceDirection = transform.forward * -1;
        bounceDirection.y = 0;
        _rigidbody.velocity = new Vector3(0, 0, 0);
        _rigidbody.AddForce(bounceDirection * (CarSpeed * 0.1f), ForceMode.Impulse);
        if (HP <= 0)
            if (_state != 3)
            {
                _animator.SetBool("FallDown", true);
                ZombieDead();
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false);
            if(_state==1)
                _state = 0;
            
            HP = HP - 25;
            _animator.SetBool("Run", false);
            Vector3 bounceDirection = transform.forward * -1;
            bounceDirection.y = 0;
            _rigidbody.velocity = new Vector3(0, 0, 0);
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
            _rigidbody.AddForce(bounceDirection , ForceMode.Impulse);
            StartCoroutine(Run(1f));
            if (HP <= 0)
                if (_state != 3)
                {
                    _animator.SetBool("FallDown", true);
                    //gameObject.tag = "DeadZombie";
                    _state = 3;
                    StartCoroutine(UnActive());
                }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(!_onGround)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _onGround = true;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            _onGround = true;
        }
    }
    
    IEnumerator ZombieSoundOn()
    {


        int random = Random.Range(10,120);
        yield return new WaitForSeconds(random);
        random = Random.Range(0, ZombieSound.Count);
        ZombieSound[random].SetActive(true);
        StartCoroutine(ZombieSoundOn());
    }
    IEnumerator Run(float time)
    {
        
        yield return new WaitForSeconds(time);
        _animator.SetBool("Run", true);
        _state = 1;
    }

    IEnumerator StandUp(float time)
    {
        yield return new WaitForSeconds(time);
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _animator.SetBool("FallDown", false);
        _state = 0;
        StartCoroutine(Run(1f));
    }

    IEnumerator LongFallDown()
    {
        yield return new WaitForSeconds(5f);
        if(_animator.GetBool("FallDown"))
        {
            StartCoroutine(StandUp(1f));
        }
        else { StartCoroutine(Run(1f)); }
    }
    IEnumerator ShotStunDown()
    {
        yield return new WaitForSeconds(0.5f);
        
            if (_animator.GetBool("FallDown"))
            {
                StartCoroutine(StandUp(1f));
            }
            else { StartCoroutine(Run(1f)); }
        
    }

    IEnumerator UnActive()
    {
        yield return new WaitForSeconds(1f);
        StatusManager.Instance.Score = StatusManager.Instance.Score + 1;
        for(int i=0;i<DropCoinNum;i++)
        {
            DropCoinSpawner.GetComponent<DropCoinSpawner>().CoinDrop(transform.position);
        }
        gameObject.SetActive(false);
    }

    void ChangeLayerDeadZombie()
    {
        int layerIDDeadZombie = LayerMask.NameToLayer("DeadZombie");

        if (gameObject.layer != layerIDDeadZombie)
        {
            gameObject.layer = layerIDDeadZombie;
        }
    }
    void ChangeLayerDefault()
    {
        int layerIDDefault = LayerMask.NameToLayer("Default");
        if (gameObject.layer != layerIDDefault)
        {
            gameObject.layer = layerIDDefault;
        }
    }
    
    
}
