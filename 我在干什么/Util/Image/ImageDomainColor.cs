﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Inst.Util.Image
{
	public static unsafe class ImageDomainColor
	{
		
			[StructLayout(LayoutKind.Sequential)]
			public struct MajorColor : IComparable<MajorColor>
			{
				internal int Color;
				internal int Amount;
				public MajorColor(int Color, int Amount)
				{
					this.Color = Color;
					this.Amount = Amount;
				}
				public int CompareTo(MajorColor obj)
				{
					return this.Amount.CompareTo(obj.Amount);
				}
			}

			// https://www.coolphptools.com/color_extract
			// https://www.wookmark.com/image/268753/30-inspiring-examples-of-levitation-photography-inspirationfeed-com
			public static List<MajorColor> PrincipalColorAnalysis(Bitmap Bmp, int PCAAmount, int Delta = 24)
			{
				List<MajorColor> MC = new List<MajorColor>();

				int X, Y, Width, Height, Stride, Index, TotalColorAmount = 0;
				int HalfDelta;
				byte* Pointer, Scan0;
				BitmapData BmpData = Bmp.LockBits(new Rectangle(0, 0, Bmp.Width, Bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
				Height = Bmp.Height; Width = Bmp.Width; Stride = BmpData.Stride; Scan0 = (byte*)BmpData.Scan0;

				int[] Table = new int[256 * 256 * 256];
				int[] NonZero = new int[Width * Height];
				int[] Map = new int[256];

				if (Delta > 2)
					HalfDelta = Delta / 2 - 1;
				else
					HalfDelta = 0;

				for (Y = 0; Y < 256; Y++)
				{
					Map[Y] = ((Y + HalfDelta) / Delta) * Delta;
					if (Map[Y] > 255) Map[Y] = 255;
				}
				for (Y = 0; Y < Height; Y++)
				{
					Pointer = Scan0 + Stride * Y;
					for (X = 0; X < Width; X++)
					{
						Index = ((Map[*Pointer]) << 16 )+ ((Map[*(Pointer + 1)])<<8) +( Map[*(Pointer + 2)] ) ;
						if (Table[Index] == 0&&Index>10)                  //      还没有出现过该颜色
						{
							
							NonZero[TotalColorAmount] = Index;  //      记录下有颜色的位置，同时也记录下了该颜色
							TotalColorAmount++;                 //      颜色总数+1
						}
						Table[Index]++;                         //      对应的颜色数加1
						Pointer += 3;                          //      移动到下一个像素
					}
				}
				MajorColor[] Result = new MajorColor[TotalColorAmount];
				for (Y = 0; Y < TotalColorAmount; Y++)
				{
					Result[Y].Amount = Table[NonZero[Y]];
					Result[Y].Color = NonZero[Y];
				}
				Array.Sort(Result);                             // 系统自带的这个排序算法比一般自己写的都要快
				Array.Reverse(Result);
			int nowValidColorNum = 0;
			for (Y = 0; Y < TotalColorAmount ; Y++)
			{
				var judgeColor = Color.FromArgb(Result[Y].Color);
				var colorDistance = Math.Abs(judgeColor.R - judgeColor.G) + Math.Abs(judgeColor.B - judgeColor.G) + Math.Abs(judgeColor.R - judgeColor.B);
				if (colorDistance > 30)
				{
					MC.Add(new MajorColor(Result[Y].Color, Result[Y].Amount));
					if (++nowValidColorNum >= PCAAmount) break;
				}
				
				
			}
				Bmp.UnlockBits(BmpData);
				GC.Collect();                                   // 立即释放掉分配的64MB的内存
				return MC;
			}
		
	}
}
