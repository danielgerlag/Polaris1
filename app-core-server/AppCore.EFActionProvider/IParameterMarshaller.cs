﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Providers;
using System.Data.Services;

namespace AppCore.EFActionProvider
{
    public interface IParameterMarshaller
    {
        object[] Marshall(DataServiceOperationContext operationContext, ServiceAction serviceAction, object[] parameters);
    }
}
