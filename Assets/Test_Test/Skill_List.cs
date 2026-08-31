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
    }
 
    #region 스킬 목록

    public void Sword(Player_Test player, RaycastHit hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if(player._lineRenderer.startColor != _initialColor)
            {
                player._lineRenderer.startColor = _initialColor; 
            }
             
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Monster"))
            {  
                if ((hit.point - player.transform.position).sqrMagnitude > 3f)
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
                
               if (Input.GetMouseButtonDown(0))
               {
                    Monster_Test monster = hit.collider.gameObject.GetComponent<Monster_Test>();
                    if ((hit.point - player.transform.position).sqrMagnitude <= 3f)
                    {
                       monster.HP -= 1;

                       player._state = Player_Test.State.None;
                       player._lineRenderer.enabled = false;
                    }
               }
                    
            }
        }
        Debug.Log("Sword");
    }
    public void Bow(Player_Test player, RaycastHit hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (player._lineRenderer.startColor != _initialColor)
            {
                player._lineRenderer.startColor = _initialColor;
            }

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Monster"))
            {
                if ((hit.point - player.transform.position).sqrMagnitude > 300f)
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

                if (Input.GetMouseButtonDown(0))
                {
                    Monster_Test monster = hit.collider.gameObject.GetComponent<Monster_Test>();
                    if ((hit.point - player.transform.position).sqrMagnitude <= 300f)
                    {
                        monster.HP -= 1;

                        player._state = Player_Test.State.None;
                        player._lineRenderer.enabled = false;
                    }
                }

            }
        }
        Debug.Log("Bow");
    }
   
    public void Fireball(Player_Test player, RaycastHit hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (player._lineRenderer.startColor != _initialColor)
            {
                player._lineRenderer.startColor = _initialColor;
            }

            if (hit.collider != null)
            {
                if ((hit.point - player.transform.position).sqrMagnitude > 200f)
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

                if (Input.GetMouseButtonDown(0))
                {
                    if ((hit.point - player.transform.position).sqrMagnitude <= 200f)
                    {
                        Debug.Log("여기 들어오니?111");
                    
                        //이거 다시 정리
                          int monsterNumber = Physics.OverlapSphereNonAlloc(hit.point, 100f, _colliders, _layerMaskUnit);

                        Debug.Log("여기 들어오니?222");
                        Debug.Log($"{monsterNumber}");
                        Debug.Log($"{_layerMaskUnit}");


                        if(monsterNumber == 0)
                        {
                            Debug.Log("혹시 0?");
                            return;

                        }

                          for(int i = 0;  i < monsterNumber; i++)
                          {
                             
                            _colliders[i].gameObject.GetComponent<Unit_Test>().HP -= 1;
                          } 
                        Debug.Log("여기 들어오니?333"); 
                          

                           player._state = Player_Test.State.None;
                           player._lineRenderer.enabled = false;

                    }
                }

            }
        }

        Debug.Log("파이어볼");
    }
    public void Scroll(Player_Test player, RaycastHit hit)
    {
        Debug.Log("Scroll");
    }
    public void Moving(Player_Test player, RaycastHit hit)
    {
        Debug.Log("Moving");
    }
    #endregion
}
