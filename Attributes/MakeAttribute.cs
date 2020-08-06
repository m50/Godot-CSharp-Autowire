using Godot;
using System;
using System.Reflection;

namespace Autowire.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MakeAttribute : Attribute
    {
        public Type t;
        public string p = "";

        public MakeAttribute()
        {
            t = null;
        }

        public MakeAttribute(Type type)
        {
            t = type;
        }

        public MakeAttribute(string scenePath)
        {
            t = null;
            p = scenePath;
        }

        public void Make(FieldInfo f, Node node)
        {
            if (p != "")
            {
                if (!p.EndsWith(".tscn"))
                {
                    GD.PrintErr($"Path '{p}' is not a Scene resource, unable to instance it.");
                    return;
                }
                var scene = GD.Load(p) as PackedScene;
                var instance = scene.Instance();
                if (f.FieldType != instance.GetType())
                {
                    GD.PrintErr($"Expected Type '{f.FieldType}', but got '{instance.GetType()}'");
                    return;
                }
                node.AddChild(instance);
                f.SetValue(node, instance);
            }
            else
            {
                if (!f.FieldType.IsSubclassOf(typeof(Node)))
                {
                    GD.PrintErr($"Cannot add non-Node type {f.FieldType} as a child.");
                    return;
                }
                if (t != null && !t.IsSubclassOf(typeof(Node)))
                {
                    GD.PrintErr($"Cannot add non-Node type {t} as a child.");
                    return;
                }
                var instance = (Activator.CreateInstance(t ?? f.FieldType) as Node);
                if (f.FieldType.IsSubclassOf(instance.GetType()))
                {
                    GD.PrintErr($"Expected Type '{f.FieldType}', but got '{instance.GetType()}'");
                    return;
                }
                node.AddChild(instance as Node);
                f.SetValue(node, instance);
            }
        }
    }
}
