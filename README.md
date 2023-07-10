# DeviceIoControl assembly
[![Auto build](https://github.com/DKorablin/DeviceIoControl/actions/workflows/release.yml/badge.svg)](https://github.com/DKorablin/DeviceIoControl/releases/latest)
[![Nuget](https://img.shields.io/nuget/v/AlphaOmega.DeviceIoControl)](https://www.nuget.org/packages/AlphaOmega.DeviceIoControl)

Wrapper for DeviceIoControl function. Primary task of this assembly was to read SMART data from supported devices.
S.M.A.R.T.rar contains original source code written by Andrew I. Reshin (11.02.2001) in C++

Usage:

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

Supported structures:
- IOCTL_VOLUME
  - GET_VOLUME_DISK_EXTENTS
  - IS_CLUSTERED
- IOCTL_DISC
  - PERFORMANCE
  - IS_WRITABLE
  - PERFORMANCE_OFF
  - SMART_GET_VERSION
  - SMART_SEND_DRIVE_COMMAND
  - SMART_RCV_DRIVE_DATA
  - GET_DRIVE_GEOMETRY_EX
- IOCTL_STORAGE
  - CHECK_VERIFY
  - CHECK_VERIFY2
  - MEDIA_REMOVAL
  - EJECT_MEDIA
  - GET_MEDIA_TYPES_EX
  - GET_MEDIA_SERIAL_NUMBER
  - GET_HOTPLUG_INFO
  - GET_DEVICE_NUMBER
  - PREDICT_FAILURE
  - QUERY_PROPERTY
- FSCTL
  - LOCK_VOLUME
  - UNLOCK_VOLUME
  - DISMOUNT_VOLUME
  - IS_VOLUME_MOUNTED
  - FILESYSTEM_GET_STATISTICS
  - GET_NTFS_VOLUME_DATA
  - GET_VOLUME_BITMAP