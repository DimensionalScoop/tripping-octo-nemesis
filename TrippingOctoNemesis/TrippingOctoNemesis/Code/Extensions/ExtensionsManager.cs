using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X45Game;
using X45Game.Drawing;

using X45Game.Input;
using X45Game.Extensions;
using System.Reflection;
using System.Diagnostics;

namespace TrippingOctoNemesis.Extensions
{
    public class ExtensionsManager
    {
        public List<Extension> Extensions = new List<Extension>();
        const string extensionFileName = "extension";

        public List<Assembly> LoadedAssemblies = new List<Assembly>();
        public List<Map> Maps = new List<Map>();
        public List<Plugin> Plugins = new List<Plugin>();


        public void AddDirectory(string path)
        {
            Extensions.Add(new Extension(new DirectoryInfo(path)));
        }

        public void Load(Game game)
        {
            LoadDiscriptions();
            SortExtensions();
            CompileExtensions();
            Maps = CreateMaps().ToList();
            Plugins = CreatePlugins(game).ToList();

            Debug.WriteLine("Loaded extensions:");
            foreach (var elem in Extensions)
                Debug.WriteLine(elem.Info.Name + ", by " + elem.Info.Author + ". Type:" + Enum.GetName(typeof(ExtensionTypes), elem.Info.Type));
        }

        private void LoadDiscriptions()
        {
            for(int i=0;i<Extensions.Count;i++)
            {
                var elem = Extensions[i];
                var files= elem.Directory.GetFiles(extensionFileName+".*");
                if (files.Count() == 0||files.Count()>1) { Console.WriteLine("Could not find " + extensionFileName + " in " + elem.Directory.FullName); continue; }

                var result=CompileFiles(files.First().FullName);
                if (HasErrors(result, files.First().FullName))
                {
                    Console.WriteLine("Error loading extension " + files.First().FullName);
                    Extensions.RemoveAt(i);
                    i--;
                    continue;
                }
                elem.DescriptionFile = files.First();
                elem.Info = GetInstance<ExtensionDescription>(result.CompiledAssembly);
            }
        }
        
        private T GetInstance<T>(Assembly assembly) where T:class
        {
            var type = GetType<T>(assembly);

            T extension = Activator.CreateInstance(type) as T;
            return extension;
        }
        private Type GetType<T>(Assembly assembly) where T : class
        {
            var type = assembly.GetTypes().First(p => p.IsClass && p.IsPublic && p.IsSubclassOf(typeof(T)));

            if (type == null)
            {
                string str = "Error loading extension assembly " + assembly.FullName + "\r\n" + "Could not find any Extension subclass.";
                Console.WriteLine(str);
                return null;
            }

            return type;
        }
        private IEnumerable<Type> GetTypes<T>(Assembly assembly) where T : class
        {
            var type = assembly.GetTypes().Where(p => p.IsClass && p.IsPublic && p.IsSubclassOf(typeof(T)));

            return type;
        }

        private void SortExtensions()
        {
            Extensions.RemoveAll(p => p.Info == null);

            var sorted = new List<Extension>();

            sorted.AddRange(Extensions.FindAll(p => p.Info.Dependencies == null || p.Info.Dependencies.Length == 0));
            Extensions.RemoveAll(p =>p.Info.Dependencies==null|| p.Info.Dependencies.Length == 0);

            List<string> solvedDependencies = sorted.ConvertAll<string>(p => p.Info.Name);

            for (int i = 0; i < Extensions.Count; i++)
            {Debug.Assert(i != -1);

                if (SufficientDependencies(Extensions[i], solvedDependencies))
                {
                    sorted.Add(Extensions[i]);
                    solvedDependencies.Add(Extensions[i].Info.Name);
                    Extensions.RemoveAt(i);
                    i = -1;
                    continue;
                }
            }

            if (Extensions.Count > 0)
            {
                Console.WriteLine("Could not solve all extension dependencies. Extensions not loaded:");
                foreach (var elem in Extensions)
                    Console.WriteLine(elem.Info.Name);
                Console.WriteLine("\nMissing extensions:");
                var missing = new List<string>();
                foreach (var elem in Extensions)
                    missing.AddRange(elem.Info.Dependencies);
                foreach (var elem in solvedDependencies)
                    missing.RemoveAll(p => p == elem);
                foreach (var elem in missing)
                    Console.WriteLine(elem);
            }

            Extensions = sorted;
        }
        private bool SufficientDependencies(Extension ext, List<string> solvedDependencies)
        {
            for (int i = 0; i < ext.Info.Dependencies.Length; i++)
                if (!solvedDependencies.Contains(ext.Info.Dependencies[i]))
                    return false;
            return true;
        }

