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
        Instantiate(_appDatas.spear, _appDatas.posSpear[.Range(0, _appDatas.posSpear.Count)], Quaternion.identity);
    }
}
