using System;

namespace Terrence
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    internal sealed class TerrenceServiceAttribute : Attribute
    {
        public string Command { get; private set; }

        public TerrenceServiceAttribute(string command)
        {
            Command = command;
        }
    }
}
