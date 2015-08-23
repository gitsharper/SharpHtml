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

	public class Script : Tag {
		protected override string _TagName { get { return "script"; } }


		/////////////////////////////////////////////////////////////////////////////

		public Script()
		{
		}
		
		
		/////////////////////////////////////////////////////////////////////////////

		public Script( string src, params string [] attributes )
		{
			base.AddAttribute( "src", src );
			base.AddAttributes( attributes );
			//base.SetTagAlign( TagFormatOptions.Horizontal );

		}


		/////////////////////////////////////////////////////////////////////////////

		public static Script NewScriptRef( string src )
		{
			return new Script( src );
		}

		/////////////////////////////////////////////////////////////////////////////

		public static Script NewScriptWithCode( string code )
		{
			return ( Script ) new Script { }
				.SetValue( code );
		}
	}
}
