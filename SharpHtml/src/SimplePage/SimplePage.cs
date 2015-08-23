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

namespace SharpHtml.Simple {


	/////////////////////////////////////////////////////////////////////////////

	public class SimplePage<T> where T : SimpleHtml, new() {

		public SimpleHtml Html { get; set; }

		//
		// also css overrides, additional js, and so on
		//


		/////////////////////////////////////////////////////////////////////////////

		public SimplePage<T> AddHeaderContent( IEnumerable<Tag> tags )
		{
			Html.BodyHeader.AppendChildren( tags );
			return this;
		}


		public SimplePage<T> AddHeaderContent( params Tag [] tags )
		{
			return AddHeaderContent( (IEnumerable<Tag>) tags );
		}


		/////////////////////////////////////////////////////////////////////////////

		public SimplePage<T> AddBodyContent( IEnumerable<Tag> tags )
		{
			Html.BodyMain.AppendChildren( tags );
			return this;
		}


		public SimplePage<T> AddBodyContent( params Tag [] tags )
		{
			return AddBodyContent( (IEnumerable<Tag>) tags );
		}


		/////////////////////////////////////////////////////////////////////////////

		public SimplePage<T> AddFooterContent( IEnumerable<Tag> tags )
		{
			Html.BodyFooter.AppendChildren( tags );
			return this;
		}


		public SimplePage<T> AddFooterContent( params Tag [] tags )
		{
			return AddFooterContent( (IEnumerable<Tag>) tags );
		}


		/////////////////////////////////////////////////////////////////////////////

		public virtual SimplePage<T> Initialize( string pageTitle, string includePath )
		{
			Html.Initialize( pageTitle, includePath );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public SimplePage()
		{
			Html = new T { };
		}

	}

}
