using System.Collections.Generic;

namespace CHECS
{
    public abstract class CodeGenerator
    {
        public abstract string GeneratedScriptsFolder { get; }
        public abstract IEnumerable<GeneratedScript> GenerateScripts();
    }
}