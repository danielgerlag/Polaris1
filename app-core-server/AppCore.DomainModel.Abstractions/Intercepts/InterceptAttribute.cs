using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.DomainModel.Abstractions.Intercepts
{
    [AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]    
    public class InterceptAttribute : Attribute
    {
        public Type Intercept { get; set; }        
        public Stage Stage { get; set; }
        public int Order { get; set; }

        public InterceptAttribute(Type intercept, Stage stage, int order)
        {
            Intercept = intercept;
            Stage = stage;
            Order = order;
        }
    }

    public enum Stage { OnAddBeforeCommit, OnChangeBeforeCommit, OnAddAfterCommit, OnChangeAfterCommit }
}
