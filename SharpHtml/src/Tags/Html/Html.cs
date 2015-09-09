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

		//
		// this must be added to the <head> element otherwise it will never
		// be rendered
		//
		public Style Style { get; protected set; } = new Style { };


		/////////////////////////////////////////////////////////////////////////////

		//
		// override this if you need some other logic for DOCTYPE
		//

		public override string Render()
		{
			Style.RenderStyles( this );
			return "<!DOCTYPE html>\r\n" 
				+ Render( TagRenderMode.Normal );
		}


		/////////////////////////////////////////////////////////////////////////////

		public Html()
		{
		}

	}

}
