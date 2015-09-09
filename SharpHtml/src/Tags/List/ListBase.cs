using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpHtml {

	/////////////////////////////////////////////////////////////////////////////

	public abstract class ListBase : Tag {

		/////////////////////////////////////////////////////////////////////////////

		public ListBase AddListItems( TagList tags )
		{
			foreach( var tag in tags ) {
				AddChild( new Li().AddChild( tag ) );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public ListBase AddListItems( IEnumerable<string> strs )
		{
			foreach( var str in strs ) {
				AddChild( new Li( str ) );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public ListBase AddListItems( params string [] strs )
		{
			return AddListItems( (IEnumerable<string>) strs );
		}



		/////////////////////////////////////////////////////////////////////////////

		public ListBase AddListItems( IEnumerable<Tag> tags )
		{
			foreach( var tag in tags ) {
				AddChild( new Li().AddChild( tag ) );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public ListBase AddListItems( params Tag [] tags )
		{
			return AddListItems( (IEnumerable<Tag>) tags );
		}




	}

}
