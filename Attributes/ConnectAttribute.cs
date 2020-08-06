using Godot;
using System;
using System.Reflection;
using static Godot.Object;

namespace Autowire.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ConnectAttribute : Attribute
    {
        public string path;
        public string sig;
        public uint f = 0;
        public Godot.Collections.Array b = null;

        public ConnectAttribute(string signal)
        {
            path = ".";
            sig = signal;
        }
        public ConnectAttribute(string nodePath, string signal)
        {
            path = nodePath;
            sig = signal;
        }
        public ConnectAttribute(string nodePath, string signal, ConnectFlags flags)
        {
            path = nodePath;
            sig = signal;
            f = (uint)flags;
        }
        public ConnectAttribute(string nodePath, string signal, Godot.Collections.Array binds, ConnectFlags flags)
        {
            path = nodePath;
            sig = signal;
            f = (uint)flags;
            b = binds;
        }

        public void Connect(MethodInfo m, Node node)
        {
            Node n = node.GetNode(path);
            if (n == null)
            {
                throw new NullReferenceException($"Cannot find Node for NodePath '{path}'");
            }
            n.Connect(sig, node, m.Name, b, f);
        }
    }
}
