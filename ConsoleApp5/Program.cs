using Common;
using SimpleInjector;
using System;
using System.Linq;
using System.IO;
using System.Reflection;

namespace ConsoleApp5
{
    public class DefaultPlugin : IPlugin
    {
        public void Execute()
        {
            Console.WriteLine(GetType().FullName);
        }
    }

    class Program
    {
        private static Container container = new Container();

        static void Main(string[] args)
        {
            container.ResolveUnregisteredType += Container_ResolveUnregisteredType;

            container.Register<ILogger, ConsoleLogger>();
            container.Options.AllowOverridingRegistrations = true;

            container.Verify();

            string pluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

            var pluginAssemblies =
                from file in new DirectoryInfo(pluginDirectory).GetFiles()
                where file.Extension.ToLower() == ".dll"
                select Assembly.Load(AssemblyName.GetAssemblyName(file.FullName));

            var pluginTypes = pluginAssemblies
                .SelectMany(a => 
                    a.Modules
                        .SelectMany(m => 
                            m.GetTypes()
                                .Where(t => typeof(IPlugin).IsAssignableFrom(t))));

            var pluginType = pluginTypes.First();
            var plugin2 = container.GetInstance(pluginType) as IPlugin;
            plugin2.Execute();

        }

        private static void Container_ResolveUnregisteredType(object sender, UnregisteredTypeEventArgs e)
        {
            if (typeof(IPlugin).IsAssignableFrom(e.UnregisteredServiceType))
            {
                var pluginType = e.UnregisteredServiceType;
                var constructor = pluginType.GetConstructors().Single();

                var parameters = constructor.GetParameters()
                    .Select(p => container.GetInstance(p.ParameterType)).ToArray();
                
                e.Register(() => constructor.Invoke(parameters));

                //e.Register(() => Activator.CreateInstance(e.UnregisteredServiceType));

            }
        }
    }
}
