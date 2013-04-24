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
using X45Game.Effect;
using X45Game.Input;
using X45Game.Extensions;
using System.Reflection;
using System.Diagnostics;

namespace TrippingOctoNemesis.Extensions
{
    class ExtensionsManager
    {
        public List<Extension> Extensions = new List<Extension>();
        const string extensionFileName = "extension";


        public void BindInterfaceExtensions(Game game)
        {
            foreach(var extension in Extensions)
                if ((extension.Type & ExtensionTypes.Interface) == ExtensionTypes.Interface)
                {
                    var drawableGameComponents = extension.ImportedTypes.FindAll(p => p.IsSubclassOf(typeof(DrawableGameComponent)));

                    foreach (var component in drawableGameComponents)
                        game.Components.Add(Activator.CreateInstance(component, game) as DrawableGameComponent);//XXX: possible script error
                }
        }

        public void AddExtension(DirectoryInfo dir)
        {
            FileInfo extensionFile = dir.GetFiles().FirstOrDefault(p => p.Name.Substring(0, p.Name.Length - p.Extension.Length).ToLower() == extensionFileName);
            if (extensionFile == null) return;

            CompilerResults result = LoadScript(extensionFile.FullName);

            if (CheckForErrors(result,extensionFile.Name) == true) return;

            var extension=FindExtension(result.CompiledAssembly);
            if (extension!=null)
            {
                Extensions.Add(extension);

                foreach (var scriptFile in extension.Scripts)
                {
                    var script = LoadScript(dir.FullName + "\\" + scriptFile);
                    CheckForErrors(script, dir.FullName+"\\"+scriptFile);
                    extension.ImportedTypes.AddRange(script.CompiledAssembly.GetTypes());
                }
            }
        }

        private bool CheckForErrors(CompilerResults result, string fileName)
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
                string str = "Error loading exptension " + fileName + "\r\n" + errors.ToString();
                Console.WriteLine(str);
                return true;
            }
            return false;
        }

        private Extension FindExtension(Assembly assembly)
        {
            var type = assembly.GetTypes().First(p => p.IsClass && p.IsPublic && p.IsSubclassOf(typeof(Extension)));

            if (type == null)
            {
                string str = "Error loading exptension assembly " + assembly.FullName + "\r\n" + "Could not find any Extension subclass.";
                Console.WriteLine(str);
                return null;
            }

            Extension extension = (Extension)Activator.CreateInstance(type);
            Extensions.Add(extension);
            return extension;
        }

        //private void GetPlugins(Assembly assembly,Type classType)
        //{
        //    foreach (Type type in assembly.GetTypes())
        //    {
        //        if (!type.IsClass || type.IsNotPublic) continue;
        //        Type[] interfaces = type.GetInterfaces();
        //        if (((IList<Type>)interfaces).Contains(typeof(IScript)))
        //        {
        //            IScript iScript = (IScript)Activator.CreateInstance(type);
        //            iScript.Initialize(m_host);
        //            // add the script details to a collection 

        //            ScriptDetails.Add(string.Format("{0} ({1})\r\n",
        //            iScript.Name, iScript.Description));
        //        }
        //    }
        //}

        private CompilerResults LoadScript(string filepath)
        {
            string language = CSharpCodeProvider.GetLanguageFromExtension(Path.GetExtension(filepath));
            CodeDomProvider codeDomProvider = CSharpCodeProvider.CreateProvider(language);
            CompilerParameters compilerParams = new CompilerParameters();
            compilerParams.GenerateExecutable = false;
            compilerParams.GenerateInMemory = true;
#if DEBUG
            compilerParams.IncludeDebugInformation = true;
#else
            compilerParams.IncludeDebugInformation = false;
#endif

            //string path=Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().);
            //string extAssembly = Path.Combine(
            //             ,
            //              "Extensibility.dll");
            compilerParams.ReferencedAssemblies.Add(Assembly.GetCallingAssembly().Location);
            //XXX
            compilerParams.ReferencedAssemblies.Add("mscorlib.dll");
            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add("System.Drawing.dll");
            compilerParams.ReferencedAssemblies.Add("System.Core.dll");
            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add("System.Xml.dll");
            compilerParams.ReferencedAssemblies.Add("System.Xml.Linq.dll");
            compilerParams.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Microsoft XNA\XNA Game Studio\v4.0\References\Windows\x86\Microsoft.Xna.Framework.dll");
            compilerParams.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Microsoft XNA\XNA Game Studio\v4.0\References\Windows\x86\Microsoft.Xna.Framework.Graphics.dll");
            compilerParams.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Microsoft XNA\XNA Game Studio\v4.0\References\Windows\x86\Microsoft.Xna.Framework.Game.dll");
            //compilerParams.ReferencedAssemblies.Add("Microsoft.Xna.Framework.Graphics.dll");
            compilerParams.ReferencedAssemblies.Add("X45Game.dll");

            return codeDomProvider.CompileAssemblyFromFile(compilerParams, filepath);
        }
    }
}