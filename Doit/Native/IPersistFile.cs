﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Doit.Native
{
	[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
	[ComImport]
	[Guid("0000010b-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPersistFile : IPersist
	{
		new void GetClassID(out Guid pClassID);
		[PreserveSig]
		int IsDirty();

		[PreserveSig]
		void Load([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName, STGM dwMode);

		[PreserveSig]
		void Save([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [In, MarshalAs(UnmanagedType.Bool)] bool fRemember);

		[PreserveSig]
		void SaveCompleted([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName);
		[PreserveSig]
		void GetCurFile([In, MarshalAs(UnmanagedType.LPWStr)] string ppszFileName);
	}
}