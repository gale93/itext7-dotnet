/*

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

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
using System.Collections.Generic;
using iText.IO.Font;
using iText.Kernel;
using iText.Kernel.Pdf;

namespace iText.Kernel.Font {
    public sealed class PdfFontFactory {
        /// <summary>This is the default encoding to use.</summary>
        private static String DEFAULT_ENCODING = "";

        /// <summary>This is the default value of the <VAR>embedded</VAR> variable.</summary>
        private static bool DEFAULT_EMBEDDING = false;

        /// <summary>This is the default value of the <VAR>cached</VAR> variable.</summary>
        private static bool DEFAULT_CACHED = true;

        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateFont() {
            return CreateFont(FontConstants.HELVETICA, PdfEncodings.WINANSI);
        }

        public static PdfFont CreateFont(PdfDictionary fontDictionary) {
            if (CheckFontDictionary(fontDictionary, PdfName.Type1, false)) {
                return new PdfType1Font(fontDictionary);
            }
            else {
                if (CheckFontDictionary(fontDictionary, PdfName.Type0, false)) {
                    return new PdfType0Font(fontDictionary);
                }
                else {
                    if (CheckFontDictionary(fontDictionary, PdfName.TrueType, false)) {
                        return new PdfTrueTypeFont(fontDictionary);
                    }
                    else {
                        if (CheckFontDictionary(fontDictionary, PdfName.Type3, false)) {
                            return new PdfType3Font(fontDictionary);
                        }
                        else {
                            throw new PdfException(PdfException.DictionaryDoesntHaveSupportedFontData);
                        }
                    }
                }
            }
        }

        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateFont(String fontProgram) {
            return CreateFont(fontProgram, DEFAULT_ENCODING);
        }

        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateFont(String fontProgram, String encoding) {
            return CreateFont(fontProgram, encoding, DEFAULT_EMBEDDING);
        }

        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateTtcFont(byte[] ttc, int ttcIndex, String encoding, bool embedded, bool cached) {
            FontProgram fontProgram = FontProgramFactory.CreateFont(ttc, ttcIndex, cached);
            return CreateFont(fontProgram, encoding, embedded);
        }

        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateTtcFont(String ttc, int ttcIndex, String encoding, bool embedded, bool cached) {
            FontProgram fontProgram = FontProgramFactory.CreateFont(ttc, ttcIndex, cached);
            return CreateFont(fontProgram, encoding, embedded);
        }

        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateFont(String fontProgram, bool embedded) {
            return CreateFont(fontProgram, DEFAULT_ENCODING, embedded);
        }

        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateFont(String fontProgram, String encoding, bool embedded) {
            return CreateFont(fontProgram, encoding, embedded, DEFAULT_CACHED);
        }

        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateFont(String fontProgram, String encoding, bool embedded, bool cached) {
            FontProgram fp = FontProgramFactory.CreateFont(fontProgram, cached);
            return CreateFont(fp, encoding, embedded);
        }

        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateFont(FontProgram fontProgram, String encoding, bool embedded) {
            if (fontProgram == null) {
                return null;
            }
            else {
                if (fontProgram is Type1Font) {
                    return new PdfType1Font((Type1Font)fontProgram, encoding, embedded);
                }
                else {
                    if (fontProgram is TrueTypeFont) {
                        if (PdfEncodings.IDENTITY_H.Equals(encoding) || PdfEncodings.IDENTITY_V.Equals(encoding)) {
                            return new PdfType0Font((TrueTypeFont)fontProgram, encoding);
                        }
                        else {
                            return new PdfTrueTypeFont((TrueTypeFont)fontProgram, encoding, embedded);
                        }
                    }
                    else {
                        if (fontProgram is CidFont) {
                            if (((CidFont)fontProgram).CompatibleWith(encoding)) {
                                return new PdfType0Font((CidFont)fontProgram, encoding);
                            }
                            else {
                                return null;
                            }
                        }
                        else {
                            return null;
                        }
                    }
                }
            }
        }

        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateFont(FontProgram fontProgram, String encoding) {
            return CreateFont(fontProgram, encoding, DEFAULT_EMBEDDING);
        }

        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateFont(FontProgram fontProgram) {
            return CreateFont(fontProgram, DEFAULT_ENCODING);
        }

        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateFont(byte[] fontProgram, String encoding) {
            return CreateFont(fontProgram, encoding, DEFAULT_EMBEDDING);
        }

        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateFont(byte[] fontProgram, bool embedded) {
            return CreateFont(fontProgram, null, embedded);
        }

        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateFont(byte[] fontProgram, String encoding, bool embedded) {
            return CreateFont(fontProgram, encoding, embedded, DEFAULT_CACHED);
        }

        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateFont(byte[] fontProgram, String encoding, bool embedded, bool cached) {
            FontProgram fp = FontProgramFactory.CreateFont(null, fontProgram, cached);
            return CreateFont(fp, encoding, embedded);
        }

        /// <exception cref="System.IO.IOException"/>
        public static PdfType3Font CreateType3Font(PdfDocument document, bool colorized) {
            return new PdfType3Font(document, colorized);
        }

        /// <summary>
        /// Creates
        /// <see cref="PdfFont"/>
        /// based on registered
        /// <see cref="iText.IO.Font.FontProgram"/>
        /// 's.
        /// </summary>
        /// <seealso cref="Register(System.String)"/>
        /// <seealso cref="Register(System.String, System.String)"/>
        /// <seealso cref="RegisterFamily(System.String, System.String, System.String)"/>
        /// <seealso cref="RegisterDirectory(System.String)"/>
        /// <seealso cref="RegisterSystemDirectories()"/>
        /// <seealso cref="GetRegisteredFamilies()"/>
        /// <seealso cref="GetRegisteredFonts()"/>
        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateRegisteredFont(String fontName, String encoding, bool embedded, int style, bool
             cached) {
            FontProgram fp = FontProgramFactory.CreateRegisteredFont(fontName, style, cached);
            return CreateFont(fp, encoding, embedded);
        }

        /// <summary>
        /// Creates
        /// <see cref="PdfFont"/>
        /// based on registered
        /// <see cref="iText.IO.Font.FontProgram"/>
        /// 's.
        /// </summary>
        /// <seealso cref="Register(System.String)"/>
        /// <seealso cref="Register(System.String, System.String)"/>
        /// <seealso cref="RegisterFamily(System.String, System.String, System.String)"/>
        /// <seealso cref="RegisterDirectory(System.String)"/>
        /// <seealso cref="RegisterSystemDirectories()"/>
        /// <seealso cref="GetRegisteredFamilies()"/>
        /// <seealso cref="GetRegisteredFonts()"/>
        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateRegisteredFont(String fontName, String encoding, bool embedded, bool cached) {
            return CreateRegisteredFont(fontName, encoding, embedded, FontConstants.UNDEFINED, cached);
        }

        /// <summary>
        /// Creates
        /// <see cref="PdfFont"/>
        /// based on registered
        /// <see cref="iText.IO.Font.FontProgram"/>
        /// 's.
        /// </summary>
        /// <seealso cref="Register(System.String)"/>
        /// <seealso cref="Register(System.String, System.String)"/>
        /// <seealso cref="RegisterFamily(System.String, System.String, System.String)"/>
        /// <seealso cref="RegisterDirectory(System.String)"/>
        /// <seealso cref="RegisterSystemDirectories()"/>
        /// <seealso cref="GetRegisteredFamilies()"/>
        /// <seealso cref="GetRegisteredFonts()"/>
        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateRegisteredFont(String fontName, String encoding, bool embedded) {
            return CreateRegisteredFont(fontName, encoding, embedded, FontConstants.UNDEFINED);
        }

        /// <summary>
        /// Creates
        /// <see cref="PdfFont"/>
        /// based on registered
        /// <see cref="iText.IO.Font.FontProgram"/>
        /// 's.
        /// </summary>
        /// <seealso cref="Register(System.String)"/>
        /// <seealso cref="Register(System.String, System.String)"/>
        /// <seealso cref="RegisterFamily(System.String, System.String, System.String)"/>
        /// <seealso cref="RegisterDirectory(System.String)"/>
        /// <seealso cref="RegisterSystemDirectories()"/>
        /// <seealso cref="GetRegisteredFamilies()"/>
        /// <seealso cref="GetRegisteredFonts()"/>
        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateRegisteredFont(String fontName, String encoding, bool embedded, int style) {
            return CreateRegisteredFont(fontName, encoding, embedded, style, DEFAULT_CACHED);
        }

        /// <summary>
        /// Creates
        /// <see cref="PdfFont"/>
        /// based on registered
        /// <see cref="iText.IO.Font.FontProgram"/>
        /// 's.
        /// </summary>
        /// <seealso cref="Register(System.String)"/>
        /// <seealso cref="Register(System.String, System.String)"/>
        /// <seealso cref="RegisterFamily(System.String, System.String, System.String)"/>
        /// <seealso cref="RegisterDirectory(System.String)"/>
        /// <seealso cref="RegisterSystemDirectories()"/>
        /// <seealso cref="GetRegisteredFamilies()"/>
        /// <seealso cref="GetRegisteredFonts()"/>
        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateRegisteredFont(String fontName, String encoding) {
            return CreateRegisteredFont(fontName, encoding, false, FontConstants.UNDEFINED);
        }

        /// <summary>
        /// Creates
        /// <see cref="PdfFont"/>
        /// based on registered
        /// <see cref="iText.IO.Font.FontProgram"/>
        /// 's.
        /// </summary>
        /// <seealso cref="Register(System.String)"/>
        /// <seealso cref="Register(System.String, System.String)"/>
        /// <seealso cref="RegisterFamily(System.String, System.String, System.String)"/>
        /// <seealso cref="RegisterDirectory(System.String)"/>
        /// <seealso cref="RegisterSystemDirectories()"/>
        /// <seealso cref="GetRegisteredFamilies()"/>
        /// <seealso cref="GetRegisteredFonts()"/>
        /// <exception cref="System.IO.IOException"/>
        public static PdfFont CreateRegisteredFont(String fontName) {
            return CreateRegisteredFont(fontName, null, false, FontConstants.UNDEFINED);
        }

        /// <summary>Register a font by giving explicitly the font family and name.</summary>
        /// <param name="familyName">the font family</param>
        /// <param name="fullName">the font name</param>
        /// <param name="path">the font path</param>
        public static void RegisterFamily(String familyName, String fullName, String path) {
            FontProgramFactory.RegisterFontFamily(familyName, fullName, path);
        }

        /// <summary>Register a ttf- or a ttc-file.</summary>
        /// <param name="path">the path to a ttf- or ttc-file</param>
        public static void Register(String path) {
            Register(path, null);
        }

        /// <summary>Register a font file and use an alias for the font contained in it.</summary>
        /// <param name="path">the path to a font file</param>
        /// <param name="alias">the alias you want to use for the font</param>
        public static void Register(String path, String alias) {
            FontProgramFactory.RegisterFont(path, alias);
        }

        /// <summary>Register all the fonts in a directory.</summary>
        /// <param name="dir">the directory</param>
        /// <returns>the number of fonts registered</returns>
        public static int RegisterDirectory(String dir) {
            return FontProgramFactory.RegisterFontDirectory(dir);
        }

        /// <summary>Register fonts in some probable directories.</summary>
        /// <remarks>
        /// Register fonts in some probable directories. It usually works in Windows,
        /// Linux and Solaris.
        /// </remarks>
        /// <returns>the number of fonts registered</returns>
        public static int RegisterSystemDirectories() {
            return FontProgramFactory.RegisterSystemFontDirectories();
        }

        /// <summary>Gets a set of registered font names.</summary>
        /// <returns>a set of registered fonts</returns>
        public static ICollection<String> GetRegisteredFonts() {
            return FontProgramFactory.GetRegisteredFonts();
        }

        /// <summary>Gets a set of registered font names.</summary>
        /// <returns>a set of registered font families</returns>
        public static ICollection<String> GetRegisteredFamilies() {
            return FontProgramFactory.GetRegisteredFontFamilies();
        }

        /// <summary>Checks if a certain font is registered.</summary>
        /// <param name="fontname">the name of the font that has to be checked.</param>
        /// <returns>true if the font is found</returns>
        public static bool IsRegistered(String fontname) {
            return FontProgramFactory.IsRegisteredFont(fontname);
        }

        [Obsolete]
        protected internal static bool CheckFontDictionary(PdfDictionary fontDic, PdfName fontType, bool isException
            ) {
            if (fontDic == null || fontDic.Get(PdfName.Subtype) == null || !fontDic.Get(PdfName.Subtype).Equals(fontType
                )) {
                if (isException) {
                    throw new PdfException(PdfException.DictionaryDoesntHave1FontData).SetMessageParams(fontType.GetValue());
                }
                return false;
            }
            return true;
        }
    }
}
