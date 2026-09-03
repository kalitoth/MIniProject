using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems; 


public class Skill_List : MonoBehaviour
{
     

    Dictionary<int, Action<Player_Test,RaycastHit>> _skillList = new Dictionary<int, Action<Player_Test, RaycastHit>>();

    Color _initialColor = Color.white;
    Color _abledColor = Color.red;
    Color _ableColor = Color.blue;

    Monster_Test[] _monster = new Monster_Test[20];
    //List<Monster_Test> _monster = new List<Monster_Test>(20);
    Collider[] _colliders = new Collider[20];
    LayerMask _layerMaskUnit;

     
    public Dictionary<int, Action<Player_Test, RaycastHit>> SkillList
    {
        get { return _skillList; }
    }

    private void Awake()
    {
        _layerMaskUnit = 1 << LayerMask.NameToLayer("Monster") | 1 << LayerMask.NameToLayer("Player"); 

        _skillList.Add(0, Sword);
        _skillList.Add(1, Bow);
        _skillList.Add(2, Fireball);
        _skillList.Add(3, Scroll);
        _skillList.Add(4, Jump);
    }
 
    #region 스킬 목록

    //  1. 근접 단일 공격
    public void Sword(Player_Test player, RaycastHit hit)
    {
        float range = 2;
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
                       Monster_Test monster = hit.collider.gameObject.GetComponent<Monster_Test>();
                       monster.HP -= 1;

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
        float range = 15;
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
                        Monster_Test monster = hit.collider.gameObject.GetComponent<Monster_Test>();
                        monster.HP -= 1;

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
        float fierballRange = 3f;

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
                    
                        
                          int monsterNumber = Physics.OverlapSphereNonAlloc(hit.point, fierballRange, _colliders, _layerMaskUnit);

                        Debug.Log("여기 들어오니?222");
                        Debug.Log($"{monsterNumber}");
                        Debug.Log($"{_layerMaskUnit}");


                        if(monsterNumber == 0)
                        {
                            Debug.Log("혹시 0?"); 

                        }

                          for(int i = 0;  i < monsterNumber; i++)
                          {
                             
                            _colliders[i].gameObject.GetComponent<Unit_Test>().HP -= 1;
                          } 
                            Debug.Log("여기 들어오니?333");

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
                        Monster_Test monster = hit.collider.gameObject.GetComponent<Monster_Test>();

                        if (monster == null)
                        {
                            Debug.Log($"monster가 null");
                            return;
                        }

                        _monster[player._skillNum] = monster;
                        player._skillNum++;
                            Debug.Log($"스크롤 들어오나?2222");
                            Debug.Log($"{player._skillNum}");
                        if(player._skillNum == 3)
                        {
                            for(int i = 0; i < player._skillNum; i++)
                            {
                                _monster[i].HP -= 1;
                            }

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

    void SkillNum(Player_Test player)
    {
        
    }
    void Initialized(Player_Test player)
    {
        player.UnitState &= ~Unit_Test.State.Skill;
        player.GetComponent<PlayerMoving>().enabled = true;
        player._lineRenderer.enabled = false;

        player.UsingSkillNum = 0;
    }

    #endregion
}
