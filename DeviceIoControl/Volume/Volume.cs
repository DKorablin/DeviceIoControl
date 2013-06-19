using System;
using AlphaOmega.Debug.Native;

namespace AlphaOmega.Debug
{
	/// <summary>Volume IO control commands</summary>
	public class Volume
	{
		private readonly DeviceIoControl _device;
		private VolumeAPI.VOLUME_DISK_EXTENTS? _diskExtents;
		/// <summary>Device</summary>
		public DeviceIoControl Device { get { return this._device; } }

		/// <summary>Represents a physical location on a disk.</summary>
		[Obsolete("Вызов не работает в Windows 7", false)]
		public VolumeAPI.VOLUME_DISK_EXTENTS DiskExtents
		{
			get
			{
				if(this._diskExtents == null)
					this._diskExtents = this.Device.IoControl<VolumeAPI.VOLUME_DISK_EXTENTS>(
						Constant.IOCTL_VOLUME.GET_VOLUME_DISK_EXTENTS,
						null);
				return this._diskExtents.Value;
			}
		}

		/// <summary>Create instance of Volume IO control commands</summary>
		/// <param name="device">Device</param>
		internal Volume(DeviceIoControl device)
		{
			this._device = device;
		}
	}
}