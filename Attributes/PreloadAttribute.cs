using Godot;
using System;
using System.Reflection;

namespace Autowire.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PreloadAttribute : Attribute
    {
        public string res;

        public PreloadAttribute(string resourcePath)
        {
            res = resourcePath;
        }

        public void Preload(FieldInfo f, Node node)
        {
            var resource = GD.Load(res);
            if (resource == null)
            {
                GD.PrintErr($"Cannot find Resource for Resource Path '{res}'");
                return;
            }
            if (f.FieldType != resource.GetType())
            {
                GD.PrintErr($"For Resource '{res}', expected Type '{f.FieldType}', but got '{resource.GetType()}'");
                return;
            }
            f.SetValue(node, resource);
        }
    }
}
