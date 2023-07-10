using AlphaOmega.Debug.Native;

namespace AlphaOmega.Debug
{
	/// <summary>S.M.A.R.T. attribute &amp; threshold value</summary>
	public struct AttributeThresholds
	{
		/// <summary>Attribute value</summary>
		public DiscApi.DRIVEATTRIBUTE Attribute { get; }

		/// <summary>Threshold value</summary>
		public DiscApi.ATTRTHRESHOLD Threshold { get ; }

		internal AttributeThresholds(DiscApi.DRIVEATTRIBUTE attribute, DiscApi.ATTRTHRESHOLD threshold)
			: this()
		{
			this.Attribute = attribute;
			this.Threshold = threshold;
		}
	}
}