// AtRules.cs
//
// Copyright 2010 Microsoft Corporation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using NUglify.Css;
using NUglify.Tests.Css.Common;
using NUnit.Framework;

namespace NUglify.Tests.Css
{
    /// <summary>
    /// Summary description for AtRules
    /// </summary>
    [TestFixture]
    public class Bugs
    {
        [Test]
        public void Bug33()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug56()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug74()
        {
            TestHelper.Instance.RunTest();
        }

        [Test]
        public void Bug181()
        {
	        var uglifyResult = Uglify.Css("p { color: red; }", new CssSettings { Indent = "   ", OutputMode = OutputMode.MultipleLines });
	        Assert.AreEqual("p\n{\n   color: #f00\n}", uglifyResult.Code);
        }

        [Test]
        public void Bug189()
        {
	        TestHelper.Instance.RunTest("-lines:m -comments:all");
        }
    }
}