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

	public class FixedSidebarLayout {

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
		public Div Content { get; private set; }

		/////////////////////////////////////////////////////////////////////////////

		public FixedSidebarLayout()
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
			Sidebar = new Nav { }
					.SetId( "sidebar" )
					.SetStyleMode( StyleMode.IncludeInStyles )
					.AddStyle( "position", "fixed" )
					.Width( SidebarWidth )
					.Padding( "4px" )
					;

			//
			// StyleBlock's don't support IStyle<T>, probably should
			//
			Sidebar.AddStyleBlock( 
				StyleBlockAddAs.Id, "a", 
					"display : block", 
					"float : none", 
					"margin : 0", 
					"width : auto",
					"height : auto",
					"padding : 4px"
			);
		}																																	 

	}

}
