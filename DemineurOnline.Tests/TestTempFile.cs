using System;
using System.IO;
using System.Reflection;

namespace DemineurOnline.Tests
{
	public class TestTempFile : IDisposable
	{
		public static TestTempFile New => new TestTempFile();

		public string FilePath { get; }

		public TestTempFile()
		{
			FilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Guid.NewGuid().ToString() + ".csv");
		}

		public void Dispose()
		{
			DeleteSilently();
		}

		private void DeleteSilently()
		{
			try
			{
				File.Delete(FilePath);
			}
			catch (Exception)
			{
			}
		}
	}
}