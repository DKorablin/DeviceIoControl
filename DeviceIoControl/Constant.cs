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
		public const Int32 BUFFER_SIZE = 1024;//512 - На моей новой машине объект не влезает в 512 байт
		public const UInt32 NUM_ATTRIBUTE_STRUCTS = 30;
		public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

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
			public static UInt32 GET_VOLUME_DISK_EXTENTS = Constant.CTL_CODE(BASE, 0, WinApi.FILE_ACCESS.ANY_ACCESS);
			/// <summary>Determines whether the specified volume is clustered</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365195%28v=vs.85%29.aspx</remarks>
			public static UInt32 IS_CLUSTERED = Constant.CTL_CODE(BASE, 12, WinApi.FILE_ACCESS.ANY_ACCESS);
		}
		/// <summary>Disc</summary>
		public struct IOCTL_DISC
		{
			public const UInt16 BASE = 0x00000007;

			/// <summary>Retrieves information about the physical disk's geometry: type, number of cylinders, tracks per cylinder, sectors per track, and bytes per sector</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365169%28v=vs.85%29.aspx</remarks>
			[Obsolete("IOCTL_DISK.GET_DRIVE_GEOMETRY has been superseded by IOCTL_DISK.GET_DRIVE_GEOMETRY_EX, which retrieves additional information.",false)]
			public static UInt32 GET_DRIVE_GEOMETRY = Constant.CTL_CODE(BASE, 0x0000, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Retrieves information about the type, size, and nature of a disk partition</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365179%28v=vs.85%29.aspx</remarks>
			[Obsolete("IOCTL_DISK.GET_PARTITION_INFO is superseded by IOCTL_DISK.GET_PARTITION_INFO_EX, which retrieves partition information for AT and Extensible Firmware Interface (EFI) partitions.",false)]
			public static UInt32 GET_PARTITION_INFO = Constant.CTL_CODE(BASE, 0x0001, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Sets partition information for the specified disk partition</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365190%28v=vs.85%29.aspx</remarks>
			[Obsolete("IOCTL_DISK.SET_PARTITION_INFO has been superseded by IOCTL_DISK.SET_PARTITION_INFO_EX, which retrieves layout information for AT and EFI (Extensible Firmware Interface) partitions.",false)]
			public static UInt32 SET_PARTITION_INFO = Constant.CTL_CODE(BASE, 0x0002, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			
			/// <summary>Retrieves information for each entry in the partition tables for a disk</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365173%28v=vs.85%29.aspx</remarks>
			[Obsolete("IOCTL_DISK.GET_DRIVE_LAYOUT has been superseded by IOCTL_DISK.GET_DRIVE_LAYOUT_EX, which retrieves layout information for AT and EFI (Extensible Firmware Interface) partitions.",false)]
			public static UInt32 GET_DRIVE_LAYOUT = Constant.CTL_CODE(BASE, 0x0003, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Partitions a disk as specified by drive layout and partition information data</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365188%28v=vs.85%29.aspx</remarks>
			[Obsolete("IOCTL_DISK.SET_DRIVE_LAYOUT has been superseded by IOCTL_DISK.SET_DRIVE_LAYOUT_EX, which retrieves layout information for AT and EFI (Extensible Firmware Interface) partitions.",false)]
			public static UInt32 SET_DRIVE_LAYOUT = Constant.CTL_CODE(BASE, 0x0004, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			
			public static UInt32 VERIFY = Constant.CTL_CODE(BASE, 0x0005, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static UInt32 FORMAT_TRACKS = Constant.CTL_CODE(BASE, 0x0006, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			public static UInt32 REASSIGN_BLOCKS = Constant.CTL_CODE(BASE, 0x0007, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			/// <summary>
			/// Increments a reference counter that enables the collection of disk performance statistics, such as the numbers of bytes read and written since the driver last processed this request, for a corresponding disk monitoring application.
			/// In Microsoft Windows 2000 this IOCTL is handled by the filter driver diskperf.
			/// In Windows XP and later operating systems, the partition manager handles this request for disks and ftdisk.sys and dmio.sys handle this request for volumes.
			/// </summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365183%28v=vs.85%29.aspx</remarks>
			public static UInt32 PERFORMANCE = Constant.CTL_CODE(BASE, 0x0008, WinApi.FILE_ACCESS.ANY_ACCESS);
			/// <summary>Determines whether the specified disk is writable</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365182%28v=vs.85%29.aspx</remarks>
			public static UInt32 IS_WRITABLE = Constant.CTL_CODE(BASE, 0x0009, WinApi.FILE_ACCESS.ANY_ACCESS);
			[Obsolete("This control code is obsolete", true)]
			public static UInt32 LOGGING = Constant.CTL_CODE(BASE, 0x000a, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static UInt32 FORMAT_TRACKS_EX = Constant.CTL_CODE(BASE, 0x000b, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			[Obsolete("This control code is obsolete", true)]
			public static UInt32 HISTOGRAM_STRUCTURE = Constant.CTL_CODE(BASE, 0x000c, WinApi.FILE_ACCESS.ANY_ACCESS);
			[Obsolete("This control code is obsolete", true)]
			public static UInt32 HISTOGRAM_DATA = Constant.CTL_CODE(BASE, 0x000d, WinApi.FILE_ACCESS.ANY_ACCESS);
			[Obsolete("This control code is obsolete", true)]
			public static UInt32 HISTOGRAM_RESET = Constant.CTL_CODE(BASE, 0x000e, WinApi.FILE_ACCESS.ANY_ACCESS);
			[Obsolete("This control code is obsolete", true)]
			public static UInt32 REQUEST_STRUCTURE = Constant.CTL_CODE(BASE, 0x000f, WinApi.FILE_ACCESS.ANY_ACCESS);
			[Obsolete("This control code is obsolete", true)]
			public static UInt32 REQUEST_DATA = Constant.CTL_CODE(BASE, 0x0010, WinApi.FILE_ACCESS.ANY_ACCESS);
			/// <summary>Disables the counters that were enabled by previous calls to IOCTL_DISK_PERFORMANCE. This request is available in Windows XP and later operating systems. Caller must be running at IRQL = PASSIVE_LEVEL</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365184%28v=vs.85%29.aspx</remarks>
			public static UInt32 PERFORMANCE_OFF = Constant.CTL_CODE(BASE, 0x0018, WinApi.FILE_ACCESS.ANY_ACCESS);

			/// <summary>Returns version information, a capabilities mask, and a bitmask for the device</summary>
			public static UInt32 SMART_GET_VERSION = Constant.CTL_CODE(BASE, 0x0020, WinApi.FILE_ACCESS.READ_ACCESS);
			/// <summary>Sends one of the following Self-Monitoring Analysis and Reporting Technology (SMART) commands to the device</summary>
			public static UInt32 SMART_SEND_DRIVE_COMMAND = Constant.CTL_CODE(BASE, 0x0021, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			/// <summary>Returns the ATA-2 identify data, the Self-Monitoring Analysis and Reporting Technology (SMART) thresholds, or the SMART attributes for the device</summary>
			public static UInt32 SMART_RCV_DRIVE_DATA = Constant.CTL_CODE(BASE, 0x0022, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);

			/// <summary>Returns information about the physical disk's geometry (media type, number of cylinders, tracks per cylinder, sectors per track, and bytes per sector)</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365171%28v=vs.85%29.aspx</remarks>
			public static UInt32 GET_DRIVE_GEOMETRY_EX = Constant.CTL_CODE(BASE, 0x0028, WinApi.FILE_ACCESS.ANY_ACCESS);
		}
		/// <summary>Storage</summary>
		public struct IOCTL_STORAGE
		{
			public const UInt16 BASE = 0x0000002d;

			/// <summary>Determines whether the media has changed on a removable-media device that the caller has opened for read or write access</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff560535%28v=vs.85%29.aspx</remarks>
			public static UInt32 CHECK_VERIFY = Constant.CTL_CODE(BASE, 0x0200, WinApi.FILE_ACCESS.READ_ACCESS);
			/// <summary>Determines whether the media has changed on a removable-media device - the caller has opened with FILE_READ_ATTRIBUTES</summary>
			/// <remarks>
			/// Because no file system is mounted when a device is opened in this way, this request can be processed much more quickly than an IOCTL_STORAGE_CHECK_VERIFY request.
			/// http://msdn.microsoft.com/en-us/library/windows/hardware/ff560538%28v=vs.85%29.aspx
			/// </remarks>
			public static UInt32 CHECK_VERIFY2 = Constant.CTL_CODE(BASE, 0x0200, WinApi.FILE_ACCESS.ANY_ACCESS);
			/// <summary>Enables or disables the mechanism that ejects media, for those devices possessing that locking capability</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa363416%28v=vs.85%29.aspx</remarks>
			public static UInt32 MEDIA_REMOVAL = Constant.CTL_CODE(BASE, 0x0201, WinApi.FILE_ACCESS.READ_ACCESS);
			/// <summary>Causes the device to eject the media if the device supports ejection capabilities</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff560542%28v=vs.85%29.aspx</remarks>
			public static UInt32 EJECT_MEDIA = Constant.CTL_CODE(BASE, 0x0202, WinApi.FILE_ACCESS.READ_ACCESS);
			public static UInt32 LOAD_MEDIA = Constant.CTL_CODE(BASE, 0x0203, WinApi.FILE_ACCESS.READ_ACCESS);
			public static UInt32 LOAD_MEDIA2 = Constant.CTL_CODE(BASE, 0x0203, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static UInt32 RESERVE = Constant.CTL_CODE(BASE, 0x0204, WinApi.FILE_ACCESS.READ_ACCESS);
			public static UInt32 RELEASE = Constant.CTL_CODE(BASE, 0x0205, WinApi.FILE_ACCESS.READ_ACCESS);
			public static UInt32 FIND_NEW_DEVICES = Constant.CTL_CODE(BASE, 0x0206, WinApi.FILE_ACCESS.READ_ACCESS);

			public static UInt32 EJECTION_CONTROL = Constant.CTL_CODE(BASE, 0x0250, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static UInt32 MCN_CONTROL = Constant.CTL_CODE(BASE, 0x0251, WinApi.FILE_ACCESS.ANY_ACCESS);

			public static UInt32 GET_MEDIA_TYPES = Constant.CTL_CODE(BASE, 0x0300, WinApi.FILE_ACCESS.ANY_ACCESS);
			/// <summary>Retrieves information about the types of media supported by a device</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff560563(v=vs.85).aspx</remarks>
			public static UInt32 GET_MEDIA_TYPES_EX = Constant.CTL_CODE(BASE, 0x0301, WinApi.FILE_ACCESS.ANY_ACCESS);
			/// <summary>Retrieves the serial number of a USB device</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff560557%28v=vs.85%29.aspx</remarks>
			public static UInt32 GET_MEDIA_SERIAL_NUMBER = Constant.CTL_CODE(BASE, 0x0304, WinApi.FILE_ACCESS.ANY_ACCESS);
			/// <summary>Retrieves the hotplug configuration of the specified device</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff560554%28v=vs.85%29.aspx</remarks>
			public static UInt32 GET_HOTPLUG_INFO = Constant.CTL_CODE(BASE, 0x0305, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static UInt32 SET_HOTPLUG_INFO = Constant.CTL_CODE(BASE, 0x0306, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);

			public static UInt32 RESET_BUS = Constant.CTL_CODE(BASE, 0x0400, WinApi.FILE_ACCESS.READ_ACCESS);
			public static UInt32 RESET_DEVICE = Constant.CTL_CODE(BASE, 0x0401, WinApi.FILE_ACCESS.READ_ACCESS);
			public static UInt32 BREAK_RESERVATION = Constant.CTL_CODE(BASE, 0x0405, WinApi.FILE_ACCESS.READ_ACCESS);
			public static UInt32 PERSISTENT_RESERVE_IN = Constant.CTL_CODE(BASE, 0x0406, WinApi.FILE_ACCESS.READ_ACCESS);
			public static UInt32 PERSISTENT_RESERVE_OUT = Constant.CTL_CODE(BASE, 0x0407, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);

			/// <summary>Retrieves the device type, device number, and, for a partitionable device, the partition number of a device</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff560551%28v=vs.85%29.aspx</remarks>
			public static UInt32 GET_DEVICE_NUMBER = Constant.CTL_CODE(BASE, 0x0420, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Polls for a prediction of device failure</summary>
			/// <remarks>
			/// This request works with the IDE disk drives that support self-monitoring analysis and reporting technology (SMART).
			/// If the drive is a SCSI drive, the class driver attempts to verify
			/// if the SCSI disk supports the equivalent IDE SMART technology by check
			/// the inquiry information on the Information Exception Control Page, X3T10/94-190 Rev 4.
			/// </remarks>
			public static UInt32 PREDICT_FAILURE = Constant.CTL_CODE(BASE, 0x0440, WinApi.FILE_ACCESS.ANY_ACCESS);

			/// <summary>Retrieves the geometry information for the device</summary>
			/// <remarks>https://msdn.microsoft.com/en-us/library/windows/desktop/aa363417%28v=vs.85%29.aspx</remarks>
			public static UInt32 READ_CAPACITY = Constant.CTL_CODE(BASE, 0x0450, WinApi.FILE_ACCESS.READ_ACCESS);
			
			/// <summary>Returns properties of a storage device or adapter. The request indicates the kind of information to retrieve, such as inquiry data for a device or capabilities and limitations of an adapter</summary>
			/// <remarks>
			/// <see cref="Constant.IOCTL_STORAGE.QUERY_PROPERTY"/> can also be used to determine whether the port driver supports a particular property or which fields in the property descriptor can be modified with a subsequent change-property request.
			/// http://msdn.microsoft.com/en-us/library/windows/hardware/ff560590%28v=vs.85%29.aspx
			/// </remarks>
			public static UInt32 QUERY_PROPERTY = Constant.CTL_CODE(BASE, 0x0500, WinApi.FILE_ACCESS.ANY_ACCESS);

			/*public static UInt32 GET_BC_PROPERTIES = Constant.CTL_CODE(0x0600, WinNT.FILE_ACCESS.READ_ACCESS);
			public static UInt32 ALLOCATE_BC_STREAM = Constant.CTL_CODE(0x0601, WinNT.FILE_ACCESS.READ_ACCESS | WinNT.FILE_ACCESS.WRITE_ACCESS);
			public static UInt32 FREE_BC_STREAM = Constant.CTL_CODE(0x0602, WinNT.FILE_ACCESS.READ_ACCESS | WinNT.FILE_ACCESS.WRITE_ACCESS);*/

			/// <summary>Windows applications can use this control code to query the storage device for detailed firmware information</summary>
			/// <remarks>
			/// A successful call will return information about firmware revisions, activity status, as well as read/write attributes for each slot.
			/// The amount of data returned will vary based on storage protocol.
			/// </remarks>
			public static UInt32 FIRMWARE_GET_INFO = Constant.CTL_CODE(BASE, 0x0700, WinApi.METHOD.BUFFERED, WinApi.FILE_ACCESS.ANY_ACCESS);
		}
		/// <summary>SCSI</summary>
		public struct IOCTL_SCSI
		{
			public const UInt16 BASE = 0x00000004;

			public static UInt32 PASS_THROUGH = Constant.CTL_CODE(BASE, 0x0401, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			public static UInt32 MINIPORT = Constant.CTL_CODE(BASE, 0x0402, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			public static UInt32 GET_INQUIRY_DATA = Constant.CTL_CODE(BASE, 0x0403, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static UInt32 GET_CAPABILITIES = Constant.CTL_CODE(BASE, 0x0404, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static UInt32 PASS_THROUGH_DIRECT = Constant.CTL_CODE(BASE, 0x0405, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			public static UInt32 GET_ADDRESS = Constant.CTL_CODE(BASE, 0x0406, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static UInt32 RESCAN_BUS = Constant.CTL_CODE(BASE, 0x0407, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static UInt32 GET_DUMP_POINTERS = Constant.CTL_CODE(BASE, 0x0408, WinApi.FILE_ACCESS.ANY_ACCESS);
			public static UInt32 FREE_DUMP_POINTERS = Constant.CTL_CODE(BASE, 0x0409, WinApi.FILE_ACCESS.ANY_ACCESS);
		}

		/// <summary>Changer</summary>
		public struct IOCTL_CHANGER
		{
			public const UInt16 BASE = 0x00000030;

			/// <summary>Retrieves the product data for the specified device</summary>
			public static UInt32 GET_PRODUCT_DATA = Constant.CTL_CODE(BASE, 0x0002, WinApi.METHOD.BUFFERED, WinApi.FILE_ACCESS.READ_ACCESS);
		}
		/// <summary>IDE</summary>
		public struct IOCTL_IDE
		{
			public const UInt16 BASE = Constant.IOCTL_SCSI.BASE;

			public static UInt32 PASS_THROUGH = Constant.CTL_CODE(BASE, 0x040a, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
		}
		/// <summary>ATA</summary>
		public struct IOCTL_ATA
		{
			public const UInt16 BASE = Constant.IOCTL_SCSI.BASE;
			/// <summary>Allows an application to send almost any ATA command to a target device</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff559309%28v=vs.85%29.aspx</remarks>
			public static UInt32 PASS_THROUGH = Constant.CTL_CODE(BASE, 0x040b, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
			public static UInt32 PASS_THROUGH_DIRECT = Constant.CTL_CODE(BASE, 0x040c, WinApi.FILE_ACCESS.READ_ACCESS | WinApi.FILE_ACCESS.WRITE_ACCESS);
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
			public static UInt32 LOCK_VOLUME = Constant.CTL_CODE(BASE, 6, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Unlocks a volume</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa364814%28v=vs.85%29.aspx</remarks>
			public static UInt32 UNLOCK_VOLUME = Constant.CTL_CODE(BASE, 7, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Dismounts a volume regardless of whether or not the volume is currently in use</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa364562%28v=vs.85%29.aspx</remarks>
			public static UInt32 DISMOUNT_VOLUME = Constant.CTL_CODE(BASE, 8, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Determines whether the specified volume is mounted, or if the specified file or directory is on a mounted volume</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa364574(v=vs.85).aspx</remarks>
			public static UInt32 IS_VOLUME_MOUNTED = Constant.CTL_CODE(BASE, 10, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Retrieves the information from various file system performance counters</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa364565%28v=vs.85%29.aspx</remarks>
			public static UInt32 FILESYSTEM_GET_STATISTICS = Constant.CTL_CODE(BASE, 24, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Retrieves information about the specified NTFS file system volume</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa364569%28v=vs.85%29.aspx</remarks>
			public static UInt32 GET_NTFS_VOLUME_DATA = Constant.CTL_CODE(BASE, 25, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			public static UInt32 GET_NTFS_FILE_RECORD = Constant.CTL_CODE(BASE, 26, WinApi.FILE_ACCESS.ANY_ACCESS);
			
			/// <summary>Retrieves a bitmap of occupied and available clusters on a volume</summary>
			/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa364573%28v=vs.85%29.aspx</remarks>
			public static UInt32 GET_VOLUME_BITMAP = Constant.CTL_CODE(BASE, 27, WinApi.METHOD.NEITHER, WinApi.FILE_ACCESS.ANY_ACCESS);

			public static UInt32 FIND_FILES_BY_SID = Constant.CTL_CODE(BASE, 35, WinApi.METHOD.NEITHER, WinApi.FILE_ACCESS.ANY_ACCESS);

			/// <summary>Retrieves the locations of boot sectors for a volume</summary>
			public static UInt32 GET_BOOT_AREA_INFO = Constant.CTL_CODE(BASE, 140, WinApi.METHOD.BUFFERED, WinApi.FILE_ACCESS.ANY_ACCESS);
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
		{
			return CTL_CODE(deviceType, function, WinApi.METHOD.BUFFERED, access);
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
		/// <param name="method">Defines the method codes for how buffers are passed for I/O and file system controls.</param>
		/// <param name="access">Defines the access check value for any access.</param>
		/// <remarks>
		/// The macro can be used for defining IOCTL and FSCTL function control codes.
		/// All IOCTLs must be defined this way to ensure that no overlaps occur between Microsoft and OEMs and IHVs.
		/// </remarks>
		/// <returns>IO control code</returns>
		private static UInt32 CTL_CODE(UInt16 deviceType, UInt16 function, WinApi.METHOD method, WinApi.FILE_ACCESS access)
		{
			return (UInt32)((deviceType << 16) | ((UInt16)access << 14) | (function << 2) | (Byte)method);
		}

		private static Dictionary<Byte, String> _attributeNames;

		/// <summary>S.M.A.R.T. attributes names</summary>
		public static Dictionary<Byte, String> AttributeNames
		{
			get
			{//http://www.hdsentinel.com/help/en/56_attrib.html
				if(Constant._attributeNames == null)
				{
					Dictionary<Byte, String> result = new Dictionary<Byte, String>();
					result.Add(0x0, "<Unknown>");
					// Frequency of errors when reading data from a disk, the origin of which is due to the disk hardware.
					// Для всех дисков Seagate, Samsung (семейства F1 и более новые) и Fujitsu 2,5″ это — число внутренних коррекций данных, проведенных до выдачи в интерфейс, следовательно на пугающе огромные цифры можно реагировать спокойно.
					result.Add(0x01, "Raw Read Error Rate");
					// Overall disk performance. If the attribute value decreases, then there is a high probability that there is a problem with the disk.
					result.Add(0x02, "Throughput Performance");
					// Time to spin up a pack of discs from rest to operating speed.
					// It grows with wear of the mechanics (increased friction in the bearing, etc.), and may also indicate poor-quality power (for example, a voltage drop when the disk starts).
					result.Add(0x03, "Spin-Up Time");
					// Полное число циклов запуск-остановка шпинделя.
					// У дисков некоторых производителей (например, Seagate) — счётчик включения режима энергосбережения.
					// В поле raw value хранится общее количество запусков/остановок диска.
					result.Add(0x04, "Start/Stop Count");
					// Число операций переназначения секторов.
					// Когда диск обнаруживает ошибку чтения/записи, он помечает сектор «переназначенным» и переносит данные в специально отведённую резервную область.
					// Вот почему на современных жёстких дисках нельзя увидеть bad-блоки — все они спрятаны в переназначенных секторах.
					//Этот процесс называют remapping, а переназначенный сектор — remap.
					// Чем больше значение, тем хуже состояние поверхности дисков.
					// Поле raw value содержит общее количество переназначенных секторов.
					// Рост значения этого атрибута может свидетельствовать об ухудшении состояния поверхности блинов диска.
					result.Add(0x05, "Reallocated Sectors Count");
					// Запас канала чтения. Назначение этого атрибута не документировано. В современных накопителях не используется.
					result.Add(0x06, "Read Channel Margin");
					// Частота ошибок при позиционировании блока магнитных головок.
					// Чем их больше, тем хуже состояние механики и/или поверхности жёсткого диска.
					// Также на значение параметра может повлиять перегрев и внешние вибрации (например, от соседних дисков в корзине).
					result.Add(0x07, "Seek Error Rate");
					// Средняя производительность операции позиционирования магнитными головками.
					// Если значение атрибута уменьшается (замедление позиционирования), то велика вероятность проблем с механической частью привода головок.
					result.Add(0x08, "Seek Time Performance");
					// Число часов (минут, секунд — в зависимости от производителя), проведённых во включенном состоянии.
					// В качестве порогового значения для него выбирается паспортное время наработки на отказ (MTBF — mean time between failure).
					result.Add(0x09, "Power-On Hours (POH)");
					// Число повторных попыток раскрутки дисков до рабочей скорости в случае, если первая попытка была неудачной.
					// Если значение атрибута увеличивается, то велика вероятность неполадок с механической частью.
					result.Add(0x0A, "Spin-Up Retry Count");
					// Количество повторов запросов рекалибровки в случае, если первая попытка была неудачной.
					// Если значение атрибута увеличивается, то велика вероятность проблем с механической частью.
					result.Add(0x0B, "Recalibration Retries");
					result.Add(0x0C, "Device Power Cycle Count");//Количество полных циклов включения-выключения диска.
					// Число ошибок при чтении, по вине программного обеспечения, которые не поддались исправлению.
					// Все ошибки имеют не механическую природу и указывают лишь на неправильную размётку/взаимодействие с диском программ или операционной системы.
					result.Add(0x0D, "Soft Read Error Rate");
					// "Pre-Fail" Attribute used at least in HP devices.
					result.Add(0xB4, "Unused Reserved Block Count Total");
					// Western Digital and Samsung attribute.
					result.Add(0xB7, "SATA Downshift Error Count");
					result.Add(0xB8, "End-to-End error");//Данный атрибут — часть технологии HP SMART IV, это означает, что после передачи через кэш памяти буфера данных паритет данных между хостом и жестким диском не совпадают.
					// Western Digital attribute.
					result.Add(0xB9, "Head Stability");
					// Western Digital attribute.
					result.Add(0xBA, "Induced Op-Vibration Detection");
					// The count of errors that could not be recovered using hardware ECC (see attribute 0xC3).
					result.Add(0xBB, "Reported Uncorrectable Errors");
					// The count of aborted operations due to HDD timeout.
					// Normally this attribute value should be equal to zero and if the value is far above zero, then most likely there will be some serious problems with power supply or an oxidized data cable.
					result.Add(0xBC, "Command Timeout");
					// HDD producers implement a Fly Height Monitor that attempts to provide additional protections for write operations by detecting when a recording head is flying outside its normal operating range.
					// If an unsafe fly height condition is encountered, the write process is stopped, and the information is rewritten or reallocated to a safe region of the hard drive.
					// This attribute indicates the count of these errors detected over the lifetime of the drive.
					// This feature is implemented in most modern Seagate drives and some of Western Digital’s drives, beginning with the WD Enterprise WDE18300 and WDE9180 Ultra2 SCSI hard drives, and will be included on all future WD Enterprise products.
					result.Add(0xBD, "High Fly Writes");
					// Value is equal to (100−temp. °C), allowing manufacturer to set a minimum threshold which corresponds to a maximum temperature.
					result.Add(0xBE, "Temperature Difference from 100");
					// The count of errors resulting from externally induced shock & vibration.
					result.Add(0xBF, "G-sense error rate");
					// Count of times the heads are loaded off the media.
					// Heads can be unloaded without actually powering off.
					result.Add(0xC0, "Power-off retract count / Emergency Retract Cycle Count (Fujitsu)");
					// Count of load/unload cycles into head landing zone position.
					// The typical lifetime rating for laptop (2.5-in) hard drives is 300,000 to 600,000 load cycles.
					// Some laptop drives are programmed to unload the heads whenever there has not been any activity for about five seconds.
					// Many Linux installations write to the file system a few times a minute in the background.
					// As a result, there may be 100 or more load cycles per hour, and the load cycle rating may be exceeded in less than a year.
					result.Add(0xC1, "Load Cycle Count / Load/Unload Cycle Count (Fujitsu)");
					// Current internal temperature.
					// Здесь хранятся показания встроенного термодатчика для механической части диска — банки (HDA — Hard Disk Assembly).
					// Информация снимается со встроенного термодатчика, которым служит одна из магнитных головок, обычно нижняя в банке.
					// В битовых полях атрибута фиксируются текущая, минимальная и максимальная температура.
					// Не все программы, работающие со SMART, правильно разбирают эти поля, так что к их показаниям стоит относиться критически.
					result.Add(0xC2, "HDA temperature");
					// (Vendor specific raw value.) The raw value has different structure for different vendors and is often not meaningful as a decimal number.
					// Число коррекции ошибок аппаратной частью диска (чтение, позиционирование, передача по внешнему интерфейсу).
					// На дисках с SATA-интерфейсом значение нередко ухудшается при повышении частоты системной шины — SATA очень чувствителен к разгону.
					result.Add(0xC3, "Hardware ECC Recovered");
					// Count of remap operations. The raw value of this attribute shows the total count of attempts to transfer data from reallocated sectors to a spare area. Both successful & unsuccessful attempts are counted.
					result.Add(0xC4, "Reallocation Event Count");
					// Число секторов, являющихся кандидатами на замену.
					// Они не были ещё определены как плохие, но считывание с них отличается от чтения стабильного сектора, это так называемые подозрительные или нестабильные сектора.
					// В случае успешного последующего прочтения сектора он исключается из числа кандидатов.
					// В случае повторных ошибочных чтений накопитель пытается восстановить его и выполняет операцию переназначения (remaping).
					// Рост значения этого атрибута может свидетельствовать о физической деградации жёсткого диска.
					result.Add(0xC5, "Current Pending Sector Count");
					// Число неисправимых ошибок при обращении к сектору.
					// {Возможно, имелось в виду «число некорректируемых (средствами диска) секторов», но никак не число самих ошибок!}
					// В случае увеличения числа ошибок велика вероятность критических дефектов поверхности и/или механики накопителя.
					result.Add(0xC6, "Uncorrectable Sector Count");
					// Число ошибок, возникающих при передаче данных по внешнему интерфейсу в режиме UltraDMA (нарушения целостности пакетов и т. п.).
					// Рост этого атрибута свидетельствует о плохом (мятом, перекрученном) кабеле и плохих контактах.
					// Также подобные ошибки появляются при разгоне шины PCI, сбоях питания, сильных электромагнитных наводках, а иногда и по вине драйвера.
					// Возможно причина в некачественном шлейфе. Для исправления попробуйте использовать SATA шлейф без защёлок, имеющий плотное соединение с контактами диска.
					result.Add(0xC7, "UltraDMA CRC Error Count");
					// Показывает общее количество ошибок, происходящих при записи сектора.
					// Показывает общее число ошибок записи на диск. Может служить показателем качества поверхности и механики накопителя.
					result.Add(0xC8, "Write Error Rate / Multi-Zone Error Rate");
					// Частота появления «программных» ошибок при чтении данных с диска.
					// Данный параметр показывает частоту появления ошибок при операциях чтения с поверхности диска по вине программного обеспечения, а не аппаратной части накопителя.
					result.Add(0xC9, "Soft read error rate");
					result.Add(0xCA, "Data Address Mark errors");//Number of Data Address Mark (DAM) errors (or) vendor-specific.
					result.Add(0xCB, "Run out cancel");//Количество ошибок ECC.
					result.Add(0xCC, "Soft ECC correction");//Количество ошибок ECC, скорректированных программным способом.
					result.Add(0xCD, "Thermal asperity rate (TAR)");//Number of thermal asperity errors.
					result.Add(0xCE, "Flying height");//Высота между головкой и поверхностью диска.
					result.Add(0xCF, "Spin high current");//Величина силы тока при раскрутке диска.
					result.Add(0xD0, "Spin buzz");//Number of buzz routines to spin up the drive.
					result.Add(0xD1, "Offline seek performance");//Производительность поиска во время офлайновых операций (Drive’s seek performance during offline operations.)
					// (found in a Maxtor 6B200M0 200GB and Maxtor 2R015H1 15GB disks)
					result.Add(0xD2, "Vibration During Write");
					// Vibration During Write
					result.Add(0xD3, "Vibration During Write");
					// Shock During Write
					result.Add(0xD4, "Shock During Write");
					// Distance the disk has shifted relative to the spindle (usually due to shock or temperature). Unit of measure is unknown.
					// Дистанция смещения блока дисков относительно шпинделя. В основном возникает из-за удара или падения. Единица измерения неизвестна. При увеличении атрибута диск быстро становится неработоспособным.
					result.Add(0xDC, "Disk Shift");
					result.Add(0xDD, "G-Sense Error Rate");//Число ошибок, возникших из-за внешних нагрузок и ударов. Атрибут хранит показания встроенного датчика удара.
					result.Add(0xDE, "Loaded Hours");//Время, проведённое блоком магнитных головок между выгрузкой из парковочной области в рабочую область диска и загрузкой блока обратно в парковочную область.
					result.Add(0xDF, "Load/Unload Retry Count");//Количество новых попыток выгрузок/загрузок блока магнитных головок в/из парковочной области после неудачной попытки.
					result.Add(0xE0, "Load Friction");//Величина силы трения блока магнитных головок при его выгрузке из парковочной области.
					result.Add(0xE1, "Load Cycle Count");//Количество циклов перемещения блока магнитных головок в парковочную область.
					result.Add(0xE2, "Load 'In'-time");//Время, за которое привод выгружает магнитные головки из парковочной области на рабочую поверхность диска.
					result.Add(0xE3, "Torque Amplification Count");//Количество попыток скомпенсировать вращающий момент.
					result.Add(0xE4, "Power-Off Retract Cycle");//Количество повторов автоматической парковки блока магнитных головок в результате выключения питания.
					result.Add(0xE6, "Drive Life Protection Status");//Current state of drive operation based upon the Life Curve
					result.Add(0xE7, "Temperature");//Drive Temperature
					// Number of physical erase cycles completed on the drive as a percentage of the maximum physical erase cycles the drive is designed to endure
					// Intel SSD reports the number of available reserved space as a percentage of reserved space in a brand new SSD.
					result.Add(0xE8, "Endurance Remaining / Available Reserved Space");
					// Number of hours elapsed in the power-on state.
					// Intel SSD reports a normalized value of 100 (when the SSD is new) and declines to a minimum value of 1. It decreases while the NAND erase cycles increase from 0 to the maximum-rated cycles.
					result.Add(0xE9, "Power-On Hours / Media Wearout Indicator");
					// Decoded as: byte 0-1-2 = average erase count (big endian) and byte 3-4-5 = max erase count (big endian)
					result.Add(0xEA, "Average erase count AND Maximum Erase Count");
					// decoded as: byte 0-1-2 = good block count (big endian) and byte 3-4 = system(free) block count.
					result.Add(0xEB, "Good Block Count AND System(Free) Block Count");
					// Time while head is positioning
					// Count of times the link is reset during a data transfer.
					result.Add(0xF0, "Head flying hours / Transfer Error Rate (Fujitsu)");
					// Total count of LBAs written
					result.Add(0xF1, "Total LBAs Written");
					// Total count of LBAs read.
					// Some S.M.A.R.T. utilities will report a negative number for the raw value since in reality it has 48 bits rather than 32.
					result.Add(0xF2, "Total LBAs Read");
					// Count of errors while reading from a disk
					result.Add(0xFA, "Read Error Retry Rate");
					// Count of "Free Fall Events" detected
					result.Add(0xFE, "Free Fall Protection");
					Constant._attributeNames = result;
				}
				return Constant._attributeNames;
			}
		}
	}
}