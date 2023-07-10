using System;
using System.Text;
using AlphaOmega.Debug;

namespace Demo
{
	class Program
	{
		static void Main(String[] args)
		{
			//try
			//{
				using(DeviceIoControl device = new DeviceIoControl("c:\\"))
				{
					// Unsafe test code
					//Boolean isMoreDataAvailable;
					//var info = device.FileSystem.GetStatisticsEx(out isMoreDataAvailable);

					//WindowsIdentity.IPrincipal principal = Thread.CurrentPrincipal;
					//WindowsIdentity id = principal.Identity as WindowsIdentity;
					//var data = device.FileSystem.FindFilesBySid("Test@test.ru");

					// ERROR: Incorrect function
					//var changerProductData = device.Changer.ProductData;

					// ERROR: The program issued a command but the command length is incorrect
					//var firmare = device.Storage.FirmwareInfo();

					var pt=device.Storage.Properties.ProductType;
					Boolean isWritable = device.Disc.IsWritable;
					Boolean checkVerify = device.Storage.CheckVerify;
					Boolean checkVerify2 = device.Storage.CheckVerify2;
					Boolean isOn = device.IsDeviceOn;
					Console.WriteLine(String.Format("Device is {0}", isOn ? "on" : "off"));
					if(device.Disc.Version.HasValue)
					{
						String capabilities = "Capabilities: ";
						if(device.Disc.Version.Value.IsAtaSupported)
							capabilities += "ATA,";
						if(device.Disc.Version.Value.IsAtapiSupported)
							capabilities += "ATAPI,";
						if(device.Disc.Version.Value.IsSmartSupported)
							capabilities += "SCSI";
						Console.WriteLine(capabilities);
					}

					Console.WriteLine("===PROPERTIES===");
					if(device.Storage.Properties.Device.HasValue)
						Console.WriteLine(String.Format("Device: {0}", device.Storage.Properties.Device.Value.Size));
					if(device.Storage.Properties.Adapter.HasValue)
						Console.WriteLine(String.Format("Adapter: {0}", device.Storage.Properties.Adapter.Value.Size));
					if(device.Storage.Properties.Id.HasValue)
						Console.WriteLine(String.Format("ID: {0:n0}", device.Storage.Properties.Id.Value.Size));
					if(device.Storage.Properties.WriteCache.HasValue)
						Console.WriteLine(String.Format("WriteCache: {0}", device.Storage.Properties.WriteCache.Value.Size));
					if(device.Storage.Properties.Miniport.HasValue)
						Console.WriteLine(String.Format("Miniport: {0}", device.Storage.Properties.Miniport.Value.Size));
					if(device.Storage.Properties.AccessAlignment.HasValue)
						Console.WriteLine(String.Format("AccessAlignment: {0}", device.Storage.Properties.AccessAlignment.Value.Size));
					if(device.Storage.Properties.SeekPenalty.HasValue)
						Console.WriteLine(String.Format("SeekPenalty: {0}", device.Storage.Properties.SeekPenalty.Value.Size));
					if(device.Storage.Properties.Trim.HasValue)
						Console.WriteLine(String.Format("Trim: {0}", device.Storage.Properties.Trim.Value.Size));
					if(device.Storage.Properties.WriteAggregation.HasValue)
						Console.WriteLine(String.Format("WriteAggregation: {0}", device.Storage.Properties.WriteAggregation.Value.Size));
					if(device.Storage.Properties.Power.HasValue)
						Console.WriteLine(String.Format("Power: {0}", device.Storage.Properties.Power.Value.Size));
					if(device.Storage.Properties.CopyOffload.HasValue)
						Console.WriteLine(String.Format("CopyOffload: {0}", device.Storage.Properties.CopyOffload.Value.Size));
					if(device.Storage.Properties.Resiliency.HasValue)
						Console.WriteLine(String.Format("Resilency: {0}", device.Storage.Properties.Resiliency.Value.Size));

					UInt64 totalSize = 0;
					foreach(var bitmap in device.FileSystem.GetVolumeBitmap())
						totalSize += bitmap.BitmapSize;
					
					if(device.Disc.Smart != null)
					{
						Console.WriteLine("===INFO===");
						//Info
						String deviceInfo = String.Format(@"Type: {0}
Serial number: {1}\
Firmware version: {2}
Model number: {3}
Capabilities: 0x{4:X}
User addressable space {5:n0} sectors (LBA mode only)",
							device.Disc.Smart.SystemParams.Type,
							device.Disc.Smart.SystemParams.SerialNumber,
							device.Disc.Smart.SystemParams.FirmwareRev,
							device.Disc.Smart.SystemParams.ModelNumber,
							device.Disc.Smart.SystemParams.wCapabilities,
							device.Disc.Smart.SystemParams.ulTotalAddressableSectors);
						Console.WriteLine(deviceInfo);
						//Info

						//Params
						String deviceParams = String.Format(@"Number of cylinders: {0:n0}
Number of heads: {1:n0}
Current number of cylinders: {2:n0}
Current number of heads: {3:n0}
Current Sectors per track: {4:n0}
Current Sector capacity: {5:n0}",
							device.Disc.Smart.SystemParams.wNumCyls,
							device.Disc.Smart.SystemParams.wNumHeads,
							device.Disc.Smart.SystemParams.wNumCurrentCyls,
							device.Disc.Smart.SystemParams.wNumCurrentHeads,
							device.Disc.Smart.SystemParams.wNumCurrentSectorsPerTrack,
							device.Disc.Smart.SystemParams.ulCurrentSectorCapacity);
						Console.WriteLine(deviceParams);
						//Params

						//SMART
						StringBuilder smartAttributes = new StringBuilder();
						foreach(var attr in device.Disc.Smart)
							smartAttributes.AppendFormat(@"{0:X} {1} Value: {2} Worst: {3} Threshold: {4} RawValue: {5:n0}
",
								attr.Attribute.bAttrID,
								attr.Attribute.AttributeName,
								attr.Attribute.bAttrValue,
								attr.Attribute.bWorstValue,
								attr.Threshold.bWarrantyThreshold,
								attr.Attribute.RawValue);
						Console.WriteLine(smartAttributes);
						//SMART
					}

					/*using(DiscPerformance perf = device.GetDiscPerfomance())
					{
						WinNT.DISK_PERFORMANCE perf1 = perf.QueryPerfomanceInfo();
						System.Threading.Thread.Sleep(1000);
						WinNT.DISK_PERFORMANCE perf2 = perf.QueryPerfomanceInfo();
					}*/
				}
			//} catch(Win32Exception exc)
			//{
			//	Console.WriteLine("{0} ({1:X})", exc.Message, exc.NativeErrorCode);
			//	Console.WriteLine("===================================");
			//	Console.WriteLine(exc.StackTrace);
			//	Console.ReadKey();
			//}
		}
	}
}
