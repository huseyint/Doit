using System.Collections.Generic;

namespace Doit.Infrastructure
{
	public interface ISingleInstanceApp
	{ 
		void SignalExternalCommandLineArgs(IList<string> args); 
	}
}