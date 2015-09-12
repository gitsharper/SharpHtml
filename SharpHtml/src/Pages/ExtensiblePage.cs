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

//using Westwind.Utilities.Dynamic;

using SharpHtml;

namespace SharpHtml.Pages {


	/////////////////////////////////////////////////////////////////////////////

	// http://weblog.west-wind.com/posts/2012/Feb/08/Creating-a-dynamic-extensible-C-Expando-Object

	public class ExtensiblePage : Expando {

		public BasicPage Page { get; private set; } = new BasicPage { };

		public Tag Header { get; set; } = new Header { };
		public Tag Content { get; set; } = new Div { };
		public Tag Footer { get; set; } = new Footer { };


		/////////////////////////////////////////////////////////////////////////////
		
		public T Get<T>( string name )
			where T : class
		{
			object @object;
			if( TryGetProperty(name, out @object)) {
				return @object as T;
			}

			return default( T );
		}


		/////////////////////////////////////////////////////////////////////////////

		public T GetValue<T>( string name )
			where T : struct
		{
			object @object;
			if( TryGetProperty( name, out @object ) ) {
				return (T) @object;
			}

			return default( T );
		}


		/////////////////////////////////////////////////////////////////////////////
		//
		//
		//
		/////////////////////////////////////////////////////////////////////////////

		public dynamic HeaderAddLayout( ILayout layout )
		{
			return AddLayout( Header, layout );
		}


		/////////////////////////////////////////////////////////////////////////////

		public dynamic ContentAddLayout( ILayout layout )
		{
			return AddLayout( Content, layout );
		}


		/////////////////////////////////////////////////////////////////////////////

		public dynamic FooterAddLayout( ILayout layout )
		{
			return AddLayout( Footer, layout );
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Allows us to to associate an ILayout instance with the current page. The ILayout
		/// can be anything that implements the ILayout but in practice it will will be derived 
		/// from Tag or have Tag members that ILayout.Initialize() will add to the the Tag passed 
		/// to Initialize() thereby creating children that can be manipulated by user code.
		/// 
		/// All public properties of the ILayout instance will be added to the dynamic properties
		/// of the page except properties derived from Tag that have the same name as local properties
		/// (Header, Content, and Footer), those will replace the local properties. These 
		/// "replaced" properties can be accessed as normal on the page instance, but will now
		/// reference a Tag based object provided by the layout.
		/// </summary>
		/// <param name="toTag">The Tag that ILayout is allowed access too</param>
		/// <param name="layout">The layout</param>
		/// <returns></returns>

		public dynamic AddLayout( Tag toTag, ILayout layout )
		{
			// ******
			//
			// need to insert stuff in tag, could be one, or many
			//
			layout.Initialize( toTag );

			// ******
			var tagType = typeof( Tag );
			var layoutType = layout.GetType();

			// ******
			//
			// replace only where the properties are Tags', or derived from Tag
			//			
			var localPropInfos = typeof( ExtensiblePage ).GetProperties( BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly )
															.Where( p => p.PropertyType.IsDerivedFrom( tagType ) );

			var layoutPropInfos = layoutType.GetProperties( BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly );
			foreach( var propInfo in layoutPropInfos ) {
				var propName = propInfo.Name;
				var propType = propInfo.PropertyType;
				//
				// match names, and the property from the layout must be derived from Tag to replace a property on
				// the page (we've already made sure the local property list only containes properties derived from Tag)
				//
				var localPropInfo = localPropInfos.FirstOrDefault( p => propName == p.Name && p.PropertyType.IsDerivedFrom( tagType ) );

				if( null != localPropInfo ) {
					localPropInfo.SetValue( this, propInfo.GetValue( layout ) );
				}
				else {
					//
					// as dynamic property, note each time the property is read it's value is
					// retreived from the layout property that's been referenced - we're not
					// saving the value into the property bag, we're storing an object to retrevie 
					// the property
					//
					this.AddPropertyReference( propName, propInfo, layout );
				}
			}

			// ******
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		protected void Initialize( BasicPage sp )
		{
			// ******
			Page = sp;

			// ******
			//
			// save to property bag in case one or more are overridden by layouts,
			// (access dynamically, or via dictionary)
			//
			this [ "PageHeader" ] = Header = Page.Header;
			this [ "PageContent" ] = Content = Page.Content;
			this [ "PageFooter" ] = Footer = Page.Footer;
		}

		/////////////////////////////////////////////////////////////////////////////

		public ExtensiblePage( BasicPage sp )
		{
			Initialize( sp );
		}



		/////////////////////////////////////////////////////////////////////////////

		public static ExtensiblePage Create( string title, string language = BasicHtml.DefaultLanguage, string includePath = "", params string [] attrAndStyles )
		{
			var sp = new BasicPage( title, includePath, language, attrAndStyles );
			return new ExtensiblePage( sp );
		}

	}
}
