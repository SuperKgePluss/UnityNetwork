using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cainos.CustomizablePixelCharacter
{
    [CustomEditor(typeof(PixelCharacterController))]
    public class CharacterControllerEditor : Editor
    {
        private PixelCharacterController instance;

        private SerializedProperty defaultMovement;
        private SerializedProperty leftKey;
        private SerializedProperty rightKey;
        private SerializedProperty crouchKey;
        private SerializedProperty jumpKey;
        private SerializedProperty moveModifierKey;
        private SerializedProperty attackKey;

        private SerializedProperty walkSpeedMax;
        private SerializedProperty walkAcc;
        private SerializedProperty runSpeedMax;
        private SerializedProperty runAcc;
        private SerializedProperty crouchSpeedMax;
        private SerializedProperty crouchAcc;
        private SerializedProperty airSpeedMax;
        private SerializedProperty airAcc;
        private SerializedProperty groundBrakeAcc;
        private SerializedProperty airBrakeAcc;
        private SerializedProperty jumpSpeed;
        private SerializedProperty jumpCooldown;
        private SerializedProperty jumpGravityMutiplier;
        private SerializedProperty fallGravityMutiplier;

        private SerializedProperty groundCheckRadius;

        private PropertyField IsDead;


        private bool foldout_input = true;
        private bool foldout_movement = true;
        private bool foldout_runtime = true;

        private void OnEnable()
        {
            instance = target as PixelCharacterController;

            defaultMovement = serializedObject.FindProperty("defaultMovement");
            leftKey = serializedObject.FindProperty("leftKey");
            rightKey = serializedObject.FindProperty("rightKey");
            crouchKey = serializedObject.FindProperty("crouchKey");
            jumpKey = serializedObject.FindProperty("jumpKey");
            moveModifierKey = serializedObject.FindProperty("moveModifierKey");
            attackKey = serializedObject.FindProperty("attackKey");

            walkSpeedMax = serializedObject.FindProperty("walkSpeedMax");
            walkAcc = serializedObject.FindProperty("walkAcc");
            runSpeedMax = serializedObject.FindProperty("runSpeedMax");
            runAcc = serializedObject.FindProperty("runAcc");
            crouchSpeedMax = serializedObject.FindProperty("crouchSpeedMax");
            crouchAcc = serializedObject.FindProperty("crouchAcc");
            airSpeedMax = serializedObject.FindProperty("airSpeedMax");
            airAcc = serializedObject.FindProperty("airAcc");
            airSpeedMax = serializedObject.FindProperty("airSpeedMax");
            groundBrakeAcc = serializedObject.FindProperty("groundBrakeAcc");
            groundBrakeAcc = serializedObject.FindProperty("groundBrakeAcc");
            airBrakeAcc = serializedObject.FindProperty("airBrakeAcc");
            jumpSpeed = serializedObject.FindProperty("jumpSpeed");
            jumpCooldown = serializedObject.FindProperty("jumpCooldown");
            jumpGravityMutiplier = serializedObject.FindProperty("jumpGravityMutiplier");
            fallGravityMutiplier = serializedObject.FindProperty("fallGravityMutiplier");

            groundCheckRadius = serializedObject.FindProperty("groundCheckRadius");

            IsDead = ExposeProperties.GetProperty("IsDead", instance);

        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            foldout_input = EditorGUILayout.Foldout(foldout_input, "Input", true);
            if (foldout_input)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(defaultMovement);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(leftKey);
                EditorGUILayout.PropertyField(rightKey);
                EditorGUILayout.PropertyField(crouchKey);
                EditorGUILayout.PropertyField(jumpKey);
                EditorGUILayout.PropertyField(moveModifierKey);
                EditorGUILayout.PropertyField(attackKey);
                EditorGUI.indentLevel--;
            }


            foldout_movement = EditorGUILayout.Foldout(foldout_movement, "Movement", true);
            if (foldout_movement)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(walkSpeedMax);
                EditorGUILayout.PropertyField(walkAcc);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(runSpeedMax);
                EditorGUILayout.PropertyField(runAcc);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(crouchSpeedMax);
                EditorGUILayout.PropertyField(crouchAcc);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(airSpeedMax);
                EditorGUILayout.PropertyField(airAcc);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(groundBrakeAcc);
                EditorGUILayout.PropertyField(airBrakeAcc);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(jumpSpeed);
                EditorGUILayout.PropertyField(jumpCooldown);
                EditorGUILayout.PropertyField(jumpGravityMutiplier);
                EditorGUILayout.PropertyField(fallGravityMutiplier);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(groundCheckRadius);
                EditorGUI.indentLevel--;
            }

            foldout_runtime = EditorGUILayout.Foldout(foldout_runtime, "Runtime", true);
            if (foldout_runtime)
            {
                EditorGUI.indentLevel++;
                ExposeProperties.Expose(IsDead);
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
