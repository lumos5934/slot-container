using UnityEditor;
using UnityEngine;

namespace LLib.Editor
{
    [CustomEditor(typeof(ExampleContainerTest))]
    public class ExampleContainerTestEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            ExampleContainerTest t = (ExampleContainerTest)target;

            GUILayout.Space(10);
            GUILayout.Label("Inventory Controls", EditorStyles.boldLabel);

            GUILayout.BeginVertical("box");

            GUILayout.Label("Add Item");
            t.addItemConfig.key = EditorGUILayout.TextField("Key", t.addItemConfig.key);
            t.addItemConfig.name = EditorGUILayout.TextField("Name", t.addItemConfig.name);
            t.addItemConfig.maxStack = EditorGUILayout.IntField("Max Stack", t.addItemConfig.maxStack);
            t.addAmount = EditorGUILayout.IntField("Amount", t.addAmount);

            if (GUILayout.Button("Add"))
            {
                t.Add();
            }

            GUILayout.Space(10);

            GUILayout.Label("Remove Item");
            t.removeIndex = EditorGUILayout.IntField("Index", t.removeIndex);
            t.removeAmount = EditorGUILayout.IntField("Amount", t.removeAmount);

            if (GUILayout.Button("Remove"))
            {
                t.Remove();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Clear"))
            {
                t.Clear();
            }

            GUILayout.EndVertical();

            GUILayout.Space(10);

            DrawInventory(t);
        }

        private void DrawInventory(ExampleContainerTest t)
        {
            var container = t.container;

            if (container == null)
            {
                EditorGUILayout.HelpBox("Container is null", MessageType.Warning);
                return;
            }

            var slots = container.Slots;

            GUILayout.Label("Inventory View", EditorStyles.boldLabel);

            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            GUILayout.Label("Idx", GUILayout.Width(40));
            GUILayout.Label("Item", GUILayout.Width(120));
            GUILayout.Label("Count", GUILayout.Width(60));
            GUILayout.Label("Key", GUILayout.Width(100));
            GUILayout.EndHorizontal();

            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];

                GUILayout.BeginHorizontal();

                GUILayout.Label(i.ToString(), GUILayout.Width(40));

                if (slot.IsEmpty)
                {
                    GUILayout.Label("(empty)", GUILayout.Width(120));
                    GUILayout.Label("-", GUILayout.Width(60));
                    GUILayout.Label("-", GUILayout.Width(100));
                }
                else
                {
                    GUILayout.Label(slot.Item.Name, GUILayout.Width(120));
                    GUILayout.Label(slot.Count.ToString(), GUILayout.Width(60));
                    GUILayout.Label(slot.Item.Key, GUILayout.Width(100));
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }
    }
}
