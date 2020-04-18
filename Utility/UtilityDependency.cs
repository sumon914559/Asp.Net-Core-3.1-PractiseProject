using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Utility.Helpers;

namespace Utility
{
   public class UtilityDependency
    {
        public static void AllDependency(IServiceCollection services)
        {
            services.AddSingleton(typeof(TaposRSA));
            
        }
    }
}
