using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 사용할 함수 생각
// AssetDatabase.CreateAsset
// ScriptalbeSingleton의 GetFilePath()를 사용하려면, Save함수를 통해 미리 저장한 후 사용해야 한다.

// 아니면 직접 만드는게 편한 것 같기도 하다.
namespace HexaCraft
{
    public class HCManagers : ScriptableSingleton<HCManagers> 
    {
        
    }
}

