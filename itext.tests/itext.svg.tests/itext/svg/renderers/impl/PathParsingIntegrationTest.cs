/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using iText.Svg.Exceptions;
using iText.Svg.Renderers;
using iText.Test;
using iText.Test.Attributes;

namespace iText.Svg.Renderers.Impl {
    public class PathParsingIntegrationTest : SvgIntegrationTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/svg/renderers/impl/PathParsingIntegrationTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/svg/renderers/impl/PathParsingIntegrationTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            ITextTest.CreateDestinationFolder(destinationFolder);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void NormalTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "normal");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void MixTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "mix");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void NoWhitespace() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "noWhitespace");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ZOperator() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "zOperator");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void MissingOperandArgument() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "missingOperandArgument");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void DecimalPointHandlingTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "decimalPointHandling");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void InvalidOperatorTest() {
            NUnit.Framework.Assert.That(() =>  {
                ConvertAndCompareVisually(sourceFolder, destinationFolder, "invalidOperator");
            }
            , NUnit.Framework.Throws.InstanceOf<SvgProcessingException>())
;
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void InvalidOperatorCSensTest() {
            NUnit.Framework.Assert.That(() =>  {
                ConvertAndCompareVisually(sourceFolder, destinationFolder, "invalidOperatorCSens");
            }
            , NUnit.Framework.Throws.InstanceOf<SvgProcessingException>())
;
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void MoreThanOneHParam() {
            // TODO-2331 Update the cmp after the issue is resolved
            // UPD: Seems to be fixed now, but leaving the TODO and issue open because the scope of the issue might be bigger than
            // this test
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "moreThanOneHParam");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void NegativeAfterPositiveHandlingTest01() {
            //TODO update after DEVSIX-2331 - several (negative) line operators
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "negativeAfterPositiveHandling");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void NegativeAfterPositiveHandlingTest02() {
            //TODO update after DEVSIX-2333 (negative viewbox) fix
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "negativeAfterPositiveHandlingExtendedViewbox");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void InsignificantSpacesTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "insignificantSpaces");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PrecedingSpacesTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "precedingSpaces");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        [LogMessage(SvgLogMessageConstant.UNMAPPEDTAG)]
        public virtual void Text_path_Test() {
            //TODO: update cmp-file after DEVSIX-2255
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "textpath");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        [LogMessage(SvgLogMessageConstant.UNMAPPEDTAG)]
        public virtual void TextPathExample() {
            //TODO: update when DEVSIX-2255 implemented
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "textPathExample");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathH() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "pathH");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathV() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "pathV");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathHV() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "pathHV");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathRelativeAbsoluteCombinedTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "pathRelativeAbsoluteCombined");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void PathHVExponential() {
            // TODO DEVSIX-2906 This file has large numbers (2e+10) in it. At the moment we do not post-process such big numbers
            // and simply print them to the output PDF. Not all the viewers are able to process such large numbers
            // and hence different results in different viewers. Acrobat is not able to process the numbers
            // and the result is garbled visual representation. GhostScript, however, renders the PDF just fine
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "pathHVExponential");
        }
    }
}
