using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modularity
{
	[AttributeUsage(AttributeTargets.Field)]
	internal class SynchronousOnlyEventAttribute : Attribute
	{
	}
}
