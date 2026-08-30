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
    


    public Dictionary<int, Action<Player_Test, RaycastHit>> SkillList
    {
        get { return _skillList; }
    }

    private void Awake()
    {
        _skillList.Add(0, Attack);
        _skillList.Add(1, Defence);
        _skillList.Add(2, Moving);
    }
  
    #region 스킬 목록

    public void Attack(Player_Test player, RaycastHit hit)
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
                    if ((monster.transform.position - player.transform.position).sqrMagnitude <= 3f)
                    {
                       monster.HP -= 1;

                       player._state = Player_Test.State.None;
                       player._lineRenderer.enabled = false;
                    }
               }
                    
            }
        }
        Debug.Log("Attack");
    }
    public void Defence(Player_Test player, RaycastHit _hit)
    {
        Debug.Log("Defence");
    }
    public void Moving(Player_Test player, RaycastHit _hit)
    {
        Debug.Log("Moving");
    }

    public void Fireball(Player_Test _player)
    {
        Debug.Log("파이어볼");
    }

    #endregion
}
