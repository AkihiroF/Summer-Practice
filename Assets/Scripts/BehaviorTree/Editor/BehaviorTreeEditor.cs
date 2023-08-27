using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BehaviorTree.Editor.SaveSystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace BehaviorTree.Editor
{
    public class BehaviorTreeEditor : EditorWindow
    {
        private BehaviorTreeGraphView _graphView;
        private VisualElement menu;
        private PopupField<string> dropDownList;
        

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
            toolbar.Add(CreateButton("Create Tree", ShowMenu));

            menu = new VisualElement();
            menu.style.display = DisplayStyle.None;
            
            TextField textField = new TextField("Введите название:");
            menu.Add(textField);
            menu.Add(CreateButton("Save", () =>
            {
                CreateTree(textField.value);
                HideMenu();
            }));
            CreateListTrees();
            _graphView.NameTree = dropDownList;
            toolbar.Add(dropDownList);
            rootVisualElement.Add(menu);
            rootVisualElement.Add(toolbar);
        }

        private void CreateListTrees()
        {
            var treesName = GetNamesTrees();
            dropDownList = new PopupField<string>("Trees", treesName,0);
        }

        private List<string> GetNamesTrees()
        {
            var treesName = System.IO.Directory.GetDirectories(NodeSaver.PathSaveTrees).ToList();
            var currentNames = new List<string>();
            foreach (var directory in treesName)
            {
                currentNames.Add(System.IO.Path.GetFileName(directory));
            }

            return currentNames;
        }

        // Helper method to create a button with a given text and click event
        private Button CreateButton(string text, Action clickEvent)
        {
            Button button = new Button(clickEvent: clickEvent);
            button.text = text;
            return button;
        }

        private void ShowMenu()
        {
            menu.style.display = DisplayStyle.Flex;
        }

        private void HideMenu()
        {
            menu.style.display = DisplayStyle.None;
        }

        private void CreateTree(string name)
        {
            var path = NodeSaver.PathSaveTrees + name + "/";
            _graphView.ClearGraph();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }

            dropDownList.choices = GetNamesTrees();
            dropDownList.value = name;
        }
    }
}
