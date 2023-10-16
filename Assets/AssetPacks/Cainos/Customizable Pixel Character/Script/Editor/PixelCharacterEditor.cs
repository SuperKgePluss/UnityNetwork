using UnityEditor;
using UnityEngine;
using System.Collections;


namespace Cainos.CustomizablePixelCharacter
{
    [CustomEditor(typeof(PixelCharacter))]
    public class PixelCharacterEditor : Editor
    {
        private PixelCharacter instance;

        private SerializedProperty animator;
        private SerializedProperty hat;
        private SerializedProperty hair;
        private SerializedProperty hairClipped;
        private SerializedProperty eye;
        private SerializedProperty eyeBase;
        private SerializedProperty facewear;
        private SerializedProperty cloth;
        private SerializedProperty skirt;
        private SerializedProperty pants;
        private SerializedProperty socks;
        private SerializedProperty shoes;
        private SerializedProperty back;
        private SerializedProperty expression;
        private SerializedProperty body;
        private SerializedProperty weaponSlot;

        private PropertyField HatMaterial;
        private PropertyField HairMaterial;
        private PropertyField EyeMaterial;
        private PropertyField EyeBaseMaterial;
        private PropertyField FacewearMaterial;
        private PropertyField ClothMaterial;
        private PropertyField PantsMaterial;
        private PropertyField SocksMaterial;
        private PropertyField ShoesMaterial;
        private PropertyField BackMaterial;
        private PropertyField BodyMaterial;
        private PropertyField ClipHair;
        private SerializedProperty blinkInterval;

        private PropertyField Facing;
        private PropertyField Expression;
        private PropertyField AttackAction;
        private PropertyField IsCrouching;
        private PropertyField IsGrounded;
        private PropertyField IsAttacking;
        private PropertyField IsDead;
        private PropertyField MovingBlend;

        private bool foldout_objects = false;
        private bool foldout_appearance = true;
        private bool foldout_runtime = true;

        public void OnEnable()
        {
            instance = target as PixelCharacter;

            animator = serializedObject.FindProperty("animator");
            hat = serializedObject.FindProperty("hat");
            hair = serializedObject.FindProperty("hair");
            hairClipped = serializedObject.FindProperty("hairClipped");
            eye = serializedObject.FindProperty("eye");
            eyeBase = serializedObject.FindProperty("eyeBase");
            facewear = serializedObject.FindProperty("facewear");
            cloth = serializedObject.FindProperty("cloth");
            skirt = serializedObject.FindProperty("skirt");
            pants = serializedObject.FindProperty("pants");
            socks = serializedObject.FindProperty("socks");
            shoes = serializedObject.FindProperty("shoes");
            back = serializedObject.FindProperty("back");
            expression = serializedObject.FindProperty("expression");
            body = serializedObject.FindProperty("body");
            weaponSlot = serializedObject.FindProperty("weaponSlot");

            HatMaterial = ExposeProperties.GetProperty("HatMaterial", instance);
            HairMaterial = ExposeProperties.GetProperty("HairMaterial", instance);
            EyeMaterial = ExposeProperties.GetProperty("EyeMaterial", instance);
            EyeBaseMaterial = ExposeProperties.GetProperty("EyeBaseMaterial", instance);
            FacewearMaterial = ExposeProperties.GetProperty("FacewearMaterial", instance);
            ClothMaterial = ExposeProperties.GetProperty("ClothMaterial", instance);
            PantsMaterial = ExposeProperties.GetProperty("PantsMaterial", instance);
            SocksMaterial = ExposeProperties.GetProperty("SocksMaterial", instance);
            ShoesMaterial = ExposeProperties.GetProperty("ShoesMaterial", instance);
            BackMaterial = ExposeProperties.GetProperty("BackMaterial", instance);
            BodyMaterial = ExposeProperties.GetProperty("BodyMaterial", instance);
            ClipHair = ExposeProperties.GetProperty("ClipHair", instance);
            blinkInterval = serializedObject.FindProperty("blinkInterval");

            Expression = ExposeProperties.GetProperty("Expression", instance);
            AttackAction = ExposeProperties.GetProperty("AttackAction", instance);
            Facing = ExposeProperties.GetProperty("Facing", instance);
            IsCrouching = ExposeProperties.GetProperty("IsCrouching", instance);
            IsGrounded = ExposeProperties.GetProperty("IsGrounded", instance);
            IsAttacking = ExposeProperties.GetProperty("IsAttacking", instance);
            IsDead = ExposeProperties.GetProperty("IsDead", instance);
            MovingBlend = ExposeProperties.GetProperty("MovingBlend", instance);
        }

