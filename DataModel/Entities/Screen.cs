using System;

namespace DataModel.Entities
{
	public class Screen : Enitity
	{
		public string Name { get; set; }
		public string Title { get; set; }
		public int MyProperty { get; set; }

		public IntPtr Pointer { get; set; }
	}
}
