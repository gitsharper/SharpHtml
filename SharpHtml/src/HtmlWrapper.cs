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
using System.Threading.Tasks;


//
// this was started to create support for Nmp (net macro processor) when the author
// wanted to simplify the building of html code with the macro processor. in doing
// this he (me) discovered it was MUCH simpler to write SharpHtml (somewhat smaller
// that it is now) and use it directly and skip using Nmp completly
//




/////////////////////////////////////////////////////////////////////////////

namespace SharpHtml {

	/////////////////////////////////////////////////////////////////////////////

	public class HtmlWrapper {


		/////////////////////////////////////////////////////////////////////////////

		public Tag CreateTag( string tagName, string id = "", IEnumerable<string> attrAndStyles = null )
		{
			return new QuickTag( tagName, id );
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag CreateHtml( string title, IEnumerable<string> attrAndStyles = null )
		{
			return new Html( title, attrAndStyles );
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag CreateTable( string caption, int border = 0, string id = "", IEnumerable<string> attrAndstyles = null )
		{
			return new Table( caption, border, id, attrAndstyles );
		}


		/////////////////////////////////////////////////////////////////////////////

		//public Tag CreateTable( TableDef td )
		//{
		//	return null;
		//}


		/////////////////////////////////////////////////////////////////////////////

		// json encoded TableDef

		public Tag CreateTable( string jsonStr )
		{
			return null;
		}


		/////////////////////////////////////////////////////////////////////////////

		public HtmlWrapper()
		{
		}

	}


}
