using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modularity.Test.Stubs
{
	public class ModuleStub : ModularityModule
	{
		public int Counter = 0;
		public override void Initialize()
		{
			Counter++;
		}
	}
	public class ModuleStub2 : ModularityModule
	{
		public int Counter = 0;
		public override void Initialize()
		{
			Counter++;
		}
	}
}
