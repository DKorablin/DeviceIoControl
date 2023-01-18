using AlphaOmega.Debug.Native;

namespace AlphaOmega.Debug
{
	/// <summary>Changer IO control commands</summary>
	public class Changer
	{
		private ChangerApi.CHANGER_PRODUCT_DATA? _productData;

		/// <summary>Device</summary>
		private DeviceIoControl Device { get; }

		/// <summary>Retrieves the product data for the specified device</summary>
		public ChangerApi.CHANGER_PRODUCT_DATA ProductData
		{//ERROR: Incorrect function
			get
			{
				return (this._productData == null
					? this._productData = this.Device.IoControl<ChangerApi.CHANGER_PRODUCT_DATA>(
						Constant.IOCTL_CHANGER.GET_PRODUCT_DATA,
						null)
					: this._productData).Value;
			}
		}

		/// <summary>Create instance of changer IO commands class</summary>
		/// <param name="device">Opened device</param>
		internal Changer(DeviceIoControl device)
		{
			this.Device = device;
		}
	}
}