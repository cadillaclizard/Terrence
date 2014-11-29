using System;
using System.Linq;
using System.Reflection;

namespace Terrence
{
    static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length == 0)
                return;

            var services = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetCustomAttributes(typeof(TerrenceServiceAttribute), true).Length > 0);

            foreach (var type in services)
            {
                try
                {
                    var attribute = (TerrenceServiceAttribute) Attribute.GetCustomAttribute(type, typeof(TerrenceServiceAttribute));

                    if (args[0].Substring(1) != attribute.Command)
                        continue;

                    var instance = (ITerrenceService) Activator.CreateInstance(type);
                    instance.Run(args.Skip(1).ToArray());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    return;
                }
            }
        }
    }
}
