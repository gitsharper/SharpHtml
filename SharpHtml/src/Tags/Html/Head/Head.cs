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

namespace SharpHtml {


	/*
		The following elements can go inside the <head> element:
	<title> (this element is required in an HTML document)
	<style>
	<base>
	<link>
	<meta>
	<script>
	<noscript>
	*/


	/////////////////////////////////////////////////////////////////////////////

	public class Head : Tag {
		protected override string _TagName { get { return "head"; } }

		bool haveTitle;

		/////////////////////////////////////////////////////////////////////////////

		public Head AddTitle( string title )
		{
			if( haveTitle ) {
				throw new Exception( "Head has already been supplied with a title" );
			}
			AddChild( new Title( title ) );
			haveTitle = true;
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Head AddLink( string href, string rel )
		{
			AddChild( new Link( href, rel ) );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Head AddScript( string src, params string [] attributes )
		{
			AddChild( new Script( src, attributes ) );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		//public new Head AddStyle( string href, string rel )
		//{
		//	return this;
		//}


		/////////////////////////////////////////////////////////////////////////////

		public Head AddMeta( string name, string content )
		{
			AddChild( new Meta( name, content ) );
			return this;
		}


	}
}