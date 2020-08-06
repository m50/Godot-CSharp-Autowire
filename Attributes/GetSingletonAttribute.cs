using Godot;
using System;
using System.Reflection;

namespace Autowire.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class GetSingletonAttribute : Attribute
    {
        public string path;

        public GetSingletonAttribute(string nodePath = "")
        {
            path = nodePath;
        }

        public void SetNode(FieldInfo f, Node node)
        {
            Node nodeInstance;
            if (path == "")
            {
                var autoloadName = f.Name.TrimStart('_').UcFirst();
                nodeInstance = node.GetNodeOrNull($"/root/{autoloadName}");
            }
            else nodeInstance = node.GetNodeOrNull($"/root/path");
            if (nodeInstance == null)
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
