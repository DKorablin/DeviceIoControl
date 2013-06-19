using System;
using AlphaOmega.Debug.Native;

namespace AlphaOmega.Debug
{
	/// <summary>Storage IO control commands</summary>
	public class Storage
	{
		private readonly DeviceIoControl _device;
		private Properties _properties;
		private StorageAPI.STORAGE_HOTPLUG_INFO? _hotplugInfo;
		private StorageAPI.STORAGE_DEVICE_NUMBER? _deviceNumber;
		private StorageAPI.GET_MEDIA_TYPES? _mediaTypes;

		/// <summary>Device</summary>
		public DeviceIoControl Device { get { return this._device; } }
		/// <summary>Device descriptors</summary>
		public Properties Properties
		{
			get
			{
				if(this._properties == null)
					this._properties = new Properties(this.Device);
				return this._properties;
			}
		}
		/// <summary>Retrieves the hotplug configuration of the specified device.</summary>
		public StorageAPI.STORAGE_HOTPLUG_INFO HotPlugInfo
		{
			get
			{
				if(this._hotplugInfo == null)
					this._hotplugInfo = this.Device.IoControl<StorageAPI.STORAGE_HOTPLUG_INFO>(
						Constant.IOCTL_STORAGE.GET_HOTPLUG_INFO,
						null);
				return this._hotplugInfo.Value;
			}
		}
		/// <summary>Contains information about a device.</summary>
		public StorageAPI.STORAGE_DEVICE_NUMBER DeviceNumber
		{
			get
			{
				if(this._deviceNumber == null)
					this._deviceNumber = this.Device.IoControl<StorageAPI.STORAGE_DEVICE_NUMBER>(
						Constant.IOCTL_STORAGE.GET_DEVICE_NUMBER,
						null);
				return this._deviceNumber.Value;
			}
		}
		/// <summary>Contains information about the media types supported by a device.</summary>
		public StorageAPI.GET_MEDIA_TYPES MediaTypes
		{
			get
			{
				if(this._mediaTypes == null)
					this._mediaTypes = this.Device.IoControl<StorageAPI.GET_MEDIA_TYPES>(
						Constant.IOCTL_STORAGE.GET_MEDIA_TYPES_EX,
						null);
				return this._mediaTypes.Value;
			}
		}

		/// <summary>Create instance of storage IO commands class</summary>
		/// <param name="device">Device</param>
		internal Storage(DeviceIoControl device)
		{
			this._device = device;
		}
	}
}