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
	/// <summary>
	/// This is a basic implementation of an Html page. There are no imported
	/// .css files, scripts, or anthing else.
	/// 
	/// Add tags to HeaderContainer, MainContainer, or FooterContainer.
	/// </summary>

	public class BasicHtml : Html {

		// ******
		public const string HeaderContainerId = "header-container";
		public const string MainContainerId = "main-container";
		public const string FooterContainerId = "footer-container";
		public const string DefaultLanguage = "en";

		// ******

		// ******
		//
		// <include ...> tag, using this worked out well until the author discovered
		// that it broke <a> anchors inside of a page
		//
		public string IncludePath { get; private set; } = string.Empty;
		public string PageTitle { get; private set; } = string.Empty;
		public string Language { get; private set; } = DefaultLanguage;

		// ******
		public Head Head { get; protected set; } = new Head { };
		public Body Body { get; protected set; } = new Body { };

		// ******
		public Div Header { get; set; } = new Div { };
		public Div Content { get; set; } = new Div { };
		public Div Footer { get; set; } = new Div { };


		/////////////////////////////////////////////////////////////////////////////

		Tag lastMeta;

		public BasicHtml AddMetaDescription( params string [] values )
		{
			foreach( var value in values ) {
				Head.InsertChildAfter( lastMeta, lastMeta = Meta.NewDescription( value ) );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		Style lastStyle;

		public BasicHtml AddStylesheetRef( params string [] values )
		{
			foreach( var value in values ) {
				Head.InsertChildBefore( lastStyle, Link.NewStylesheetRef( value ) );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public BasicHtml AddScriptRef( bool addToHead, params string [] srcs )
		{
			foreach( var src in srcs ) {
				var script = Script.NewScriptRef( src );
				script.SetTagAlign( TagFormatOptions.HorizontalWithReturn );

				if( addToHead ) {
					Head.AddChild( script );
				}
				else {
					Body.AddChild( script );
				}
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public BasicHtml AddScript( bool addToHead, string src )
		{
			var script = Script.NewScriptWithCode( src );
			if( addToHead ) {
				Head.AddChild( script );
			}
			else {
				Body.AddChild( script );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public virtual IEnumerable<Tag> EnumHtml()
		{
			yield return Head;
			yield return Body;
		}


		///////////////////////////////////////////////////////////////////////////////

		public virtual IEnumerable<Tag> EnumHead()
		{
			// ******
			//
			// note, breaks <a> anchors inside of a page
			//
			if( !string.IsNullOrEmpty( IncludePath ) ) {
				yield return new Base { }.AddAttribute( "href", IncludePath );
			}

			// ******
			//
			// if you include any stylesheet refs here do it _BEFORE_ "lastType = Style"
			//
			yield return Meta.NewCharset();
			yield return Meta.NewXUACompatible( "IE=edge" );
			yield return lastMeta = new Title( PageTitle );
			yield return Meta.NewViewport();

			//
			// stylesheets here
			//

			yield return lastStyle = Style;
		}


		/////////////////////////////////////////////////////////////////////////////

		public virtual IEnumerable<Tag> EnumBody()
		{
			// ******
			yield return Header.SetId( HeaderContainerId )
				.AddCssClass( HeaderContainerId );

			// ******
			yield return Content.SetId( MainContainerId )
				.AddCssClass( MainContainerId );

			// ******
			yield return Footer.SetId( FooterContainerId )
				.AddCssClass( FooterContainerId );
		}


		/////////////////////////////////////////////////////////////////////////////

		public virtual BasicHtml Initialize( string title, string language = DefaultLanguage, string includePath = "", params string [] attrAndstyles )
		{
			//
			// using the IEnumerable<Tag> methods allows the return of descrete
			// Tags without having to create a plethora of methods that we have to
			// create/call/update - tag generation is in a couple places and we can just
			// iterate the method that contains them
			//


			// ******
			PageTitle = string.IsNullOrWhiteSpace( title ) ? string.Empty : title.Trim();
			Language = string.IsNullOrWhiteSpace( language ) ? string.Empty : language.Trim();
			IncludePath = string.IsNullOrWhiteSpace( includePath ) ? string.Empty : includePath.Trim();

			// ******
			if( !string.IsNullOrWhiteSpace(Language)) {
				AddAttribute( "lang", Language );
			}


			// ******
			foreach( var tag in EnumHtml() ) {
				Children.AddChild( tag );
			}

			// ******
			foreach( var tag in EnumHead() ) {
				Head.Children.AddChild( tag );
			}

			// ******
			foreach( var tag in EnumBody() ) {
				Body.Children.AddChild( tag );
			}

			// ******
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public BasicHtml()
		{
			//
			// be sure to call Initialize()
			//
		}


		/////////////////////////////////////////////////////////////////////////////

		public BasicHtml( string title, string language = DefaultLanguage, string includePath = "", params string [] attrAndstyles )
		{
			Initialize( title, language, includePath, attrAndstyles );
		}

	}
}
