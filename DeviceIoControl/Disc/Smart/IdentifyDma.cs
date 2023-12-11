using System;

namespace AlphaOmega.Debug
{
	/// <summary>Device DMA/UDMA modes</summary>
	public readonly struct IdentifyDma
	{
		/// <summary>DMA type</summary>
		public enum DmaType
		{
			/// <summary>UDMA</summary>
			UDma = 0,
			/// <summary>DMA</summary>
			Dma = 1,
		}

		/// <summary>DMA/UDMA structure indexes</summary>
		private static readonly Int32[] DmaIndex = new Int32[] { 88, 63, };

		/// <summary>DMA/UDMA structure max bit pos</summary>
		private static readonly Int32[] DmaMaxPos = new Int32[] { 5, 2, };

		/// <summary>Device info</summary>
		private readonly SmartInfoCollection _info;

		/// <summary>Type of DMA</summary>
		private readonly DmaType _type;

		/// <summary>Max DMA/UDMA mode supported</summary>
		public Int64? Max
		{
			get
			{
				Int64 pos = this.GetHighBitPos(false);
				return pos != 1 ? pos : (Int64?)null;
			}
		}
		/// <summary>DMA/UDMA mode selected</summary>
		public Int64? Selected
		{
			get
			{
				Int64 pos = this.GetHighBitPos(true);
				return this.Max.HasValue && pos != -1 ? pos : (Int64?)null;
			}
		}

		/// <summary>Create instance of DMA/UDMA info structure</summary>
		/// <param name="info">Device info</param>
		/// <param name="type">DMA/UDMA</param>
		public IdentifyDma(SmartInfoCollection info, DmaType type)
		{
			this._info = info ?? throw new ArgumentNullException(nameof(info));
			this._type = type;
		}

		private Int64 GetHighBitPos(Boolean isSelected)
		{
			return 0;
			/*Int32 index = IdentifyDma.DmaIndex[(Int32)this.Type];
			Int32 maxPos = IdentifyDma.DmaMaxPos[(Int32)this.Type];
			return Utils.HighBitPos(
				isSelected ? this.Info.SystemParams[index] >> 8 : this.Info.SystemParams[index],
				maxPos);*/
		}

		/// <summary>DMA/UDMA values as string</summary>
		/// <returns></returns>
		public override String ToString()
		{
			Int64? max = this.Max;
			Int64? selected = this.Selected;

			String result = String.Empty;
			if(max != null)
			{
				result = $"Max {this._type.ToString().ToUpperInvariant()} mode supported {max}{(max > 0 ? " and below" : String.Empty)}";
				if(selected != null)
					result += Environment.NewLine + $"{this._type.ToString().ToUpperInvariant()} mode selected {selected}";
			}
			return result;
		}
	}
}