        private void CompileExtensions()
        {
            foreach (var elem in Extensions)
            {
                string[] files = elem.Directory.GetFiles("*" + elem.DescriptionFile.Extension, SearchOption.AllDirectories)
                    .Select(p=>p.FullName)
                    .Where(p=>p!=elem.DescriptionFile.FullName)
                    .ToArray();

                var result = CompileFiles(LoadedAssemblies, files);
                if (HasErrors(result, elem.Directory.FullName))
                {
                    Console.WriteLine("Errors in extension "+elem.Info.Name);
                    continue;
                }
                var assembly = result.CompiledAssembly;

                LoadedAssemblies.Add(assembly);
                elem.Maps = GetTypes<Map>(assembly).ToList();
                elem.Encounters = GetTypes<Encounter>(assembly).ToList();
                elem.Groups = GetTypes<Group>(assembly).ToList();
                elem.Enemies = GetTypes<Enemy>(assembly).ToList();
                elem.Plugins = GetTypes<Plugin>(assembly).ToList();
                elem.Scripts = GetTypes<Script>(assembly).ToList();
            }
        }

        private IEnumerable<Map> CreateMaps()
        {
            foreach (var elem in Extensions)
                if(elem.Maps!=null)
                foreach (var map in elem.Maps)
                {
                    yield return Activator.CreateInstance(typeof(Map)) as Map;
                }
        }

        private IEnumerable<Plugin> CreatePlugins(Game game)
        {
            foreach (var elem in Extensions)
                if (elem.Plugins!= null)
                foreach (var plugin in elem.Plugins)
                {
                    var item=Activator.CreateInstance(plugin,game) as Plugin;
                    game.Components.Add(item);
                    yield return item;
                }
        }

        private CompilerResults CompileFiles(params string[] filepath)
        {
            return CompileFiles(null, filepath);
        }
        private CompilerResults CompileFiles(List<Assembly> additionalAssemblys,params string[] filepath)
        {
            string language = CSharpCodeProvider.GetLanguageFromExtension(Path.GetExtension(filepath.First()));//XXX: Might miss-interpret .py-files
            CodeDomProvider codeDomProvider = CSharpCodeProvider.CreateProvider(language);
            CompilerParameters compilerParams = new CompilerParameters();
            compilerParams.GenerateExecutable = false;
            compilerParams.GenerateInMemory = true;
#if DEBUG
            compilerParams.IncludeDebugInformation = true;
#else
            compilerParams.IncludeDebugInformation = false;
#endif

            //compilerParams.ReferencedAssemblies.Add(Assembly.GetCallingAssembly().Location);
            //compilerParams.ReferencedAssemblies.Add("mscorlib.dll");
            //compilerParams.ReferencedAssemblies.Add("System.dll");
            //compilerParams.ReferencedAssemblies.Add("System.Drawing.dll");
            //compilerParams.ReferencedAssemblies.Add("System.Core.dll");
            //compilerParams.ReferencedAssemblies.Add("System.dll");
            //compilerParams.ReferencedAssemblies.Add("System.Xml.dll");
            //compilerParams.ReferencedAssemblies.Add("System.Xml.Linq.dll");
            //compilerParams.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Microsoft XNA\XNA Game Studio\v4.0\References\Windows\x86\Microsoft.Xna.Framework.dll");//XXX: Path to XNA dlls is static
            //compilerParams.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Microsoft XNA\XNA Game Studio\v4.0\References\Windows\x86\Microsoft.Xna.Framework.Graphics.dll");
            //compilerParams.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Microsoft XNA\XNA Game Studio\v4.0\References\Windows\x86\Microsoft.Xna.Framework.Game.dll");
            ////compilerParams.ReferencedAssemblies.Add("Microsoft.Xna.Framework.Graphics.dll");
            //compilerParams.ReferencedAssemblies.Add("X45Game.dll");

            var assemblies = AppDomain.CurrentDomain
                            .GetAssemblies()
                            .Where(a => !a.IsDynamic)
                            .Select(a => a.Location)
                            .ToArray();
            compilerParams.ReferencedAssemblies.AddRange(assemblies);//XXX: might not include all assemblies

            if (additionalAssemblys != null&&additionalAssemblys.Count>0)
                compilerParams.ReferencedAssemblies.AddRange(additionalAssemblys.Select(p => p.FullName).ToArray());//XXX: Might not work at all

            return codeDomProvider.CompileAssemblyFromFile(compilerParams, filepath);
        }

        private bool HasErrors(CompilerResults result, string fileName)
        {
            if (result.Errors.HasErrors)
            {
                StringBuilder errors = new StringBuilder();
                foreach (CompilerError err in result.Errors)
                {
                    errors.Append(string.Format("\r\n{0}({1},{2}): {3}: {4}",
                                fileName, err.Line, err.Column,
                                err.ErrorNumber, err.ErrorText));
                }
                Debug.WriteLine(errors);
                return true;
            }
            return false;
        }
    }
}