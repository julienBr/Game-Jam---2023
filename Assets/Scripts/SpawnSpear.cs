using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpear : MonoBehaviour
{
    [SerializeField] private AppDatas _appDatas;

    private void OnEnable()
    {
        CameraController.SpearFall += SpearSpawn;
    }
    
    private void OnDisable()
    {
        CameraController.SpearFall -= SpearSpawn;
    }

    private void SpearSpawn()
    {
        for (int i = 0; i < _appDatas.posSpear.Count; i++)
        {
            Instantiate(_appDatas.spear, _appDatas.posSpear[i].Position, Quaternion.identity);
        }
    }
}