        public override void OnInspectorGUI()
        {
            if (instance == null) return;
            serializedObject.Update();

            foldout_objects = EditorGUILayout.Foldout(foldout_objects, "Objects", true);
            if (foldout_objects)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(animator);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(hat);
                EditorGUILayout.PropertyField(hair);
                EditorGUILayout.PropertyField(hairClipped);
                EditorGUILayout.PropertyField(eye);
                EditorGUILayout.PropertyField(eyeBase);
                EditorGUILayout.PropertyField(facewear);
                EditorGUILayout.PropertyField(cloth);
                EditorGUILayout.PropertyField(skirt);
                EditorGUILayout.PropertyField(pants);
                EditorGUILayout.PropertyField(socks);
                EditorGUILayout.PropertyField(shoes);
                EditorGUILayout.PropertyField(back);
                EditorGUILayout.PropertyField(expression);
                EditorGUILayout.PropertyField(body);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(weaponSlot);
                EditorGUI.indentLevel--;
            }


            foldout_appearance = EditorGUILayout.Foldout(foldout_appearance, "Appearance", true);
            if (foldout_appearance)
            {
                EditorGUI.indentLevel++;
                ExposeProperties.Expose(HatMaterial, typeof(Material));
                ExposeProperties.Expose(HairMaterial, typeof(Material));
                ExposeProperties.Expose(EyeMaterial, typeof(Material));
                ExposeProperties.Expose(EyeBaseMaterial, typeof(Material));
                ExposeProperties.Expose(FacewearMaterial, typeof(Material));
                ExposeProperties.Expose(ClothMaterial, typeof(Material));
                ExposeProperties.Expose(PantsMaterial, typeof(Material));
                ExposeProperties.Expose(SocksMaterial, typeof(Material));
                ExposeProperties.Expose(ShoesMaterial, typeof(Material));
                ExposeProperties.Expose(BackMaterial, typeof(Material));
                ExposeProperties.Expose(BodyMaterial, typeof(Material));
                EditorGUILayout.Space();
                ExposeProperties.Expose(ClipHair);
                EditorGUILayout.PropertyField(blinkInterval);

                EditorGUI.indentLevel--;
            }

            foldout_runtime = EditorGUILayout.Foldout(foldout_runtime, "Runtime", true);
            if (foldout_runtime)
            {
                EditorGUI.indentLevel++;
                ExposeProperties.Expose(Expression);
                ExposeProperties.Expose(AttackAction);
                ExposeProperties.Expose(Facing);
                ExposeProperties.Expose(IsCrouching);
                ExposeProperties.Expose(IsGrounded);
                ExposeProperties.Expose(IsAttacking);
                ExposeProperties.Expose(IsDead);
                ExposeProperties.Expose(MovingBlend);
                EditorGUILayout.Space();

                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUI.indentLevel * 10);
                if (GUILayout.Button("Injured Front")) instance.InjuredFront();
                if (GUILayout.Button("Injured Back")) instance.InjuredBack();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUI.indentLevel * 10);
                if (GUILayout.Button("Attack")) instance.Attack();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUI.indentLevel * 10);
                if (GUILayout.Button("Drop Weapon")) instance.DropWeapon();
                GUILayout.EndHorizontal();

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
