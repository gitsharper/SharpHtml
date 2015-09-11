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
using System.Reflection;

using Westwind.Utilities.Dynamic;

using SharpHtml;

namespace SharpHtml.Pages {


	/////////////////////////////////////////////////////////////////////////////

	// http://weblog.west-wind.com/posts/2012/Feb/08/Creating-a-dynamic-extensible-C-Expando-Object

	public class ExtensiblePage : Westwind.Utilities.Dynamic.Expando {

		public const string DefaultIncludePath = "include/";
		public const string DefaultPageTitle = "Page";


		public SimplePage Page { get; private set; } = new SimplePage { };

		public Tag Header { get; set; } = new Header { };
		public Tag Content { get; set; } = new Div { };
		public Tag Footer { get; set; } = new Footer { };


		///////////////////////////////////////////////////////////////////////////////
		//
		// don't need these since we're not adding anything, but if we use this as a
		// template for something else it's a reminder of how we can add tags to Header,
		// Content, and Footer
		//
		///////////////////////////////////////////////////////////////////////////////

		public IEnumerable<Tag> PageHeader()
		{
			foreach( var tag in Page.PageHeader() ) {
				yield return tag;
			}
			yield break;
		}


		///////////////////////////////////////////////////////////////////////////////

		public IEnumerable<Tag> PageContent()
		{
			foreach( var tag in Page.PageContent() ) {
				yield return tag;
			}
			yield break;
		}


		///////////////////////////////////////////////////////////////////////////////

		public IEnumerable<Tag> PageFooter()
		{
			foreach( var tag in Page.PageFooter() ) {
				yield return tag;
			}
			yield break;
		}


		/////////////////////////////////////////////////////////////////////////////
		//
		//
		//
		/////////////////////////////////////////////////////////////////////////////


		// add Layouts, or whatever - html plugins, need to tell us where they
		// go, and any assets they require so we can add them




		/////////////////////////////////////////////////////////////////////////////

		//
		// extension method ?
		//

		public dynamic AddLayout( Tag toTag, ILayout layout )
		{
			//var properties = layout.GetType().GetRuntimeProperties().Where( pi => pi.PropertyType.;
			var type = layout.GetType();
			var layoutPropInfos = type.GetProperties( BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly );
			var localPropInfos = typeof( ExtensiblePage ).GetProperties( BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly );

			foreach( var propInfo in layoutPropInfos ) {
				var propName = propInfo.Name;
				var propType = propInfo.PropertyType;

				var localPropInfo = localPropInfos.FirstOrDefault( p => propName == p.Name && propType == p.PropertyType );

				if( null != localPropInfo ) {
					//
					// save old prop somewhere
					//
					localPropInfo.SetValue( this, propInfo.GetValue( layout ) );
				}
				else {
					//
					// as dynamic property
					//
					this [ propName ] = MakeGetReferencedObject( propInfo, layout );
				}
			}

			//
			// need to insert stuff in tag, but could be one or many
			//

			layout.Initialize( toTag );

			return this;

		}


		/////////////////////////////////////////////////////////////////////////////

		protected void Initialize( SimplePage sp )
		{
			// ******
			Page = sp;

			// ******
			Header = Page.Header;
			Content = Page.Content;
			Footer = Page.Footer;
		}

		/////////////////////////////////////////////////////////////////////////////

		public ExtensiblePage( SimplePage sp )
		{
			Initialize( sp );
		}



		/////////////////////////////////////////////////////////////////////////////

		public static void Who() { }

		public static ExtensiblePage Create( string title, string language = BasicHtml.DefaultLanguage, string includePath = "", params string [] attrAndStyles )
		{
			var sp = new SimplePage( title, includePath, language, attrAndStyles );
			return new ExtensiblePage( sp );
		}

	}
}
