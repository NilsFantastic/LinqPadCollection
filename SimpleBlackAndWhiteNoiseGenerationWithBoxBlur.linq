<Query Kind="Program">
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
	var noise = new Noise();
	var blur = new Blur();
	var noisePixels = noise.GenerateAlphaNoise(Program.width, Program.height);
	Program.GenerateBitmap(noisePixels).Dump();
	Program.GenerateBitmap(blur.Box(noisePixels)).Dump();
}

static class Program{
	public static int width = 200;
	public static int height = 200;
	public static void ForXY(Action<int, int> action){
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				action(x, y);
			}
		}
	}
	
	public static Bitmap GenerateBitmap(int[,] pixelsAlphas){
		var im = new Bitmap(width,height);
		ForXY((x,y)=>im.SetPixel(x,y, Color.FromArgb(pixelsAlphas[x,y],0, 0, 0)));
		return im;
	}
}

class Noise{
	public int[,] GenerateAlphaNoise(int width, int height){
		var list = new int[width,height];
		var random = new Random();
		Program.ForXY((x,y)=>list[x,y] = random.Next(100));
		return list;
	}
}

class Blur{
	public int[,] Box(int[,] list){
		Program.ForXY((x,y)=>{
			list[x,y] = 
			list[Bx(x-1),By(y+1)]+  list[x,By(y+1)]+  list[Bx(x+1),By(y+1)]+
			list[Bx(x-1),y]+        list[x,y]+        list[Bx(x+1),y]+
			list[Bx(x-1),By(y-1)]+  list[x,By(y-1)]+  list[Bx(x+1),By(y-1)];
			list[x,y] = list[x,y] /9;
		});
		return list;
	}
	int Bx(int x){
		return Clamp(x, 0, Program.width-1);
	}
	int By(int y){
		return Clamp(y, 0, Program.height-1);
	}
	static int Clamp(int value, int low, int heigh){
		return Math.Min(Math.Max(low, value), heigh);
	}
}
