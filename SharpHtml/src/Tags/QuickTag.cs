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
	/// QuickTag should not exist, but sometimes it's just easier 
	/// 
	/// </summary>
	
	public class QuickTag : Tag {
		protected override string _TagName => _tagName;
		private string _tagName;

		// ******
		protected StringBuilder textContent = new StringBuilder {};


		/////////////////////////////////////////////////////////////////////////////

		public QuickTag WriteToContent( params string [] args )
		{
			foreach( var arg in args ) {
				textContent.Append( arg );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public QuickTag WriteLineToContent( params string [] args )
		{
			foreach( var arg in args ) {
				textContent.AppendLine( arg );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		protected override string RenderContent()
		{
			// ******
			if( 0 == textContent.Length ) {
				return base.RenderContent();
			}

			// ******
			return textContent.ToString();
		}


		/////////////////////////////////////////////////////////////////////////////

		public QuickTag SetTagName( string tagName )
		{
			// ******
			if( string.IsNullOrWhiteSpace( tagName ) ) {
				throw new ArgumentNullException( "tagName" );
			}

			// ******
			_tagName = tagName;

			// ******
			return this;
		}



		/////////////////////////////////////////////////////////////////////////////

		public QuickTag( string tagName = "", string id = "", params string [] attrAndStyles )
			: this( tagName, id, (IEnumerable<string>) attrAndStyles )
		{

		}

		/////////////////////////////////////////////////////////////////////////////

		public QuickTag( string tagName = "", string id = "", IEnumerable<string> attrAndStyles = null )
		{
			SetTagName( tagName );
			SetId( id );
			AddAttributesAndStyles( attrAndStyles );
		}


		/////////////////////////////////////////////////////////////////////////////

		public QuickTag()
		{

		}

	}
}
