using System;
using System.Runtime.InteropServices;

namespace AlphaOmega.Debug.Native
{
	/// <summary>File System control API</summary>
	public struct FsctlAPI
	{
		/// <summary>Represents volume data. This structure is passed to the <see cref="T:Constant.FSCTL.GET_NTFS_VOLUME_DATA"/> control code.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct NTFS_VOLUME_DATA_BUFFER
		{
			/// <summary>The serial number of the volume. This is a unique number assigned to the volume media by the operating system.</summary>
			public UInt64 VolumeSerialNumber;
			/// <summary>The number of sectors in the specified volume.</summary>
			public UInt64 NumberSectors;
			/// <summary>The number of used and free clusters in the specified volume.</summary>
			public UInt64 TotalClusters;
			/// <summary>The number of free clusters in the specified volume.</summary>
			public UInt64 FreeClusters;
			/// <summary>The number of reserved clusters in the specified volume.</summary>
			public UInt64 TotalReserved;
			/// <summary>The number of bytes in a sector on the specified volume.</summary>
			public UInt32 BytesPerSector;
			/// <summary>The number of bytes in a cluster on the specified volume. This value is also known as the cluster factor.</summary>
			public UInt32 BytesPerCluster;
			/// <summary>The number of bytes in a file record segment.</summary>
			public UInt32 BytesPerFileRecordSegment;
			/// <summary>The number of clusters in a file record segment.</summary>
			public UInt32 ClustersPerFileRecordSegment;
			/// <summary>The length of the master file table, in bytes.</summary>
			public UInt64 MftValidDataLength;
			/// <summary>The starting logical cluster number of the master file table.</summary>
			public UInt64 MftStartLcn;
			/// <summary>The starting logical cluster number of the master file table mirror.</summary>
			public UInt64 Mft2StartLcn;
			/// <summary>The starting logical cluster number of the master file table zone.</summary>
			public UInt64 MftZoneStart;
			/// <summary>The ending logical cluster number of the master file table zone.</summary>
			public UInt64 MftZoneEnd;
		}
		/// <summary>Contains the starting LCN to the<see cref="T:Constant.FSCTL.GET_VOLUME_BITMAP"/> control code.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct STARTING_LCN_INPUT_BUFFER
		{
			/// <summary>
			/// The LCN from which the operation should start when describing a bitmap.
			/// This member will be rounded down to a file-system-dependent rounding boundary, and that value will be returned.
			/// </summary>
			/// <remarks>Its value should be an integral multiple of eight.</remarks>
			public UInt64 StartingLcn;
		}
		/// <summary>Represents the occupied and available clusters on a disk. This structure is the output buffer for the <see cref="T:Constant.FSCTL.GET_VOLUME_BITMAP"/> control code.</summary>
		/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa365726%28v=vs.85%29.aspx</remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct VOLUME_BITMAP_BUFFER
		{
			/// <summary>Starting LCN requested as an input to the operation.</summary>
			public UInt64 StartingLcn;
			/// <summary>The number of clusters on the volume, starting from the starting LCN returned in the StartingLcn member of this structure.</summary>
			public UInt64 BitmapSize;
			/// <summary>
			/// Array of bytes containing the bitmap that the operation returns.
			/// The bitmap is bitwise from bit zero of the bitmap to the end.
			/// Thus, starting at the requested cluster, the bitmap goes from bit 0 of byte 0, bit 1 of byte 0 ... bit 7 of byte 0, bit 0 of byte 1, and so on.
			/// The value 1 indicates that the cluster is allocated (in use).
			/// The value 0 indicates that the cluster is not allocated (free).
			/// </summary>
			/// <remarks>
			/// Необходимо возвращать массив байт, а на массив байт накладывать первую структуру, а затем читать буфер.
			/// Но если разбить на куски, скажем, по 512 байт, то чтение всего массива может занять несколько минут. Проще сразу передать достаточно большой массив.
			/// </remarks>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100000000)]
			//[MarshalAs(UnmanagedType.ByValArray, SizeConst = Constant.BUFFER_SIZE)]
			public Byte[] Buffer;
		}
	}
}