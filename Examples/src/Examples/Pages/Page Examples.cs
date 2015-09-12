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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using SharpHtml;
using SharpHtml.Layouts;

using static SharpHtml.TableTemplate;
using SharpHtml.Pages;
using SharpHtml.Pages.Layouts;

namespace Examples {


	/////////////////////////////////////////////////////////////////////////////

	public class PageExamples {

		/////////////////////////////////////////////////////////////////////////////

		void Render( Tag tag, [CallerFilePath] string pathToSource = "", [CallerMemberName] string callerName = "" )
		{
			var result = tag.Render();

			var outputFilePath = $"{Path.GetDirectoryName( pathToSource )}\\{callerName}.html";

			File.WriteAllText( outputFilePath, result );
		}



		///////////////////////////////////////////////////////////////////////////////
		//
		//public void CreatePageWithFixedSidebar()
		//{
		//	// ******
		//	var title = "Page with Fixed Sidebar";
		//	var includePath = "include/";
		//
		//	var page = new SimplePage<SimpleHtml> { }.Initialize( title, includePath );
		//	var html = page.Html;
		//	var main = html.BodyMain;
		//
		//	// ******
		//	html.AddToHeader( new H1( title ).AddCssClass( "title" ) );
		//
		//	// ******
		//	var layout = new FixedSidebarLayout { };
		//
		//	layout.Sidebar.AddChild( new P( "This is the sidebar" ) )
		//		.AddCssClass( "footer-container" );
		//
		//	page.AddBodyContent( layout.Sidebar );
		//
		//	layout.Content.AppendChildren( SimplePageData.initializrBodyContent() );
		//	layout.Content.AppendChildren( SimplePageData.initializrBodyContent() );
		//
		//	page.AddBodyContent( layout.Content );
		//
		//	//
		//	// finally make the body a bit wider
		//	//
		//	main.AddStyle( "width", "1226px" );
		//
		//	// ******
		//	Render( page.Html );
		//}
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//public void CreatePageWithSidebar()
		//{
		//	// ******
		//	var title = "Page with Sidebar";
		//	var includePath = "include/";
		//
		//	var page = new SimplePage<SimpleHtml> { }.Initialize( title, includePath );
		//	var html = page.Html;
		//	var main = html.BodyMain;
		//
		//	// ******
		//	html.AddToHeader( new H1( title ).AddCssClass( "title" ) );
		//
		//	// ******
		//	var layout = new SidebarLayout { };
		//
		//	layout.Sidebar.AddChild( new P( "This is the sidebar" ) )
		//		.AddCssClass( "footer-container" )
		//		;
		//
		//	page.AddBodyContent( layout.Sidebar );
		//
		//	layout.Content.AppendChildren( SimplePageData.initializrBodyContent() );
		//	layout.Content.AppendChildren( SimplePageData.initializrBodyContent() );
		//	page.AddBodyContent( layout.Content );
		//
		//	//
		//	// finally make the body a bit wider
		//	//
		//	main.AddStyle( "width", "1226px" );
		//
		//	// ******
		//	Render( page.Html );
		//}
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//public void CreatePageWithContent2()
		//{
		//	// ******
		//	var title = "Page with Content";
		//	var includePath = "include/";
		//
		//	var page = new SimplePage<SimpleHtml> { }.Initialize( title, includePath );
		//	page.Html.AddToHeader( new H1( title ).AddCssClass( "title" ) );
		//
		//	// ******
		//	//
		//	// can add content with a single call
		//	//
		//	page.AddBodyContent( SimplePageData.initializrBodyContent() );
		//
		//	// ******
		//	Render( page.Html );
		//}
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//public void CreatePageWithContent1()
		//{
		//	// ******
		//	var title = "Page with Content";
		//	var includePath = "include/";
		//
		//	var page = new SimplePage<SimpleHtml> { }.Initialize( title, includePath );
		//	page.Html.AddToHeader( new H1( title ).AddCssClass( "title" ) );
		//
		//	// ******
		//	//
		//	// loop through data and add to body
		//	//
		//	foreach( var tag in SimplePageData.initializrBodyContent() ) {
		//		page.Html.BodyMain.AddChild( tag );
		//	}
		//
		//	// ******
		//	Render( page.Html );
		//}
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//public void CreatePage()
		//{
		//	var title = "Test";
		//	var includePath = "include/";
		//	var page = new SimplePage<SimpleHtml> { }
		//									.Initialize( title, includePath );
		//	Render( page.Html );
		//}


		///////////////////////////////////////////////////////////////////////////////

		public void CreateExtensiblePageWithLayout()
		{
			// ******
			var page = ExtensiblePage.Create( "Extensible Page" );
			SidebarLayout sidebarLayout;

			// ******
			//
			// AddLayout() returns a dynamic object that references and page
			//
			var dynPage = page.AddLayout( page.Content, sidebarLayout = new SidebarLayout { LayoutWidth = "1400px" } );
			//
			// we can use it dynamically
			//
			dynPage.Sidebar.AddChild( new A { }.SetValue( "Fixed sidebar content" ) );
			//
			// or, since the sidebar properties are now set on the page we can access them
			// in a type safe way - in other words we get compile time checking
			//
			var sidebar1 = page.Get<Tag>( "Sidebar" );
			sidebar1.AddChild( new A { }.SetValue( "Some more content" ) );
			//
			// and we can alos access the sidebar by making use of the FoxedSidebarLayout
			// itself, it's up to you
			//
			var sidebar2 = sidebarLayout.Sidebar;
			sidebar2.AddChild( new A { }.SetValue( "A last bit of content" ) );

			for( var i = 0; i < 16; i += 1 ) {
				page.Content.AddChild( new P { }.SetValue( "Some content" ) );
			}

			Render( page.Page );
		}


		///////////////////////////////////////////////////////////////////////////////

		public void CreateExtensiblePage()
		{
			var page = ExtensiblePage.Create( "Extensible Page" );
			page.Content.AddChild( new Div { }.SetValue( "some content" ) );
			Render( page.Page );
		}


		///////////////////////////////////////////////////////////////////////////////

		public void CreateSimplePage()
		{
			var page = new BasicPage( "Simple Page" );
			Render( page );
		}


		///////////////////////////////////////////////////////////////////////////////

		public void CreateBasicPage()
		{
			var page = new BasicHtml( "Basic Page" );
			Render( page );
		}


	}
}

