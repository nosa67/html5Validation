using System;
using System.Collections.Generic;
using System.Text;

namespace html5Validation.Validator
{
    public class ValidationH5Error : Exception
    {
        public ValidationH5Error() { }

        public ValidationH5Error(string message) : base(message) { }
    }
}
