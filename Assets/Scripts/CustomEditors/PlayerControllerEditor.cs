using UnityEditor;
using Control;

[CustomEditor(typeof(ShipController))]
public class PlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ShipController controller = target as ShipController;

        SerializedProperty haveCannonsProp = serializedObject.FindProperty("haveCannons");
        SerializedProperty shipCannonsProp = serializedObject.FindProperty("shipCannons");
        SerializedProperty haveExplosiveBarrelsProp = serializedObject.FindProperty("haveExplosiveBarrels");
        SerializedProperty shipExplosiveBarrelsProp = serializedObject.FindProperty("shipExplosiveBarrels");

        EditorGUILayout.PropertyField(haveCannonsProp);
        if (haveCannonsProp.boolValue == true)
            EditorGUILayout.PropertyField(shipCannonsProp);
        
        EditorGUILayout.PropertyField(haveExplosiveBarrelsProp);
        if (haveExplosiveBarrelsProp.boolValue == true)
            EditorGUILayout.PropertyField(shipExplosiveBarrelsProp);

        this.serializedObject.ApplyModifiedProperties();
    }
}
