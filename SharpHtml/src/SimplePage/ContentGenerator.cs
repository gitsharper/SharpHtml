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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpHtml;

namespace SharpHtml.Simple {

	//
	// used ContentGenerator before I remembered/realized a method can return
	// IEnumerable<T> for use by foreach()
	//
	//    IEnumerable<Tag> method(...)
	//

	public abstract class ContentGenerator : IEnumerable<Tag> {

		/////////////////////////////////////////////////////////////////////////////

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Get and return he next Tag, if it's null we're done. Smart or not, we're
		/// allowing derived classes to just generate their data and not worry about
		/// enumerators and 'yielding'
		/// </summary>
		/// <returns></returns>

		abstract public IEnumerator<Tag> GetEnumerator();

		/////////////////////////////////////////////////////////////////////////////

		// pass in Children of container Tag

		public ContentGenerator Add( TagList tags )
		{
			foreach( var tag in this ) {
				tags.AddChild( tag );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public ContentGenerator()
		{
		}

	}

}
