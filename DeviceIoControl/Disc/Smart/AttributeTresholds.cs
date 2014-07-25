using System;
using AlphaOmega.Debug.Native;

namespace AlphaOmega.Debug
{
	/// <summary>S.M.A.R.T. attribute &amp; threshold value</summary>
	public struct AttributeTresholds
	{
		/// <summary>Attribute value</summary>
		public DiscAPI.DRIVEATTRIBUTE Attribute;
		/// <summary>Threshold value</summary>
		public DiscAPI.ATTRTHRESHOLD Threshold;
	}
}