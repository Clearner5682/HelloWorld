using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class EngineContext
    {
        private static IEngine engine;
        public static void Init(IServiceProvider serviceProvider)
        {
            if (engine == null)
            {
                engine = new Engine(serviceProvider);
            }
        }

        public static IEngine Current
        {
            get
            {
                return engine;
            }
        }
    }
}
