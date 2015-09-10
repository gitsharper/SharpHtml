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

	public class SimplePage : BasicHtml {

		public const string HeaderId = "header";
		public const string ContentId = "content";
		public const string FooterId = "footer";
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
		// this can be confusing. we are overriding the members Header, Content, and Footer
		// from BasicHtml. we do this so we can move code developed using BasicHtml to use
		// SimpleHtml without change when adding items to these elements
		//
		// can't use virtual because we're changing the types of the Header/Footer tags
		//
		//  <body>
		//
		//    <div BasicHtml.Header >
		//      <header SimpleHtml.Header >
		//      </header>
		//    </div>
		//
		//    <div BasicHtml.Content >
		//      <div SimpleHtml.Content >
		//      </div>
		//    </div>
		//
		//    <div BasicHtml.Footer >
		//      <footer SimpleHtme.Footer >
		//      </footer>
		//    </div>
		//
		//  </body>
		//
		// as to why ...
		//
		public new Header Header { get; set; } = new Header { };
		public new Div Content { get; set; } = new Div { };
		public new Footer Footer { get; set; } = new Footer { };



		///////////////////////////////////////////////////////////////////////////////

		public virtual IEnumerable<Tag> PageHeader()
		{
			yield return this.Header.SetId( HeaderId )
				.AddCssClass( "wrapper" )
				.AddCssClass( "clearfix" );
		}


		///////////////////////////////////////////////////////////////////////////////

		public virtual IEnumerable<Tag> PageContent()
		{
			yield return this.Content.SetId( ContentId )
				.AddCssClass( "main" )
				.AddCssClass( "wrapper" )
				.AddCssClass( "clearfix" )
				;//.Width( "1450px" );

			//
			// sidebar in content
			//
		}


		///////////////////////////////////////////////////////////////////////////////

		public virtual IEnumerable<Tag> PageFooter()
		{
			yield return this.Footer.SetId( FooterId )
				.AddCssClass( "wrapper" );
		}


		/////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// This string is prepended to all stylesheet and script references, when Html is browsed
		/// locally this allows us to put the assets much anywhere. if this is being
		/// generated for use on an acutal webserver it is probably best to use the default which
		/// is a local "include/" directory with "css" and other directories benath that
		/// </summary>
		public string IncludeBase { get; set; } = "include/";

		public List<string> StylesheetRefs { get; private set; } = new List<string> {
			"css/normalize.min.css",
			"css/main.css",
		};

		public List<string> HeadScriptRefs { get; private set; } = new List<string> {
			"js/vendor/modernizr-2.8.3-respond-1.4.2.min.js",
		};

		public List<string> BodyScriptRefs { get; private set; } = new List<string> {
			"js/main.js",
		};


		public override BasicHtml Initialize( string title, string language = DefaultLanguage, string includePath = "", params string [] attrAndstyles )
		{
			// ******
			base.Initialize( title, language, includePath, attrAndstyles );

			// ******
			//
			// head, the stylesheets give us basic styles including Header/Content/Footer
			//
			foreach( var ss in StylesheetRefs) {
				AddStylesheetRef( IncludeBase + ss );
			}

			foreach( var script in HeadScriptRefs) {
				AddScriptRef( true, IncludeBase + script );
			}

			// ******
			//
			// body
			//
			const string JQUERY_VERSION = "1.11.3";

			AddScriptRef( false, $"https://ajax.googleapis.com/ajax/libs/jquery/{JQUERY_VERSION}/jquery.min.js" );
			AddScript( false, $"window.jQuery || document.write( '<script src=\"js/vendor/jquery-{JQUERY_VERSION}.min.js\"><\\/script>' )" );
			
			foreach( var script in BodyScriptRefs ) {
				AddScriptRef( false, IncludeBase + script );
			}

			foreach( var tag in PageHeader() ) {
				base.Header.AddChild( tag );
			}

			foreach( var tag in PageContent() ) {
				base.Content.AddChild( tag );
			}

			foreach( var tag in PageFooter() ) {
				base.Footer.AddChild( tag );
			}

			// ******
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
			return htmlOpenBlock
				+ Render( TagRenderMode.NormalNoOpenTag );
		}


		/////////////////////////////////////////////////////////////////////////////

		public SimplePage()
		{
			//
			// be sure to call Initialize()
			//
		}

		/////////////////////////////////////////////////////////////////////////////

		public SimplePage( string title, string language = DefaultLanguage, string includePath = "", params string [] attrAndstyles )
		{
			Initialize( title, language, includePath, attrAndstyles );
		}

	}
}
