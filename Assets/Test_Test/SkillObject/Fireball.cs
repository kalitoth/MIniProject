using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    int _damage = 2;
    float _size = 4f;

    private void Awake()
    {
        transform.localScale = Vector3.one * _size;
    }
    public void SkillDamageUnitStat(Unit_Test unitStat)
    {
          _damage += Mathf.RoundToInt((unitStat.Intelligence - 10) * 0.5f);
    }

    //소환된 물체는 다음 프레임부터 업데이트 적용
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Monster") || collision.collider.CompareTag("Player"))
        {
            Unit_Test unit = collision.gameObject.GetComponent<Unit_Test>();
            unit.TakeDamage(unit, _damage);
           //unit.HP -= _damage;
           //unit.Animator.SetTrigger("TGetHit");
            Debug.Log("콜라이더 히트 안들어오나?");
        }


        this.gameObject.SetActive(false);
    }
    
 
}
