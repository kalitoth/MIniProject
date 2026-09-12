using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems; 


public class Skill_List : MonoBehaviour
{
    [SerializeField]
    AudioClip[] clip = new AudioClip[4];
     
    [SerializeField]
    GameObject _SkillObjectContainer;

    //프리팹
    [SerializeField]
    Bow _BowObject;
    [SerializeField]
    Fireball _FireballlObject;
    [SerializeField]
    MagicMissile _MagicMissileObject;

    #region 스킬 오브젝트 풀

    int _bowGen = 2;
    Queue<Bow> _bowQueue;
    List<Bow> _bowList;

    int _fireballGen = 2;
    Queue<Fireball> _fireballQueue;
    List<Fireball> _fireballList;

    int _magicMissilelGen = 10;
    Queue<MagicMissile> _magicMissileQueue;
    List<MagicMissile> _magicMissileList;


    void BowPoolReady()
    {

        for (int i = 0; i < _bowGen; i++)
        {
            Bow Object = Instantiate(_BowObject, _SkillObjectContainer.transform);

            _bowQueue.Enqueue(Object);
            Object.gameObject.SetActive(false);
        }
    }
    void FireballPoolReady()
    {

        for (int i = 0; i < _fireballGen; i++)
        {
            Fireball Object = Instantiate(_FireballlObject, _SkillObjectContainer.transform);

            _fireballQueue.Enqueue(Object);
            Object.gameObject.SetActive(false);
        }
    }
    void MagicMissilePoolReady()
    {

        for (int i = 0; i < _magicMissilelGen; i++)
        {
            MagicMissile Object = Instantiate(_MagicMissileObject, _SkillObjectContainer.transform);

            _magicMissileQueue.Enqueue(Object);
            Object.gameObject.SetActive(false);
        }
    }
    
    void BowReturn()
    {
        

        for(int i = _bowList.Count-1; i >= 0 ; i--)
        {
            if(_bowList[i] == null )
            {
                continue;
            }
            if (!_bowList[i].gameObject.activeSelf)
            {
                _bowQueue.Enqueue(_bowList[i]);
                _bowList.RemoveAt(i);
            }
        }
    }

    void FireballReturn()
    {
        for (int i = _fireballList.Count - 1; i >= 0; i--)
        {
            if (_fireballList[i] == null)
            {
                continue;
            }
            if (!_fireballList[i].gameObject.activeSelf)
            {
                _fireballQueue.Enqueue(_fireballList[i]);
                _fireballList.RemoveAt(i);
            }
        }
    }
    void MagicMissileReturn()
    {
        

        for (int i = _magicMissileList.Count - 1; i >= 0; i--)
        {
            if (_magicMissileList[i] == null)
            {
                continue;
            }
            if (!_magicMissileList[i].gameObject.activeSelf)
            {
                _magicMissileQueue.Enqueue(_magicMissileList[i]);
                _magicMissileList.RemoveAt(i);
            }
        }
    }
    #endregion

    Dictionary<int, Action<Player_Test,RaycastHit>> _skillList = new Dictionary<int, Action<Player_Test, RaycastHit>>();

    Color _initialColor = Color.white;
    Color _abledColor = Color.red;
    Color _ableColor = Color.blue;
     
    LayerMask _layerMaskUnit;

    RaycastHit[] _hitsArray = new RaycastHit[10]; 
    public Dictionary<int, Action<Player_Test, RaycastHit>> SkillList
    {
        get { return _skillList; }
    }

    private void Awake()
    {
        _bowQueue = new Queue<Bow>(_bowGen);
        _bowList = new List<Bow>(_bowGen);

        _fireballQueue = new Queue<Fireball>(_fireballGen);
        _fireballList = new List<Fireball>(_fireballGen);

        _magicMissileQueue = new Queue<MagicMissile>(_magicMissilelGen);
        _magicMissileList = new List<MagicMissile>(_magicMissilelGen);
         
        BowPoolReady();
        FireballPoolReady();
        MagicMissilePoolReady();
          
        _layerMaskUnit = 1 << LayerMask.NameToLayer("Monster") | 1 << LayerMask.NameToLayer("Player"); 

        //스킬 풀
        _skillList.Add(0, Sword);
        _skillList.Add(1, Bow);
        _skillList.Add(2, Fireball);
        _skillList.Add(3, Scroll);
        _skillList.Add(4, Jump);
    }

