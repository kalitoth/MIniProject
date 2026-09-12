using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlKey : MonoBehaviour
{
    [SerializeField]
    Image _controlKey;

    [SerializeField]
    Image _button;

    public void ActiveControlKey()
  {
        _controlKey.gameObject.SetActive(true);
        _button.gameObject.SetActive(false);
  }
}
