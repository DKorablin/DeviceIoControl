using System;
using AlphaOmega.Debug.Native;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace AlphaOmega.Debug
{
	/// <summary>Volume IO control commands</summary>
	public class Volume
	{
		/// <summary>The device</summary>
		private readonly DeviceIoControl _device;
		private Boolean? _isClustered;
		private VolumeApi.VOLUME_DISK_EXTENTS? _diskExtents;

		/// <summary>Represents a physical location on a disk</summary>
		/// <remarks>The call does not work in Windows 7</remarks>
		public VolumeApi.VOLUME_DISK_EXTENTS DiskExtents
			=> this._diskExtents
				?? (this._diskExtents = this._device.IoControl<VolumeApi.VOLUME_DISK_EXTENTS>(
					Constant.IOCTL_VOLUME.GET_VOLUME_DISK_EXTENTS, null)).Value;

		/// <summary>Determines whether the specified volume is clustered</summary>
		/// <exception cref="Win32Exception">The volume is offline or Cluster service is not installed</exception>
		public Boolean IsClustered
		{
			get
			{
				if(this._isClustered == null)
					if(this._device.IoControl(Constant.IOCTL_VOLUME.IS_CLUSTERED)) this._isClustered = true;
					else
					{
						Int32 error = Marshal.GetLastWin32Error();
						this._isClustered = error == (Int32)Constant.ERROR.NO_ERROR
							? false
							: throw new Win32Exception(error);
					}
				return this._isClustered.Value;
			}
		}

		/// <summary>Create instance of Volume IO control commands</summary>
		/// <param name="device">Device</param>
		internal Volume(DeviceIoControl device)
			=> this._device = device;
	}
}