using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;
using AlphaOmega.Debug.Native;

namespace AlphaOmega.Debug
{
	/// <summary>Self-Monitoring Analysis and Reporting Technology information class</summary>
	public class SmartInfo : IEnumerable<AttributeTresholds>
	{
		private readonly DeviceIoControl _device;
		private DiscAPI.IDSECTOR? _info;
		private DiscAPI.SENDCMDOUTPARAMS? _enabledParams;
		private DiscAPI.SENDCMDOUTPARAMS? _attributeParams;
		private DiscAPI.SENDCMDOUTPARAMS? _thresholdParams;
		private DiscAPI.SENDCMDOUTPARAMS? _statusParams;
		private DiscAPI.DRIVEATTRIBUTE[] _attributes;
		private DiscAPI.ATTRTHRESHOLD[] _thresholds;

		/// <summary>Device</summary>
		public DeviceIoControl Device { get { return this._device; } }

		/// <summary>Native info structure</summary>
		public DiscAPI.IDSECTOR SystemParams
		{
			get
			{
				if(!this._info.HasValue)
				{
					UInt32 bytesReturned;
					DiscAPI.SENDCMDOUTPARAMS prms = this.GetDeviceInfo(out bytesReturned);
					using(BytesReader reader = new BytesReader(prms.bBuffer))
						this._info = reader.BytesToStructure<DiscAPI.IDSECTOR>(0);
				}
				return this._info.Value;
			}
		}

		/*/// <summary>Device UDMA modes</summary>
		public IdentifyDma? Udma
		{
			get { (this.SystemParams[53] & 4) == 4 ? new IdentifyDma(this, IdentifyDma.DmaType.Udma) : (IdentifyDma?)null; }
		}*/
		/*/// <summary>Device DMA modes</summary>
		public IdentifyDma Dma
		{
			get { return new IdentifyDma(this, IdentifyDma.DmaType.Dma); }
		}*/
		/*/// <summary>Device PIO mode</summary>
		public IdentifyPio? Pio
		{
			get { return (this.SystemParams[53] & 0x0002) == 0x0002 ? new IdentifyPio(this) : (IdentifyPio?)null; }
		}*/

