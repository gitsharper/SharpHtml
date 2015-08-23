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

namespace SharpHtml {

	/////////////////////////////////////////////////////////////////////////////

	public class Col : Tag {
		protected override string _TagName { get { return "col"; } }
		protected override TagRenderMode RenderMode { get { return TagRenderMode.VoidTag; } }
		protected override TagFormatOptions MultipleTagFormatAlign => TagFormatOptions.Horizontal;
		public override StyleMode StyleMode { get { return StyleMode.IncludeInTag; } }


		/////////////////////////////////////////////////////////////////////////////

		//public void SetSpan( int span )
		//{
		//	const string SPAN = "span";

		//	// ******
		//	if( span < 0 ) {
		//		return;
		//	}

		//	// ******
		//	if( Attributes.ContainsKey( SPAN ) ) {
		//		if( 0 == span ) {
		//			Attributes.Remove( SPAN );
		//		}
		//		else {
		//			Attributes.Add( SPAN, span, true );
		//		}
		//	}
		//	else if( span > 0 ) {
		//		Attributes.Add( SPAN, span, false );
		//	}
		//}


		/////////////////////////////////////////////////////////////////////////////

		//public override string Render()
		//{
		//	//
		//	// any children, or data in Value are ignored
		//	//
		//	return Render( TagRenderMode.SelfClosing );
		//}


		/////////////////////////////////////////////////////////////////////////////

		public Col()
		{
			//MultipleTagFormatAlign = TagFormatOptions.Horizontal;
		}


	}

}
