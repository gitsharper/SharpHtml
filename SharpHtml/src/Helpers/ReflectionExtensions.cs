using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHtml {

	/////////////////////////////////////////////////////////////////////////////

	public static class ReflectionExtensions {

		/////////////////////////////////////////////////////////////////////////////

		public static bool IsDerivedFrom( this Type possibleDerivedType, Type typeToLocate )
		{
			if( null == typeToLocate ) {
				throw new ArgumentNullException( nameof( typeToLocate ) );
			}

			if( null == possibleDerivedType ) {
				throw new ArgumentNullException( nameof( possibleDerivedType ) );
			}

			var typeToCheck = possibleDerivedType;
			while( null != typeToCheck ) {
				if( typeToLocate == typeToCheck ) {
					return true;
				}
				typeToCheck = typeToCheck.BaseType;
			}

			return false;
		}


	}

}
