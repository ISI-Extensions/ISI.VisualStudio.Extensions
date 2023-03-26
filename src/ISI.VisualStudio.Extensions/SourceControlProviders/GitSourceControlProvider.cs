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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.VisualStudio.Extensions
{
	[SourceControlProvider]
	public class GitSourceControlProvider : ISourceControlProvider
	{
		public const string SourceControlProviderUuid = "11b8e6d7-c08b-4385-b321-321078cdd1f8";
		
		Guid ISourceControlProvider.SourceControlProviderUuid => Guid.Parse(SourceControlProviderUuid);
		Guid[] ISourceControlProvider.SourceControlProviderPackageUuids => new[] { Guid.Parse("7fe30a77-37f9-4cf2-83dd-96b207028e1b") };

		protected ISI.Extensions.Git.GitApi GitApi { get; }
		protected ISI.Extensions.Scm.ISourceControlClientApi SourceControlClientApi => GitApi;

		public GitSourceControlProvider(ISI.Extensions.Git.GitApi gitApi)
		{
			GitApi = gitApi;
		}

		public bool UsesScp(string path)
		{
			return SourceControlClientApi.UsesScc(path);
		}
	}
}
