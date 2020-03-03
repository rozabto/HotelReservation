using System;
using System.Collections.Generic;

namespace Application.Common.Exceptions
{
    public class ModelStateException : Exception
    {
        public IReadOnlyList<string> ModelStates { get; }

        public ModelStateException(IReadOnlyList<string> modelStates, string message) : base(message)
        {
            ModelStates = modelStates;
        }
    }
}