    private void Update()
    {
        BowReturn();
        FireballReturn();
        MagicMissileReturn();
    }
    //근접 공격은 구체 없다
    #region 스킬 목록

    //  1. 근접 단일 공격
    public void Sword(Player_Test player, RaycastHit hit)
    {
        float range = 3;
        float sqrRange = range * range;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            LineColorInitial(player);
           
            
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Monster"))
            {
                LineColorChangable(player, hit, sqrRange);

                if (player.UnitState.HasFlag(Unit_Test.State.Battle))
                {
                    if (player.UsingSkillNum == 0)
                    {
                        return;
                    }
                }

                if (Input.GetMouseButtonDown(0))
               {
                    
                    if ((hit.point - player.transform.position).sqrMagnitude <= sqrRange)
                    {
                        int damage = 3 + Mathf.RoundToInt((player.Strength - 10) * 0.5f);
                        Unit_Test monster = hit.collider.gameObject.GetComponent<Unit_Test>();
                        monster.TakeDamage(monster,damage);
                        player.UnitAudioSource.PlayOneShot(clip[0]);

                        Initialized(player);
                    }
               }
                    
            }
        }
        Debug.Log("Sword");
    }

    //  2. 원거리 단일 공격
    public void Bow(Player_Test player, RaycastHit hit)
    {
        float range = 20;
        float sqrRange = range * range;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            LineColorInitial(player);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Monster"))
            {
                LineColorChangable(player, hit, sqrRange);

                if (player.UnitState.HasFlag(Unit_Test.State.Battle))
                {
                    if (player.UsingSkillNum == 0)
                    {
                        return;
                    }
                }


                if (Input.GetMouseButtonDown(0))
                {

                    if ((hit.point - player.transform.position).sqrMagnitude <= sqrRange)
                    {
                        //Bow skillObject = Instantiate(_SkillObject, hit.point, player.transform.rotation).AddComponent<Bow>();
                        //skillObject.SkillDamageUnitStat(player);

                        Bow _skillObject = _bowQueue.Dequeue();
                        _skillObject.gameObject.SetActive(true);
                        _skillObject.gameObject.transform.position = hit.point;
                        _skillObject.SkillDamageUnitStat(player);
                        _bowList.Add(_skillObject);

                        player.UnitAudioSource.PlayOneShot(clip[1]);

                        Initialized(player);
                    }
                }

            }
        }
        Debug.Log("Bow");
    }
   //   3. 원거리 범위 공격
    public void Fireball(Player_Test player, RaycastHit hit)
    {
        float range = 15;
        float sqrRange = range * range;
        //float fierballRange = 3f;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            LineColorInitial(player);

            if (hit.collider != null)
            {
                LineColorChangable(player, hit, sqrRange);

                if (player.UnitState.HasFlag(Unit_Test.State.Battle))
                {
                    if (player.UsingSkillNum == 0)
                    {
                        return;
                    }
                }


                if (Input.GetMouseButtonDown(0))
                {

                    if ((hit.point - player.transform.position).sqrMagnitude <= sqrRange)
                    {
                        Debug.Log("여기 들어오니?111");
                    
                         
                        Debug.Log("여기 들어오니?222"); 
                        Debug.Log($"{_layerMaskUnit}");

                       //Fireball skillObject = Instantiate(_FireballlObject, hit.point, player.transform.rotation).AddComponent<Fireball>();
                       //
                       //skillObject.SkillDamageUnitStat(player);

                         
                        Fireball _skillObject = _fireballQueue.Dequeue();
                        _skillObject.gameObject.SetActive(true);
                        _skillObject.gameObject.transform.position = hit.point;
                        _skillObject.SkillDamageUnitStat(player);
                        _fireballList.Add(_skillObject);

                        player.UnitAudioSource.PlayOneShot(clip[2]);

                        Initialized(player);

                    }
                }

            }
        }

        Debug.Log("파이어볼");
    }
    //  4. 원거리 단일 스택 공격
    public void Scroll(Player_Test player, RaycastHit hit)
    {
        float range = 15;
        float sqrRange = range * range;
        

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            LineColorInitial(player);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Monster"))
            {
                LineColorChangable(player, hit, sqrRange);

                //배틀인 상태일때만
                if( player.UnitState.HasFlag(Unit_Test.State.Battle))
                {
                    if (player.UsingSkillNum == 0)
                    {
                        return;
                    }
                }
                
                if (Input.GetMouseButtonDown(0))
                {

                    if ((hit.point - player.transform.position).sqrMagnitude <= sqrRange)
                    {
                            Debug.Log($"스크롤 들어오나?1111");
                        
                        if (hit.point == null)
                        {
                            Debug.Log($"monster가 null");
                            return;
                        }
                        _hitsArray[player._skillNum] = hit;
                      
                        player._skillNum++;
                            Debug.Log($"스크롤 들어오나?2222");
                            Debug.Log($"{player._skillNum}");
                        if(player._skillNum == 3)
                        {
                           //for(int i = 0; i < player._skillNum; i++)
                           //{
                           //    
                           //    MagicMissile _skillObject = _magicMissileQueue.Dequeue();
                           //    _skillObject.gameObject.SetActive(true);
                           //    _skillObject.gameObject.transform.position = _hitsArray[i].point;
                           //    _skillObject.SkillDamageUnitStat(player);
                           //    _magicMissileList.Add(_skillObject);
                           //} 
                            StartCoroutine(MagicMissaileSound(player));

                            Initialized(player);
                            player._skillNum = 0;
                            
                        }
                        
                    }
                }

            }
        } 
        Debug.Log("Scroll");
    }
    public void Jump(Player_Test player, RaycastHit hit)
    {
        Debug.Log("Jump");
    }

    #endregion


    #region 함수 목록

    
    void LineColorInitial(Player_Test player)
    {
        if (player._lineRenderer.startColor != _initialColor)
        {
            player._lineRenderer.startColor = _initialColor;
        }
    }
    void LineColorChangable(Player_Test player, RaycastHit hit, float sqrRange)
    {
        if ((hit.point - player.transform.position).sqrMagnitude > sqrRange)
        {

            if (player._lineRenderer.startColor == _abledColor)
            {
                return;
            }
            player._lineRenderer.startColor = _abledColor;
        }
        else
        {
            if (player._lineRenderer.startColor != _ableColor)
            {
                player._lineRenderer.startColor = _ableColor;
            }

        }
    }

    IEnumerator MagicMissaileSound(Player_Test player)
    {
        
        for (int i = 0; i < 3; i++)
        {
            MagicMissile _skillObject = _magicMissileQueue.Dequeue();
            _skillObject.gameObject.SetActive(true);
            _skillObject.gameObject.transform.position = _hitsArray[i].point;
            _skillObject.SkillDamageUnitStat(player);
            _magicMissileList.Add(_skillObject);

            player.UnitAudioSource.PlayOneShot(clip[3]);
            //yield return null;
            yield return new WaitForSeconds(0.1f);
        }
       

    }
    void Initialized(Player_Test player)
    {
        player.Animator.Play("SkillActivate");
        //player._playerMoving.Animator.SetTrigger("TSkillActivate");
        player.Animator.SetBool("BSkillReady", false);

        player.UnitState &= ~Unit_Test.State.Skill;
        player.GetComponent<PlayerMoving>().enabled = true;
        player._lineRenderer.enabled = false;

        player.UsingSkillNum = 0;
    }

    #endregion
}
