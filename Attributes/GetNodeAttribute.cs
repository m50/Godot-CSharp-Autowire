using Godot;
using System;
using System.Reflection;

namespace Autowire.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class GetNodeAttribute : Attribute
    {
        public string path;
        public bool nullable;
        public bool curScene;

        public GetNodeAttribute(bool nullable = false)
        {
            path = "";
            this.nullable = nullable;
            curScene = false;
        }
        public GetNodeAttribute(string nodePath, bool nullable = false, bool currentScene = false)
        {
            path = nodePath;
            this.nullable = nullable;
            curScene = currentScene;
        }

        public void SetNode(FieldInfo f, Node node)
        {
            Node nodeInstance;
            if (curScene) nodeInstance = node.GetTree().CurrentScene.GetNodeOrNull(path);
            else if (path == "")
            {
                var autoloadName = f.Name.TrimStart('_').UcFirst();
                nodeInstance = node.GetNodeOrNull(autoloadName);
            }
            else nodeInstance = node.GetNodeOrNull(path);
            if (nodeInstance == null && !nullable)
            {
                GD.PrintErr($"Cannot find Node for NodePath '{path}'");
                return;
            }
            else if (nodeInstance != null && f.FieldType.IsSubclassOf(nodeInstance.GetType()))
            {
                GD.PrintErr($"For NodePath '{path}', expected Type '{f.FieldType}', but got '{nodeInstance.GetType()}'");
                return;
            }
            f.SetValue(node, nodeInstance);
        }
    }
}
