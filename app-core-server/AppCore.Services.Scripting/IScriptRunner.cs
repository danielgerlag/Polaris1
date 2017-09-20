using AppCore.DomainModel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Services.Scripting
{
    public interface IScriptRunner
    {
        ScriptResult Run<TContext>(TContext context, string contextName, IDbContext db, string code, string language);

        ScriptResult Run(Dictionary<string, object> parameters, IDbContext db, string code, string language);

        object ResolveExpression<TContext>(TContext context, string contextName, IDbContext db, string code, string language);

        TResult ResolveExpression<TResult>(Dictionary<string, object> parameters, IDbContext db, string code, string language);
    }
}
