namespace SharpHtml {

	/////////////////////////////////////////////////////////////////////////////

	public class Body : Tag {
		protected override string _TagName { get { return "body"; } }
		public override StyleMode StyleMode { get { return StyleMode.IncludeInStyles; } }

	}
}