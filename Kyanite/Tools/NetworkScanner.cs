using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Security.Cryptography;
using static Kyanite.Functions;
using System.Xml;

namespace Kyanite.Tools
{
	public static class NetworkScanner
	{
		public static List<int> HighPriority = new();
		public static List<string> Known = new();

		public async static Task<List<NetworkDevice>> Scan(string ip)
		{
			Execute("nmap", $"192.168.*.* -oG {Path.Combine(AppContext.BaseDirectory, ComputeHash(ip, SHA256.Create()) + ".txt")} -A");

			Regex ipPattern = new(@"d+\.d+\.d+\.d+");
			Regex statusPattern = new(@"(Up|Down|Unknown)");
			Regex portsPattern = new(@"(?<=Port:\s).*");

		}

		static List<NetworkDevice> ParseNmapOutput(string output)
		{
			var devices = new List<NetworkDevice>();
			NetworkDevice currentDevice = new NetworkDevice();
			OpenPort currentPort = new OpenPort();
			bool inOpenPortsSection = false;

			var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var line in lines)
			{

				if (line.Contains("Nmap scan report for"))
				{
					if (currentDevice.IP != null)
					{
						devices.Add(currentDevice);
					}

					var deviceMatch = Regex.Match(line, @"Nmap scan report for (\S+) \((\S+)\)");
					if (deviceMatch.Success)
					{
						currentDevice.IP = deviceMatch.Groups[1].Value;
						currentDevice.Name = deviceMatch.Groups[2].Value;
						currentDevice.OpenPorts = new List<OpenPort>();
					}
				}

				else if (line.Contains("MAC Address"))
				{
					var macMatch = Regex.Match(line, @"MAC Address: (\S+) \((.*?)\)");
					if (macMatch.Success)
					{
						currentDevice.MAC = macMatch.Groups[1].Value;
						currentDevice.Manufacturer = macMatch.Groups[2].Value;
					}
				}

				else if (line.Contains("OS details"))
				{
					var osMatch = Regex.Match(line, @"OS details: (.*?) \(.*\)");
					if (osMatch.Success)
					{
						currentDevice.OS = osMatch.Groups[1].Value;
						currentDevice.OSVersion = osMatch.Groups[1].Value;
					}
				}

				else if (line.StartsWith("PORT"))
				{
					inOpenPortsSection = true;
				}
				else if (inOpenPortsSection && line.StartsWith(" "))
				{
					var portMatch = Regex.Match(line, @"(\d+)/(\w+)\s+(\S+)\s+(.*)");
					if (portMatch.Success)
					{
						currentPort.Port = int.Parse(portMatch.Groups[1].Value);
						currentPort.Type = portMatch.Groups[2].Value;
						currentPort.Service = portMatch.Groups[3].Value;
						currentPort.Version = portMatch.Groups[4].Value;
						currentDevice.OpenPorts.Add(currentPort);
					}
				}
			}
 
			if (currentDevice.IP != null)
			{
				devices.Add(currentDevice);
			}

			return devices;
		}
	}

	public struct OpenPort
	{
		public int Port { get; set; }
		public string Type { get; set; }
		public string Service { get; set; }
		public string Version { get; set; }
	}
	public struct NetworkDevice
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
