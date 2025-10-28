﻿using System;
using System.Collections;
using System.Collections.Generic;
using AlphaOmega.Debug.Native;

namespace AlphaOmega.Debug
{
	/// <summary>Self-Monitoring Analysis and Reporting Technology information class</summary>
	public class SmartInfoCollection : IEnumerable<AttributeThresholds>
	{
		/// <summary>The device</summary>
		private readonly DeviceIoControl _device;

		private DiscApi.IDSECTOR? _info;
		private DiscApi.SENDCMDOUTPARAMS? _enabledParams;
		private DiscApi.SENDCMDOUTPARAMS? _statusParams;
		private DiscApi.ATTRTHRESHOLD[] _thresholds;

		/// <summary>Native info structure</summary>
		public DiscApi.IDSECTOR SystemParams
		{
			get
			{
				if(this._info == null)
				{
					DiscApi.SENDCMDOUTPARAMS sendParams = this.GetDeviceInfo(out _);
					using(PinnedBufferReader reader = new PinnedBufferReader(sendParams.bBuffer))
						this._info = reader.BytesToStructure<DiscApi.IDSECTOR>(0);
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
		public DiscApi.SENDCMDOUTPARAMS SmartEnabled
			=> (this._enabledParams
				?? (this._enabledParams = this.SendCommand(DiscApi.IDEREGS.SMART.ENABLE_SMART))).Value;

		/// <summary>SMART status native structure</summary>
		public DiscApi.SENDCMDOUTPARAMS StatusParams
			=> (this._statusParams
				?? (this._statusParams = this.SendCommand(DiscApi.IDEREGS.SMART.RETURN_SMART_STATUS))).Value;

		/// <summary>SMART thresholds</summary>
		/// <remarks>Thresholds are cached because they are unchanged</remarks>
		public DiscApi.ATTRTHRESHOLD[] Thresholds
		{
			get
			{
				if(this._thresholds == null)
				{
					UInt32 padding = 2;
					DiscApi.SENDCMDOUTPARAMS sendParams = this.GetThresholdParamsNative();
					using(PinnedBufferReader reader = new PinnedBufferReader(sendParams.bBuffer))
					{
						this._thresholds = new DiscApi.ATTRTHRESHOLD[Constant.NUM_ATTRIBUTE_STRUCTS];
						for(Int32 loop = 0;loop < this._thresholds.Length;loop++)
							this._thresholds[loop] = reader.BytesToStructure<DiscApi.ATTRTHRESHOLD>(ref padding);
					}
				}
				return this._thresholds;
			}
		}
		/// <summary>Create instance of S.M.A.R.T. info structure</summary>
		/// <param name="device">Device info</param>
		internal SmartInfoCollection(DeviceIoControl device)
			=> this._device = device;

		/// <summary>Get SMART attributes with thresholds</summary>
		/// <returns></returns>
		public IEnumerator<AttributeThresholds> GetEnumerator()
		{
			DiscApi.DRIVEATTRIBUTE[] attributes = this.GetAttributes();
			DiscApi.ATTRTHRESHOLD[] thresholds = this.Thresholds;

			if(attributes.Length != thresholds.Length)
				throw new InvalidOperationException("Invalid size of attributes and thresholds");

			for(UInt32 loop = 0; loop < attributes.Length; loop++)
				yield return new AttributeThresholds(attributes[loop], thresholds[loop]);
		}
		
		IEnumerator IEnumerable.GetEnumerator()
			=> this.GetEnumerator();
		
		/// <summary>S.M.A.R.T. attributes</summary>
		public DiscApi.DRIVEATTRIBUTE[] GetAttributes()
		{
			DiscApi.DRIVEATTRIBUTE[] result;
			DiscApi.SENDCMDOUTPARAMS sendParams = this.GetAttributeParamsNative();
			using(PinnedBufferReader reader = new PinnedBufferReader(sendParams.bBuffer))
			{
				UInt32 padding = 2; 
				result = new DiscApi.DRIVEATTRIBUTE[Constant.NUM_ATTRIBUTE_STRUCTS];
				for(Int32 loop = 0;loop < result.Length;loop++)
					result[loop] = reader.BytesToStructure<DiscApi.DRIVEATTRIBUTE>(ref padding);
			}
			return result;
		}
		
		/// <summary>S.M.A.R.T. attributes native structure</summary>
		public DiscApi.SENDCMDOUTPARAMS GetAttributeParamsNative()
		{
			this.ToggleEnableSmart();
			return this.ReadAttributes(DiscApi.IDEREGS.SMART.READ_ATTRIBUTES);
		}
		
		/// <summary>S.M.A.R.T. threshold native structure</summary>
		/// <remarks>Reset thresholds cache</remarks>
		public DiscApi.SENDCMDOUTPARAMS GetThresholdParamsNative()
		{
			this.ToggleEnableSmart();
			this._thresholds = null;
			return this.ReadAttributes(DiscApi.IDEREGS.SMART.READ_THRESHOLDS);
		}
		
		private void ToggleEnableSmart()
			=> _ = this.SmartEnabled;
		
		private DiscApi.SENDCMDOUTPARAMS SendCommand(DiscApi.IDEREGS.SMART featureReg)
		{
			DiscApi.SENDCMDINPARAMS inParams = DiscApi.SENDCMDINPARAMS.Create(true);
			inParams.cBufferSize = Constant.BUFFER_SIZE;
			inParams.irDriveRegs.bFeaturesReg = featureReg;

			return this.DeviceIoControl(Constant.IOCTL_DISC.SMART_SEND_DRIVE_COMMAND,
				inParams,
				out _);
		}
		
		private DiscApi.SENDCMDOUTPARAMS ReadAttributes(DiscApi.IDEREGS.SMART featureReg)
		{
			DiscApi.SENDCMDINPARAMS inParams = DiscApi.SENDCMDINPARAMS.Create(true);
			inParams.cBufferSize = Constant.BUFFER_SIZE;
			inParams.irDriveRegs.bFeaturesReg = featureReg;

			return this.DeviceIoControl(Constant.IOCTL_DISC.SMART_RCV_DRIVE_DATA,
				inParams,
				out _);
		}
		
		/// <summary>Get system device info structure</summary>
		/// <param name="bytesReturned">Length of system device info</param>
		/// <returns>System device info structure</returns>
		private DiscApi.SENDCMDOUTPARAMS GetDeviceInfo(out UInt32 bytesReturned)
		{
			DiscApi.SENDCMDINPARAMS inParams = DiscApi.SENDCMDINPARAMS.Create(false);
			inParams.cBufferSize = Constant.BUFFER_SIZE;
			inParams.irDriveRegs.bCommandReg = DiscApi.IDEREGS.IDE.ID_CMD;

			DiscApi.SENDCMDOUTPARAMS outParams = this.DeviceIoControl(Constant.IOCTL_DISC.SMART_RCV_DRIVE_DATA,
				inParams,
				out bytesReturned);

			return outParams;
		}

		/// <summary>Send device IO control command</summary>
		/// <param name="inParams">In command params</param>
		/// <param name="dwIoControlCode">Control code</param>
		/// <param name="bytesReturned">Bytes returned from command operation</param>
		/// <exception cref="InvalidOperationException">Driver is in the error state</exception>
		/// <returns>Out device params</returns>
		private DiscApi.SENDCMDOUTPARAMS DeviceIoControl(UInt32 dwIoControlCode, DiscApi.SENDCMDINPARAMS inParams, out UInt32 bytesReturned)
		{
			DiscApi.SENDCMDOUTPARAMS result = this._device.IoControl<DiscApi.SENDCMDOUTPARAMS>(
				dwIoControlCode,
				inParams,
				out bytesReturned);

			return result.DriverStatus.bDriverError != DiscApi.DRIVERSTATUS.SMART_ERROR.NO_ERROR
				? throw new InvalidOperationException(result.DriverStatus.bDriverError.ToString())
				: result;
		}
	}
}