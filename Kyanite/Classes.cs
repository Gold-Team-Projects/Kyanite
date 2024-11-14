using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyanite
{
	public enum Priority
	{
		Low,
		Normal,
		High
	}
	public struct OpenPort
	{
		public int Port { get; set; }
		public string Type { get; set; }
		public string Service { get; set; }
		public string Version { get; set; }
	}
	public struct Device
	{
		public string Name { get; set; }
		public string IP { get; set; }
		public string MAC { get; set; }
		public string Manufacturer { get; set; }
		public string DeviceType { get; set; }
		public string OS { get; set; }
		public string OSVersion { get; set; }
		public List<OpenPort> OpenPorts { get; set; }
	}
}
