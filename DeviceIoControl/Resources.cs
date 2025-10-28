using System;
using System.Resources;

namespace AlphaOmega.Debug
{
	/// <summary>Constant string descriptions</summary>
	public static class Resources
	{
		private static ResourceManager _attributeNames;

		private static ResourceManager AttributeNames
			=> _attributeNames ?? (_attributeNames = new ResourceManager("AlphaOmega.Debug.AttributeNames", typeof(Resources).Assembly));

		/// <summary>Gets S.M.A.R.T. attributes name</summary>
		/// <param name="attributeId">The attribute ID</param>
		/// <returns>Attribute description</returns>
		public static String GetAttributeName(Byte attributeId)//http://www.hdsentinel.com/help/en/56_attrib.html
			=> Resources.AttributeNames.GetString("0x" + attributeId.ToString("X2")) ?? "<Unknown>";
	}
}