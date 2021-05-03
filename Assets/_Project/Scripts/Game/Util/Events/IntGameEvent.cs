using UnityEditor;
using UnityEngine;
using Util.Variable;

namespace Util.Events
{
    // [CreateAssetMenu(menuName = "Custom/Events/int")]

    public class IntGameEvent : ObservableIntVariable, IEventBase
    {
        [MenuItem("Assets/Create/Custom/Events/int")]
        public static void CreateEvent()
        {
            var instance = ScriptableObject.CreateInstance<IntGameEvent>();
            AssetDatabase.CreateAsset(instance, AssetDatabase.GetAssetPath(Selection.activeObject) + "/NewAsset" );
        }

        // [MenuItem("Assets/Create/Custom/Tile/Probability")]
        // public static void CreateTile()
        // {
        //     AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ProbabilityTile>()));
        // }
    }

    public interface IEventBase
    {

    }
}