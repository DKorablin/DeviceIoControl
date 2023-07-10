using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using AlphaOmega.Debug.Native;

namespace AlphaOmega.Debug
{
	/// <summary>Device info</summary>
	public class DeviceIoControl : IDisposable
	{
		private Boolean _disposed = false;

		private Disc _disc;
		private Storage _storage;
		private Volume _volume;
		private FileSystem _fs;
		private Changer _changer;

		/// <summary>Opened device handle</summary>
		private IntPtr DeviceHandle { get; }

		/// <summary>Device ID</summary>
		public Byte? DeviceId { get; }

		/// <summary>Name of the device</summary>
		public String DeviceName { get; }

		/// <summary>Disc IO commands</summary>
		public Disc Disc
		{
			get { return this._disc ?? (this._disc = new Disc(this)); }
		}

		/// <summary>Storage IO commands</summary>
		public Storage Storage
		{
			get { return this._storage ?? (this._storage = new Storage(this)); }
		}

		/// <summary>Volume IO commands</summary>
		public Volume Volume
		{
			get { return this._volume ?? (this._volume = new Volume(this)); }
		}

		/// <summary>File system IO commands</summary>
		/// <remarks>FSCTL will be can be null if device opened by ID</remarks>
		public FileSystem FileSystem
		{
			get
			{
				return this._fs == null && this.DeviceId == null
					? this._fs = new FileSystem(this)
					: this._fs;
			}
		}

		/// <summary>Changer IO control commands</summary>
		public Changer Changer
		{
			get { return this._changer ?? (this._changer = new Changer(this)); }
		}

		/// <summary>Get device power state</summary>
		/// <exception cref="Win32Exception">Can't gen device power state</exception>
		public Boolean IsDeviceOn
		{
			get
			{
				Boolean isOn = false;
				if(Methods.GetDevicePowerState(this.DeviceHandle, out isOn))
					return isOn;
				else
					throw new Win32Exception();
			}
		}

		/// <summary>Create instance of device info class by drive letter</summary>
		public DeviceIoControl(String deviceName)
			: this(null, deviceName)
		{
		}

		/// <summary>Create instance of device info class by device ID</summary>
		/// <param name="deviceId">Device ID</param>
		public DeviceIoControl(Byte deviceId)
			: this(deviceId, null)
		{
		}

		/// <summary>Create instance of device info class by device ID or drive letter</summary>
		/// <param name="deviceId">ID of device</param>
		/// <param name="deviceName">name of device</param>
		/// <exception cref="ArgumentException">Device id does not specified</exception>
		public DeviceIoControl(Byte? deviceId, String deviceName)
			: this(deviceId, deviceName,
			WinApi.FILE_ACCESS_FLAGS.GENERIC_READ | WinApi.FILE_ACCESS_FLAGS.GENERIC_WRITE,
			WinApi.FILE_SHARE.READ | WinApi.FILE_SHARE.WRITE)
		{
		}

		/// <summary>Create instance of device info class by device ID or drive letter</summary>
		/// <param name="deviceId">ID of device</param>
		/// <param name="deviceName">name of device</param>
		/// <param name="accessMode">Device desired access flags</param>
		/// <param name="shareMode">Opened device share mode</param>
		/// <exception cref="ArgumentException">Device id does not specified</exception>
		public DeviceIoControl(Byte? deviceId, String deviceName,WinApi.FILE_ACCESS_FLAGS accessMode, WinApi.FILE_SHARE shareMode)
		{//DriveInfo.GetDrives()
			this.DeviceId = deviceId;

			if(deviceId.HasValue)
				this.DeviceName = DeviceIoControl.GetDeviceName(deviceId.Value);
			else if(!String.IsNullOrEmpty(deviceName))
			{
				Char deviceLetter = Array.Find(deviceName.ToCharArray(), delegate (Char ch) { return Char.IsLetter(ch); });
				this.DeviceName = String.Format(CultureInfo.CurrentUICulture, Constant.DriveWinNTArg1, deviceLetter);
			} else
				throw new ArgumentException("Device id does not specified", nameof(deviceId));

			this.DeviceHandle = Methods.OpenDevice(this.DeviceName, accessMode, shareMode);
		}

		/// <summary>Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation</summary>
		/// <typeparam name="T">Return type</typeparam>
		/// <param name="dwIoControlCode">
		/// The control code for the operation.
		/// This value identifies the specific operation to be performed and the type of device on which to perform it
		/// </param>
		/// <param name="inParams">A pointer to the input buffer that contains the data required to perform the operation</param>
		/// <returns>Return type</returns>
		public T IoControl<T>(UInt32 dwIoControlCode, Object inParams) where T : struct
		{
			UInt32 bytesReturned;
			return this.IoControl<T>(dwIoControlCode, inParams, out bytesReturned);
		}

