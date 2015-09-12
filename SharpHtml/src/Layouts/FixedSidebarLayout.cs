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

namespace SharpHtml.Pages.Layouts {

	/////////////////////////////////////////////////////////////////////////////

	public class FixedSidebarLayout : ILayout {

		// first, this layout is not fluid/responsive, the sidebar is has a fixed width

		//
		// Rules for this layout
		//
		// if you want fixed width sidebar that does not shrink or expand you need to give it an
		// "absolute" width value - no percentages. in practice, give your sidebar an absolute
		// width no-matter-what, it will make your life easier
		//
		// if you want a fixed with content - no shrink or expanding - you need to give it an
		// "absolute" width value - no percentages.
		//
		// if you don't mind your sidebar or content's width shrinking or expanding then you can
		// use percentages for either, or both.
		//
		// if you don't want your sidebard and content areas colliding when the browser width is
		// narrowed you need to set LayoutWidth to some absolute (not percentages) width value.
		// OR make shure whomever creates the Tag that is passed to Initialize (or one of it's 
		// parents) has an absolute width set.
		//
		//   in addition, you must set an absolute value for the sidebar
		// 

		const string ContentWidth = "80%";
		const string SidebarWidth = "220px";

		public Tag Sidebar { get; private set; }
		public Div Content { get; private set; }

		public string LayoutWidth = string.Empty;


		/////////////////////////////////////////////////////////////////////////////

		public Tag Initialize( Tag tag )
		{
			// ******
			if( null == tag ) {
				throw new ArgumentNullException( nameof( tag ) );
			}

			// ******
			if( !string.IsNullOrWhiteSpace( LayoutWidth ) ) {
				tag.Width( LayoutWidth.Trim() );
			}

			// ******
			tag.AppendChildren( Sidebar, Content );
			return tag;
		}


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
				.Width( ContentWidth )
				.Padding( "8px" )
				.Float( "right" );

			// ******
			Sidebar = new Nav { }
					.SetId( "sidebar" )
					.SetStyleMode( StyleMode.IncludeInStyles )
					.Width( SidebarWidth )
					.Position( "fixed" )
					.Padding( "4px" );

			//
			// links (<a>) in sidebard
			//
			Sidebar.AddStyleBlock(
				StyleBlockAddAs.Id,
					"a", // <a>
					"display : block",
					"float : none",
					"margin : 0",
					"width : auto",
					"height : auto",
					"padding : 4px"
					//,"color : red"
			);
		}

	}

}
