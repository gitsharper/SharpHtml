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

	public class AssetsMap {

		public string AssetsBase { get; private set; } = "include";
		public string CssFolder { get; private set; } = "css";
		public string ScriptFolder { get; private set; } = "js";
		public string ImageFolder { get; private set; } = "img";

		public string Assets => AssetsBase;
		public string Css => Path.Combine( AssetsBase, CssFolder );
		public string Script => Path.Combine( AssetsBase, ScriptFolder );
		public string Images => Path.Combine( AssetsBase, ImageFolder );

		/////////////////////////////////////////////////////////////////////////////

		public AssetsMap SetBase( string assetsBase )
		{
			AssetsBase = string.IsNullOrWhiteSpace( assetsBase ) ? string.Empty : assetsBase.Trim();
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public AssetsMap SetCssFolder( string path )
		{
			CssFolder = string.IsNullOrWhiteSpace( path ) ? string.Empty : path.Trim();
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public AssetsMap SetScriptFolder( string path )
		{
			ScriptFolder = string.IsNullOrWhiteSpace( path ) ? string.Empty : path.Trim();
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public AssetsMap SetImageFolder( string path )
		{
			ImageFolder = string.IsNullOrWhiteSpace( path ) ? string.Empty : path.Trim();
			return this;
		}

	}


	/////////////////////////////////////////////////////////////////////////////

	public class BasicPage : BasicHtml {

		const string JQUERY_VERSION = "1.11.3";

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
		public Tag Header { get; set; } = new Header { };
		public Tag Content { get; set; } = new Div { };
		public Tag Footer { get; set; } = new Footer { };



		public AssetsMap Assets { get; } = new AssetsMap { };
		public Stylesheet Stylesheet { get; } = new Stylesheet { };



		///////////////////////////////////////////////////////////////////////////////

		public virtual IEnumerable<StylesheetRule> StylesheetDefs()
		{
			yield return new RuleBlock( "html" )
				.Color( "#222" )
				.FontSize( "1em" )
				.LineHeight( "1.4" )
				;

			yield return new RuleBlock( "::-moz-selection" )
			.Background( "#b3d4fc" )
			.TextShadow( "none" )
			;

			yield return new RuleBlock( "html" )
				.Color( "#222" )
				.FontSize( "1em" )
				.LineHeight( "1.4" )
				;

			yield return new RuleBlock( "-moz-selection" )
				.Background( "#b3d4fc" )
				.TextShadow( "none" )
				;

			yield return new RuleBlock( "selection" )
				.Background( "#b3d4fc" )
				.TextShadow( "none" )
				;

			yield return new RuleBlock( "hr" )
				.Display( "block" )
				.Height( "1px" )
				.Border( "0" )
				.BorderTop( "1px solid #ccc" )
				.Margin( "1em 0" )
				.Padding( "0" )
				;

			yield return new RuleBlock( "audio, canvas, iframe, img, svg, video" )
				.VerticalAlign( "middle" )
				;

			yield return new RuleBlock( "fieldset" )
				.Border( "0" )
				.Margin( "0" )
				.Padding( "0" )
				;

			yield return new RuleBlock( "textarea" )
				.Resize( "vertical" )
				;

			yield return new RuleBlock( "" )
				.AddClass( ".browserupgrade" )
				.Margin( "0.2em 0" )
				.Background( "#ccc" )
				.Color( "#000" )
				.Padding( "0.2em 0" )
				;

			/* ===== Initializr Styles ==================================================
 Author: Jonathan Verrecchia - verekia.com/initializr/responsive-template
 ========================================================================== */


			yield return new RuleBlock( "body" )
				.Font( "16px/26px Helvetica, Helvetica Neue, Arial" )
				;

			yield return new RuleBlock( "" )
				.AddClass( ".wrapper" )
				.Width( "90%" )
				.Margin( "0 5%" )
				;

			/* ===================
ALL: Orange Theme
=================== */

			yield return new RuleBlock( "" )
				.AddClass( ".header-container" )
				.BorderBottom( "20px solid #e44d26" )
				;

			yield return new RuleBlock( "aside" )
				.AddClass( ".footer-container", ".main" )
				.BorderTop( "20px solid #e44d26" )
				;

			yield return new RuleBlock( "aside" )
				.AddClass( ".header-container", ".footer-container", ".main" )
				.Background( "#f16529" )
				;

			yield return new RuleBlock( "" )
				.AddClass( ".title" )
				.Color( "white" )
				;

			/* ==============
	MOBILE: Menu
	 ============== */

			yield return new RuleBlock( "nav ul" )
				.Margin( "0" )
				.Padding( "0" )
				.ListStyleType( "none" )
				;

			yield return new RuleBlock( "nav a" )
				.Display( "yield return new StylesheetBlock" )
				.MarginBottom( "10px" )
				.Padding( "15px 0" )
				.TextAlign( "center" )
				.TextDecoration( "none" )
				.FontWeight( "bold" )
				.Color( "white" )
				.Background( "#e44d26" )
				;

			yield return new RuleBlock( "nav a:hover, nav a:visited" )
				.Color( "white" )
				;

			yield return new RuleBlock( "nav a:hover" )
				.TextDecoration( "underline" )
				;

			/* ==============
				MOBILE: Main
				 ============== */

			yield return new RuleBlock( "" )
				.AddClass( ".main" )
				.Padding( "30px 0" )
				;

			yield return new RuleBlock( "article h1" )
				.AddClass( ".main" )
				.FontSize( "2em" )
				;

			yield return new RuleBlock( "aside" )
				.AddClass( ".main" )
				.Color( "white" )
				.Padding( "0px 5% 10px" )
				;

			yield return new RuleBlock( "footer" )
				.AddClass( ".footer-container" )
				.Color( "white" )
				.Padding( "20px 0" )
				;

			/* ===============
				ALL: IE Fixes
				 =============== */

			yield return new RuleBlock( "" )
				.AddClass( ".ie7", ".title" )
				.PaddingTop( "20px" )
				;


			/* ==========================================================================
				 Helper classes
				 ========================================================================== */

			yield return new RuleBlock( "" )
				.AddClass( ".hidden" )
				.Display( "none !important" )
				.Visibility( "hidden" )
				;

			yield return new RuleBlock( "" )
				.AddClass( ".visuallyhidden" )
				.Border( "0" )
				.Clip( "rect( \"0 0 0 0\" )" )
				.Height( "1px" )
				.Margin( "-1px" )
				.Overflow( "hidden" )
				.Padding( "0" )
				.Position( "absolute" )
				.Width( "1px" )
				;

			yield return new RuleBlock( "" )
				.AddClass( ".visuallyhidden.focusable:active", ".visuallyhidden.focusable:focus" )
				.Clip( "auto" )
				.Height( "auto" )
				.Margin( "0" )
				.Overflow( "visible" )
				.Position( "static" )
				.Width( "auto" )
				;

			yield return new RuleBlock( "" )
				.AddClass( ".invisible" )
				.Visibility( "hidden" )
				;

			yield return new RuleBlock( "" )
				.AddClass( ".clearfix:before", ".clearfix:after" )
				.Content( "" )
				.Display( "table" )
				;

			yield return new RuleBlock( "" )
				.AddClass( ".clearfix:after" )
				.Clear( "both" )
				;

			yield return new RuleBlock( "" )
				.AddClass( ".clearfix" )
				.Zoom( "1" )
				;

			/* ==========================================================================
				 Author's custom styles
				 ========================================================================== */


			/* ==========================================================================
				 Media Queries
				 ========================================================================== */

			yield return new MediaBlock( "only screen and (min-width: 480px)" )
				.Add(
					new RuleBlock( "nav a" )
						.Float( "left" )
						.Width( "27%" )
						.Margin( "0 1.7%" )
						.Padding( "25px 2%" )
						.MarginBottom( "0" ),

					new RuleBlock( "nav li:first-child a" )
						.MarginLeft( "0" ),

					new RuleBlock( "nav li:last-child a" )
						.MarginRight( "0" ),

					/* ========================
						INTERMEDIATE: IE Fixes
						 ======================== */

					new RuleBlock( "nav ul li" )
						.Display( "inline" ),

					new RuleBlock( "nav a" )
						.AddClass( ".oldie" )
						.Margin( "0 0.7%" )
				);


			yield return new MediaBlock( "only screen and (min-width: 768px)" )
				.Add(
					/* ====================
						WIDE( "CSS3 Effects
							==================== */

					new RuleBlock( "aside" )
						.AddClass( ".header-container", ".main" )
						.ArbitraryStyle( "-webkit-box-shadow", "0 5px 10px #aaa" )
						.ArbitraryStyle( "-moz-box-shadow", "0 5px 10px #aaa" )
						.BoxShadow( "0 5px 10px #aaa" ),

					/* ============
						WIDE( "Menu
							============ */

					new RuleBlock( "" )
						.AddClass( ".title" )
						.Float( "left" ),

					new RuleBlock( "nav" )
						.Float( "right" )
						.Width( "38%" ),

					/* ============
						WIDE( "Main
							============ */

					new RuleBlock( "article" )
						.AddClass( ".main" )
						.Float( "left" )
						.Width( "57%" ),

					new RuleBlock( "aside" )
						.AddClass( ".main" )
						.Float( "right" )
						.Width( "28%" )
				);


			var media = new MediaBlock( "only screen and (min-width: 1140px)" );

			/* ===============
				Maximal Width
					=============== */

				media.AddRuleBlock( "" )
					.AddClass( ".wrapper" )
					.Width( "1026px" ) /* 1140px - 10% for margins */
					.Margin( "0 auto" )
					;

			yield return media;


			//yield return new RuleBlock( "" )
			//	;
		}


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
			foreach( var ss in StylesheetRefs ) {
				AddStylesheetRef( IncludeBase + ss );
			}

			foreach( var script in HeadScriptRefs ) {
				AddScriptRef( true, IncludeBase + script );
			}

			// ******
			//
			// body
			//
			AddScriptRef( false, $"https://ajax.googleapis.com/ajax/libs/jquery/{JQUERY_VERSION}/jquery.min.js" );
			AddScript( false, $"window.jQuery || document.write( '<script src=\"js/vendor/jquery-{JQUERY_VERSION}.min.js\"><\\/script>' )" );

			foreach( var script in BodyScriptRefs ) {
				AddScriptRef( false, IncludeBase + script );
			}

			foreach( var tag in PageHeader() ) {
				base.HeaderContainer.AddChild( tag );
			}

			foreach( var tag in PageContent() ) {
				base.BodyContainer.AddChild( tag );
			}

			foreach( var tag in PageFooter() ) {
				base.FooterContainer.AddChild( tag );
			}


			Stylesheet.Add( StylesheetDefs() );
			var ssText = Stylesheet.Render();



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

		public BasicPage()
		{
			//
			// be sure to call Initialize()
			//
		}

		/////////////////////////////////////////////////////////////////////////////

		public BasicPage( string title, string language = DefaultLanguage, string includePath = "", params string [] attrAndstyles )
		{
			Initialize( title, language, includePath, attrAndstyles );
		}

	}
}
