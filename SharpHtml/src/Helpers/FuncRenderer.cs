namespace SharpHtml {

	public delegate string RenderMethod( object parameter );

	/////////////////////////////////////////////////////////////////////////////

	public class FuncRenderer : IRender {

		protected RenderMethod renderMethod;
		protected object parameter;

		///////////////////////////////////////////////////////////////////////////
		 
		public string Render()
		{
			return null != renderMethod ? renderMethod( parameter ) : string.Empty;
		}

		///////////////////////////////////////////////////////////////////////////

		public FuncRenderer( RenderMethod rm, object parameter = null )
		{
			this.renderMethod = rm;
			this.parameter = parameter;
		}
	}
}