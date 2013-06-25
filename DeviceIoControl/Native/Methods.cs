using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace AlphaOmega.Debug.Native
{
	/// <summary>Native methods</summary>
	internal static class Methods
	{
		/// <summary>
		/// Creates or opens a file or I/O device.
		/// The most commonly used I/O devices are as follows: file, file stream, directory, physical disk, volume, console buffer, tape drive, communications resource, mailslot, and pipe.
		/// The function returns a handle that can be used to access the file or device for various types of I/O depending on the file or device and the flags and attributes specified.
		/// </summary>
		/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa363858%28v=vs.85%29.aspx</remarks>
		/// <param name="lpFileName">The name of the file or device to be created or opened. You may use either forward slashes (/) or backslashes (\) in this name.</param>
		/// <param name="dwDesiredAccess">The requested access to the file or device, which can be summarized as read, write, both or neither zero).</param>
		/// <param name="dwShareMode">
		/// The requested sharing mode of the file or device, which can be read, write, both, delete, all of these, or none (refer to the following table).
		/// Access requests to attributes or extended attributes are not affected by this flag.
		/// </param>
		/// <param name="lpSecurityAttributes">A pointer to a SECURITY_ATTRIBUTES structure that contains two separate but related data members: an optional security descriptor, and a Boolean value that determines whether the returned handle can be inherited by child processes.</param>
		/// <param name="dwCreationDisposition">An action to take on a file or device that exists or does not exist.</param>
		/// <param name="dwFlagsAndAttributes">The file or device attributes and flags, FILE_ATTRIBUTE_NORMAL being the most common default value for files.</param>
		/// <param name="hTemplateFile">A valid handle to a template file with the GENERIC_READ access right. The template file supplies file attributes and extended attributes for the file that is being created.</param>
		/// <returns>If the function succeeds, the return value is an open handle to the specified file, device, named pipe, or mail slot.</returns>
		[DllImport("kernel32.dll", EntryPoint = "CreateFileA", SetLastError = true, CharSet = CharSet.Ansi)]
		private static extern IntPtr CreateFile(
			[In] String lpFileName,
			WinAPI.FILE_ACCESS_FLAGS dwDesiredAccess,
			WinAPI.FILE_SHARE dwShareMode,
			IntPtr lpSecurityAttributes,
			WinAPI.CreateDisposition dwCreationDisposition,
			UInt32 dwFlagsAndAttributes,
			IntPtr hTemplateFile);

		/// <summary>Opens specified device</summary>
		/// <param name="lpFileName">Device path</param>
		/// <returns>Handle to the opened device</returns>
		public static IntPtr OpenDevice(String lpFileName)
		{
			return Methods.OpenDevice(lpFileName,
				WinAPI.FILE_ACCESS_FLAGS.GENERIC_READ | WinAPI.FILE_ACCESS_FLAGS.GENERIC_WRITE,
				WinAPI.FILE_SHARE.READ | WinAPI.FILE_SHARE.WRITE);
		}
		/// <summary>Opens specified device</summary>
		/// <param name="lpFileName">Device path</param>
		/// <param name="dwDesiredAccess">Desired access flage</param>
		/// <param name="dwShareMode">Share mode</param>
		/// <exception cref="ArgumentNullException">lpFileName is null or empty</exception>
		/// <exception cref="Win32Exception">Device does not opened</exception>
		/// <returns>Handle to the opened device</returns>
		public static IntPtr OpenDevice(String lpFileName, WinAPI.FILE_ACCESS_FLAGS dwDesiredAccess, WinAPI.FILE_SHARE dwShareMode)
		{
			if(String.IsNullOrEmpty(lpFileName))
				throw new ArgumentNullException("lpFileName");

			IntPtr result = Methods.CreateFile(lpFileName,
				dwDesiredAccess,
				dwShareMode,
				IntPtr.Zero,
				WinAPI.CreateDisposition.OPEN_EXISTING,
				0,
				IntPtr.Zero);

			if(result == Constant.INVALID_HANDLE_VALUE)
				throw new Win32Exception();
			return result;
		}

		[DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
		public static extern Boolean CloseHandle(IntPtr hObject);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", EntryPoint = "GetDevicePowerState", SetLastError = true)]
		public static extern Boolean GetDevicePowerState(IntPtr hDevice, out Boolean pfOn);

		[DllImport("kernel32.dll", EntryPoint = "RequestDeviceWakeup", SetLastError = true)]
		public static extern Boolean RequestDeviceWakeup(IntPtr hDevice);

		[DllImport("kernel32.dll", EntryPoint = "CancelDeviceWakeupRequest", SetLastError = true)]
		public static extern Boolean CancelDeviceWakeupRequest(IntPtr hDevice);

		/// <summary>Determines whether a disk drive is a removable, fixed, CD-ROM, RAM disk, or network drive.</summary>
		/// <remarks>To determine whether a drive is a USB-type drive, call SetupDiGetDeviceRegistryProperty and specify the SPDRP_REMOVAL_POLICY property.</remarks>
		/// <param name="lpRootPathName">
		/// The root directory for the drive.
		/// A trailing backslash is required. If this parameter is NULL, the function uses the root of the current directory.
		/// </param>
		/// <returns>The return value specifies the type of drive.</returns>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", EntryPoint = "GetDriveTypeA", SetLastError = true)]
		public static extern WinAPI.DRIVE GetDriveTypeA(
			[In] String lpRootPathName);

		/// <summary>Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.</summary>
		/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa363216%28v=vs.85%29.aspx</remarks>
		/// <param name="hDevice">A handle to the device on which the operation is to be performed. The device is typically a volume, directory, file, or stream.</param>
		/// <param name="dwIoControlCode">The control code for the operation. This value identifies the specific operation to be performed and the type of device on which to perform it.</param>
		/// <param name="lpInBuffer">
		/// A pointer to the input buffer that contains the data required to perform the operation. The format of this data depends on the value of the dwIoControlCode parameter.
		/// This parameter can be NULL if dwIoControlCode specifies an operation that does not require input data.
		/// </param>
		/// <param name="nInBufferSize">The size of the input buffer, in bytes.</param>
		/// <param name="lpOutBuffer">
		/// A pointer to the output buffer that is to receive the data returned by the operation. The format of this data depends on the value of the dwIoControlCode parameter.
		/// This parameter can be NULL if dwIoControlCode specifies an operation that does not return data.
		/// </param>
		/// <param name="nOutBufferSize">The size of the output buffer, in bytes.</param>
		/// <param name="lpBytesReturned">A pointer to a variable that receives the size of the data stored in the output buffer, in bytes.</param>
		/// <param name="lpOverlapped">
		/// A pointer to an OVERLAPPED structure.
		/// If hDevice was opened without specifying FILE_FLAG_OVERLAPPED, lpOverlapped is ignored.
		/// </param>
		/// <returns>If the operation completes successfully, the return value is nonzero.</returns>
		[DllImport("kernel32.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
		private static extern Boolean DeviceIoControl(
			IntPtr hDevice,
			UInt32 dwIoControlCode,
			IntPtr lpInBuffer,
			UInt32 nInBufferSize,
			IntPtr lpOutBuffer,
			UInt32 nOutBufferSize,
			ref UInt32 lpBytesReturned,
			[In] ref NativeOverlapped lpOverlapped);

		public static T DeviceIoControl<T>(
			IntPtr hDevice,
			UInt32 dwIoControlCode,
			Object inParams,
			out UInt32 lpBytesReturned) where T : struct
		{
			T result;
			if(Methods.DeviceIoControl<T>(hDevice, dwIoControlCode, inParams, out lpBytesReturned, out result))
				return result;
			else
				throw new Win32Exception();
		}
		/// <summary>Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.</summary>
		/// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/aa363216%28v=vs.85%29.aspx</remarks>
		/// <param name="hDevice">A handle to the device on which the operation is to be performed. The device is typically a volume, directory, file, or stream.</param>
		/// <param name="dwIoControlCode">The control code for the operation. This value identifies the specific operation to be performed and the type of device on which to perform it.</param>
		/// <param name="inParams">
		/// A pointer to the input buffer that contains the data required to perform the operation. The format of this data depends on the value of the dwIoControlCode parameter.
		/// This parameter can be NULL if dwIoControlCode specifies an operation that does not require input data.
		/// </param>
		/// <param name="lpBytesReturned">A pointer to a variable that receives the size of the data stored in the output buffer, in bytes.</param>
		/// <param name="outBuffer">
		/// A pointer to the output buffer that is to receive the data returned by the operation. The format of this data depends on the value of the dwIoControlCode parameter.
		/// This parameter can be NULL if dwIoControlCode specifies an operation that does not return data.
		/// </param>
		/// <returns>If the operation completes successfully, the return value is nonzero.</returns>
		public static Boolean DeviceIoControl<T>(
			IntPtr hDevice,
			UInt32 dwIoControlCode,
			Object inParams,
			out UInt32 lpBytesReturned,
			out T outBuffer) where T : struct
		{
			Int32 nInBufferSize = 0;
			IntPtr lpInBuffer = IntPtr.Zero;
			IntPtr lpOutBuffer = IntPtr.Zero;

			try
			{
				if(inParams != null)
				{
					nInBufferSize = Marshal.SizeOf(inParams);
					lpInBuffer = Marshal.AllocHGlobal(nInBufferSize);
					Marshal.StructureToPtr(inParams, lpInBuffer, true);
				}

				outBuffer = new T();
				Int32 nOutBufferSize = Marshal.SizeOf(typeof(T));

				lpOutBuffer = Marshal.AllocHGlobal(nOutBufferSize);
				Marshal.StructureToPtr(outBuffer, lpOutBuffer, true);

				lpBytesReturned = 0;
				NativeOverlapped lpOverlapped = new NativeOverlapped();

				Boolean result = Methods.DeviceIoControl(
					hDevice,
					dwIoControlCode,
					lpInBuffer,
					(UInt32)nInBufferSize,
					lpOutBuffer,
					(UInt32)nOutBufferSize,
					ref lpBytesReturned,
					ref lpOverlapped);
				//if(result) В некоторых случаях даже при отрицательном значении необходимо читать исходящий буфер
				outBuffer = (T)Marshal.PtrToStructure(lpOutBuffer, typeof(T));

				return result;
			} finally
			{
				if(lpInBuffer != IntPtr.Zero)
					Marshal.FreeHGlobal(lpInBuffer);
				if(lpOutBuffer != IntPtr.Zero)
					Marshal.FreeHGlobal(lpOutBuffer);
			}
		}
	}
}