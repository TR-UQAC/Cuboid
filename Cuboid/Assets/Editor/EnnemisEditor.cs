using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Ennemis)), CanEditMultipleObjects]
public class EnnemisEditor : Editor {

    private SerializedProperty
        vie, vieMax,
        immortel,

        contact, dmgContact,

        attaque, dmgAttaque, fireRate,
        tirerSurJoueur,

        ePower, eRadius, upwardsModifier,

        deplacement,
        speed, maxSpeed, hauteurSaut, fMode,
        facingRight,

        resitGlace, resitPoison, resitFoudre,

        glace, poison, foudre,
        glacer, empoisoner, paralyse;

    private void OnEnable() {

        vie      = serializedObject.FindProperty("ennemiStats.vie");
        vieMax   = serializedObject.FindProperty("ennemiStats.vieMax");
        immortel = serializedObject.FindProperty("ennemiStats.immortel");

        contact    = serializedObject.FindProperty("comp.contact");
        dmgContact = serializedObject.FindProperty("comp.dmgContact");

        attaque    = serializedObject.FindProperty("comp.attaque");
        dmgAttaque = serializedObject.FindProperty("comp.dmgAttaque");
        fireRate   = serializedObject.FindProperty("comp.fireRate");
        tirerSurJoueur = serializedObject.FindProperty("tirerSurJoueur");

        ePower          = serializedObject.FindProperty("comp.statAttaque.ePower");
        eRadius         = serializedObject.FindProperty("comp.statAttaque.eRadius");
        upwardsModifier = serializedObject.FindProperty("comp.statAttaque.upwardsModifier");

        glace = serializedObject.FindProperty("comp.statAttaque.degGlace");
        poison = serializedObject.FindProperty("comp.statAttaque.degPoison");
        foudre = serializedObject.FindProperty("comp.statAttaque.degFoudre");

        deplacement = serializedObject.FindProperty("comp.deplacement");
        speed       = serializedObject.FindProperty("ennemiStats.speed");
        fMode       = serializedObject.FindProperty("ennemiStats.fMode");
        maxSpeed    = serializedObject.FindProperty("ennemiStats.maxSpeed");
        hauteurSaut = serializedObject.FindProperty("ennemiStats.m_JumpForce");

        facingRight = serializedObject.FindProperty("facingRight");

        resitGlace  = serializedObject.FindProperty("ennemiStats.resitGlace");
        resitPoison = serializedObject.FindProperty("ennemiStats.resitPoison");
        resitFoudre = serializedObject.FindProperty("ennemiStats.resitFoudre");

        glacer = serializedObject.FindProperty("ennemiStats.glacer");
        empoisoner = serializedObject.FindProperty("ennemiStats.empoisoner");
        paralyse = serializedObject.FindProperty("ennemiStats.paralyse");
    }

