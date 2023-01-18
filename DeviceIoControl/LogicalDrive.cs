using System;

namespace AlphaOmega.Debug
{
	/// <summary>Represents logical drive name and type</summary>
	public struct LogicalDrive
	{
		/// <summary>Name of a logical drive.</summary>
		/// <remarks>
		/// For example, if the computer's hard drive is the first logical drive, the first element returned is "C:\".
		/// </remarks>
		public String Name { get; }

		/// <summary>Disk drive is a removable, fixed, CD-ROM, RAM disk, or network drive.</summary>
		public WinApi.DRIVE Type { get; }

		internal LogicalDrive(String name, WinApi.DRIVE type)
			:this()
		{
			this.Name = name;
			this.Type = type;
		}
	}
}