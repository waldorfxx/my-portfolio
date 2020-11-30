using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManagerInitiator : MonoBehaviour
{
    public TimerManager prefab;

    private TimerManager _manager;
    void Start()
    {
        _manager = GameObject.FindObjectOfType<TimerManager>();
        if (_manager == null)
            _manager = Instantiate(prefab);
        else
            _manager.StopTimer();
    }

    public void StartTimer()
    {
        _manager.StartTimer();
    }
}
