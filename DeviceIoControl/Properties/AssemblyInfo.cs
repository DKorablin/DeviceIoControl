using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("8c60d1a6-b10e-49c8-bc21-738dce7090e6")]

[assembly: System.CLSCompliant(false)]

#if !NETSTANDARD
[assembly: AssemblyProduct("DeviceIoControl")]
[assembly: AssemblyTitle("DeviceIoControl")]
[assembly: AssemblyDescription("Native DeviceIoControl wrapper assembly")]
[assembly: AssemblyCompany("Danila Korablin")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2013-2020")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
#endif