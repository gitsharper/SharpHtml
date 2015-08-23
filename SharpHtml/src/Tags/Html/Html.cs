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
using System.Text;
using System.Threading.Tasks;

namespace SharpHtml {


	/////////////////////////////////////////////////////////////////////////////

	public class Html : Tag {
		protected override string _TagName { get { return "html"; } }

		// ******
		public string Language { get; set; } = "en";

		// ******
		public Title Title { get; protected set; }
		public Style Style { get; protected set; }
		public Head Head { get; protected set; }
		public Body Body { get; protected set; }


		/////////////////////////////////////////////////////////////////////////////

		public Html SetTitle( string title )
		{
			Title.SetValue( title );
			return this;
		}

		/////////////////////////////////////////////////////////////////////////////

		public override string Render()
		{
			Style.RenderStyles( this );
			AddAttribute( "lang", Language );
			return "<!DOCTYPE html>\r\n" + Render( TagRenderMode.Normal );
		}


		/////////////////////////////////////////////////////////////////////////////

		public void Defaults( string title, IEnumerable<string> attrAndstyles )
		{
			// ******
			AddAttributesAndStyles( attrAndstyles );

			// ******
			Children.AddChild( Head = new Head { } );

			Head.AddChild( Title = new Title( title ) );
			Head.AddChild( Style = new Style { } );

			// ******
			Children.AddChild( Body = new Body { } );
		}


		/////////////////////////////////////////////////////////////////////////////

		public Html()
		{

		}


		/////////////////////////////////////////////////////////////////////////////

		public Html( string title = "untitled", IEnumerable<string> attrAndstyles = null )
		{
			Defaults( title, attrAndstyles );
			//this.Body.AddStyle( "background", "lightgray" );
		}

	}




}
