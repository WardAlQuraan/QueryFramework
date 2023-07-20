using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.ATTRIBUTES
{
    [AttributeUsage(AttributeTargets.Class ,Inherited = true)]
    public class SoftDeleteAttribute : Attribute
    {

    }
}
