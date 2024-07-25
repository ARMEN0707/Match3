// Copyright (c) 2012-2022 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace FuryLion.ObjectPool
{
    public class CellPool : Pool<CellPool, CellPoolItem, CellPoolComponent, CellType>
    {
#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void SubsystemRegistration()
        {
            _instance = null;
        }
#endif
    
        [RuntimeInitializeOnLoadMethod]
        private static void AfterSceneLoad()
        {
            Instance.InitPool();
        }
    }
}