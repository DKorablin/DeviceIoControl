using System;
using AlphaOmega.Debug.Native;

namespace AlphaOmega.Debug
{
	/// <summary>Collect perfomance counters from HDD</summary>
	public class Performance : IDisposable
	{
		private readonly DeviceIoControl _device;
		private Boolean _counterIsActive = false;
		/// <summary>Device</summary>
		public DeviceIoControl Device { get { return this._device; } }
		/// <summary>Create instance of deviceperfomance counters</summary>
		/// <param name="device">device</param>
		internal Performance(DeviceIoControl device)
		{
			this._device = device;
		}
		/// <summary>Query perfomance counters</summary>
		/// <returns>Perfomance counters</returns>
		public DiscAPI.DISK_PERFORMANCE QueryPerfomanceInfo()
		{
			DiscAPI.DISK_PERFORMANCE result = this.Device.IoControl<DiscAPI.DISK_PERFORMANCE>(
				Constant.IOCTL_DISC.PERFORMANCE,
				null);
			this._counterIsActive = true;
			return result;
		}
		/// <summary>Close perfomace counters manager</summary>
		public void Dispose()
		{
			if(this._counterIsActive)
			{
				this.Device.IoControl<DiscAPI.DISK_PERFORMANCE>(
					Constant.IOCTL_DISC.PERFORMANCE_OFF,
					null);
				this._counterIsActive = false;
			}
		}
	}
}