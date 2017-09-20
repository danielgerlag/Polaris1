using AppCore.DomainModel.Interface;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Services.Scripting
{
    public class ScriptRunner : IScriptRunner
    {


        public ScriptResult Run<TContext>(TContext context, string contextName, IDbContext db, string code, string language)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(contextName, context);
            return Run(parameters, db, code, language);
        }

        public ScriptResult Run(Dictionary<string, object> parameters, IDbContext db, string code, string language)
        {
            ScriptResult result = new ScriptResult();
            try
            {
                var engine = IoC.Container.ResolveKeyed<ScriptEngine>(language);
                var scope = engine.CreateScope();

                foreach (var param in parameters)
                    scope.SetVariable(param.Key, param.Value);

                scope.SetVariable("db", db);
                scope.SetVariable("ioc", new IoC.Adaptor());
                scope.SetVariable("log", result.Log);
                engine.Execute(PrepareScript(code, language), scope);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Log.Add(ex.Message);
            }
            return result;
        }


        public object ResolveExpression<TContext>(TContext context, string contextName, IDbContext db, string code, string language)
        {
            var engine = IoC.Container.ResolveKeyed<ScriptEngine>(language);
            var scope = engine.CreateScope();
            scope.SetVariable(contextName, context);
            scope.SetVariable("db", db);
            scope.SetVariable("ioc", new IoC.Adaptor());

            return engine.Execute(PrepareScript(code, language), scope);
        }

        public TResult ResolveExpression<TResult>(Dictionary<string, object> parameters, IDbContext db, string code, string language)
        {
            var engine = IoC.Container.ResolveKeyed<ScriptEngine>(language);
            var scope = engine.CreateScope();
            foreach (var param in parameters)
                scope.SetVariable(param.Key, param.Value);
            scope.SetVariable("db", db);
            scope.SetVariable("ioc", new IoC.Adaptor());

            return engine.Execute<TResult>(PrepareScript(code, language), scope);
        }

        private string PrepareScript(string script, string language)
        {
            StringBuilder sb = new StringBuilder();
            if (language == "Python")
            {
                sb.Append("import clr\r\n");
                sb.Append("import System\r\n");
                sb.Append("clr.AddReference(\"System.Core\")\r\n");
                sb.Append("clr.ImportExtensions(System.Linq)\r\n");
                sb.Append("clr.AddReference(\"AIMS.DomainModel\")\r\n");
                sb.Append("import AIMS.DomainModel.Services\r\n");
            }
            sb.Append(script);
            return sb.ToString();

        }
    }
}
