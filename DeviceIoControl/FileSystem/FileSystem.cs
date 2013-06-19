using System;
using AlphaOmega.Debug.Native;
using System.Runtime.InteropServices;
using System.ComponentModel;

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
			get
			{
				UInt32 bytesReturned;
				IntPtr outParams;
				return this.Device.IoControl<IntPtr>(
					Constant.FSCTL.IS_VOLUME_MOUNTED,
					null,
					out bytesReturned,
					out outParams);
			}
		}
		/// <summary>Create instance of file system IO commands class</summary>
		/// <param name="device">Device</param>
		public FileSystem(DeviceIoControl device)
		{
			this._device = device;
		}
		/// <summary>Retrieves a bitmap of occupied and available clusters on a volume.</summary>
		/// <returns>Represents the occupied and available clusters on a disk.</returns>
		public FsctlAPI.VOLUME_BITMAP_BUFFER GetVolumeBitmap()
		{
			FsctlAPI.VOLUME_BITMAP_BUFFER result = new FsctlAPI.VOLUME_BITMAP_BUFFER() { Buffer = new Byte[] { }, BitmapSize = 0, StartingLcn = 0, };
			UInt64 startingLcn = 0;

			UInt32 bytesReturned;
			Boolean moreDataAvailable;
			do
			{
				FsctlAPI.VOLUME_BITMAP_BUFFER part = this.GetVolumeBitmap(startingLcn, out bytesReturned, out moreDataAvailable);
				if(moreDataAvailable)
					startingLcn = bytesReturned;

				result.StartingLcn = part.StartingLcn;
				result.BitmapSize = part.BitmapSize;
				Int32 length = (Int32)bytesReturned + result.Buffer.Length;

				Array.Resize(ref result.Buffer, length);
				Array.Copy(part.Buffer,
					0,
					result.Buffer,
					result.Buffer.Length - bytesReturned,
					length);
			} while(moreDataAvailable);
			return result;
		}
		/// <summary>Retrieves a bitmap of occupied and available clusters on a volume.</summary>
		/// <param name="startingLcn">
		/// The LCN from which the operation should start when describing a bitmap.
		/// This member will be rounded down to a file-system-dependent rounding boundary, and that value will be returned.
		/// Its value should be an integral multiple of eight.
		/// </param>
		/// <param name="bytesReturned">Returned length of bytes</param>
		/// <param name="moreDataAvailable">Not all data readed</param>
		/// <returns>Represents the occupied and available clusters on a disk.</returns>
		public FsctlAPI.VOLUME_BITMAP_BUFFER GetVolumeBitmap(UInt64 startingLcn,out UInt32 bytesReturned, out Boolean moreDataAvailable)
		{
			FsctlAPI.STARTING_LCN_INPUT_BUFFER fsInParams = new FsctlAPI.STARTING_LCN_INPUT_BUFFER();
			fsInParams.StartingLcn = startingLcn;//0xA000;

			FsctlAPI.VOLUME_BITMAP_BUFFER result;
			Boolean code = this.Device.IoControl<FsctlAPI.VOLUME_BITMAP_BUFFER>(
				Constant.FSCTL.GET_VOLUME_BITMAP,
				fsInParams,
				out bytesReturned,
				out result);
			Int32 errorCode = Marshal.GetLastWin32Error();

			if(code)
				moreDataAvailable = false;
			else
				if(errorCode == (Int32)Constant.ERROR.MORE_DATA)
					moreDataAvailable = true;
				else throw new Win32Exception(errorCode);
			return result;
		}
		/// <summary>
		/// Locks a volume if it is not in use.
		/// A locked volume can be accessed only through handles to the file object (*hDevice) that locks the volume.
		/// </summary>
		/// <returns>Lock action status</returns>
		public Boolean LockVolume()
		{
			UInt32 bytesReturned;
			IntPtr outParams;

			return this.Device.IoControl<IntPtr>(
				Constant.FSCTL.LOCK_VOLUME,
				null,
				out bytesReturned,
				out outParams);
		}
		/// <summary>Unlocks a volume.</summary>
		/// <returns>Unlock action status</returns>
		public Boolean UnlockVolume()
		{
			UInt32 bytesReturned;
			IntPtr outParams;

			return this.Device.IoControl<IntPtr>(
				Constant.FSCTL.UNLOCK_VOLUME,
				null,
				out bytesReturned,
				out outParams);
		}
		/// <summary>Dismounts a volume regardless of whether or not the volume is currently in use.</summary>
		/// <returns>Dismount action status</returns>
		public Boolean DismountVolume()
		{
			UInt32 bytesReturned;
			IntPtr outParams;

			return this.Device.IoControl<IntPtr>(
				Constant.FSCTL.DISMOUNT_VOLUME,
				null,
				out bytesReturned,
				out outParams);
		}
	}
}