using System;
using System.Collections.Generic;

/*
	Joe McLain, September 2015

	Removed all serialization and Xml References
*/

namespace SharpHtml {

	public class PropertyBag : PropertyBag<object> { }

	public class PropertyBag<TValue> : Dictionary<string, TValue> { }

}
