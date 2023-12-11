using System;
using AlphaOmega.Debug.Native;

namespace AlphaOmega.Debug
{
	/// <summary>Collect perfomance counters from HDD</summary>
	public class Performance : IDisposable
	{
		/// <summary>The device</summary>
		private readonly DeviceIoControl _device;
		private Boolean _counterIsActive = false;
		
		/// <summary>Create instance of deviceperfomance counters</summary>
		/// <param name="device">The device</param>
		internal Performance(DeviceIoControl device)
			=> this._device = device;

		/// <summary>Query performance counters</summary>
		/// <returns>Performance counters</returns>
		public DiscApi.DISK_PERFORMANCE QueryPerformanceInfo()
		{
			DiscApi.DISK_PERFORMANCE result = this._device.IoControl<DiscApi.DISK_PERFORMANCE>(
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
				this._device.IoControl<DiscApi.DISK_PERFORMANCE>(
					Constant.IOCTL_DISC.PERFORMANCE_OFF,
					null);
				this._counterIsActive = false;
			}
		}
	}
}