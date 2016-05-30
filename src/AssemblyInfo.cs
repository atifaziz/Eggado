﻿#region Copyright (c) 2011 Atif Aziz. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

#region Imports

using System.Reflection;
using CLSCompliantAttribute = System.CLSCompliantAttribute;
using ComVisible = System.Runtime.InteropServices.ComVisibleAttribute;

#endregion

//
// General description
//

[assembly: AssemblyTitle("Eggado")]
[assembly: AssemblyDescription("ADO.NET Modernizer")]
[assembly: AssemblyProduct("Eggado")]
[assembly: AssemblyCompany("https://bitbucket.org/raboof/eggado")]
[assembly: AssemblyCopyright("Copyright \u00a9 2011, Atif Aziz. All rights reserved.")]
[assembly: AssemblyCulture("")]

//
// Version information
//

[assembly: AssemblyVersion("1.0.18429.0")]
[assembly: AssemblyFileVersion("1.0.18429.1439")]

//
// Configuration (test, debug, release)
//

#if TEST
    #if !DEBUG
    #warning Test builds should be compiled using the DEBUG configuration.
    #endif
    [assembly: AssemblyConfiguration("Test")]
#elif DEBUG
    [assembly: AssemblyConfiguration("Debug")]
#else
    [assembly: AssemblyConfiguration("Release")]
#endif

//
// COM visibility and CLS compliance
//

[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]