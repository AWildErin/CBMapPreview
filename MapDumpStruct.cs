using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBMapPreview
{
	public struct MapDumpStruct
	{
		public MapDumpStruct()
		{
		}

		public string RoomName { get; set; } = "";
		public int PosX { get; set; } = 0;
		public int PosY { get; set; }= 0;
		public int GridType { get; set; } = 0;
		public int RoomType { get; set; } = 0;
		public int RoomZone { get; set; } = 0;
	}
}
