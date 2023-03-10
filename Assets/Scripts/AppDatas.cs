using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public struct TransformDatas
{
    public Vector3 Position;
    public Vector3 Rotation;
}

[CreateAssetMenu(fileName = "Appdata")]
public class AppDatas : ScriptableObject
{
    public GameObject spear;
    public List<TransformDatas> posSpear;
    public int index;
}