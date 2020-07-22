DeviceIoControl assembly
===============

Wrapper for DeviceIoControl function. Primary task of this assembly was to read SMART data from supported devices.
S.M.A.R.T.rar contains original source code written by Andrew I. Reshin (11.02.2001) in C++

Usage:
<pre>
using(DeviceIoControl device = new DeviceIoControl(@"c:\"))
{
	Boolean isOn = device.IsDeviceOn;
	Console.WriteLine(String.Format("Device is {0}", isOn ? "on" : "off"));

	if(device.Disc.Version != null)
	{
		String capabilities = "Capabilities: ";
		if(device.Disc.Version.Value.IsAtaSupported)
			capabilities += "ATA,";
		if(device.Disc.Version.Value.IsAtapiSupported)
			capabilities += "ATAPI,";
		if(device.Disc.Version.Value.IsSmartSupported)
			capabilities += "SCSI";
		Console.WriteLine(capabilities);
	}

	if(device.Disc.Smart != null)
	{
		Console.WriteLine("===INFO===");
		//Info
		String deviceInfo = String.Format(@"Type: {0}
Serial number: {1}\
Firmware version: {2}
Model number: {3}
Capabilities: 0x{4:X}
User addressable space {5:n0} sectors (LBA mode only)",
			device.Disc.Smart.SystemParams.Type,
			device.Disc.Smart.SystemParams.SerialNumber,
			device.Disc.Smart.SystemParams.FirmwareRev,
			device.Disc.Smart.SystemParams.ModelNumber,
			device.Disc.Smart.SystemParams.wCapabilities,
			device.Disc.Smart.SystemParams.ulTotalAddressableSectors);
		Console.WriteLine(deviceInfo);

		String deviceParams = String.Format(@"Number of cylinders: {0:n0}
Number of heads: {1:n0}
Current number of cylinders: {2:n0}
Current number of heads: {3:n0}
Current Sectors per track: {4:n0}
Current Sector capacity: {5:n0}",
			device.Disc.Smart.SystemParams.wNumCyls,
			device.Disc.Smart.SystemParams.wNumHeads,
			device.Disc.Smart.SystemParams.wNumCurrentCyls,
			device.Disc.Smart.SystemParams.wNumCurrentHeads,
			device.Disc.Smart.SystemParams.wNumCurrentSectorsPerTrack,
			device.Disc.Smart.SystemParams.ulCurrentSectorCapacity);
		Console.WriteLine(deviceParams);
	}
}
</pre>

Supported messages:
<ul>
	<li>IOCTL_VOLUME
		<ul>
			<li>GET_VOLUME_DISK_EXTENTS</li>
			<li>IS_CLUSTERED</li>
		</ul>
	</li>
	<li>IOCTL_DISC
		<ul>
			<li>PERFORMANCE</li>
			<li>IS_WRITABLE</li>
			<li>PERFORMANCE_OFF</li>
			<li>SMART_GET_VERSION</li>
			<li>SMART_SEND_DRIVE_COMMAND</li>
			<li>SMART_RCV_DRIVE_DATA</li>
			<li>GET_DRIVE_GEOMETRY_EX</li>
		</ul>
	</li>
	<li>IOCTL_STORAGE
		<ul>
			<li>CHECK_VERIFY</li>
			<li>CHECK_VERIFY2</li>
			<li>MEDIA_REMOVAL</li>
			<li>EJECT_MEDIA</li>
			<li>GET_MEDIA_TYPES_EX</li>
			<li>GET_MEDIA_SERIAL_NUMBER</li>
			<li>GET_HOTPLUG_INFO</li>
			<li>GET_DEVICE_NUMBER</li>
			<li>PREDICT_FAILURE</li>
			<li>QUERY_PROPERTY</li>
		</ul>
	</li>
	<li>FSCTL
		<ul>
			<li>LOCK_VOLUME</li>
			<li>UNLOCK_VOLUME</li>
			<li>DISMOUNT_VOLUME</li>
			<li>IS_VOLUME_MOUNTED</li>
			<li>FILESYSTEM_GET_STATISTICS</li>
			<li>GET_NTFS_VOLUME_DATA</li>
			<li>GET_VOLUME_BITMAP</li>
		</ul>
	</li>
</ul>
