using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Inst.Util
{
	public class ApplicationInfomations
	{
		private Process handleApp;
		private Icon icon;
		private Color iconMainColor;
		public Icon Icon
		{
			get => icon; set
			{
				icon = value;
				if (icon != null)
				{
					var colors = Inst.Util.Image.ImageDomainColor.PrincipalColorAnalysis(icon.ToBitmap(), 1);
					iconMainColor = Color.FromArgb(255, colors[0].Color%256, (colors[0].Color/256)%256, colors[0].Color/65536);
				}
				else IconMainColor = Color.Gray;
			}
		}
		public Color IconMainColor { get => iconMainColor; set => iconMainColor = value; }



		public ApplicationInfomations()
		{

		}

		public ApplicationInfomations(Process handleApp)
		{
			this.handleApp = handleApp;
			try
			{
				var icon = ApplicationInfomations.GetMaxIcon(handleApp.MainModule.FileName);
				this.Icon = icon;
			}
			catch (Exception ex)
			{
				this.Icon = null;
				Console.WriteLine(ex.Message);
			}
			finally
			{
				if (this.Icon == null)
				{
					try
					{
						this.icon = ApplicationInfomations.GetIcon(handleApp.MainModule.FileName,true);
					}
					catch (Exception)
					{
						this.Icon = null;
					}
				}
			}
			

		}
		#region 获取图标
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct SHFILEINFO
		{
			public IntPtr hIcon;
			public int iIcon;
			public uint dwAttributes;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		}
		[DllImport("Shell32.dll", EntryPoint = "SHGetFileInfo", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

		[DllImport("User32.dll", EntryPoint = "DestroyIcon")]
		public static extern int DestroyIcon(IntPtr hIcon);
		#region API 参数的常量定义

		public enum FileInfoFlags : uint
		{
			SHGFI_ICON = 0x000000100,  //  get icon
			SHGFI_DISPLAYNAME = 0x000000200,  //  get display name
			SHGFI_TYPENAME = 0x000000400,  //  get type name
			SHGFI_ATTRIBUTES = 0x000000800,  //  get attributes
			SHGFI_ICONLOCATION = 0x000001000,  //  get icon location
			SHGFI_EXETYPE = 0x000002000,  //  return exe type
			SHGFI_SYSICONINDEX = 0x000004000,  //  get system icon index
			SHGFI_LINKOVERLAY = 0x000008000,  //  put a link overlay on icon
			SHGFI_SELECTED = 0x000010000,  //  show icon in selected state
			SHGFI_ATTR_SPECIFIED = 0x000020000,  //  get only specified attributes
			SHGFI_LARGEICON = 0x000000000,  //  get large icon
			SHGFI_SMALLICON = 0x000000001,  //  get small icon
			SHGFI_OPENICON = 0x000000002,  //  get open icon
			SHGFI_SHELLICONSIZE = 0x000000004,  //  get shell size icon
			SHGFI_PIDL = 0x000000008,  //  pszPath is a pidl
			SHGFI_USEFILEATTRIBUTES = 0x000000010,  //  use passed dwFileAttribute
			SHGFI_ADDOVERLAYS = 0x000000020,  //  apply the appropriate overlays
			SHGFI_OVERLAYINDEX = 0x000000040   //  Get the index of the overlay
		}
		public enum FileAttributeFlags : uint
		{
			FILE_ATTRIBUTE_READONLY = 0x00000001,
			FILE_ATTRIBUTE_HIDDEN = 0x00000002,
			FILE_ATTRIBUTE_SYSTEM = 0x00000004,
			FILE_ATTRIBUTE_DIRECTORY = 0x00000010,
			FILE_ATTRIBUTE_ARCHIVE = 0x00000020,
			FILE_ATTRIBUTE_DEVICE = 0x00000040,
			FILE_ATTRIBUTE_NORMAL = 0x00000080,
			FILE_ATTRIBUTE_TEMPORARY = 0x00000100,
			FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200,
			FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400,
			FILE_ATTRIBUTE_COMPRESSED = 0x00000800,
			FILE_ATTRIBUTE_OFFLINE = 0x00001000,
			FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000,
			FILE_ATTRIBUTE_ENCRYPTED = 0x00004000
		}
		#endregion
		/// <summary>
		/// 通过文件路径获取文件类型的关联图标
		/// </summary>
		/// <param name="fileName">文件路径</param>
		/// <param name="isLargeIcon">是否返回大图标</param>
		/// <returns>获取到的图标</returns>
		public static Icon GetIcon(string fileName, bool isLargeIcon)
		{
			SHFILEINFO shFileInfo = new SHFILEINFO();
			IntPtr hI;
			FileInfoFlags flag = isLargeIcon ? FileInfoFlags.SHGFI_LARGEICON : FileInfoFlags.SHGFI_SMALLICON;
			hI = SHGetFileInfo(fileName, 0, ref shFileInfo, (uint)Marshal.SizeOf(shFileInfo), (uint)FileInfoFlags.SHGFI_ICON | (uint)FileInfoFlags.SHGFI_USEFILEATTRIBUTES | (uint)flag);

			Icon icon = Icon.FromHandle(shFileInfo.hIcon).Clone() as Icon;

			DestroyIcon(shFileInfo.hIcon); //释放资源
			return icon;
		}
		#endregion
		public static Icon GetMaxIcon(string path) { 
				
			//选中文件中的图标总数
			var iconTotalCount = PrivateExtractIcons(path, 0, 0, 0, null, null, 0, 0);

			//用于接收获取到的图标指针
			IntPtr[] hIcons = new IntPtr[iconTotalCount];
			//对应的图标id
			int[] ids = new int[iconTotalCount];
			//成功获取到的图标个数
			var successCount = PrivateExtractIcons(path, 0, 256, 256, hIcons, ids, iconTotalCount, 0);
			if (successCount == 0) return null;
			Icon icon = Icon.FromHandle(hIcons[0]);
			int maxSize = icon.Width;
			int maxIndex = 0;
			//遍历并保存图标
			for (var i = 1; i < successCount; i++)
			{
				//指针为空，跳过
				if (hIcons[i] == IntPtr.Zero) continue;

				using (var ico = Icon.FromHandle(hIcons[i]))
				{
					if (ico.Width > maxSize)
					{
						DestroyIcon(hIcons[maxIndex]);
						maxSize = ico.Width;
						icon = ico;
						maxIndex = i;
					}
				}
				//内存回收
				
			}
			return icon;
		}


			//details: https://msdn.microsoft.com/en-us/library/windows/desktop/ms648075(v=vs.85).aspx
			//Creates an array of handles to icons that are extracted from a specified file.
			//This function extracts from executable (.exe), DLL (.dll), icon (.ico), cursor (.cur), animated cursor (.ani), and bitmap (.bmp) files. 
			//Extractions from Windows 3.x 16-bit executables (.exe or .dll) are also supported.
			[DllImport("User32.dll")]
			public static extern int PrivateExtractIcons(
				string lpszFile, //file name
				int nIconIndex,  //The zero-based index of the first icon to extract.
				int cxIcon,      //The horizontal icon size wanted.
				int cyIcon,      //The vertical icon size wanted.
				IntPtr[] phicon, //(out) A pointer to the returned array of icon handles.
				int[] piconid,   //(out) A pointer to a returned resource identifier.
				int nIcons,      //The number of icons to extract from the file. Only valid when *.exe and *.dll
				int flags        //Specifies flags that control this function.
			);

		}
	}
