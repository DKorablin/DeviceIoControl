using System;
using AlphaOmega.Debug.Native;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections.Generic;

namespace AlphaOmega.Debug
{
	/// <summary>File system control commands</summary>
	public class FileSystem
	{
		private readonly DeviceIoControl _device;
		private FsctlAPI.NTFS_VOLUME_DATA_BUFFER? _volumeData;

		/// <summary>Device</summary>
		public DeviceIoControl Device { get { return this._device; } }
		/// <summary>Represents volume data.</summary>
		public FsctlAPI.NTFS_VOLUME_DATA_BUFFER VolumeData
		{
			get
			{
				if(this._volumeData == null)
					this._volumeData = this.Device.IoControl<FsctlAPI.NTFS_VOLUME_DATA_BUFFER>(
						Constant.FSCTL.GET_NTFS_VOLUME_DATA, null);
				return this._volumeData.Value;
			}
		}
		/// <summary>Determines whether the specified volume is mounted</summary>
		public Boolean IsVolumeMounted
		{
			get { return this.Device.IoControl(Constant.FSCTL.IS_VOLUME_MOUNTED); }
		}
		/// <summary>Create instance of file system IO commands class</summary>
		/// <param name="device">Device</param>
		public FileSystem(DeviceIoControl device)
		{
			this._device = device;
		}
		/// <summary>Retrieves a bitmap of occupied and available clusters on a volume.</summary>
		/// <returns>Represents the occupied and available clusters on a disk.</returns>
		public IEnumerable<FsctlAPI.VOLUME_BITMAP_BUFFER> GetVolumeBitmap()
		{
			UInt64 startingLcn = 0;
			UInt32 bytesReturned;
			Boolean moreDataAvailable = false;

			do
			{
				FsctlAPI.VOLUME_BITMAP_BUFFER part = this.GetVolumeBitmap(startingLcn, out bytesReturned, out moreDataAvailable);
				if(moreDataAvailable)
					startingLcn = bytesReturned;
				yield return part;
			} while(moreDataAvailable);
		}
		/// <summary>Retrieves a bitmap of occupied and available clusters on a volume.</summary>
		/// <param name="startingLcn">
		/// The LCN from which the operation should start when describing a bitmap.
		/// This member will be rounded down to a file-system-dependent rounding boundary, and that value will be returned.
		/// Its value should be an integral multiple of eight.
		/// </param>
		/// <param name="bytesReturned">Returned length of bytes</param>
		/// <param name="moreDataAvailable">Not all data readed</param>
		/// <exception cref="Win32Exception">Win32 Exception occured. See winerror.h for details</exception>
		/// <returns>Represents the occupied and available clusters on a disk.</returns>
		public FsctlAPI.VOLUME_BITMAP_BUFFER GetVolumeBitmap(UInt64 startingLcn,out UInt32 bytesReturned, out Boolean moreDataAvailable)
		{
			FsctlAPI.STARTING_LCN_INPUT_BUFFER fsInParams = new FsctlAPI.STARTING_LCN_INPUT_BUFFER();
			fsInParams.StartingLcn = startingLcn;//0xA000;

			FsctlAPI.VOLUME_BITMAP_BUFFER result;
			Boolean resultCode = this.Device.IoControl<FsctlAPI.VOLUME_BITMAP_BUFFER>(
				Constant.FSCTL.GET_VOLUME_BITMAP,
				fsInParams,
				out bytesReturned,
				out result);
			Int32 error = Marshal.GetLastWin32Error();

			if(resultCode)
				moreDataAvailable = false;
			else
				if(error == (Int32)Constant.ERROR.MORE_DATA)
					moreDataAvailable = true;
				else throw new Win32Exception(error);
			return result;
		}
		/// <summary>Retrieves the information from various file system performance counters.</summary>
		/// <returns>Contains statistical information from the file system.</returns>
		public FsctlAPI.FILESYSTEM_STATISTICS GetStatistics()
		{
			UInt32 bytesReturned;
			FsctlAPI.FILESYSTEM_STATISTICS result = this.Device.IoControl<FsctlAPI.FILESYSTEM_STATISTICS>(
				Constant.FSCTL.FILESYSTEM_GET_STATISTICS,
				null,
				out bytesReturned);

			/*const UInt32 SizeOfFileSystemStat = 56;
			using(BytesReader reader = new BytesReader(result.Data))
			{
				UInt32 padding = 0;
				switch(result.FileSystemType)
				{
				case FsctlAPI.FILESYSTEM_STATISTICS.FILESYSTEM_STATISTICS_TYPE.NTFS:
					FsctlAPI.NTFS_STATISTICS ntfsStat = reader.BytesToStructure<FsctlAPI.NTFS_STATISTICS>(ref padding);
					break;
				case FsctlAPI.FILESYSTEM_STATISTICS.FILESYSTEM_STATISTICS_TYPE.FAT:
					FsctlAPI.FAT_STATISTICS fatStat = reader.BytesToStructure<FsctlAPI.FAT_STATISTICS>(ref padding);
					break;
				case FsctlAPI.FILESYSTEM_STATISTICS.FILESYSTEM_STATISTICS_TYPE.EXFAT:
					FsctlAPI.EXFAT_STATISTICS exFatStat = reader.BytesToStructure<FsctlAPI.EXFAT_STATISTICS>(ref padding);
					break;
				}
			}*/
			return result;
		}
		/// <summary>
		/// Locks a volume if it is not in use.
		/// A locked volume can be accessed only through handles to the file object (*hDevice) that locks the volume.
		/// </summary>
		/// <exception cref="Win32Exception">Device exception</exception>
		/// <returns>Lock action status</returns>
		public void LockVolume()
		{
			this.Device.IoControl<IntPtr>(Constant.FSCTL.LOCK_VOLUME, null);
		}
		/// <summary>Unlocks a volume.</summary>
		/// <exception cref="Win32Exception">Device exception</exception>
		/// <returns>Unlock action status</returns>
		public void UnlockVolume()
		{
			this.Device.IoControl<IntPtr>(Constant.FSCTL.UNLOCK_VOLUME, null);
		}
		/// <summary>Dismounts a volume regardless of whether or not the volume is currently in use.</summary>
		/// <exception cref="Win32Exception">Device exception</exception>
		/// <returns>Dismount action status</returns>
		public void DismountVolume()
		{
			this.Device.IoControl<IntPtr>(Constant.FSCTL.DISMOUNT_VOLUME, null);
		}
	}
}