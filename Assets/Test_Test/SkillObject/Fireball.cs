using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    int _damage = 1;
    float _size = 3f;

    private void Awake()
    {
        transform.localScale = Vector3.one * _size;
    }
    public void SkillDamageUnitStat(Unit_Test unitStat)
  {
        _damage += Mathf.RoundToInt((unitStat.Intelligence - 10) * 0.5f);
  }

    //소환된 물체는 다음 프레임부터 업데이트 적용
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster") || other.CompareTag("Player"))
        {
            Unit_Test unit = other.GetComponent<Unit_Test>();

            unit.HP -= 1;
            unit.Animator.SetTrigger("BGetHit");
        }


        this.gameObject.SetActive(false);
    }
}
