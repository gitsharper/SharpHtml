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
using System.IO;
using System.Linq;
using System.Text;

namespace SharpHtml {


	/////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// 
	/// Add content to the enclosing tag, use to dump raw html into the output,
	/// but make sure you're adding it to a tag that can hold content ! 
	/// 
	/// </summary>
	
	public class RawContent : Tag {
		protected override string _TagName => "raw_content";

		// ******
		protected StringBuilder textContent = new StringBuilder {};


		/////////////////////////////////////////////////////////////////////////////

		public RawContent FormatTo( string fmt, params object [] args )
		{
			textContent.AppendFormat( fmt, args );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public RawContent WriteTo( params string [] args )
		{
			foreach( var arg in args ) {
				textContent.Append( arg );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public RawContent WriteLineTo( params string [] args )
		{
			foreach( var arg in args ) {
				textContent.AppendLine( arg );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public override string Render()
		{
			//
			// only content, no tags or anything else
			//
			return textContent.ToString();
		}


		/////////////////////////////////////////////////////////////////////////////

		public RawContent()
		{

		}


		/////////////////////////////////////////////////////////////////////////////

		public RawContent( StringBuilder sb )
		{
			if( null != sb ) {
				textContent = sb;
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public RawContent( string contents )
		{
			textContent.Append( contents );
		}

	}
}
