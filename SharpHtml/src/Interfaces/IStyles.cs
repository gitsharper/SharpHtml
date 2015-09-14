using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IStyles {
}

public interface IStyles<T> : IStyles where T : class {

	T AddComment( string value );
	T ReplaceStyle( string key, string value );
	T AddStyle( string key, string value );
	T AddStyles( IEnumerable<string> styles );
	T AddStyles( params string [] styles );

}

