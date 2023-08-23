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

            Button saveButton = new Button(clickEvent: () => { _graphView.SaveData(); });
            saveButton.text = "Save Tree";
            toolbar.Add(saveButton);
            
            Button loadButton = new Button(clickEvent: () => { _graphView.LoadData(); });
            loadButton.text = "Load Tree";
            toolbar.Add(loadButton);
            
            Button clearButton = new Button(clickEvent: () => { _graphView.ClearGraph(); });
            clearButton.text = "Clear";
            toolbar.Add(clearButton);

            TextField nameTree = new TextField("Name Tree");
            _graphView.NameTree = nameTree;
            toolbar.Add(nameTree);

            rootVisualElement.Add(toolbar);
        }
    }
}