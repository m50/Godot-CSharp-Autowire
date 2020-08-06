using Godot;
using System.Reflection;
using System.Collections.Generic;
using Autowire.Attributes;

namespace Autowire
{
    public static class NodeAutoWire
    {
        public static void AutoWire(this Node node)
        {
            _WireFields(node);
            _WireMethods(node);
        }

        private static void _WireFields(Node node)
        {
            List<FieldInfo> fields = new List<FieldInfo>(node
                .GetType()
                .GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            foreach (var field in fields)
            {
                field.GetCustomAttribute<MakeAttribute>()?.Make(field, node);
                field.GetCustomAttribute<GetNodeAttribute>()?.SetNode(field, node);
                field.GetCustomAttribute<PreloadAttribute>()?.Preload(field, node);
                field.GetCustomAttribute<GetSingletonAttribute>()?.SetNode(field, node);
            }
            List<PropertyInfo> props = new List<PropertyInfo>(node
                .GetType()
                .GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            props.RemoveAll(prop => prop.GetCustomAttribute<OnReadyAttribute>() == null);
            props.ForEach(prop => prop.GetCustomAttribute<OnReadyAttribute>()?.SetProp(prop, node));
        }

        private static void _WireMethods(Node node)
        {
            List<MethodInfo> methods = new List<MethodInfo>(node
                .GetType()
                .GetMethods(BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
            foreach (var method in methods)
            {
                method.GetCustomAttribute<ConnectAttribute>()?.Connect(method, node);
                method.GetCustomAttribute<OnReadyAttribute>()?.CallMethod(method, node);
            }
        }
    }
}
