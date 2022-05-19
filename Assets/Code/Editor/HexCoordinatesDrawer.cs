using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HexCoordinates))]
public class HexCoordinatesDrawer : PropertyDrawer {
    public override void OnGUI(Rect position2D, SerializedProperty property, GUIContent label) {
        HexCoordinates hexCoordinates = new HexCoordinates(
            property.FindPropertyRelative("x").intValue,
            property.FindPropertyRelative("z").intValue
            );
        position2D = EditorGUI.PrefixLabel(position2D, label);
        GUI.Label(position2D, hexCoordinates.ToString());
    }
}
