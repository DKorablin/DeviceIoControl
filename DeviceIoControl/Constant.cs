using System;
using System.Collections.Generic;

namespace AlphaOmega.Debug
{
	internal struct Constant
	{
		public const String DeviceWin32 = "\\\\.\\SMARTVSD";
		public const String DeviceWinNTArg1 = "\\\\.\\PhysicalDrive{0}";
		public const String DriveWinNTArg1 = "\\\\.\\{0}:";
		public const String DeviceScsiArg1 = "\\\\.\\Scsi{0}:";//TODO: http://read.pudn.com/downloads47/sourcecode/crypt/159311/DiskSerialNumber.cpp__.htm
		public const Int32 BUFFER_SIZE = 1024;//512 - On my new machine, the object doesn't fit in 512 bytes.
		public const UInt32 NUM_ATTRIBUTE_STRUCTS = 30;
		public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

		/// <summary>WIN32 Error</summary>
		public enum ERROR
		{
			/// <summary>The operation completed successfully</summary>
			NO_ERROR = 0,
			/// <summary>The media is write protected</summary>
			WRITE_PROTECT = 0x13,
			/// <summary>The device is not ready</summary>
			NOT_READY = 0x15,
			/// <summary>Insufficient buffer specified</summary>
			INSUFFICIENT_BUFFER = 0x7A,
			/// <summary>More data is available</summary>
			MORE_DATA = 0xEA,
			INVALID_FLAGS = 0x3EC,
		}

		/// <summary>Volume</summary>
		public struct IOCTL_VOLUME
		{
			public const UInt16 BASE = 0x00000056;
			/// <summary>Retrieves the physical location of a specified volume on one or more disks</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365194%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 GET_VOLUME_DISK_EXTENTS = Constant.CTL_CODE(BASE, 0, WinApi.FILE_ACCESS.ANY_ACCESS);
			/// <summary>Determines whether the specified volume is clustered</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365195%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 IS_CLUSTERED = Constant.CTL_CODE(BASE, 12, WinApi.FILE_ACCESS.ANY_ACCESS);
		}
		/// <summary>Disc</summary>
		public struct IOCTL_DISC
		{
			public const UInt16 BASE = 0x00000007;

