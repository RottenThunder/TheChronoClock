using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IP
{
    [CreateAssetMenu(fileName = "NewAllDungeonData", menuName = "AllDungeonData")]
    public class AllDungeonData : ScriptableObject
    {
        public DungeonData[] dungeonDatas;
    }
}