﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClariusLabs.NuGet.Toolkit
{
	using global::NuPattern.Runtime;
	using global::NuPattern.Runtime.ToolkitInterface;
	using global::System;
	using global::System.Collections.Generic;
	using global::System.ComponentModel;
	using global::System.ComponentModel.Design;
	using global::System.Drawing.Design;
	using Runtime = global::NuPattern.Runtime;

	/// <summary>
	/// Description for NuGetPackage.DefaultView
	/// </summary>
	[Description("Description for NuGetPackage.DefaultView")]
	[ToolkitInterface(ExtensionId = "dea21b17-80c8-451d-ae19-0c0f05a01ecc", DefinitionId = "e0e80bd9-d562-48de-8641-a1406409c64f", ProxyType = typeof(DefaultView))]
	[System.CodeDom.Compiler.GeneratedCode("NuPattern Toolkit Library", "1.3.20.0")]
	public partial interface IDefaultView : IToolkitInterface
	{

		/// <summary>
		/// Gets the parent element.
		/// </summary>
		INuGetPackage Parent { get; }

		/// <summary>
		/// Deletes this element.
		/// </summary>
		void Delete();

		/// <summary>
		/// Gets the generalized interface (<see cref="Runtime.IView"/>) of this element.
		/// </summary>
		Runtime.IView AsView();
	}
}
