using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.EXCEPTION_RESPONSE
{
    public class FamilyValidationException : Exception
    {
        public FamilyValidationException():base()
        {
        }

        public FamilyValidationException(string? message) : base(message)
        {
        }
    }
}
