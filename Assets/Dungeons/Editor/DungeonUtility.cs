using UnityEngine;
using UnityEditor;

public static class DungeonUtility
{
    public static T GetObjectFromGUId<T>(string guid) where T: Object
    {
        var path = AssetDatabase.GUIDToAssetPath(guid);
        try
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            return asset;
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
    }
}