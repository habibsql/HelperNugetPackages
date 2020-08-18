using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Utitlity.Nuget.Packages.DynamicCodeExecution
{
    /* Nuget Package Dependency: Microsoft.CodeAnalysis.CSharp.Scripting  */

    /// <summary>
    /// Dynamic code evaluation checking. Runtime code is compliled and evaluated.
    /// </summary>
    public class DynamicCodeExecutionTests
    {

        [Fact]
        public async Task ExecuteExpressionTest()
        {
            var dic = new Dictionary<string, object>
            {
                { "a", 100 },
                { "b", 200 }
            };

            var emp = new Employee
            {
                Dictionary = dic
            };

            bool matched = await Match(emp);

            Assert.True(matched);

            dic["a"] = 101;
            matched = await Match(emp);

            Assert.False(matched);
        }

        private async Task<bool> Match(Employee emp)
        {
            string conditions = @$"var x = (int)Dictionary[""a""]; x == 100";

            Script<bool> script = CSharpScript.Create<bool>(conditions, globalsType: typeof(Employee));

            Compilation comp = script.GetCompilation();
            ScriptRunner<bool> runner = script.CreateDelegate();
            bool result = await runner.Invoke(emp);

            //string conditions = @$"var x = param[0]; x == 100;";
            //var result = await CSharpScript.EvaluateAsync<bool>(conditions, null, globals: emp);
            //Console.WriteLine(result);

            return result;
        }

        public class Employee
        {
            public int Salary { get; set; }

            public Department Department { get; set; }

            public Dictionary<string, object> Dictionary { get; set; }
        }

        public class Department
        {
            public string Name { get; set; }
        }
    }
}
