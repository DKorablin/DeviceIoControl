# DeviceIoControl assembly
[![Auto build](https://github.com/DKorablin/DeviceIoControl/actions/workflows/release.yml/badge.svg)](https://github.com/DKorablin/DeviceIoControl/releases/latest)
[![NuGet](https://img.shields.io/nuget/v/AlphaOmega.DeviceIoControl)](https://www.nuget.org/packages/AlphaOmega.DeviceIoControl)
[![NuGet Downloads](https://img.shields.io/nuget/dt/AlphaOmega.DeviceIoControl)](https://www.nuget.org/packages/AlphaOmega.DeviceIoControl)

Wrapper around the Windows DeviceIoControl Win32 API providing strongly-typed access to common IOCTL and FSCTL control codes (volume, disk, storage, SMART, filesystem statistics, etc.). Primary purpose is reading S.M.A.R.T. data and low-level device characteristics from physical drives.

Original SMART C++ source (by Andrew I. Reshin, 11.02.2001) is preserved in S.M.A.R.T.rar.

## Install

NuGet:

```
dotnet add package AlphaOmega.DeviceIoControl
```

Targets: .NET 8, .NET Standard 2.0, .NET Framework 3.5

## Quick usage

```csharp
using AlphaOmega.DeviceIoControl;

using(var device = new DeviceIoControl(@"c:\")) // Path to a logical drive or volume root
{
    bool isOn = device.IsDeviceOn;
    Console.WriteLine($"Device is {(isOn ? "on" : "off")}");

    // DISC SMART version & capabilities
    var version = device.Disc.Version?.Value;
    if(version != null)
    {
        Console.WriteLine("Capabilities: " +
            string.Join(", ", new [] {
                version.IsAtaSupported ? "ATA" : null,
                version.IsAtapiSupported ? "ATAPI" : null,
                version.IsSmartSupported ? "SMART" : null,
            }.Where(x => x != null)));
    }

    // SMART data
    var smart = device.Disc.Smart;
    if(smart != null)
    {
        Console.WriteLine("=== INFO ===");
        Console.WriteLine($"Model: {smart.SystemParams.ModelNumber}\n" +
                          $"Serial: {smart.SystemParams.SerialNumber}\n" +
                          $"Firmware: {smart.SystemParams.FirmwareRev}\n" +
                          $"User LBA capacity: {smart.SystemParams.ulTotalAddressableSectors:n0} sectors");
    }
}
```

## Additional examples

Enumerate disk extents of a volume:
```csharp
using(var device = new DeviceIoControl(@"c:\"))
{
    var extents = device.Volume.DiskExtents; // IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS
    foreach(var e in extents.Extents)
        Console.WriteLine($"Disk #{e.DiskNumber} offset {e.StartingOffset} length {e.ExtentLength}");
}
```

Query storage device number:
```csharp
using(var device = new DeviceIoControl(@"c:\"))
{
    var devNum = device.Storage.DeviceNumber; // IOCTL_STORAGE_GET_DEVICE_NUMBER
    Console.WriteLine($"DeviceType={devNum.DeviceType} Number={devNum.DeviceNumber} Partition={devNum.PartitionNumber}");
}
```

Safely eject removable media:
```csharp
using(var device = new DeviceIoControl(@"e:\"))
{
    device.Storage.MediaRemoval(true); // Prevent removal lock
    device.Storage.EjectMedia();       // IOCTL_STORAGE_EJECT_MEDIA
}
```

## Supported structures / control codes
- IOCTL_VOLUME
  - GET_VOLUME_DISK_EXTENTS
  - IS_CLUSTERED
- IOCTL_DISC
  - PERFORMANCE / PERFORMANCE_OFF
  - IS_WRITABLE
  - SMART_GET_VERSION
  - SMART_SEND_DRIVE_COMMAND
  - SMART_RCV_DRIVE_DATA
  - GET_DRIVE_GEOMETRY_EX
- IOCTL_STORAGE
  - CHECK_VERIFY / CHECK_VERIFY2
  - MEDIA_REMOVAL / EJECT_MEDIA
  - GET_MEDIA_TYPES_EX
  - GET_MEDIA_SERIAL_NUMBER
  - GET_HOTPLUG_INFO
  - GET_DEVICE_NUMBER
  - PREDICT_FAILURE
  - QUERY_PROPERTY
- FSCTL
  - LOCK_VOLUME / UNLOCK_VOLUME / DISMOUNT_VOLUME
  - IS_VOLUME_MOUNTED
  - FILESYSTEM_GET_STATISTICS
  - GET_NTFS_VOLUME_DATA
  - GET_VOLUME_BITMAP

## Error handling
Each property method internally calls DeviceIoControl. If the underlying API returns an error (GetLastError) it surfaces via thrown IOException / Win32Exception. Some properties can be null when not supported by device / OS.

## Security / permissions
Most queries require only read access. Operations like locking, dismounting, media removal or eject need elevated privileges and exclusive access. Always ensure handles are disposed before performing sensitive tasks.

## Performance notes
SMART queries may spin up sleeping disks. Cache results externally if invoked frequently. Methods avoid unnecessary allocations and pinvoke marshaling is minimized.

## Building
Clone and build with .NET SDK 8+ (for multi-targeting). Legacy .NET Framework 3.5 build may require VS with older targeting packs installed.

## Disclaimer
Low-level device operations can cause data loss if misused (e.g. improper locking/eject). Use read-only APIs unless you understand the implications.