		/// <summary>Send a SMART_ENABLE_SMART_OPERATIONS command to the drive (DrvNum == 0..3)</summary>
		public DiscAPI.SENDCMDOUTPARAMS EnabledParams
		{
			get
			{
				if(!this._enabledParams.HasValue)
					this._enabledParams = this.SendCommand(DiscAPI.IDEREGS.SMART.ENABLE_SMART);
				return this._enabledParams.Value;
			}
		}
		/// <summary>SMART attributes native structure</summary>
		public DiscAPI.SENDCMDOUTPARAMS AttributeParams
		{
			get
			{
				if(!this._attributeParams.HasValue)
				{
					this.ToggleEnableSmart();
					this._attributeParams = this.ReadAttributes(DiscAPI.IDEREGS.SMART.READ_ATTRIBUTES);
				}
				return this._attributeParams.Value;
			}
		}
		/// <summary>SMART threshold native structure</summary>
		public DiscAPI.SENDCMDOUTPARAMS ThresholdParams
		{
			get
			{
				if(!this._thresholdParams.HasValue)
				{
					this.ToggleEnableSmart();
					this._thresholdParams = this.ReadAttributes(DiscAPI.IDEREGS.SMART.READ_THRESHOLDS);
				}
				return this._thresholdParams.Value;
			}
		}
		/// <summary>SMART status native structure</summary>
		public DiscAPI.SENDCMDOUTPARAMS StatusParams
		{
			get
			{
				if(!this._statusParams.HasValue)
					this._statusParams = this.SendCommand(DiscAPI.IDEREGS.SMART.RETURN_SMART_STATUS);
				return this._statusParams.Value;
			}
		}
		/// <summary>SMART attibutes</summary>
		public DiscAPI.DRIVEATTRIBUTE[] Attributes
		{
			get
			{
				if(this._attributes == null)
				{
					UInt32 padding = 2;
					//using(BytesReader reader = new BytesReader(SmartInfo.ConvertToByte(this.AttributeParams.bBuffer)))
					using(BytesReader reader = new BytesReader(this.AttributeParams.bBuffer))
					{
						this._attributes = new DiscAPI.DRIVEATTRIBUTE[Constant.NUM_ATTRIBUTE_STRUCTS];
						for(Int32 loop = 0;loop < this._attributes.Length;loop++)
							this._attributes[loop] = reader.BytesToStructure<DiscAPI.DRIVEATTRIBUTE>(ref padding);
					}
				}
				return _attributes;
			}
		}
		/// <summary>SMART thresholds</summary>
		public DiscAPI.ATTRTHRESHOLD[] Thresholds
		{
			get
			{
				if(this._thresholds == null)
				{
					UInt32 padding = 2;
					//using(BytesReader reader = new BytesReader(SmartInfo.ConvertToByte(this.ThresholdParams.bBuffer)))
					using(BytesReader reader = new BytesReader(this.ThresholdParams.bBuffer))
					{
						this._thresholds = new DiscAPI.ATTRTHRESHOLD[Constant.NUM_ATTRIBUTE_STRUCTS];
						for(Int32 loop = 0;loop < this._thresholds.Length;loop++)
							this._thresholds[loop] = reader.BytesToStructure<DiscAPI.ATTRTHRESHOLD>(ref padding);
					}
				}
				return this._thresholds;
			}
		}
		/// <summary>Get SMART attribute with threshold value</summary>
		/// <param name="index">Index from smart attribute array</param>
		/// <returns></returns>
		public AttributeTresholds this[UInt32 index]
		{
			get
			{
				if(index < this.Attributes.Length)
					if(this.Attributes[index].bAttrID == this.Thresholds[index].bAttrID)
						return new AttributeTresholds() { Attribute = this.Attributes[index], Threshold = this.Thresholds[index], };
					else throw new ArgumentException("Invalid Attribute/Threshold ID");
				else throw new ArgumentOutOfRangeException();
			}
		}
		/// <summary>Create instance of S.M.A.R.T. info structure</summary>
		/// <param name="device">Device info</param>
		internal SmartInfo(DeviceIoControl device)
		{
			this._device = device;
		}
		/// <summary>Get SMART attributes with thresholds</summary>
		/// <returns></returns>
		public IEnumerator<AttributeTresholds> GetEnumerator()
		{
			if(this.Attributes.Length != this.Thresholds.Length)
				throw new InvalidOperationException();
			for(UInt32 loop = 0;loop < this.Attributes.Length /*this.Attributes.Length*/;loop++)
				yield return this[loop];
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		private static Byte[] ConvertToByte(UInt16[] buffer)
		{
			List<Byte> result = new List<Byte>();
			foreach(UInt16 item in buffer)
				result.AddRange(BitConverter.GetBytes(item));
			return result.ToArray();
		}
		private void ToggleEnableSmart()
		{
			DiscAPI.SENDCMDOUTPARAMS enabled = this.EnabledParams;
		}
		private DiscAPI.SENDCMDOUTPARAMS SendCommand(DiscAPI.IDEREGS.SMART featureReg)
		{
			DiscAPI.SENDCMDINPARAMS inParams = DiscAPI.SENDCMDINPARAMS.Create(true);
			inParams.cBufferSize = Constant.BUFFER_SIZE;
			inParams.irDriveRegs.bFeaturesReg = featureReg;

			UInt32 bytesReturned;
			return this.DeviceIoControl(Constant.IOCTL_DISC.SMART_SEND_DRIVE_COMMAND,
				inParams,
				out bytesReturned);
		}
		private DiscAPI.SENDCMDOUTPARAMS ReadAttributes(DiscAPI.IDEREGS.SMART featureReg)
		{
			DiscAPI.SENDCMDINPARAMS inParams = DiscAPI.SENDCMDINPARAMS.Create(true);
			inParams.cBufferSize = Constant.BUFFER_SIZE;
			inParams.irDriveRegs.bFeaturesReg = featureReg;

			UInt32 bytesReturned;
			return this.DeviceIoControl(Constant.IOCTL_DISC.SMART_RCV_DRIVE_DATA,
				inParams,
				out bytesReturned);
		}
		/// <summary>Get system device info structure</summary>
		/// <param name="bytesReturned">Length of system device info</param>
		/// <returns>System device info structure</returns>
		private DiscAPI.SENDCMDOUTPARAMS GetDeviceInfo(out UInt32 bytesReturned)
		{
			DiscAPI.SENDCMDINPARAMS inParams = DiscAPI.SENDCMDINPARAMS.Create(false);
			inParams.cBufferSize = Constant.BUFFER_SIZE;
			inParams.irDriveRegs.bCommandReg = DiscAPI.IDEREGS.IDE.ID_CMD;

			DiscAPI.SENDCMDOUTPARAMS outParams = this.DeviceIoControl(Constant.IOCTL_DISC.SMART_RCV_DRIVE_DATA,
				inParams,
				out bytesReturned);
			return outParams;
		}
		/// <summary>Send device IO control command</summary>
		/// <param name="inParams">In command params</param>
		/// <param name="dwIoControlCode">Control code</param>
		/// <param name="bytesReturned">Bytes returned from command operation</param>
		/// <returns>Out device params</returns>
		private DiscAPI.SENDCMDOUTPARAMS DeviceIoControl(UInt32 dwIoControlCode, DiscAPI.SENDCMDINPARAMS inParams, out UInt32 bytesReturned)
		{
			DiscAPI.SENDCMDOUTPARAMS result = this.Device.IoControl<DiscAPI.SENDCMDOUTPARAMS>(
				(UInt32)dwIoControlCode,
				inParams,
				out bytesReturned);

			if(result.DriverStatus.bDriverError != DiscAPI.DRIVERSTATUS.SMART_ERROR.NO_ERROR)
				throw new InvalidOperationException(result.DriverStatus.bDriverError.ToString());
			else
				return result;
		}
	}
}