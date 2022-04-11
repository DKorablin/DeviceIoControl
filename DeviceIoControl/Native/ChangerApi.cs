using System;
using System.Runtime.InteropServices;

namespace AlphaOmega.Debug.Native
{
	/// <summary>Changer IOCTL structures</summary>
	public static class ChangerApi
	{
		/// <summary>Represents product data for a changer device</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct CHANGER_PRODUCT_DATA
		{
			/// <summary>The device manufacturer's name. This is acquired directly from the device inquiry data.</summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public Byte[] VendorId;
			/// <summary>The product identification, as defined by the vendor. This is acquired directly from the device inquiry data.</summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public Byte[] ProductId;
			/// <summary>The product revision, as defined by the vendor.</summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public Byte[] Revision;
			/// <summary>A unique value used to globally identify this device, as defined by the vendor.</summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public Byte[] SerialNumber;
			/// <summary>The device type of data transports, as defined by SCSI-2. This member must be FILE_DEVICE_CHANGER</summary>
			public Byte DeviceType;
		}
	}
}