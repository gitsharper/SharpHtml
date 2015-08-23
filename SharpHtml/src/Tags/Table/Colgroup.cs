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

	public class Colgroup : Tag {
		protected override string _TagName { get { return "colgroup"; } }

		//
		// containe only <col /> members
		//

		/////////////////////////////////////////////////////////////////////////////

		public Col Add()
		{
			Col col;
			Children.AddChild( col = new Col {} );
			return col;
		}


		/////////////////////////////////////////////////////////////////////////////

		//public void Add( Dictionary<string, string> attributes )
		//{
		//	Children.Add<Col>( string.Empty, attributes, null );
		//}


		/////////////////////////////////////////////////////////////////////////////

		//public void Add( IEnumerable<string> attributes )
		//{
		//	Children.Add<Col>( string.Empty, attributes, null );
		//}


		/////////////////////////////////////////////////////////////////////////////

		public Colgroup()
		{
		}

	}
}
