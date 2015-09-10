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

using SharpHtml;

namespace SharpHtml.Pages {


	/////////////////////////////////////////////////////////////////////////////

	public class ExtensiblePage : SimplePage {

		///////////////////////////////////////////////////////////////////////////////
		//
		// don't need these since we're not adding anything, but if we use this as a
		// template for something else it's a reminder of how we can add tags to Header,
		// Content, and Footer
		//
		///////////////////////////////////////////////////////////////////////////////

		public override IEnumerable<Tag> PageHeader()
		{
			foreach( var tag in base.PageHeader() ) {
				yield return tag;
			}
			yield break;
		}


		///////////////////////////////////////////////////////////////////////////////

		public override IEnumerable<Tag> PageContent()
		{
			foreach( var tag in base.PageContent() ) {
				yield return tag;
			}
			yield break;
		}


		///////////////////////////////////////////////////////////////////////////////

		public override IEnumerable<Tag> PageFooter()
		{
			foreach( var tag in base.PageFooter() ) {
				yield return tag;
			}
			yield break;
		}


		/////////////////////////////////////////////////////////////////////////////
		
		
		// add Layouts, or whatever - html plugins, need to tell us where they
		// go, and any assest they require so we can add them
		
		
		
		
		/////////////////////////////////////////////////////////////////////////////

		public ExtensiblePage()
		{
			//
			// be sure to call Initialize() on base
			//
		}

		/////////////////////////////////////////////////////////////////////////////

		public ExtensiblePage( string title, string language = DefaultLanguage, string includePath = "", params string [] attrAndstyles )
		{
			Initialize( title, language, includePath, attrAndstyles );
		}

	}
}
