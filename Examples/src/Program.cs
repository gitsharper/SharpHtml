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


using Examples;
//using Tests;

namespace Entry {


	/////////////////////////////////////////////////////////////////////////////

	static class Program {

		/////////////////////////////////////////////////////////////////////////////

		static void RunSimplePageExamples( IEnumerable<string> items )
		{
			// ******
			var page = new SimplePageExamples { };

			//page.CreatePage();
			//page.CreatePageWithContent1();
			//page.CreatePageWithContent2();
			page.CreatePageWithSidebar();
			page.CreatePageWithFixedSidebar();

		}


		/////////////////////////////////////////////////////////////////////////////

		static void RunTableExamples1( IEnumerable<string> items )
		{
			// ******
			var table = new TableExamples { };

			table.CreateTable();
			table.TableWithData();
			table.TableWithSomeFormatting1();
			table.TableWithSomeFormatting2();
			table.TableWithSomeFormatting3();
			table.TableWithTemplate1();
			table.TableWithTemplate2();

			//table.TransactionsTable();


		}

		
		/////////////////////////////////////////////////////////////////////////////

		static void Main( string [] args )
		{
			//RunTableExamples1( args );
			RunSimplePageExamples( args );
    }

	}
}