    public override void OnInspectorGUI() {

        serializedObject.Update();

        Ennemis ennemi = target as Ennemis;
        //***** Vie *****//
        EditorGUILayout.Space();
        ennemi.showVie = EditorGUILayout.Foldout(ennemi.showVie, "Vie", true);
        if (ennemi.showVie) {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(immortel, new GUIContent("Immortel"));
            if (!immortel.boolValue) {
                EditorGUILayout.IntSlider(vie, 0, vieMax.intValue, "Vie courante");
                ProgressBar((float)vie.intValue / (float)vieMax.intValue, "Vie");

                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(vieMax, new GUIContent("Vie Max"));
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }
       
        //****** Les dommages ******//
        EditorGUILayout.Space();
        ennemi.showDommage = EditorGUILayout.Foldout(ennemi.showDommage, "Dommages", true);
        if (ennemi.showDommage) {
            //***** Contact *****//
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(contact, new GUIContent("Contact"));
            if (contact.boolValue) {
                EditorGUI.indentLevel++;
                EditorGUILayout.IntSlider(dmgContact, 0, 100, "Dommage Contact");
                ProgressBar(dmgContact.intValue / 100f, "Dommage");

                ennemi.showContactDommage = EditorGUILayout.Foldout(ennemi.showContactDommage, "Contacte Explosion", true);
                if (ennemi.showContactDommage) {
                    EditorGUILayout.PropertyField(ePower, new GUIContent("Force répulsion"));
                    //EditorGUILayout.PropertyField(eRadius, new GUIContent("Rayon répultion"));
                    //EditorGUILayout.PropertyField(upwardsModifier, new GUIContent("Upwards Modifier"));

                    EditorGUILayout.PropertyField(glace, new GUIContent("Glace"));
                    EditorGUILayout.PropertyField(poison, new GUIContent("Poison"));
                    EditorGUILayout.PropertyField(foudre, new GUIContent("Foudre"));
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            //***** Attaque *****//
            EditorGUILayout.PropertyField(attaque, new GUIContent("Attaque"));
            Ennemis.typeAttaque tAttaque = (Ennemis.typeAttaque)attaque.enumValueIndex;

            if (tAttaque != Ennemis.typeAttaque.Rien) {
                EditorGUI.indentLevel++;
                EditorGUILayout.IntSlider(dmgAttaque, 0, 100, "Dommage Attaque");
                ProgressBar(dmgAttaque.intValue / 100f, "Dommage");
                EditorGUI.indentLevel--;
            }

            switch (tAttaque) {
                case Ennemis.typeAttaque.Rien:
                    break;

                case Ennemis.typeAttaque.Explosion:
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(fireRate, new GUIContent("Fire Rate"));
                    EditorGUI.indentLevel--;
                    break;

                case Ennemis.typeAttaque.Tirer:
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(fireRate, new GUIContent("Fire Rate"));
                    EditorGUILayout.PropertyField(tirerSurJoueur, new GUIContent("Tirer sur le joueur"));
                    EditorGUI.indentLevel--;
                    break;
                default:
                    break;
            }

            EditorGUI.indentLevel--;
        }
        
        //***** Deplacement *****//
        EditorGUILayout.Space();
        ennemi.showMouvement = EditorGUILayout.Foldout(ennemi.showMouvement, "Mouvement", true);
        if (ennemi.showMouvement) {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(deplacement, new GUIContent("Deplacement"));
            Ennemis.typeDeplac tDeplacement = (Ennemis.typeDeplac)deplacement.enumValueIndex;
            switch (tDeplacement) {
                case Ennemis.typeDeplac.Voler:
                case Ennemis.typeDeplac.Immobile:
                    break;

                default:
                    EditorGUILayout.PropertyField(hauteurSaut, new GUIContent("Force du Saut"));
                    break;
            }

            if (tDeplacement != Ennemis.typeDeplac.Immobile) {
                EditorGUILayout.PropertyField(speed, new GUIContent("Vitesse"));
                EditorGUILayout.PropertyField(maxSpeed, new GUIContent("maxSpeed"));
                EditorGUILayout.PropertyField(fMode, new GUIContent("FMode"));
            }
            EditorGUILayout.PropertyField(facingRight, new GUIContent("Facing Right"));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        ennemi.showElements = EditorGUILayout.Foldout(ennemi.showElements, "Éléments", true);
        if (ennemi.showElements) {
            EditorGUILayout.PropertyField(resitGlace, new GUIContent("Resite Glace"));
            EditorGUILayout.PropertyField(resitPoison, new GUIContent("Resite Poison"));
            EditorGUILayout.PropertyField(resitFoudre, new GUIContent("Resite Foudre"));

            EditorGUILayout.PropertyField(glacer, new GUIContent("Ralentit"));
            EditorGUILayout.PropertyField(empoisoner, new GUIContent("Empoisoné"));
            EditorGUILayout.PropertyField(paralyse, new GUIContent("Paralysé"));
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
