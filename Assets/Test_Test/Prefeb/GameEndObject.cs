using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndObject : MonoBehaviour
{
    [SerializeField]
    ShareRepository _shareRepository;

    private void OnDestroy()
    {
        _shareRepository._lichItemNum++;
    }
    
    public void Reference(ShareRepository shareRepository)
    {
        _shareRepository = shareRepository;
    }
}
