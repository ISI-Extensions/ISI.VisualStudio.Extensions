#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ISI.Extensions.Extensions;


namespace ISI.VisualStudio.Extensions
{
	//https://stackoverflow.com/questions/12261598/browse-multiple-folders-using-folderbrowserdialog-in-windows-application
	internal class FolderSelectDialog : IDisposable
	{
		private System.Windows.Forms.OpenFileDialog _openFileDialog = null;

		public FolderSelectDialog()
		{
			_openFileDialog = new System.Windows.Forms.OpenFileDialog();
			_openFileDialog.Filter = "Folders|\n";
			_openFileDialog.AddExtension = false;
			_openFileDialog.CheckFileExists = false;
			_openFileDialog.DereferenceLinks = true;
			_openFileDialog.Multiselect = Multiselect;

		}

		public string InitialDirectory
		{
			get => _openFileDialog.InitialDirectory;
			set => _openFileDialog.InitialDirectory = value.NullCheckedAny() ? value : Environment.CurrentDirectory;
		}

		public string Title
		{
			get => _openFileDialog.Title;
			set => _openFileDialog.Title = value ?? "Select a folder";
		}

		public bool Multiselect
		{
			get => _openFileDialog.Multiselect;
			set => _openFileDialog.Multiselect = value == false ? false : value;
		}

		public string FileName => _openFileDialog.FileName;

		public string[] FileNames => _openFileDialog.FileNames;

		public void Dispose()
		{
			_openFileDialog?.Dispose();
			_openFileDialog = null;
		}

		public bool ShowDialog()
		{
			return ShowDialog(IntPtr.Zero);
		}

		public bool ShowDialog(IntPtr hWndOwner)
		{
			var flag = false;

			if (Environment.OSVersion.Version.Major >= 6)
			{
				var reflector = new Reflector("System.Windows.Forms");

				uint num = 0;
				
				var typeIFileDialog = reflector.GetType("FileDialogNative.IFileDialog");
				
				var dialog = reflector.Call(_openFileDialog, "CreateVistaDialog");
				
				reflector.Call(_openFileDialog, "OnBeforeVistaDialog", dialog);
				
				var options = (uint)reflector.CallAs(typeof(System.Windows.Forms.FileDialog), _openFileDialog, "GetOptions");
				options |= (uint)reflector.GetEnum("FileDialogNative.FOS", "FOS_PICKFOLDERS");
				
				reflector.CallAs(typeIFileDialog, dialog, "SetOptions", options);
				
				var pfde = reflector.New("FileDialog.VistaDialogEvents", _openFileDialog);
				
				var parameters = new object[] { pfde, num };
				
				reflector.CallAs(typeIFileDialog, dialog, "Advise", parameters);
				
				num = (uint)parameters[1];
				try
				{
					var num2 = (int)reflector.CallAs(typeIFileDialog, dialog, "Show", hWndOwner);
					flag = 0 == num2;
				}
				finally
				{
					reflector.CallAs(typeIFileDialog, dialog, "Unadvise", num);
					GC.KeepAlive(pfde);
				}
			}
			else
			{
				var folderBrowserDialog = new FolderBrowserDialog();
				folderBrowserDialog.Description = this.Title;
				folderBrowserDialog.SelectedPath = this.InitialDirectory;
				folderBrowserDialog.ShowNewFolderButton = false;

				if (folderBrowserDialog.ShowDialog(new WindowWrapper(hWndOwner)) != DialogResult.OK)
				{
					return false;
				}

				_openFileDialog.FileName = folderBrowserDialog.SelectedPath;
				
				flag = true;
			}

			return flag;
		}

		public class WindowWrapper : System.Windows.Forms.IWin32Window
		{
			private readonly IntPtr _hwnd;

			public WindowWrapper(IntPtr handle)
			{
				_hwnd = handle;
			}

			public IntPtr Handle => _hwnd;
		}

		public class Reflector
		{
			private string m_ns;
			private Assembly m_asmb;

			public Reflector(string ns)
				: this(ns, ns)
			{
			}

			public Reflector(string an, string ns)
			{
				m_ns = ns;
				m_asmb = null;

				foreach (var aN in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
				{
					if (aN.FullName.StartsWith(an))
					{
						m_asmb = Assembly.Load(aN);
						break;
					}
				}
			}

			public Type GetType(string typeName)
			{
				Type type = null;
				var names = typeName.Split('.');

				if (names.Length > 0)
				{
					type = m_asmb.GetType(m_ns + "." + names[0]);
				}

				for (var i = 1; i < names.Length; ++i)
				{
					type = type.GetNestedType(names[i], BindingFlags.NonPublic);
				}

				return type;
			}

			public object New(string name, params object[] parameters)
			{
				var type = GetType(name);
				var ctorInfos = type.GetConstructors();
				foreach (var ci in ctorInfos)
				{
					try
					{
						return ci.Invoke(parameters);
					}
					catch
					{
					}
				}

				return null;
			}

			public object Call(object obj, string func, params object[] parameters)
			{
				return CallAs(obj.GetType(), obj, func, parameters);
			}

			public object CallAs(Type type, object obj, string func, params object[] parameters)
			{
				var methodInfo = type.GetMethod(func, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

				return methodInfo.Invoke(obj, parameters);
			}

			public object Get(object obj, string prop)
			{
				return GetAs(obj.GetType(), obj, prop);
			}

			public object GetAs(Type type, object obj, string prop)
			{
				var propertyInfo = type.GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

				return propertyInfo.GetValue(obj, null);
			}

			public object GetEnum(string typeName, string name)
			{
				var type = GetType(typeName);
				
				var fieldInfo = type.GetField(name);

				return fieldInfo.GetValue(null);
			}
		}
	}
}