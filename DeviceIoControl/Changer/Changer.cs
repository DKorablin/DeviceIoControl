using AlphaOmega.Debug.Native;

namespace AlphaOmega.Debug
{
	/// <summary>Changer IO control commands</summary>
	public class Changer
	{
		/// <summary>Device</summary>
		private readonly DeviceIoControl _device;
		private ChangerApi.CHANGER_PRODUCT_DATA? _productData;

		/// <summary>Retrieves the product data for the specified device</summary>
		public ChangerApi.CHANGER_PRODUCT_DATA ProductData//ERROR: Incorrect function
			=> (this._productData ?? (this._productData = this._device.IoControl<ChangerApi.CHANGER_PRODUCT_DATA>(
				Constant.IOCTL_CHANGER.GET_PRODUCT_DATA,
				null))).Value;

		/// <summary>Create instance of changer IO commands class</summary>
		/// <param name="device">Opened device</param>
		internal Changer(DeviceIoControl device)
			=> this._device = device;
	}
}