			/// <summary>Retrieves information about the physical disk's geometry: type, number of cylinders, tracks per cylinder, sectors per track, and bytes per sector</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365169%28v=vs.85%29.aspx</remarks>
			[Obsolete("IOCTL_DISC.GET_DRIVE_GEOMETRY has been superseded by IOCTL_DISC.GET_DRIVE_GEOMETRY_EX, which retrieves additional information.", false)]
			public static readonly UInt32 GET_DRIVE_GEOMETRY = Constant.CTL_CODE(BASE, 0x0000, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Retrieves information about the type, size, and nature of a disk partition</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365179%28v=vs.85%29.aspx</remarks>
			[Obsolete("IOCTL_DISK.GET_PARTITION_INFO is superseded by IOCTL_DISK.GET_PARTITION_INFO_EX, which retrieves partition information for AT and Extensible Firmware Interface (EFI) partitions.",false)]
			public static readonly UInt32 GET_PARTITION_INFO = Constant.CTL_CODE(BASE, 0x0001, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Sets partition information for the specified disk partition</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365190%28v=vs.85%29.aspx</remarks>
			[Obsolete("IOCTL_DISK.SET_PARTITION_INFO has been superseded by IOCTL_DISK.SET_PARTITION_INFO_EX, which retrieves layout information for AT and EFI (Extensible Firmware Interface) partitions.",false)]
			public static readonly UInt32 SET_PARTITION_INFO = Constant.CTL_CODE(BASE, 0x0002, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			
			/// <summary>Retrieves information for each entry in the partition tables for a disk</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365173%28v=vs.85%29.aspx</remarks>
			[Obsolete("IOCTL_DISK.GET_DRIVE_LAYOUT has been superseded by IOCTL_DISK.GET_DRIVE_LAYOUT_EX, which retrieves layout information for AT and EFI (Extensible Firmware Interface) partitions.",false)]
			public static readonly UInt32 GET_DRIVE_LAYOUT = Constant.CTL_CODE(BASE, 0x0003, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Partitions a disk as specified by drive layout and partition information data</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365188%28v=vs.85%29.aspx</remarks>
			[Obsolete("IOCTL_DISK.SET_DRIVE_LAYOUT has been superseded by IOCTL_DISK.SET_DRIVE_LAYOUT_EX, which retrieves layout information for AT and EFI (Extensible Firmware Interface) partitions.",false)]
			public static readonly UInt32 SET_DRIVE_LAYOUT = Constant.CTL_CODE(BASE, 0x0004, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			
			public static readonly UInt32 VERIFY = Constant.CTL_CODE(BASE, 0x0005, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static readonly UInt32 FORMAT_TRACKS = Constant.CTL_CODE(BASE, 0x0006, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			public static readonly UInt32 REASSIGN_BLOCKS = Constant.CTL_CODE(BASE, 0x0007, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			/// <summary>
			/// Increments a reference counter that enables the collection of disk performance statistics, such as the numbers of bytes read and written since the driver last processed this request, for a corresponding disk monitoring application.
			/// In Microsoft Windows 2000 this IOCTL is handled by the filter driver diskperf.
			/// In Windows XP and later operating systems, the partition manager handles this request for disks and ftdisk.sys and dmio.sys handle this request for volumes.
			/// </summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365183%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 PERFORMANCE = Constant.CTL_CODE(BASE, 0x0008, WinApi.FILE_ACCESS.ANY_ACCESS);

			/// <summary>Determines whether the specified disk is writable</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365182%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 IS_WRITABLE = Constant.CTL_CODE(BASE, 0x0009, WinApi.FILE_ACCESS.ANY_ACCESS);

			[Obsolete("This control code is obsolete", true)]
			public static readonly UInt32 LOGGING = Constant.CTL_CODE(BASE, 0x000a, WinApi.FILE_ACCESS.ANY_ACCESS);

			public static readonly UInt32 FORMAT_TRACKS_EX = Constant.CTL_CODE(BASE, 0x000b, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);

			[Obsolete("This control code is obsolete", true)]
			public static readonly UInt32 HISTOGRAM_STRUCTURE = Constant.CTL_CODE(BASE, 0x000c, WinApi.FILE_ACCESS.ANY_ACCESS);

			[Obsolete("This control code is obsolete", true)]
			public static readonly UInt32 HISTOGRAM_DATA = Constant.CTL_CODE(BASE, 0x000d, WinApi.FILE_ACCESS.ANY_ACCESS);

			[Obsolete("This control code is obsolete", true)]
			public static readonly UInt32 HISTOGRAM_RESET = Constant.CTL_CODE(BASE, 0x000e, WinApi.FILE_ACCESS.ANY_ACCESS);

			[Obsolete("This control code is obsolete", true)]
			public static readonly UInt32 REQUEST_STRUCTURE = Constant.CTL_CODE(BASE, 0x000f, WinApi.FILE_ACCESS.ANY_ACCESS);

			[Obsolete("This control code is obsolete", true)]
			public static readonly UInt32 REQUEST_DATA = Constant.CTL_CODE(BASE, 0x0010, WinApi.FILE_ACCESS.ANY_ACCESS);

			/// <summary>Disables the counters that were enabled by previous calls to IOCTL_DISK_PERFORMANCE. This request is available in Windows XP and later operating systems. Caller must be running at IRQL = PASSIVE_LEVEL</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365184%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 PERFORMANCE_OFF = Constant.CTL_CODE(BASE, 0x0018, WinApi.FILE_ACCESS.ANY_ACCESS);

			/// <summary>Returns version information, a capabilities mask, and a bitmask for the device</summary>
			public static readonly UInt32 SMART_GET_VERSION = Constant.CTL_CODE(BASE, 0x0020, WinApi.FILE_ACCESS.READ_ACCESS);

			/// <summary>Sends one of the following Self-Monitoring Analysis and Reporting Technology (SMART) commands to the device</summary>
			public static readonly UInt32 SMART_SEND_DRIVE_COMMAND = Constant.CTL_CODE(BASE, 0x0021, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);

			/// <summary>Returns the ATA-2 identify data, the Self-Monitoring Analysis and Reporting Technology (SMART) thresholds, or the SMART attributes for the device</summary>
			public static readonly UInt32 SMART_RCV_DRIVE_DATA = Constant.CTL_CODE(BASE, 0x0022, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);

			/// <summary>Returns information about the physical disk's geometry (media type, number of cylinders, tracks per cylinder, sectors per track, and bytes per sector)</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365171%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 GET_DRIVE_GEOMETRY_EX = Constant.CTL_CODE(BASE, 0x0028, WinApi.FILE_ACCESS.ANY_ACCESS);
		}
		/// <summary>Storage</summary>
		public struct IOCTL_STORAGE
		{
			public const UInt16 BASE = 0x0000002d;

			/// <summary>Determines whether the media has changed on a removable-media device that the caller has opened for read or write access</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff560535%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 CHECK_VERIFY = Constant.CTL_CODE(BASE, 0x0200, WinApi.FILE_ACCESS.READ_ACCESS);
			/// <summary>Determines whether the media has changed on a removable-media device - the caller has opened with FILE_READ_ATTRIBUTES</summary>
			/// <remarks>
			/// Because no file system is mounted when a device is opened in this way, this request can be processed much more quickly than an IOCTL_STORAGE_CHECK_VERIFY request.
			/// http://msdn.microsoft.com/en-us/library/windows/hardware/ff560538%28v=vs.85%29.aspx
			/// </remarks>
			public static readonly UInt32 CHECK_VERIFY2 = Constant.CTL_CODE(BASE, 0x0200, WinApi.FILE_ACCESS.ANY_ACCESS);
			/// <summary>Enables or disables the mechanism that ejects media, for those devices possessing that locking capability</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa363416%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 MEDIA_REMOVAL = Constant.CTL_CODE(BASE, 0x0201, WinApi.FILE_ACCESS.READ_ACCESS);
			/// <summary>Causes the device to eject the media if the device supports ejection capabilities</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff560542%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 EJECT_MEDIA = Constant.CTL_CODE(BASE, 0x0202, WinApi.FILE_ACCESS.READ_ACCESS);
			public static readonly UInt32 LOAD_MEDIA = Constant.CTL_CODE(BASE, 0x0203, WinApi.FILE_ACCESS.READ_ACCESS);
			public static readonly UInt32 LOAD_MEDIA2 = Constant.CTL_CODE(BASE, 0x0203, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static readonly UInt32 RESERVE = Constant.CTL_CODE(BASE, 0x0204, WinApi.FILE_ACCESS.READ_ACCESS);
			public static readonly UInt32 RELEASE = Constant.CTL_CODE(BASE, 0x0205, WinApi.FILE_ACCESS.READ_ACCESS);
			public static readonly UInt32 FIND_NEW_DEVICES = Constant.CTL_CODE(BASE, 0x0206, WinApi.FILE_ACCESS.READ_ACCESS);

			public static readonly UInt32 EJECTION_CONTROL = Constant.CTL_CODE(BASE, 0x0250, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static readonly UInt32 MCN_CONTROL = Constant.CTL_CODE(BASE, 0x0251, WinApi.FILE_ACCESS.ANY_ACCESS);

			public static readonly UInt32 GET_MEDIA_TYPES = Constant.CTL_CODE(BASE, 0x0300, WinApi.FILE_ACCESS.ANY_ACCESS);
			/// <summary>Retrieves information about the types of media supported by a device</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff560563(v=vs.85).aspx</remarks>
			public static readonly UInt32 GET_MEDIA_TYPES_EX = Constant.CTL_CODE(BASE, 0x0301, WinApi.FILE_ACCESS.ANY_ACCESS);
			/// <summary>Retrieves the serial number of a USB device</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff560557%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 GET_MEDIA_SERIAL_NUMBER = Constant.CTL_CODE(BASE, 0x0304, WinApi.FILE_ACCESS.ANY_ACCESS);
			/// <summary>Retrieves the hotplug configuration of the specified device</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff560554%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 GET_HOTPLUG_INFO = Constant.CTL_CODE(BASE, 0x0305, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static readonly UInt32 SET_HOTPLUG_INFO = Constant.CTL_CODE(BASE, 0x0306, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);

			public static readonly UInt32 RESET_BUS = Constant.CTL_CODE(BASE, 0x0400, WinApi.FILE_ACCESS.READ_ACCESS);
			public static readonly UInt32 RESET_DEVICE = Constant.CTL_CODE(BASE, 0x0401, WinApi.FILE_ACCESS.READ_ACCESS);
			public static readonly UInt32 BREAK_RESERVATION = Constant.CTL_CODE(BASE, 0x0405, WinApi.FILE_ACCESS.READ_ACCESS);
			public static readonly UInt32 PERSISTENT_RESERVE_IN = Constant.CTL_CODE(BASE, 0x0406, WinApi.FILE_ACCESS.READ_ACCESS);
			public static readonly UInt32 PERSISTENT_RESERVE_OUT = Constant.CTL_CODE(BASE, 0x0407, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);

			/// <summary>Retrieves the device type, device number, and, for a partitionable device, the partition number of a device</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff560551%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 GET_DEVICE_NUMBER = Constant.CTL_CODE(BASE, 0x0420, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Polls for a prediction of device failure</summary>
			/// <remarks>
			/// This request works with the IDE disk drives that support self-monitoring analysis and reporting technology (SMART).
			/// If the drive is a SCSI drive, the class driver attempts to verify
			/// if the SCSI disk supports the equivalent IDE SMART technology by check
			/// the inquiry information on the Information Exception Control Page, X3T10/94-190 Rev 4.
			/// </remarks>
			public static readonly UInt32 PREDICT_FAILURE = Constant.CTL_CODE(BASE, 0x0440, WinApi.FILE_ACCESS.ANY_ACCESS);

			/// <summary>Retrieves the geometry information for the device</summary>
			/// <remarks>https://msdn.microsoft.com/en-us/library/windows/desktop/aa363417%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 READ_CAPACITY = Constant.CTL_CODE(BASE, 0x0450, WinApi.FILE_ACCESS.READ_ACCESS);
			
			/// <summary>Returns properties of a storage device or adapter. The request indicates the kind of information to retrieve, such as inquiry data for a device or capabilities and limitations of an adapter</summary>
			/// <remarks>
			/// <see cref="Constant.IOCTL_STORAGE.QUERY_PROPERTY"/> can also be used to determine whether the port driver supports a particular property or which fields in the property descriptor can be modified with a subsequent change-property request.
			/// http://msdn.microsoft.com/en-us/library/windows/hardware/ff560590%28v=vs.85%29.aspx
			/// </remarks>
			public static readonly UInt32 QUERY_PROPERTY = Constant.CTL_CODE(BASE, 0x0500, WinApi.FILE_ACCESS.ANY_ACCESS);

			/*public static readonly UInt32 GET_BC_PROPERTIES = Constant.CTL_CODE(0x0600, WinNT.FILE_ACCESS.READ_ACCESS);
			public static readonly UInt32 ALLOCATE_BC_STREAM = Constant.CTL_CODE(0x0601, WinNT.FILE_ACCESS.READ_ACCESS | WinNT.FILE_ACCESS.WRITE_ACCESS);
			public static readonly UInt32 FREE_BC_STREAM = Constant.CTL_CODE(0x0602, WinNT.FILE_ACCESS.READ_ACCESS | WinNT.FILE_ACCESS.WRITE_ACCESS);*/

			/// <summary>Windows applications can use this control code to query the storage device for detailed firmware information</summary>
			/// <remarks>
			/// A successful call will return information about firmware revisions, activity status, as well as read/write attributes for each slot.
			/// The amount of data returned will vary based on storage protocol.
			/// </remarks>
			public static readonly UInt32 FIRMWARE_GET_INFO = Constant.CTL_CODE(BASE, 0x0700, WinApi.METHOD.BUFFERED, WinApi.FILE_ACCESS.ANY_ACCESS);
		}
		/// <summary>SCSI</summary>
		public struct IOCTL_SCSI
		{
			public const UInt16 BASE = 0x00000004;

			public static readonly UInt32 PASS_THROUGH = Constant.CTL_CODE(BASE, 0x0401, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			public static readonly UInt32 MINIPORT = Constant.CTL_CODE(BASE, 0x0402, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			public static readonly UInt32 GET_INQUIRY_DATA = Constant.CTL_CODE(BASE, 0x0403, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static readonly UInt32 GET_CAPABILITIES = Constant.CTL_CODE(BASE, 0x0404, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static readonly UInt32 PASS_THROUGH_DIRECT = Constant.CTL_CODE(BASE, 0x0405, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			public static readonly UInt32 GET_ADDRESS = Constant.CTL_CODE(BASE, 0x0406, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static readonly UInt32 RESCAN_BUS = Constant.CTL_CODE(BASE, 0x0407, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static readonly UInt32 GET_DUMP_POINTERS = Constant.CTL_CODE(BASE, 0x0408, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static readonly UInt32 FREE_DUMP_POINTERS = Constant.CTL_CODE(BASE, 0x0409, WinApi.FILE_ACCESS.ANY_ACCESS);
		}

		/// <summary>Changer</summary>
		public struct IOCTL_CHANGER
		{
			public const UInt16 BASE = 0x00000030;

			/// <summary>Retrieves the product data for the specified device</summary>
			public static readonly UInt32 GET_PRODUCT_DATA = Constant.CTL_CODE(BASE, 0x0002, WinApi.METHOD.BUFFERED, WinApi.FILE_ACCESS.READ_ACCESS);
		}

		/// <summary>IDE</summary>
		public struct IOCTL_IDE
		{
			public const UInt16 BASE = Constant.IOCTL_SCSI.BASE;

			public static readonly UInt32 PASS_THROUGH = Constant.CTL_CODE(BASE, 0x040a, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
		}

		/// <summary>ATA</summary>
		public struct IOCTL_ATA
		{
			public const UInt16 BASE = Constant.IOCTL_SCSI.BASE;
			/// <summary>Allows an application to send almost any ATA command to a target device</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff559309%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 PASS_THROUGH = Constant.CTL_CODE(BASE, 0x040b, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			public static readonly UInt32 PASS_THROUGH_DIRECT = Constant.CTL_CODE(BASE, 0x040c, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
		}

		/// <summary>File System</summary>
		public struct FSCTL
		{
			public const UInt16 BASE = 0x00000009;

			/// <summary>
			/// Locks a volume if it is not in use.
			/// A locked volume can be accessed only through handles to the file object (*hDevice) that locks the volume.
			/// </summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa364575%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 LOCK_VOLUME = Constant.CTL_CODE(BASE, 6, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Unlocks a volume</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa364814%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 UNLOCK_VOLUME = Constant.CTL_CODE(BASE, 7, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Dismounts a volume regardless of whether or not the volume is currently in use</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa364562%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 DISMOUNT_VOLUME = Constant.CTL_CODE(BASE, 8, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Determines whether the specified volume is mounted, or if the specified file or directory is on a mounted volume</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa364574(v=vs.85).aspx</remarks>
			public static readonly UInt32 IS_VOLUME_MOUNTED = Constant.CTL_CODE(BASE, 10, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Retrieves the information from various file system performance counters</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa364565%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 FILESYSTEM_GET_STATISTICS = Constant.CTL_CODE(BASE, 24, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Retrieves information about the specified NTFS file system volume</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa364569%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 GET_NTFS_VOLUME_DATA = Constant.CTL_CODE(BASE, 25, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			public static readonly UInt32 GET_NTFS_FILE_RECORD = Constant.CTL_CODE(BASE, 26, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Retrieves a bitmap of occupied and available clusters on a volume</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa364573%28v=vs.85%29.aspx</remarks>
			public static readonly UInt32 GET_VOLUME_BITMAP = Constant.CTL_CODE(BASE, 27, WinApi.METHOD.NEITHER, WinApi.FILE_ACCESS.ANY_ACCESS);

			public static readonly UInt32 FIND_FILES_BY_SID = Constant.CTL_CODE(BASE, 35, WinApi.METHOD.NEITHER, WinApi.FILE_ACCESS.ANY_ACCESS);

			/// <summary>Retrieves the locations of boot sectors for a volume</summary>
			public static readonly UInt32 GET_BOOT_AREA_INFO = Constant.CTL_CODE(BASE, 140, WinApi.METHOD.BUFFERED, WinApi.FILE_ACCESS.ANY_ACCESS);
		}

		/// <summary>This macro is used to create a unique system I/O control code (IOCTL)</summary>
		/// <param name="deviceType">
		/// Defines the type of device for the given IOCTL.
		/// This parameter can be no bigger then a WORD value.
		/// The values used by Microsoft are in the range 0-32767 and the values 32768-65535 are reserved for use by OEMs and IHVs.
		/// </param>
		/// <param name="function">
		/// Defines an action within the device category.
		/// That function codes 0-2047 are reserved for Microsoft, and 2048-4095 are reserved for OEMs and IHVs.
		/// The function code can be no larger then 4095.
		/// </param>
		/// <param name="access">Defines the access check value for any access.</param>
		/// <remarks>
		/// The macro can be used for defining IOCTL and FSCTL function control codes.
		/// All IOCTLs must be defined this way to ensure that no overlaps occur between Microsoft and OEMs and IHVs.
		/// </remarks>
		/// <returns>IO control code</returns>
		private static UInt32 CTL_CODE(UInt16 deviceType, UInt16 function, WinApi.FILE_ACCESS access)
			=> CTL_CODE(deviceType, function, WinApi.METHOD.BUFFERED, access);

		/// <summary>This macro is used to create a unique system I/O control code (IOCTL)</summary>
		/// <param name="deviceType">
		/// Defines the type of device for the given IOCTL.
		/// This parameter can be no bigger then a WORD value.
		/// The values used by Microsoft are in the range 0-32767 and the values 32768-65535 are reserved for use by OEMs and IHVs.
		/// </param>
		/// <param name="function">
		/// Defines an action within the device category.
		/// That function codes 0-2047 are reserved for Microsoft, and 2048-4095 are reserved for OEMs and IHVs.
		/// The function code can be no larger then 4095.
		/// </param>
		/// <param name="method">Defines the method codes for how buffers are passed for I/O and file system controls.</param>
		/// <param name="access">Defines the access check value for any access.</param>
		/// <remarks>
		/// The macro can be used for defining IOCTL and FSCTL function control codes.
		/// All IOCTLs must be defined this way to ensure that no overlaps occur between Microsoft and OEMs and IHVs.
		/// </remarks>
		/// <returns>IO control code</returns>
		private static UInt32 CTL_CODE(UInt16 deviceType, UInt16 function, WinApi.METHOD method, WinApi.FILE_ACCESS access)
			=> (UInt32)((deviceType << 16) | ((UInt16)access << 14) | (function << 2) | (Byte)method);
	}
}