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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpHtml;
//using SharpHtml.SimplePage;
using SharpHtml.Pages;

using static SharpHtml.TableTemplate;

using Examples;

namespace Entry {


	/////////////////////////////////////////////////////////////////////////////

	static class Program {

		/////////////////////////////////////////////////////////////////////////////

		//static void RunSimplePageExamples( IEnumerable<string> items )
		//{
		//	// ******
		//	var page = new SimplePageExamples { };

		//	//page.CreatePage();
		//	//page.CreatePageWithContent1();
		//	//page.CreatePageWithContent2();
		//	page.CreatePageWithSidebar();
		//	page.CreatePageWithFixedSidebar();

		//}


		/////////////////////////////////////////////////////////////////////////////

		static public IEnumerable<Tuple<string, string>> TableExamples()
		{
			// ******
			Tuple<string, string> result = null;

			result = new TableWithData().Build();
			yield return result;

			result = new TableWithFormatting1().Build();
			yield return result;

			result = new TableWithFormatting2().Build();
			yield return result;

			result = new TableWithFormatting3().Build();
			yield return result;

			result = new TableWithTemplate1().Build();
			yield return result;

			result = new TableWithTemplate2().Build();
			yield return result;
		}


		/////////////////////////////////////////////////////////////////////////////

		static void RunTableExamples1( IEnumerable<string> items )
		{
			// ******
			int index = 0;
			string directory = string.Empty;
			var tagList = new TagList { };

			// ******
			foreach( var example in TableExamples() ) {
				var htmlPath = example.Item1;
				var about = example.Item2;

				if( 0 == index ) {
					directory = Path.GetDirectoryName( htmlPath );
				}

				tagList.AddChild( new A( "file:///" + htmlPath ).SetValue( about ) );
			}

			//var div = new Div().AppendChildren( tagList );

			var list = new Ul().AddListItems( tagList );

			// ******
			//var page = new SimplePage<SimpleHtml> { };
			//page.AddBodyContent( tagList );
			//var html = page.Html.Render();

			var html = new BasicHtml( "Index" );
			html.Content.AddChild( list );
			var text = html.Render();

			File.WriteAllText( Path.Combine( directory,"index.html" ), text );
		}


		/////////////////////////////////////////////////////////////////////////////

		static void RunPageExamples( IEnumerable<string> items )
		{
			// ******
			var page = new PageExamples { };

			page.CreateBasicPage();
			page.CreateSimplePage();
			page.CreateExtensiblePage();


		}


		/////////////////////////////////////////////////////////////////////////////

		static void Main( string [] args )
		{
			//RunTableExamples1( args );

			RunPageExamples( args );
		}

	}
}
