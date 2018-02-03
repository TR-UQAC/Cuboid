using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Ennemis)), CanEditMultipleObjects]
public class EnnemisEditor : Editor {
    
    private SerializedProperty
        vie, vieMax,

        contact, dmgContact,

        attaque, dmgAttaque,

        deplacement,
        speed, hauteurSaut, fMode,

        resitGlace, resitPoison, resitFoudre,

        glace, poison, foudre;

    private void OnEnable() {
        vie     = serializedObject.FindProperty("ennemiStats.vie");
        vieMax  = serializedObject.FindProperty("ennemiStats.vieMax");
        
        contact    = serializedObject.FindProperty("comp.contact");
        dmgContact = serializedObject.FindProperty("comp.dmgContact");

        attaque    = serializedObject.FindProperty("comp.attaque");
        dmgAttaque = serializedObject.FindProperty("comp.dmgAttaque");

        speed = serializedObject.FindProperty("ennemiStats.speed");
        fMode = serializedObject.FindProperty("ennemiStats.fMode");
        deplacement = serializedObject.FindProperty("comp.deplacement");
        hauteurSaut = serializedObject.FindProperty("ennemiStats.hauteurSaut");

        resitGlace  = serializedObject.FindProperty("ennemiStats.resitGlace");
        resitPoison = serializedObject.FindProperty("ennemiStats.resitPoison");
        resitFoudre = serializedObject.FindProperty("ennemiStats.resitFoudre");

        glace  = serializedObject.FindProperty("ennemiStats.glace");
        poison = serializedObject.FindProperty("ennemiStats.poison");
        foudre = serializedObject.FindProperty("ennemiStats.foudre");
    }


    public override void OnInspectorGUI() {

        serializedObject.Update();

        //***** Vie *****//
        EditorGUILayout.LabelField("Vie", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
            EditorGUILayout.IntSlider(vie, 0, vieMax.intValue, "Vie courente");

            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(vieMax, new GUIContent("Vie Max"));
            EditorGUI.indentLevel--;
        EditorGUI.indentLevel--;

        //***** Contact *****//
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Les Dommages", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(contact, new GUIContent("Contact"));
            if (contact.boolValue) {
                EditorGUI.indentLevel++;
                EditorGUILayout.IntSlider(dmgContact, 0, 100, "Dommage Contact");
                EditorGUI.indentLevel--;
            }

            //***** Attaque *****//
            EditorGUILayout.PropertyField(attaque, new GUIContent("Attaque"));
            Ennemis.typeAttaque tAttaque = (Ennemis.typeAttaque)attaque.enumValueIndex;
            switch (tAttaque) {
                case Ennemis.typeAttaque.Rien:
                    break;

                default:
                    EditorGUI.indentLevel++;
                    EditorGUILayout.IntSlider(dmgAttaque, 0, 100, "Dommage Attaque");
                    EditorGUI.indentLevel--;
                    break;
            }
        EditorGUI.indentLevel--;

        //***** Deplacement *****//
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Les Mouvements", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(deplacement, new GUIContent("Deplacement"));
            Ennemis.typeDeplac tDeplacement = (Ennemis.typeDeplac)deplacement.enumValueIndex;
            switch (tDeplacement) {
                case Ennemis.typeDeplac.Voler:
                case Ennemis.typeDeplac.Immobile:
                    break;

                default:
                    EditorGUI.indentLevel++;
                    EditorGUILayout.Slider(hauteurSaut, 0, 100, "Hauteur saut");
                    EditorGUI.indentLevel--;
                    break;
            }

        EditorGUILayout.PropertyField(speed, new GUIContent("Vitesse"));
        EditorGUILayout.PropertyField(fMode, new GUIContent("FMode"));
        EditorGUI.indentLevel--;
        
        EditorGUILayout.PropertyField(resitGlace, new GUIContent("Resite Glace"));
        EditorGUILayout.PropertyField(resitPoison, new GUIContent("Resite Poison"));
        EditorGUILayout.PropertyField(resitFoudre, new GUIContent("Resite Foudre"));
        
        EditorGUILayout.PropertyField(glace, new GUIContent("Ralentit"));
        EditorGUILayout.PropertyField(poison, new GUIContent("Empoisoné"));
        EditorGUILayout.PropertyField(foudre, new GUIContent("Paralysé"));

        serializedObject.ApplyModifiedProperties();
    }

}
