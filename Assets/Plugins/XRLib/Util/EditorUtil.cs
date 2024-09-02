using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

#if UNITY_EDITOR
namespace WI
{
    public class EditorUtil
    {

        public static int DrawSubclassDropdown(string label, Type target, int selectedIndex)
        {
            var subclass = Assembly
                .GetAssembly(target)
                .GetTypes()
                .Where(t => t.IsSubclassOf(target))
                .Select(t2 => t2.Name).Append(target.Name).ToArray();
            return EditorGUILayout.Popup(label, selectedIndex, subclass);
        }
    }
}
#endif