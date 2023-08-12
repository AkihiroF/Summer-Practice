using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor
{
    public class BehaviorTreeEditor : EditorWindow
    {
        private BehaviorTreeGraphView _graphView;

        [MenuItem("Window/Behavior Tree Editor")]
        public static void OpenBehaviorTreeEditorWindow()
        {
            var window = GetWindow<BehaviorTreeEditor>();
            window.titleContent = new GUIContent("Behavior Tree Editor");
            window.Show();
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }

        private void ConstructGraphView()
        {
            _graphView = new BehaviorTreeGraphView
            {
                name = "Behavior Tree Graph"
            };
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            // Button actionNodeButton = new Button(clickEvent: () => { _graphView.CreateNode("Action Node", Vector2.one * 100); });
            // actionNodeButton.text = "Create Action Node";
            // toolbar.Add(actionNodeButton);
            //
            // Button conditionNodeButton = new Button(clickEvent: () => { _graphView.CreateNode("Condition Node", Vector2.one * 250); });
            // conditionNodeButton.text = "Create Condition Node";
            // toolbar.Add(conditionNodeButton);

            Button saveButton = new Button(clickEvent: () => { _graphView.SaveData(); });
            saveButton.text = "Save Tree";
            toolbar.Add(saveButton);
            
            Button loadButton = new Button(clickEvent: () => { _graphView.LoadData(); });
            loadButton.text = "Load Tree";
            toolbar.Add(loadButton);
            
            Button clearButton = new Button(clickEvent: () => { _graphView.ClearGraph(); });
            clearButton.text = "Clear";
            toolbar.Add(clearButton);

            rootVisualElement.Add(toolbar);
        }
    }
}