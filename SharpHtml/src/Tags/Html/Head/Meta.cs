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

	public class Meta : Tag {
		protected override string _TagName { get { return "meta"; } }
		protected override TagRenderMode RenderMode { get { return TagRenderMode.VoidTag; } }


		/////////////////////////////////////////////////////////////////////////////

		//public Tag Initialize( string name, string content )
		//{
		//	// ******
		//	if( string.IsNullOrWhiteSpace( name ) ) {
		//		throw new ArgumentNullException( "name" );
		//	}
		//	Attributes.Add( "name", name.Trim() );

		//	if( !string.IsNullOrWhiteSpace( content ) ) {
		//		Attributes.Add( "content", content.Trim() );
		//	}

		//	// ******
		//	return this;
		//}


		/////////////////////////////////////////////////////////////////////////////

		public Tag NameContent( string name, string content )
		{
			// ******
			if( string.IsNullOrWhiteSpace( name ) ) {
				throw new ArgumentNullException( nameof( name ) );
			}

			Attributes.Add( "name", name.Trim() );
			Attributes.Add( "content", content?.Trim() ?? string.Empty );

			// ******
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Meta AddKeyValue( string key, string value )
		{
			if( string.IsNullOrWhiteSpace( key ) ) {
				throw new ArgumentNullException( nameof( key ) );
			}
			Attributes.Add( key.Trim(), value?.Trim() ?? string.Empty );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Meta()
		{
		}


		/////////////////////////////////////////////////////////////////////////////

		//public Meta( string name, string value )
		//{
		//	Attributes.Add( name, value.Trim() );
		//}


		/////////////////////////////////////////////////////////////////////////////

		public Meta( string nameValue, string contentValue )
		{
			if( string.IsNullOrWhiteSpace( nameValue ) ) {
				throw new ArgumentNullException( nameof( nameValue ) );
			}

			Attributes.Add( "name", nameValue.Trim() );
			Attributes.Add( "content", contentValue?.Trim() ?? string.Empty );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static Meta NewDescription( string value )
		{
			return new Meta( "description", value?.Trim() ?? string.Empty );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static Meta NewCharset( string value = "utf-8" )
		{
			return (Meta) new Meta().AddAttribute( "charset", value?.Trim() ?? "utf-8" );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static Meta NewXUACompatible( string value )
		{
			return (Meta) new Meta().AddAttribute( "http-equiv", "X-UA-Compatible" )
				.AddAttribute( "content", value?.Trim() ?? string.Empty );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static Meta NewViewport( string value = "width=device-width, initial-scale=1" )
		{
			return (Meta) new Meta { }.NameContent( "viewport", value );
		}


	}
}
