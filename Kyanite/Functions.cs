using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Kyanite
{
	public static class Functions
	{
		public static string ComputeHash(string input, HashAlgorithm hasher)
		{
			byte[] data = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
			StringBuilder sb = new StringBuilder();
			foreach (byte b in data)
			{
				sb.Append(b.ToString("x2"));
			}
			return sb.ToString();
		}

		public static void Execute(string command, string args)
		{
			Process proc = new();
			proc.StartInfo.FileName = command;
			proc.StartInfo.Arguments = args;
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.RedirectStandardOutput = true;
			proc.StartInfo.CreateNoWindow = true;
			proc.Start();
			proc.WaitForExit();
		}
	}
}
