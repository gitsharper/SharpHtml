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

namespace SharpHtml.Simple {

	/////////////////////////////////////////////////////////////////////////////
	/*
		Notes:

			This is coded to match the initializr responsive template (http://www.initializr.com/)
			as it was on 20 August 2015. It was created for a small project the author was working
			on at the time. It is NOT a framework on which you can build a project, it is a starter
			project wrapped around SharpHtml. If you use it you WILL need to carefully peruse 
			SharpHtml, this code and the css resources that is uses (from initializr and the css
			and javascript libraries it	used)

			The content area has a width of 1026 pixels set by .wrapper, to increase or
			decrease this value you need to set the width of 'BodyMain'

			.

	*/

	/////////////////////////////////////////////////////////////////////////////

	public class SimpleHtml : Html {

		//
		// a "please upgrade your browser" message that will appear as the first item
		// in the 'body' if an "older" browser is being used - specifically ie 8 or lower
		//
		static string ieWarning = @"
<!--[if lt IE 8]>
  <p class='browserupgrade'>You are using an <strong>outdated</strong> browser.Please<a href='http://browsehappy.com/'>upgrade your browser</a> to improve your experience.</p>
<![endif]-->
";
		//
		// more ie detection for how to write out <html>
		//
		static string htmlOpenBlock = @"<!doctype html>
<!--[if lt IE 7]>      <html class='no - js lt - ie9 lt - ie8 lt - ie7' lang=''> <![endif]-->
<!--[if IE 7]>         <html class='no-js lt-ie9 lt-ie8' lang=''> <![endif]-->
<!--[if IE 8]>         <html class='no-js lt-ie9' lang=''> <![endif]-->
<!--[if gt IE 8]><!--> <html class='no-js' lang=''> <!--<![endif]-->"
;

		// ******
		//
		// <include ...> tag, using this worked out well until the author discovered
		// that it broke <a> anchors inside of a page
		//
		public string IncludePath { get; set; } = "";

		// ******
		//
		// null until Initialize() called, these are the tags in which to place the
		// header/body/footer content of the page
		//
		public Header BodyHeader { get; protected set; }
		public Div BodyMain { get; protected set; }
		public Footer BodyFooter { get; protected set; }

		/////////////////////////////////////////////////////////////////////////////

		Meta lastMeta;

		public SimpleHtml AddDescriptionMeta( params string [] values )
		{
			foreach( var value in values ) {
				Head.InsertChild( lastMeta, lastMeta = Meta.NewDescription( value ) );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		Link lastStyle;

		public SimpleHtml AddStylesheetRef( params string [] values )
		{
			foreach( var value in values ) {
				Head.InsertChild( lastStyle, lastStyle = Link.NewStylesheetRef( value ) );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public SimpleHtml AddScript( params string [] srcs )
		{
			foreach( var src in srcs ) {
				base.Body.AddChild( Script.NewScriptRef( src ) );
			}
			return this;
		}


		///////////////////////////////////////////////////////////////////////////////

		public virtual IEnumerable<Tag> EnumHead( string pageTitle )
		{
			//
			// this allowed the author to conceptualize what he was doing, tags are
			// created but are added to their parent tag by the caller
			//

			// ******
			if( !string.IsNullOrEmpty( IncludePath ) ) {
				yield return new Base { }.AddAttribute( "href", "include/" );
			}

			// ******
			yield return Meta.NewCharset();
			yield return Meta.NewXUACompatible( "IE=edge,chrome=1" );
			yield return lastMeta = Meta.NewViewport();

			// ******
			yield return Link.NewReference( "apple-touch-icon", "apple-touch-icon.png" );

			// ******
			yield return Link.NewStylesheetRef( "css/normalize.min.css" );
			yield return lastStyle = Link.NewStylesheetRef( "css/main.css" );

			// ******
			yield return Script.NewScriptRef( "js/vendor/modernizr-2.8.3-respond-1.4.2.min.js" );
		}


		/////////////////////////////////////////////////////////////////////////////

		public virtual IEnumerable<Tag> EnumBody()
		{
			//
			// same as 'ForHead()', here we do add child tags to their parent, but it
			// is still less messy than doing all of this inner mixed with adding these
			// tags to their parent tags
			//

			// ******
			yield return new RawContent( ieWarning );

			// ******
			var headerContainer = new Div { }.AddCssClass( "header-container" );
			BodyHeader = (Header) new Header { }.AddCssClass( "wrapper clearfix" );
			headerContainer.AddChild( BodyHeader );
			yield return headerContainer;

			// ******
			var mainContainer = new Div { }.AddCssClass("main-container" );
			BodyMain = (Div) new Div { }
				.SetId( "content" )
				.AddCssClass( "main wrapper clearfix" );
			mainContainer.AddChild( BodyMain );
			yield return mainContainer;

			// ******
			var footerContainer = new Div { }.AddCssClass("footer-container" );
			BodyFooter = (Footer) new Footer { }.AddCssClass( "wrapper" );
			footerContainer.AddChild( BodyFooter );
			yield return footerContainer;

			// ******
			//
			// jquery @ google
			//
			yield return Script.NewScriptRef( "//ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js" );
			//
			// loads local if not retreived from google
			//
			yield return Script.NewScriptWithCode(
				"window.jQuery || document.write( '<script src=\"js/vendor/jquery-1.11.2.min.js\"><\\/script>' )"
			);

			// ******
			yield return Script.NewScriptRef( "js/main.js" );
		}


		/////////////////////////////////////////////////////////////////////////////

		public SimpleHtml Initialize( string title, string includePath, params string [] attrAndstyles )
		{
			//
			// using the IEnumerable<Tag> methods allows the return of descrete
			// Tags without having to create a plethora of methods that we have to
			// create/call/update - tag generation is in a couple places and we can just
			// iterate the method that contains them
			//

			// ******
			IncludePath = string.IsNullOrWhiteSpace( includePath ) ? string.Empty : includePath;

			Title.SetValue( title );

			// ******
			foreach( var tag in EnumHead( title ) ) {
				Head.Children.AddChild( tag );
			}

			// ******
			foreach( var tag in EnumBody() ) {
				Body.Children.AddChild( tag );
			}

			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public override string Render()
		{
			//
			// need to override Html.Render() because we're using that <html> block
			// located in 'htmlOpenBlock'
			//
			Style.RenderStyles( this );
			return htmlOpenBlock + Render( TagRenderMode.NormalNoOpenTag );
		}


		/////////////////////////////////////////////////////////////////////////////

		public SimpleHtml()
		{
		}

	}
}
