using System.Diagnostics;
using System.Text.Json;

namespace CBMapPreview
{
	public partial class Form1 : Form
	{
		private Label infoLabel;
		private const int cellSize = 30;
		private const int gridSize = 19;

		List<List<MapDumpStruct>> MapArray = new List<List<MapDumpStruct>>();

		public Form1()
		{
			this.Text = "CBMapPreview";
			this.ClientSize = new Size(gridSize * cellSize, gridSize * cellSize + 30); // Adjust size to fit the grid and label

			var text = File.ReadAllText("S:\\random\\random-cpp\\SCPRoomGen\\testdata\\mapdump_MyMap.json");
			//var text = File.ReadAllText("S:\\random\\scp-ref\\scpcb\\mapdump_MyMap_Test1.json");
			MapArray = JsonSerializer.Deserialize<List<List<MapDumpStruct>>>(text);

			// Use modern event handler syntax
			this.Paint += GridForm_Paint;
			this.MouseMove += GridForm_MouseMove;

			infoLabel = new Label();
			infoLabel.Dock = DockStyle.Bottom;
			infoLabel.TextAlign = ContentAlignment.MiddleCenter;
			infoLabel.Height = 30;
			this.Controls.Add(infoLabel);

			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			MinimizeBox = false;
		}

		private float GetZone(int Y)
		{
			return Math.Min(MathF.Floor((float)(19 - Y) / 19 * 3), 3 - 1);
		}

		private void GridForm_Paint(object? sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Pen blackPen = new Pen(Color.Black, 1);

			Brush CheckpointBrush = new SolidBrush(Color.FromArgb(255, 0, 200, 0));
			Brush FourBrush = new SolidBrush(Color.FromArgb(255, 50, 50, 255));
			Brush ThreeBrush = new SolidBrush(Color.FromArgb(255, 50, 255, 255));
			Brush TwoBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 0));
			Brush OneBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));

			Brush Zone1Brush = new SolidBrush(Color.FromArgb(255, 50, 50, 50));
			Brush Zone2Brush = new SolidBrush(Color.FromArgb(255, 100, 100, 100));
			Brush Zone3Brush = new SolidBrush(Color.FromArgb(255, 150, 150, 150));

			int x = 0;
			int y = 0;
			foreach (var Row in MapArray)
			{
				foreach (var Col in Row)
				{
					Brush color = new SolidBrush(Color.White);
			
					if (Col.RoomType == 0)
					{
						float zone = GetZone(Col.PosY);
						int iZone = (int)zone;
						color = new SolidBrush(Color.FromArgb(255, 50 * iZone, 50 * iZone, 50 * iZone));
					}
					else
					{
						switch(Col.GridType)
						{
							case 255:
								color = CheckpointBrush;
								break;
							case 4:
								color = FourBrush;
								break;
							case 3:
								color = ThreeBrush;
								break;
							case 2:
								color = TwoBrush;
								break;
							case 1:
								color = OneBrush;
								break;
						}
					}
			
					int locX = (gridSize - 1 - Col.PosX) * cellSize;
					int locY = Col.PosY * cellSize;
					g.FillRectangle(color, locX, locY, cellSize, cellSize);
					g.DrawRectangle(blackPen, locX, locY, cellSize, cellSize);
				}
			}
		}

		private void GridForm_MouseMove(object? sender, MouseEventArgs e)
		{
			int row = e.Y / cellSize;
			int col = (gridSize - 1) - (e.X / cellSize);

			if (row >= 0 && row < gridSize && col >= 0 && col < gridSize)
			{
				MapDumpStruct data = MapArray[col][row];

				infoLabel.Text = $"Hovering over {data.RoomName} at X: {col}, Y: {row}. Zone: {data.RoomZone}, RoomType: {data.RoomType}, GridType: {data.GridType} ";
			}
			else
			{
				infoLabel.Text = "";
			}
		}
	}
}
