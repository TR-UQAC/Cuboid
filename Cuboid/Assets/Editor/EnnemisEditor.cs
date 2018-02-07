using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Ennemis)), CanEditMultipleObjects]
public class EnnemisEditor : Editor {

    private static bool showVie;
    private static bool showDommage;
    private static bool showMouvement;
    private static bool showElements;

    //GameObject weaponPrefab;

    private SerializedProperty
        vie, vieMax,

        contact, dmgContact,

        attaque, dmgAttaque, //fireRate,

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
        //fireRate   = serializedObject.FindProperty("fireRate");

        speed = serializedObject.FindProperty("ennemiStats.speed");
        fMode = serializedObject.FindProperty("ennemiStats.fMode");
        deplacement = serializedObject.FindProperty("comp.deplacement");
        hauteurSaut = serializedObject.FindProperty("ennemiStats.m_JumpForce");

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
        EditorGUILayout.Space();
        showVie = EditorGUILayout.Foldout(showVie, "Vie", true);
        if (showVie) {
            EditorGUI.indentLevel++;
                EditorGUILayout.IntSlider(vie, 0, vieMax.intValue, "Vie courante");
                ProgressBar((float)vie.intValue / (float)vieMax.intValue, "Vie");

                EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(vieMax, new GUIContent("Vie Max"));
                EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        }
       
        //****** Les dommages ******//
        EditorGUILayout.Space();
        showDommage = EditorGUILayout.Foldout(showDommage, "Dommages", true);
        if (showDommage) {
            //***** Contact *****//
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(contact, new GUIContent("Contact"));
            if (contact.boolValue) {
                EditorGUI.indentLevel++;
                EditorGUILayout.IntSlider(dmgContact, 0, 100, "Dommage Contact");
                ProgressBar(dmgContact.intValue / 100f, "Dommage");
                EditorGUI.indentLevel--;
            }

            //***** Attaque *****//
            EditorGUILayout.PropertyField(attaque, new GUIContent("Attaque"));
            Ennemis.typeAttaque tAttaque = (Ennemis.typeAttaque)attaque.enumValueIndex;
            switch (tAttaque) {
                case Ennemis.typeAttaque.Rien:
                    break;
                    
                case Ennemis.typeAttaque.Tirer:
                    EditorGUI.indentLevel++;
                    //EditorGUILayout.IntSlider(fireRate, 0, 10, "Fire Rate");
                    EditorGUILayout.IntSlider(dmgAttaque, 0, 100, "Dommage Attaque");
                    ProgressBar(dmgAttaque.intValue / 100f, "Dommage");
                    EditorGUI.indentLevel--;
                    break;
                    
                default:
                    EditorGUI.indentLevel++;
                    EditorGUILayout.IntSlider(dmgAttaque, 0, 100, "Dommage Attaque");
                    ProgressBar(dmgAttaque.intValue / 100f, "Dommage");
                    EditorGUI.indentLevel--;
                    break;
            }
            EditorGUI.indentLevel--;
        }
        
        //***** Deplacement *****//
        EditorGUILayout.Space();
        showMouvement = EditorGUILayout.Foldout(showMouvement, "Mouvement", true);
        if (showMouvement) {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(deplacement, new GUIContent("Deplacement"));
            Ennemis.typeDeplac tDeplacement = (Ennemis.typeDeplac)deplacement.enumValueIndex;
            switch (tDeplacement) {
                case Ennemis.typeDeplac.Voler:
                case Ennemis.typeDeplac.Immobile:
                    break;

                default:
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(hauteurSaut, new GUIContent("Force du Saut"));
                    ProgressBar(hauteurSaut.floatValue / 100f, "Hauteur saut");
                    EditorGUI.indentLevel--;
                    break;
            }

            EditorGUILayout.PropertyField(speed, new GUIContent("Vitesse"));
            EditorGUILayout.PropertyField(fMode, new GUIContent("FMode"));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        showElements = EditorGUILayout.Foldout(showElements, "Éléments", true);
        if (showElements) {
            EditorGUILayout.PropertyField(resitGlace, new GUIContent("Resite Glace"));
            EditorGUILayout.PropertyField(resitPoison, new GUIContent("Resite Poison"));
            EditorGUILayout.PropertyField(resitFoudre, new GUIContent("Resite Foudre"));

            EditorGUILayout.PropertyField(glace, new GUIContent("Ralentit"));
            EditorGUILayout.PropertyField(poison, new GUIContent("Empoisoné"));
            EditorGUILayout.PropertyField(foudre, new GUIContent("Paralysé"));
        }
        serializedObject.ApplyModifiedProperties();
    }

    // Custom GUILayout progress bar.
    void ProgressBar(float value, string label) {
        // Get a rect for the progress bar using the same margins as a textfield:
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        rect.xMin = 30f;
        rect.xMax = 300f;
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }

}
