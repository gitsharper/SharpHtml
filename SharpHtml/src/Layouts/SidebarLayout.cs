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
using System.Linq;
using System.Text;

using SharpHtml;

namespace SharpHtml.Layouts {


	/////////////////////////////////////////////////////////////////////////////

	public class SidebarLayout {

		//
		// if you change the sidebar width to make it wider you will also have to
		// change the content width otherwise the sidebar content will spill over
		// on top of content. as you decrease the size of the content you might find
		// it becoming too narrow, if that happens then you will have to increase the
		// width of the overall body content (BodyMain tag for SimpleHtml)
		//

		const string ContentWidth = "80%";
		const string SidebarWidth = "220px";

		public Tag Sidebar { get; private set; }
		public Tag Content { get; private set; }

		/////////////////////////////////////////////////////////////////////////////

		//public TwoColumnLayout()
		//{
		//	// ******
		//	Content = new Div { }
		//		.SetId( "mainContent")
		//		.SetStyleMode( StyleMode.IncludeInStyles )
		//		//.AddStyle( "display", "table-cell")
		//		.AddStyle( "width", ContentWidth )
		//		.AddStyle( "padding", "8px")
		//		.AddStyle( "float", "right")
		//		//.AddChild( new Section { } )
		//		;

		//	// ******
		//	Sidebar = new Aside { }
		//		.SetId( "sidebar" )
		//		.SetStyleMode( StyleMode.IncludeInStyles )
		//		//.AddStyle( "display", "table-cell" )
		//		.AddStyle( "width", SidebarWidth )
		//		.AddStyle( "padding", "4px" )
		//		.AddStyle( "float", "left" )
		//		;
		//}

		public SidebarLayout()
		{
			//
			// this works because the right column is set to float
			// around the sidebar (because it does not float)
			//

			// ******
			Content = new Div { };
			Content.SetId( "mainContent" )
				.SetStyleMode( StyleMode.IncludeInStyles )
				.AddStyle( "width", ContentWidth )
				.AddStyle( "padding", "8px" )
				.AddStyle( "float", "right" )
				;

			// ******
			Sidebar = new Div { }
					.SetId( "sidebar" )
					.SetStyleMode( StyleMode.IncludeInStyles )
					.AddStyle( "position", "absolute" )
					.Width( SidebarWidth )
					.Padding( "4px" )
					;
		}


	}
}
