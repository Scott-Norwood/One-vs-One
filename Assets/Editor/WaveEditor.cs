// Copyright (c) 2020 xtilly5000
// Licensed under the Creative Commons Attribution-ShareAlike 4.0 International license.
// Share — copy and redistribute the material in any medium or format.
// Adapt — remix, transform, and build upon the material for any purpose, even commercially.

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace xtilly5000.Prototypes.WaveManager
{
    #region WaveEditor Class
    /// <summary>
    /// Overrides default editor with newer, cleaner, easier to use UI.
    /// </summary>
    [CustomEditor(typeof(Wave))]
    public class WaveEditor : Editor
    {
        // ReorderableList is undocumented and used internally, but we still have access to it.
        private ReorderableList list;

        // Store the index of the currently selected entry.
        private int index;

        // Is there an entry currently selected?
        private bool hasSelected = false;

        // Is debug mode turned on?
        private bool debugMode = false;

        #region OnEnable() Method
        /// <summary>
        /// This is called the first time we draw the UI in the inspector window.
        /// It is used for setting up delegates and callbacks properly and initializing the variables.
        /// </summary>
        private void OnEnable()
        {
            // Create a new reorderable list with our values in it.
            list = new ReorderableList(serializedObject, serializedObject.FindProperty("steps"), true, true, true, true);

            // When we draw an element, we want to include these property and label fields.
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {

                // Get the element we want to draw.
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

                // Add some spacing between it to let the UI breathe.
                rect.y += 2;

                // Specify how, and where, to draw the Step index.
                EditorGUI.LabelField(
                    new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                        "Step " + index.ToString());

                // Specify how, and where, to draw the enemy id title.
                EditorGUI.LabelField(
                    new Rect(rect.x + 60, rect.y, 60, EditorGUIUtility.singleLineHeight),
                        "Enemy Id ");

                // Specify how, and where, to draw the actual id field.
                EditorGUI.PropertyField(
                    new Rect(rect.x + 120, rect.y, 60, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("id"), GUIContent.none);

                // Specify how, and where, to draw the advanced grouping title.
                EditorGUI.LabelField(
                    new Rect(rect.x + 197, rect.y, 180, EditorGUIUtility.singleLineHeight),
                        "Advanced Grouping ");

                // Specify how, and where, to draw the actual advanced grouping field.
                EditorGUI.PropertyField(
                    new Rect(rect.x + 320, rect.y, 60, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("isAdvanced"), GUIContent.none);
            };

            // This is called when the inspector draws the list title.
            list.drawHeaderCallback = (Rect rect) =>
            {
                // We want to change it to write our own custom title.
                EditorGUI.LabelField(rect, "Wave Enemy Groups");
            };

            // This is called to check if we are able to remove an entry from the list.
            list.onCanRemoveCallback = (ReorderableList l) =>
            {
                // We always want to have at least one step per wave, so make sure that it doesn't let us remove the last one.
                return l.count > 1;
            };

            // This is called when an entry is removed.
            list.onRemoveCallback = (ReorderableList l) =>
            {
                // We want to make sure that the user meant to remove the entry, so prompt them with a confirmation dialogue.
                if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete this group of enemies?", "Yes", "No"))
                {
                    // Deselect the entry and confirm the deletion.
                    ReorderableList.defaultBehaviours.DoRemoveButton(l);
                    hasSelected = false;
                }
            };

            // This is called when an entry is selected.
            list.onSelectCallback = (ReorderableList l) =>
            {
                // We want to record the index and let the class know that an entry is selected. This is used later on for other methods.
                index = l.index;
                hasSelected = true;
            };

            // This is called when an entry is added.
            list.onAddCallback = (ReorderableList l) =>
            {
                // We want to get the index of the added entry.
                int index = l.serializedProperty.arraySize;

                // Increase the array size accordingly, and update the selected entry.
                l.serializedProperty.arraySize++;
                l.index = index;

                // Get the data of the newly added entry for later use.
                SerializedProperty element = l.serializedProperty.GetArrayElementAtIndex(index);

                // Deselect the entry that was previously selected.
                hasSelected = false;
            };

            // This is called when we want to add a new entry. It gives us a dropdown of options to add.
            list.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) =>
            {
                // We want to create a new menu and make a reference to it for later.
                GenericMenu menu = new GenericMenu();

                // Add the two menu items, and pass in the CreationHandler for handling the initialization of the item.
                menu.AddItem(new GUIContent("Simple Step"), false, CreationHandler, new StepCreationParams() { isAdvanced = false });
                menu.AddItem(new GUIContent("Advanced Step"), false, CreationHandler, new StepCreationParams() { isAdvanced = true });

                // Show the menu under the mouse when right-clicked.
                menu.ShowAsContext();
            };
        }
        #endregion

        #region OnInspectorGUI() Method
        /// <summary>
        /// Here we are overriding the default inspector GUI with our own.
        /// </summary>
        public override void OnInspectorGUI()
        {
            // If we aren't in debug mode, show the custom inspector GUI.
            if (!debugMode)
            {
                // Create some spacing to let the UI breathe, and display a reference to the currently selected object at the top of the inspector window.
                EditorGUILayout.Space();
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField(new GUIContent("Base Class"), target, typeof(MonoScript), false);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.Space();

                // Update the object we are inspecting, and display the reorderable list.
                serializedObject.Update();
                list.DoLayoutList();

                // Update the object data with any changes we've made to clean up anything that might not have been saved last update.
                serializedObject.ApplyModifiedProperties();

                // Create more spacing.
                EditorGUILayout.Space();

                // If we have an entry currently selected, display all of the proper values.
                if (hasSelected)
                {
                    // First, get the element we currently have selected. This will be used for setting and getting values.
                    SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

                    // Is this entry an advanced entry? If it is, we want to do some special logic.
                    if (element.FindPropertyRelative("isAdvanced").boolValue)
                    {
                        // Create some spacing and label the fields properly.
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Control Flow", EditorStyles.boldLabel);

                        // Show the proper editable field for maxTimeUntilNextStep.
                        float maxTimeUntilNextStep = EditorGUILayout.FloatField(new GUIContent("Max Time Until Next Step", "The maximum amount of seconds before the next step."), element.FindPropertyRelative("maxTimeUntilNextStep").floatValue);
                        if (maxTimeUntilNextStep != element.FindPropertyRelative("maxTimeUntilNextStep").floatValue)
                        {
                            // The value changed, save the modifications to the object.
                            element.FindPropertyRelative("maxTimeUntilNextStep").floatValue = maxTimeUntilNextStep;
                            serializedObject.ApplyModifiedProperties();
                        }

                        // Show the proper editable field for minTimeUntilNextStep.
                        float minTimeUntilNextStep = EditorGUILayout.FloatField(new GUIContent("Min Time Until Next Step", "The minimum amount of seconds before the next step."), element.FindPropertyRelative("minTimeUntilNextStep").floatValue);
                        if (minTimeUntilNextStep != element.FindPropertyRelative("minTimeUntilNextStep").floatValue)
                        {
                            // The value changed, save the modifications to the object.
                            element.FindPropertyRelative("minTimeUntilNextStep").floatValue = minTimeUntilNextStep;
                            serializedObject.ApplyModifiedProperties();
                        }

                        // Show the proper editable field for waitUntilKill.
                        bool waitUntilKill = EditorGUILayout.Toggle(new GUIContent("Wait Until Kill", "Do we want to wait until the enemies in this step are killed before moving on to the next step?"), element.FindPropertyRelative("waitUntilKill").boolValue);
                        if (waitUntilKill != element.FindPropertyRelative("waitUntilKill").boolValue)
                        {
                            // The value changed, save the modifications to the object.
                            element.FindPropertyRelative("waitUntilKill").boolValue = !element.FindPropertyRelative("waitUntilKill").boolValue;
                            serializedObject.ApplyModifiedProperties();
                        }

                        // Create some spacing and label the fields properly.
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Spawn Rates", EditorStyles.boldLabel);

                        // Show the proper editable field for maxSpacing.
                        float maxSpacing = EditorGUILayout.FloatField(new GUIContent("Maximum Spacing", "The maximum number of time between spawns in seconds."), element.FindPropertyRelative("maxSpacing").floatValue);
                        if (maxSpacing != element.FindPropertyRelative("maxSpacing").floatValue)
                        {
                            // The value changed, save the modifications to the object.
                            element.FindPropertyRelative("maxSpacing").floatValue = maxSpacing;
                            serializedObject.ApplyModifiedProperties();
                        }

                        // Show the proper editable field for minSpacing.
                        float minSpacing = EditorGUILayout.FloatField(new GUIContent("Minimum Spacing", "The minimum number of time between spawns in seconds."), element.FindPropertyRelative("minSpacing").floatValue);
                        if (minSpacing != element.FindPropertyRelative("minSpacing").floatValue)
                        {
                            // The value changed, save the modifications to the object.
                            element.FindPropertyRelative("minSpacing").floatValue = minSpacing;
                            serializedObject.ApplyModifiedProperties();
                        }

                        // Create some spacing and label the fields properly.
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Enemy Count", EditorStyles.boldLabel);

                        // Show the proper editable field for id.
                        int id = EditorGUILayout.IntField(new GUIContent("Enemy ID", "The ID of the enemy that we want to spawn."), element.FindPropertyRelative("id").intValue);
                        if (id != element.FindPropertyRelative("id").intValue)
                        {
                            // The value changed, save the modifications to the object.
                            element.FindPropertyRelative("id").intValue = id;
                            serializedObject.ApplyModifiedProperties();
                        }

                        // Show the proper editable field for maxNumberOfEnemies.
                        int maxNumberOfEnemies = EditorGUILayout.IntField(new GUIContent("Max Number Of Enemies", "The maximum number of enemies to spawn this step."), element.FindPropertyRelative("maxNumberOfEnemies").intValue);
                        if (maxNumberOfEnemies != element.FindPropertyRelative("maxNumberOfEnemies").intValue)
                        {
                            // The value changed, save the modifications to the object.
                            element.FindPropertyRelative("maxNumberOfEnemies").intValue = maxNumberOfEnemies;
                            serializedObject.ApplyModifiedProperties();
                        }

                        // Show the proper editable field for minNumberOfEnemies.
                        int minNumberOfEnemies = EditorGUILayout.IntField(new GUIContent("Min Number Of Enemies", "The minimum number of enemies to spawn this step."), element.FindPropertyRelative("minNumberOfEnemies").intValue);
                        if (minNumberOfEnemies != element.FindPropertyRelative("minNumberOfEnemies").intValue)
                        {
                            // The value changed, save the modifications to the object.
                            element.FindPropertyRelative("minNumberOfEnemies").intValue = minNumberOfEnemies;
                            serializedObject.ApplyModifiedProperties();
                        }

                        // Create some spacing and label the fields properly.
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Other Settings", EditorStyles.boldLabel);

                        // Show the proper editable field for skipChance.
                        int skipChance = EditorGUILayout.IntField(new GUIContent("Skip Chance", "The chance, in a percentage, to skip this step."), element.FindPropertyRelative("skipChance").intValue);
                        if (skipChance != element.FindPropertyRelative("skipChance").intValue)
                        {
                            // The value changed, save the modifications to the object.
                            element.FindPropertyRelative("skipChance").intValue = skipChance;
                            serializedObject.ApplyModifiedProperties();
                        }

                        // Show the proper editable field for isAdvanced.
                        bool isAdvanced = EditorGUILayout.Toggle(new GUIContent("Is Advanced"), element.FindPropertyRelative("isAdvanced").boolValue);
                        if (isAdvanced != element.FindPropertyRelative("isAdvanced").boolValue)
                        {
                            // The value changed, save the modifications to the object.
                            element.FindPropertyRelative("isAdvanced").boolValue = !element.FindPropertyRelative("isAdvanced").boolValue;
                            serializedObject.ApplyModifiedProperties();
                        }

                        // Show the proper editable field for _debugMode.
                        bool _debugMode = EditorGUILayout.Toggle(new GUIContent("Debug Mode"), debugMode);
                        if (_debugMode != debugMode)
                        {
                            // The value changed, save the modifications to the object.
                            debugMode = _debugMode;
                            serializedObject.ApplyModifiedProperties();
                        }

                        // Create some spacing.
                        EditorGUILayout.Space();

                        // Create a special button, that when pressed, deselects the current entry.
                        if (GUILayout.Button("Deselect Step"))
                        {
                            hasSelected = false;
                        }
                    }
                    else
                    {
                        // Create some spacing and label the fields properly.
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Control Flow", EditorStyles.boldLabel);

                        // Show the proper editable field for maxTimeUntilNextStep.
                        float maxTimeUntilNextStep = EditorGUILayout.FloatField(new GUIContent("Max Time Until Next Step", "The maximum amount of seconds before the next step."), element.FindPropertyRelative("maxTimeUntilNextStep").floatValue);
                        if (maxTimeUntilNextStep != element.FindPropertyRelative("maxTimeUntilNextStep").floatValue)
                        {
                            // The value changed, save the modifications to the object.
                            element.FindPropertyRelative("maxTimeUntilNextStep").floatValue = maxTimeUntilNextStep;
                            serializedObject.ApplyModifiedProperties();
                        }
                        element.FindPropertyRelative("minTimeUntilNextStep").floatValue = element.FindPropertyRelative("maxTimeUntilNextStep").floatValue;

                        // Show the proper editable field for waitUntilKill.
                        bool waitUntilKill = EditorGUILayout.Toggle(new GUIContent("Wait Until Kill", "Do we want to wait until the enemies in this step are killed before moving on to the next step?"), element.FindPropertyRelative("waitUntilKill").boolValue);
                        if (waitUntilKill != element.FindPropertyRelative("waitUntilKill").boolValue)
                        {
                            // The value changed, save the modifications to the object.
                            element.FindPropertyRelative("waitUntilKill").boolValue = !element.FindPropertyRelative("waitUntilKill").boolValue;
                            serializedObject.ApplyModifiedProperties();
                        }

                        // Create some spacing and label the fields properly.
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Spawn Rates", EditorStyles.boldLabel);

                        // Show the proper editable field for maxSpacing.
                        float maxSpacing = EditorGUILayout.FloatField(new GUIContent("Spacing", "The number of time between spawns in seconds."), element.FindPropertyRelative("maxSpacing").floatValue);
                        if (maxSpacing != element.FindPropertyRelative("maxSpacing").floatValue)
                        {
                            // The value changed, save the modifications to the object.
                            element.FindPropertyRelative("maxSpacing").floatValue = maxSpacing;
                            serializedObject.ApplyModifiedProperties();
                        }
                        element.FindPropertyRelative("minSpacing").floatValue = element.FindPropertyRelative("maxSpacing").floatValue;
                        serializedObject.ApplyModifiedProperties();

                        // Create some spacing and label the fields properly.
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Enemy Count", EditorStyles.boldLabel);

                        // Show the proper editable field for id.
                        int id = EditorGUILayout.IntField(new GUIContent("Enemy ID", "The ID of the enemy that we want to spawn."), element.FindPropertyRelative("id").intValue);
                        if (id != element.FindPropertyRelative("id").intValue)
                        {
                            // The value changed, save the modifications to the object.
                            element.FindPropertyRelative("id").intValue = id;
                            serializedObject.ApplyModifiedProperties();
                        }

                        // Show the proper editable field for maxnumberOfEnemies.
                        int maxNumberOfEnemies = EditorGUILayout.IntField(new GUIContent("Enemy Count", "The amount of enemies to spawn."), element.FindPropertyRelative("maxNumberOfEnemies").intValue);
                        if (maxNumberOfEnemies != element.FindPropertyRelative("maxNumberOfEnemies").intValue)
                        {
                            // The value changed, save the modifications to the object.
                            element.FindPropertyRelative("maxNumberOfEnemies").intValue = maxNumberOfEnemies;
                            serializedObject.ApplyModifiedProperties();
                        }

                        // Since this is simple mode, we do not want to have these properties editable.
                        element.FindPropertyRelative("minNumberOfEnemies").intValue = element.FindPropertyRelative("maxNumberOfEnemies").intValue;
                        element.FindPropertyRelative("skipChance").intValue = 0;
                        serializedObject.ApplyModifiedProperties();

                        // Create some spacing.
                        EditorGUILayout.Space();

                        // Create a special button, that when pressed, deselects the current entry.
                        if (GUILayout.Button("Deselect Step"))
                        {
                            hasSelected = false;
                        }
                    }
                }
                else
                {
                    // There is no currently selected step, so we need to select one first. Otherwise, there may be null reference exceptions.
                    EditorGUILayout.HelpBox("Select a step to modify it!", MessageType.Info);
                }
            }
            else
            {
                // Since we are in debug mode, we want to draw the normal inspector GUI.
                base.OnInspectorGUI();

                // We also need to append this toggle at the end, otherwise we will have no way of turning debug mode back off.
                bool _debugMode = EditorGUILayout.Toggle(new GUIContent("Debug Mode"), debugMode);
                if (_debugMode != debugMode)
                {
                    // The value changed, save the modifications to the object.
                    debugMode = _debugMode;
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
        #endregion

        #region CreationHandler() Method
        /// <summary>
        /// Handles creating a new entry in the list.
        /// </summary>
        /// <param name="target">The target data the new entry should have.</param>
        private void CreationHandler(object target)
        {
            // We want to save the data and parse it into the proper struct.
            StepCreationParams data = (StepCreationParams)target;

            // Get the index of the newly added element, and adjust the array size accordingly.
            int index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;

            // Set the selected entry in the list to the newly added element.
            list.index = index;

            // Get the SerializedProperty from the element, effectively giving us a reference to where the data is stored.
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

            // Set the default values for the newly created entry.
            element.FindPropertyRelative("isAdvanced").boolValue = data.isAdvanced;
            element.FindPropertyRelative("waitUntilKill").boolValue = false;
            element.FindPropertyRelative("maxSpacing").floatValue = 1f;
            element.FindPropertyRelative("minSpacing").floatValue = 1f;
            element.FindPropertyRelative("id").intValue = 0;
            element.FindPropertyRelative("maxNumberOfEnemies").intValue = 0;
            element.FindPropertyRelative("minNumberOfEnemies").intValue = 0;
            element.FindPropertyRelative("skipChance").intValue = 0;
            element.FindPropertyRelative("maxTimeUntilNextStep").floatValue = 1f;
            element.FindPropertyRelative("minTimeUntilNextStep").floatValue = 1f;

            // Apply the changes to the entry and deselect it.
            serializedObject.ApplyModifiedProperties();
            hasSelected = false;
        }
        #endregion

        #region StepCreationParams Struct
        /// <summary>
        /// StepCreationParams contains data for the custom editor script.
        /// It lets the script know what the new list entry should have as data.
        /// </summary>
        private struct StepCreationParams
        {
            // Is the step we are creating an advanced step?
            public bool isAdvanced;
        }
        #endregion
    }
    #endregion
}