		/// <summary>Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation</summary>
		/// <typeparam name="T">Return type</typeparam>
		/// <param name="dwIoControlCode">
		/// The control code for the operation.
		/// This value identifies the specific operation to be performed and the type of device on which to perform it</param>
		/// <param name="inParams">A pointer to the input buffer that contains the data required to perform the operation</param>
		/// <param name="bytesReturned">A pointer to a variable that receives the size of the data stored in the output buffer, in bytes.</param>
		/// <returns>Return object</returns>
		public T IoControl<T>(UInt32 dwIoControlCode, Object inParams, out UInt32 bytesReturned) where T : struct
		{
			return Methods.DeviceIoControl<T>(
				this.DeviceHandle,
				dwIoControlCode,
				inParams,
				out bytesReturned);
		}

		/// <summary>Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation</summary>
		/// <param name="dwIoControlCode">
		/// The control code for the operation.
		/// This value identifies the specific operation to be performed and the type of device on which to perform it
		/// </param>
		/// <returns>Result of method execution</returns>
		public Boolean IoControl(UInt32 dwIoControlCode)
		{
			return this.IoControl(dwIoControlCode, null);
		}

		/// <summary>Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation</summary>
		/// <param name="dwIoControlCode">
		/// The control code for the operation.
		/// This value identifies the specific operation to be performed and the type of device on which to perform it
		/// </param>
		/// <param name="inParams">A pointer to the input buffer that contains the data required to perform the operation</param>
		/// <returns>Result of method execution</returns>
		public Boolean IoControl(UInt32 dwIoControlCode, Object inParams)
		{
			IntPtr dummy;
			return this.IoControl<IntPtr>(dwIoControlCode, inParams, out dummy);
		}

		/// <summary>Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation</summary>
		/// <typeparam name="T">Return type</typeparam>
		/// <param name="dwIoControlCode">
		/// The control code for the operation.
		/// This value identifies the specific operation to be performed and the type of device on which to perform it
		/// </param>
		/// <param name="inParams">A pointer to the input buffer that contains the data required to perform the operation</param>
		/// <param name="outParams">Return object</param>
		/// <returns>Result of method execution</returns>
		public Boolean IoControl<T>(UInt32 dwIoControlCode, Object inParams, out T outParams) where T : struct
		{
			UInt32 bytesReturned;
			return this.IoControl<T>(dwIoControlCode, inParams, out bytesReturned, out outParams);
		}

		/// <summary>Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation</summary>
		/// <typeparam name="T">Return type</typeparam>
		/// <param name="dwIoControlCode">
		/// The control code for the operation.
		/// This value identifies the specific operation to be performed and the type of device on which to perform it
		/// </param>
		/// <param name="inParams">A pointer to the input buffer that contains the data required to perform the operation</param>
		/// <param name="bytesReturned">A pointer to a variable that receives the size of the data stored in the output buffer, in bytes</param>
		/// <param name="outParams">Return object</param>
		/// <returns>Result of method execution</returns>
		public Boolean IoControl<T>(UInt32 dwIoControlCode, Object inParams, out UInt32 bytesReturned, out T outParams) where T : struct
		{
			return Methods.DeviceIoControl<T>(
				this.DeviceHandle,
				dwIoControlCode,
				inParams,
				out bytesReturned,
				out outParams);
		}

		/// <summary>Get all logical devices</summary>
		/// <returns>Drive name and type</returns>
		public static IEnumerable<LogicalDrive> GetLogicalDrives()
		{//DriveInfo.GetDrives();
			foreach(String drive in Environment.GetLogicalDrives())
				yield return new LogicalDrive(drive, Methods.GetDriveTypeA(drive));
		}

		/// <summary>Dispose device info and close all managed handles</summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(Boolean disposing)
		{
			if(!this._disposed
				&& this.DeviceHandle != IntPtr.Zero && this.DeviceHandle != Constant.INVALID_HANDLE_VALUE)
			{
				if(Methods.CloseHandle(this.DeviceHandle))
					this._disposed = true;
				else throw new Win32Exception();
			}
		}

		/// <summary>Destructor to close native handle</summary>
		~DeviceIoControl()
		{
			this.Dispose(false);
		}

		/// <summary>Get system device name</summary>
		/// <param name="deviceId">ID of device</param>
		/// <returns>System device name</returns>
		private static String GetDeviceName(Byte deviceId)
		{
			String result = Constant.DeviceWin32;

			if(Environment.OSVersion.Platform == PlatformID.Win32NT)
				result = String.Format(CultureInfo.CurrentUICulture, Constant.DeviceWinNTArg1, deviceId);

			return result;
		}
	}
}