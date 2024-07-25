// Copyright (c) 2012-2020 FuryLion Group. All Rights Reserved.

using UnityEditor;

namespace FuryLion.ObjectPool
{
    [CustomEditor(typeof(CellPool))]
    public class CellPoolEditor : PoolEditor
    {
        [MenuItem(Package.MenuItem + "/CellPool")]
        private static void Edit()
        {
            var instance = ReflectionUtility.GetStaticPropertyValue<SingletonScriptableObject<CellPool>, CellPool>("Instance");
            PoolEditorWindow.Open(instance);
        }
    }
}