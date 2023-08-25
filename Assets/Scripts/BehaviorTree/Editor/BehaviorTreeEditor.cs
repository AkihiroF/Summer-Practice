using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor
{
    public class BehaviorTreeEditor : EditorWindow
    {
        private BehaviorTreeGraphView _graphView;

        // Menu item to open the Behavior Tree Editor window
        [MenuItem("Window/Behavior Tree Editor")]
        public static void OpenBehaviorTreeEditorWindow()
        {
            var window = GetWindow<BehaviorTreeEditor>();
            window.titleContent = new GUIContent("Behavior Tree Editor");
            window.Show();
        }

        // Called when the window is enabled, constructs the graph view and toolbar
        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
        }

        // Called when the window is disabled, removes the graph view
        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }

        // Constructs the graph view for the behavior tree
        private void ConstructGraphView()
        {
            _graphView = new BehaviorTreeGraphView
            {
                name = "Behavior Tree Graph"
            };
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        // Generates the toolbar with buttons for saving, loading, clearing, and naming the tree
        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            toolbar.Add(CreateButton("Save Tree", _graphView.SaveData));
            toolbar.Add(CreateButton("Load Tree", _graphView.LoadData));
            toolbar.Add(CreateButton("Clear", _graphView.ClearGraph));

            TextField nameTree = new TextField("Name Tree");
            _graphView.NameTree = nameTree;
            toolbar.Add(nameTree);

            rootVisualElement.Add(toolbar);
        }

        // Helper method to create a button with a given text and click event
        private Button CreateButton(string text, Action clickEvent)
        {
            Button button = new Button(clickEvent: clickEvent);
            button.text = text;
            return button;
        }
    }
}
