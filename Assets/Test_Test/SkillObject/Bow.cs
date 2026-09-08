using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Bow : MonoBehaviour
{

    int _damage = 1;
    float _size = 0.1f;

    private void Awake()
    {
        transform.localScale = Vector3.one * _size;
    }
    public void SkillDamageUnitStat(Unit_Test unitStat)
  {
        _damage += Mathf.RoundToInt((unitStat.Strength - 10) * 0.5f);
  }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Monster"))
        {
            Unit_Test unit = collision.gameObject.GetComponent<Unit_Test>();

            unit.HP -= _damage;
            unit.Animator.SetTrigger("BGetHit");
            Debug.Log("콜라이더 히트 안들어오나?");
        }


        this.gameObject.SetActive(false);
    }
   
}
