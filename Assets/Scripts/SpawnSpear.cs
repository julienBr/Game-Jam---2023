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

    private void Start()
    {
        _appDatas.index = 0;
    }

    private void Update()
    {
        if (_appDatas.index == 10) _appDatas.index = 0;
    }

    private void SpearSpawn()
    {
        Instantiate(_appDatas.spear, _appDatas.posSpear[_appDatas.index].Position, Quaternion.Euler(_appDatas.posSpear[_appDatas.index].Rotation));
        _appDatas.index++;
    }
}