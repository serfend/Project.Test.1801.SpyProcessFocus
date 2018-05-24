using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace DotNet4.Utilities
{
	namespace UtilReg
	{

		///1.注册表基项静态域
		public enum RegDomain
		{
			///  对应于 HKEY_CLASSES_ROOT 主键
			ClassesRoot = 0,
			///  对应于 HKEY_CURRENT_USER 主键
			CurrentUser = 1,
			///  对应于HKEY_LOCAL_MACHINE 主键
			LocalMachine = 2,
			///  对应于HKEY_USER 主键
			User = 3,
			///  对应于 HEKY_CURRENT_CONFIG 主键
			CurrentConfig = 4,
			///  对应于 HKEY_DYN_DATA 主键
			//DynDa = 5,
			///  对应于 HKEY_PERFORMANCE_DATA 主键
			PerformanceData = 6,
		}

		///2.指定在注册表中存储值时所用的数据类型，或标识注册表中某个值的数据类型
		public enum RegValueKind
		{
			///  指示一个不受支持的注册表数据类型。例如，不支持Microsoft Win32 API  注册表数据类型REG_RESOURCE_LIST。使用此值指定
			Unknown = 0,
			///  指定一个以Null  结尾的字符串。此值与Win32 API  注册表数据类型REG_SZ  等效。
			String = 1,
			///  指定一个以NULL  结尾的字符串，该字符串中包含对环境变量（如%PATH%，当值被检索时，就会展开）的未展开的引用。
			ExpandString = 2,
			///  此值与Win32 API 注册表数据类型REG_EXPAND_SZ  等效。
			Binary = 3,
			///  指定任意格式的二进制数据。此值与Win32 API  注册表数据类型REG_BINARY  等效。
			DWord = 4,
			///  指定一个32  位二进制数。此值与Win32 API  注册表数据类型REG_DWORD  等效。
			///  指定一个以NULL  结尾的字符串数组，以两个空字符结束。此值与Win32 API  注册表数据类型REG_MULTI_SZ  等效。
			MultiString = 5,
			///  指定一个64  位二进制数。此值与Win32 API  注册表数据类型REG_QWORD  等效。
			QWord = 6,
		}

		///3.注册表操作类
		public class Reg
		{
			#region 字段定义  
			/// <summary>  
			/// 注册表项名称  
			/// </summary>  
			private string _subkey;
			/// <summary>  
			/// 注册表基项域  
			/// </summary>  
			private RegDomain _domain;
			#endregion
			#region 属性  
			/// <summary>
			/// 立即实例化本节点
			/// </summary>
			private RegistryKey InnerKey
			{
				get
				{
					RegistryKey tmp = GetRegDomain(_domain);
					RegistryKey target = tmp.OpenSubKey(_subkey, true);
					if (target == null) target = tmp.CreateSubKey(_subkey);
					return target;
				}
			}
			/// <summary>  
			/// 设置注册表项名称  
			/// </summary> 
			public string SubKey
			{
				get { return _subkey; }
				set { _subkey = value; }
			}

			/// <summary>  
			/// 注册表基项域  
			/// </summary>  
			public RegDomain Domain
			{
				get { return _domain; }
				set { _domain = value; }
			}


			#endregion
			#region 构造函数  
			/// <summary>
			/// 当无参数时默认在@"software\serfend\"
			/// </summary>
			public Reg()
			{
				SetNode(@"software\serfend\", RegDomain.CurrentUser);
			}
			public Reg(string subKey)
			{
				SetNode(subKey, RegDomain.CurrentUser);
			}
			/// <summary>  
			/// 其他时间默认自定义  
			/// </summary>  
			/// <param name="subKey">注册表项名称</param>  
			/// <param name="regDomain">注册表基项域</param>  
			public Reg(string subKey, RegDomain regDomain)
			{
				SetNode(subKey, regDomain);
			}
			public Reg(Reg sKey)
			{
				SetNode(sKey.SubKey, sKey.Domain);
			}
			/// <summary>
			/// 设置本节点的位置
			/// </summary>
			/// <param name="subKey">节点路径</param>
			/// <param name="regDomain">所处的域</param>
			public void SetNode(string subKey, RegDomain regDomain)
			{
				///设置注册表项名称  
				_subkey = subKey;
				///设置注册表基项域 
				_domain = regDomain;
			}
			#endregion
			#region 公有方法  
			/// <summary>
			/// 进入到它的子节点 node.in(childName).in(childChildName)...，当不命名时进入自己路径的一个新的实例
			/// </summary>
			/// <param name="childNodeName">子节点的名字</param>
			/// <returns></returns>
			public virtual Reg In(string childNodeName)
			{
				if (childNodeName == string.Empty || childNodeName == null)
				{
					return new Reg(this);
				}
				return new Reg(string.Format(@"{0}\{1}", _subkey, childNodeName), _domain);
			}
			/// <summary>
			/// 删除此节点下面的某个节点。
			/// </summary>
			/// <param name="subKey">要删除的节点名字</param>
			/// <returns></returns>
			public virtual bool Delete(string subKey)
			{
				bool result = false;
				if (subKey == string.Empty || subKey == null) return false;
				RegistryKey key = InnerKey;
				try
				{
					key.DeleteSubKey(subKey);
					result = true;
				}
				catch
				{
					result = false;
				}
				key.Close();
				return result;
			}
			public virtual bool SetInfo(string name, object content) { return SetInfo(name, content, RegValueKind.String); }
			public virtual bool SetInfo(string name, object content, RegValueKind regValueKind)
			{
				
				if (name == string.Empty || name == null) { return false; }
				RegistryKey key = InnerKey;
				bool result = false;
				try
				{
					key.SetValue(name, content, GetRegValueKind(regValueKind));
					result = true;
				}
				catch (Exception ex)
				{
					Console.WriteLine(string.Format("Reg.writeReg({0},{1},{2}).{3}", name, content, regValueKind.ToString(), ex.Message));
					result = false;
				}
				finally
				{
					key.Close();
				}
				return result;
			}
			public virtual string  GetInfo(string name)
			{
				return GetInfo(name, "");
			}
			public virtual string  GetInfo(string name, string defaultInfo)
			{
				if (name == string.Empty || name == null) { return ""; }
				RegistryKey key = InnerKey;
				object rel = key.GetValue(name);
				key.Close();
				if (rel == null || rel.ToString().Length==0)
				{
					if (defaultInfo != null)
						this.SetInfo(name, defaultInfo);
					return defaultInfo;
				}
				string tmp = rel.ToString();
				if(tmp.Contains('\0'))tmp=tmp.Substring(0, tmp.ToString().IndexOf('\0'));
				return tmp;
			}
			public virtual string[] EnumAllKey()
			{
				return InnerKey.GetValueNames();
			}
			#endregion
			#region 受保护方法
			protected RegistryKey GetRegDomain(RegDomain regDomain)
			{
				RegistryKey key;

				#region 判断注册表基项域
				switch (regDomain)
				{
					case RegDomain.ClassesRoot:
						key = Registry.ClassesRoot; break;
					case RegDomain.CurrentUser:
						key = Registry.CurrentUser; break;
					case RegDomain.LocalMachine:
						key = Registry.LocalMachine; break;
					case RegDomain.User:
						key = Registry.Users; break;
					case RegDomain.CurrentConfig:
						key = Registry.CurrentConfig; break;
					//case RegDomain.DynDa:
					//key = Registry.DynData; break;
					case RegDomain.PerformanceData:
						key = Registry.PerformanceData; break;
					default:
						key = Registry.LocalMachine; break;
				}
				#endregion

				return key;
			}
			protected RegistryValueKind GetRegValueKind(RegValueKind regValueKind)
			{
				RegistryValueKind regValueK;
				#region 判断注册表数据类型
				switch (regValueKind)
				{
					case RegValueKind.Unknown:
						regValueK = RegistryValueKind.Unknown; break;
					case RegValueKind.String:
						regValueK = RegistryValueKind.String; break;
					case RegValueKind.ExpandString:
						regValueK = RegistryValueKind.ExpandString; break;
					case RegValueKind.Binary:
						regValueK = RegistryValueKind.Binary; break;
					case RegValueKind.DWord:
						regValueK = RegistryValueKind.DWord; break;
					case RegValueKind.MultiString:
						regValueK = RegistryValueKind.MultiString; break;
					case RegValueKind.QWord:
						regValueK = RegistryValueKind.QWord; break;
					default:
						regValueK = RegistryValueKind.String; break;
				}
				#endregion
				return regValueK;
			}

			protected virtual RegistryKey AutoOpenKey(string name)
			{
				if (name == string.Empty || name == null)
				{
					return InnerKey;
				}
				RegistryKey tmp = InnerKey;
				RegistryKey key = tmp.OpenSubKey(name, true);
				if (key == null) key = tmp.CreateSubKey(name);
				tmp.Close();
				return key;
			}
			#endregion
		}

		public static class RegUtil
		{
			public static bool SetRunCurrentVersion(bool run=true,bool getValue=false)
			{
				string path = Application.ExecutablePath;
				var reg = new Reg("Software") { Domain = RegDomain.LocalMachine };
				var runVersion = reg.In("Microsoft").In("Windows").In("CurrentVersion").In("Run");
				if (getValue) return runVersion.GetInfo(Application.ProductName)!="";
				if (run)
				{
					runVersion.SetInfo(Application.ProductName, path);
				}
				else
				{
					runVersion.Delete(Application.ProductName);
				}
				return run;
			}
			public static void SetFormPos(System.Windows.Forms.Form frm)
			{
				if (frm.WindowState != System.Windows.Forms.FormWindowState.Normal) return;
				Reg fn = GetFrmLocationReg(frm);
				fn.SetInfo("x", frm.Left>0?frm.Left:0);
				fn.SetInfo("y", frm.Top > 0 ? frm.Top : 0);
				fn.SetInfo("w", frm.Width > 0 ? frm.Width : 0);
				fn.SetInfo("h", frm.Height > 0 ? frm.Height : 0);
			}
			public static int[] GetFormPos(System.Windows.Forms.Form frm)
			{
				Reg fn = GetFrmLocationReg(frm);
				return   new int[] {
					GetFormInfo(fn,"x", frm.Left),
					GetFormInfo(fn,"y", frm.Top),
					GetFormInfo(fn, "w", frm.Width),
					GetFormInfo(fn, "h", frm.Height)};
			}
			private static int GetFormInfo(Reg fn,string name,int defaultInfo)
			{
				string infoTmp = fn.GetInfo(name, defaultInfo.ToString());
				int tmp = Convert.ToInt32(infoTmp);
				if (tmp <= 0) tmp = defaultInfo;
				return tmp;
			}
			private static Reg GetFrmLocationReg(System.Windows.Forms.Form frm)
			{
				return new Reg().In("Main").In(System.Reflection.Assembly.GetExecutingAssembly().GetName().FullName).In("Setting").In("FromLocation").In(frm.Name);
			}
			private static string CookiesPath, CachePath;
			private static void CreatPath(string path)
			{
				if (!System.IO.Directory.Exists(path))
					System.IO.Directory.CreateDirectory(path);
			}
			/// <summary>
			/// 设置IE浏览器的存储临时文件的路径，一般用于小号多开\n务必与SetIEBrowserTempPathEnd一起使用
			/// </summary>
			public static void SetIEBrowserTempPath()
			{
				string TempPath = System.Environment.CurrentDirectory + "\\Temp";
				CreatPath(TempPath);

				TempPath = string.Format("{0}\\{1}", TempPath, DateTime.Now.ToString("yyyyMMddhhmmss"));
				CreatPath(TempPath);

				Reg tmpReg = new Reg(@"Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", RegDomain.CurrentUser);
				//Reg tmpRegCookies = tmpReg.In("Cookies");
				//Reg tmpCacheCookies = tmpReg.In("Cache");
				Console.WriteLine("设置IE缓存目录"+TempPath);
				string tmpCookiesPath = TempPath + "\\Cookies";
				string tmpCachePath = TempPath + "\\Cache";
				CreatPath(tmpCookiesPath);
				CreatPath(tmpCachePath);
				CookiesPath = tmpReg.GetInfo("Cookies");
				CachePath = tmpReg.GetInfo("Cache");
				tmpReg.SetInfo("Cookies", tmpCookiesPath);
				tmpReg.SetInfo("Cache", tmpCachePath);


				//InternetSetOption 0, INTERNET_OPTION_SETTINGS_CHANGED, vbNull, 0

			}
			/// <summary>
			/// 调用将路径还原
			/// </summary>
			public static void SetIEBrowserTempPathEnd()
			{
				Reg tmpReg = new Reg(@"Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", RegDomain.CurrentUser);
				tmpReg.SetInfo("Cookies", CookiesPath);
				tmpReg.SetInfo("Cache", CachePath);
				Console.WriteLine("设置路径:" + CookiesPath);
			}
		}

	}
}
