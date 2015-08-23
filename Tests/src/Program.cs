#region License
// 
// Author: Joe McLain <nmp.developer@outlook.com>
// Copyright (c) 2015, Joe McLain and Digital Writing
// 
// Licensed under Eclipse Public License, Version 1.0 (EPL-1.0)
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tests;

namespace Entry {


	/////////////////////////////////////////////////////////////////////////////

	static class Program {

		/////////////////////////////////////////////////////////////////////////////

		static void RunTests( IEnumerable<string> items )
		{
			// ******
			var page = new SimplePageTests { };

			//page.CreatePage();

		}

		
		/////////////////////////////////////////////////////////////////////////////

		static void Main( string [] args )
		{
			//try {
			//	var host = new NmpHost();
			//	Environment.ExitCode = host.Run();
			//}
			//catch( Exception ex ) {
			//	if( Debugger.IsAttached ) {
			//		Debugger.Break();
			//	}
			//}
			RunTests( args );
		}

	}
}
