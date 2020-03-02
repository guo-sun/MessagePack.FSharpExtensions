// Copyright (c) 2017 Yoshifumi Kawai

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Reflection;
using System.Reflection.Emit;

namespace MessagePack.FSharp.Internal
{
    public class DynamicAssembly
    {
        readonly AssemblyBuilder assemblyBuilder;
        readonly ModuleBuilder moduleBuilder;

        public ModuleBuilder ModuleBuilder { get { return moduleBuilder; } }

        public AssemblyBuilder AssemblyBuilder { get { return assemblyBuilder; } }

        #if NETFRAMEWORK
        string moduleName;
        public void Save() {
            assemblyBuilder.Save(moduleName + ".dll");
        }
        #endif

        public DynamicAssembly(string moduleName) // TODO filename only for net48
        {

            #if NETFRAMEWORK
            this.moduleName = moduleName;
            AssemblyBuilderAccess assemblyAccess = AssemblyBuilderAccess.RunAndSave;
            #else
            AssemblyBuilderAccess assemblyAccess = AssemblyBuilderAccess.Run;
            #endif

            this.assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(moduleName), assemblyAccess
            );

            #if NETFRAMEWORK
            this.moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName, moduleName + ".dll");
            #else
            this.moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName);
            #endif
        }
    }
}