using Godot;
using System;
using System.Reflection;

namespace Autowire.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class OnReadyAttribute : Attribute
    {
        public object v;

        public OnReadyAttribute(object value = null)
        {
            v = value;
        }

        public void SetProp(PropertyInfo p, Node node)
        {
            if (p.PropertyType != v.GetType())
            {
                GD.Print($"Expected Type '{p.PropertyType}', but got '{v.GetType()}'");
                return;
            }
            p.SetValue(node, v);
        }

        public void CallMethod(MethodInfo m, Node node)
        {
            try
            {
                m.Invoke(node, null);
            }
            catch (Exception e)
            {
                GD.Print(e.ToString());
            }
        }
    }
}
