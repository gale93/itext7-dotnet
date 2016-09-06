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
using iText.IO.Util;
using iText.Kernel;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Colorspace;
using iText.Kernel.Pdf.Extgstate;
using iText.Kernel.Pdf.Xobject;

namespace iText.Kernel.Pdf {
    /// <summary>
    /// Wrapper class that represent resource dictionary - that define named resources
    /// used by content streams operators.
    /// </summary>
    /// <remarks>
    /// Wrapper class that represent resource dictionary - that define named resources
    /// used by content streams operators. (ISO 32000-1, 7.8.3 Resource Dictionaries)
    /// </remarks>
    public class PdfResources : PdfObjectWrapper<PdfDictionary> {
        private const String F = "F";

        private const String Im = "Im";

        private const String Fm = "Fm";

        private const String Gs = "Gs";

        private const String Pr = "Pr";

        private const String Cs = "Cs";

        private const String P = "P";

        private const String Sh = "Sh";

        private IDictionary<PdfObject, PdfName> resourceToName = new Dictionary<PdfObject, PdfName>();

        private PdfResources.ResourceNameGenerator fontNamesGen = new PdfResources.ResourceNameGenerator(PdfName.Font
            , F);

        private PdfResources.ResourceNameGenerator imageNamesGen = new PdfResources.ResourceNameGenerator(PdfName.
            XObject, Im);

        private PdfResources.ResourceNameGenerator formNamesGen = new PdfResources.ResourceNameGenerator(PdfName.XObject
            , Fm);

        private PdfResources.ResourceNameGenerator egsNamesGen = new PdfResources.ResourceNameGenerator(PdfName.ExtGState
            , Gs);

        private PdfResources.ResourceNameGenerator propNamesGen = new PdfResources.ResourceNameGenerator(PdfName.Properties
            , Pr);

        private PdfResources.ResourceNameGenerator csNamesGen = new PdfResources.ResourceNameGenerator(PdfName.ColorSpace
            , Cs);

        private PdfResources.ResourceNameGenerator patternNamesGen = new PdfResources.ResourceNameGenerator(PdfName
            .Pattern, P);

        private PdfResources.ResourceNameGenerator shadingNamesGen = new PdfResources.ResourceNameGenerator(PdfName
            .Shading, Sh);

        private bool readOnly = false;

        private bool isModified = false;

        /// <summary>Creates new instance from given dictionary.</summary>
        /// <param name="pdfObject">
        /// the
        /// <see cref="PdfDictionary"/>
        /// object from which the resource object will be created.
        /// </param>
        public PdfResources(PdfDictionary pdfObject)
            : base(pdfObject) {
            BuildResources(pdfObject);
        }

        /// <summary>Creates new instance from empty dictionary.</summary>
        public PdfResources()
            : this(new PdfDictionary()) {
        }

        /// <summary>Adds font to resources and register PdfFont in the document for further flushing.</summary>
        /// <returns>added font resource name.</returns>
        public virtual PdfName AddFont(PdfDocument pdfDocument, PdfFont font) {
            pdfDocument.GetDocumentFonts().Add(font);
            return AddResource(font, fontNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="iText.Kernel.Pdf.Xobject.PdfImageXObject"/>
        /// object to the resources.
        /// </summary>
        /// <param name="image">
        /// the
        /// <see cref="iText.Kernel.Pdf.Xobject.PdfImageXObject"/>
        /// to add.
        /// </param>
        /// <returns>added image resource name.</returns>
        public virtual PdfName AddImage(PdfImageXObject image) {
            return AddResource(image, imageNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="PdfStream"/>
        /// to the resources as image.
        /// </summary>
        /// <param name="image">
        /// the
        /// <see cref="PdfStream"/>
        /// to add.
        /// </param>
        /// <returns>added image resources name.</returns>
        public virtual PdfName AddImage(PdfStream image) {
            return AddResource(image, imageNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="PdfObject"/>
        /// to the resources as image.
        /// </summary>
        /// <param name="image">
        /// the
        /// <see cref="PdfObject"/>
        /// to add. Should be
        /// <see cref="PdfStream"/>
        /// .
        /// </param>
        /// <returns>added image resources name.</returns>
        [System.ObsoleteAttribute(@"Will be removed in iText 7.1. Use more safe AddImage(PdfStream) instead.")]
        public virtual PdfName AddImage(PdfObject image) {
            if (image.GetObjectType() != PdfObject.STREAM) {
                throw new PdfException(PdfException.CannotAddNonStreamImageToResources1).SetMessageParams(image.GetType().
                    ToString());
            }
            return AddResource(image, imageNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="iText.Kernel.Pdf.Xobject.PdfFormXObject"/>
        /// object to the resources.
        /// </summary>
        /// <param name="form">
        /// the
        /// <see cref="iText.Kernel.Pdf.Xobject.PdfFormXObject"/>
        /// to add.
        /// </param>
        /// <returns>added form resource name.</returns>
        public virtual PdfName AddForm(PdfFormXObject form) {
            return AddResource(form, formNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="PdfStream"/>
        /// to the resources as form.
        /// </summary>
        /// <param name="form">
        /// the
        /// <see cref="PdfStream"/>
        /// to add.
        /// </param>
        /// <returns>added form resources name.</returns>
        public virtual PdfName AddForm(PdfStream form) {
            return AddResource(form, formNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="PdfObject"/>
        /// to the resources as form.
        /// </summary>
        /// <param name="form">
        /// the
        /// <see cref="PdfObject"/>
        /// to add. Should be
        /// <see cref="PdfStream"/>
        /// .
        /// </param>
        /// <returns>added form resources name.</returns>
        [System.ObsoleteAttribute(@"Will be removed in iText 7.1. Use more safe AddForm(PdfStream) instead.")]
        public virtual PdfName AddForm(PdfObject form) {
            if (form.GetObjectType() != PdfObject.STREAM) {
                throw new PdfException(PdfException.CannotAddNonStreamFormToResources1).SetMessageParams(form.GetType().ToString
                    ());
            }
            return AddResource(form, formNamesGen);
        }

        /// <summary>
        /// Adds the given Form XObject to the current instance of
        /// <see cref="PdfResources"/>
        /// .
        /// </summary>
        /// <param name="form">Form XObject.</param>
        /// <param name="name">Preferred name for the given Form XObject.</param>
        /// <returns>
        /// the
        /// <see cref="PdfName"/>
        /// of the newly added resource
        /// </returns>
        public virtual PdfName AddForm(PdfFormXObject form, PdfName name) {
            if (GetResourceNames(PdfName.XObject).Contains(name)) {
                name = AddResource(form, formNamesGen);
            }
            else {
                AddResource(form.GetPdfObject(), PdfName.XObject, name);
            }
            return name;
        }

        /// <summary>
        /// Adds
        /// <see cref="iText.Kernel.Pdf.Extgstate.PdfExtGState"/>
        /// object to the resources.
        /// </summary>
        /// <param name="extGState">
        /// the
        /// <see cref="iText.Kernel.Pdf.Extgstate.PdfExtGState"/>
        /// to add.
        /// </param>
        /// <returns>added graphics state parameter dictionary resource name.</returns>
        public virtual PdfName AddExtGState(PdfExtGState extGState) {
            return AddResource(extGState, egsNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="PdfDictionary"/>
        /// to the resources as graphics state parameter dictionary.
        /// </summary>
        /// <param name="extGState">
        /// the
        /// <see cref="PdfDictionary"/>
        /// to add.
        /// </param>
        /// <returns>added graphics state parameter dictionary resources name.</returns>
        public virtual PdfName AddExtGState(PdfDictionary extGState) {
            return AddResource(extGState, egsNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="PdfObject"/>
        /// to the resources as graphics state parameter dictionary.
        /// </summary>
        /// <param name="extGState">
        /// the
        /// <see cref="PdfObject"/>
        /// to add. Should be
        /// <see cref="PdfDictionary"/>
        /// .
        /// </param>
        /// <returns>added graphics state parameter dictionary resources name.</returns>
        [System.ObsoleteAttribute(@"Will be removed in iText 7.1. Use more safe AddExtGState(PdfDictionary) instead."
            )]
        public virtual PdfName AddExtGState(PdfObject extGState) {
            if (extGState.GetObjectType() != PdfObject.DICTIONARY) {
                throw new PdfException(PdfException.CannotAddNonDictionaryExtGStateToResources1).SetMessageParams(extGState
                    .GetType().ToString());
            }
            return AddResource(extGState, egsNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="PdfDictionary"/>
        /// to the resources as properties list.
        /// </summary>
        /// <param name="properties">
        /// the
        /// <see cref="PdfDictionary"/>
        /// to add.
        /// </param>
        /// <returns>added properties list resources name.</returns>
        public virtual PdfName AddProperties(PdfDictionary properties) {
            return AddResource(properties, propNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="PdfObject"/>
        /// to the resources as properties list.
        /// </summary>
        /// <param name="properties">
        /// the
        /// <see cref="PdfObject"/>
        /// to add. Should be
        /// <see cref="PdfDictionary"/>
        /// .
        /// </param>
        /// <returns>added properties list resources name.</returns>
        [System.ObsoleteAttribute(@"Will be removed in iText 7.1. Use more safe AddProperties(PdfDictionary) instead."
            )]
        public virtual PdfName AddProperties(PdfObject properties) {
            if (properties.GetObjectType() != PdfObject.DICTIONARY) {
                throw new PdfException(PdfException.CannotAddNonDictionaryPropertiesToResources1).SetMessageParams(properties
                    .GetType().ToString());
            }
            return AddResource(properties, propNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="iText.Kernel.Pdf.Colorspace.PdfColorSpace"/>
        /// object to the resources.
        /// </summary>
        /// <param name="cs">
        /// the
        /// <see cref="iText.Kernel.Pdf.Colorspace.PdfColorSpace"/>
        /// to add.
        /// </param>
        /// <returns>added color space resource name.</returns>
        public virtual PdfName AddColorSpace(PdfColorSpace cs) {
            return AddResource(cs, csNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="PdfObject"/>
        /// to the resources as color space.
        /// </summary>
        /// <param name="colorSpace">
        /// the
        /// <see cref="PdfObject"/>
        /// to add.
        /// </param>
        /// <returns>added color space resources name.</returns>
        public virtual PdfName AddColorSpace(PdfObject colorSpace) {
            return AddResource(colorSpace, csNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="iText.Kernel.Pdf.Colorspace.PdfPattern"/>
        /// object to the resources.
        /// </summary>
        /// <param name="pattern">
        /// the
        /// <see cref="iText.Kernel.Pdf.Colorspace.PdfPattern"/>
        /// to add.
        /// </param>
        /// <returns>added pattern resource name.</returns>
        public virtual PdfName AddPattern(PdfPattern pattern) {
            return AddResource(pattern, patternNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="PdfDictionary"/>
        /// to the resources as pattern.
        /// </summary>
        /// <param name="pattern">
        /// the
        /// <see cref="PdfDictionary"/>
        /// to add.
        /// </param>
        /// <returns>added pattern resources name.</returns>
        public virtual PdfName AddPattern(PdfDictionary pattern) {
            return AddResource(pattern, patternNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="PdfObject"/>
        /// to the resources as pattern.
        /// </summary>
        /// <param name="pattern">
        /// the
        /// <see cref="PdfObject"/>
        /// to add. Should be
        /// <see cref="PdfDictionary"/>
        /// or
        /// <see cref="PdfStream"/>
        /// .
        /// </param>
        /// <returns>added pattern resources name.</returns>
        [System.ObsoleteAttribute(@"Will be removed in iText 7.1. Use more safe AddPattern(PdfDictionary) instead."
            )]
        public virtual PdfName AddPattern(PdfObject pattern) {
            if (pattern is PdfDictionary) {
                throw new PdfException(PdfException.CannotAddNonDictionaryPatternToResources1).SetMessageParams(pattern.GetType
                    ().ToString());
            }
            return AddResource(pattern, patternNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="iText.Kernel.Pdf.Colorspace.PdfShading"/>
        /// object to the resources.
        /// </summary>
        /// <param name="shading">
        /// the
        /// <see cref="iText.Kernel.Pdf.Colorspace.PdfShading"/>
        /// to add.
        /// </param>
        /// <returns>added shading resource name.</returns>
        public virtual PdfName AddShading(PdfShading shading) {
            return AddResource(shading, shadingNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="PdfDictionary"/>
        /// to the resources as shading dictionary.
        /// </summary>
        /// <param name="shading">
        /// the
        /// <see cref="PdfDictionary"/>
        /// to add.
        /// </param>
        /// <returns>added shading dictionary resources name.</returns>
        public virtual PdfName AddShading(PdfDictionary shading) {
            return AddResource(shading, shadingNamesGen);
        }

        /// <summary>
        /// Adds
        /// <see cref="PdfObject"/>
        /// to the resources as shading dictionary.
        /// </summary>
        /// <param name="shading">
        /// the
        /// <see cref="PdfObject"/>
        /// to add. Should be
        /// <see cref="PdfDictionary"/>
        /// or
        /// <see cref="PdfStream"/>
        /// .
        /// </param>
        /// <returns>added shading dictionary resources name.</returns>
        [System.ObsoleteAttribute(@"Will be removed in iText 7.1. Use more safe AddShading(PdfDictionary) instead."
            )]
        public virtual PdfName AddShading(PdfObject shading) {
            if (shading is PdfDictionary) {
                throw new PdfException(PdfException.CannotAddNonDictionaryShadingToResources1).SetMessageParams(shading.GetType
                    ().ToString());
            }
            return AddResource(shading, shadingNamesGen);
        }

        protected internal virtual bool IsReadOnly() {
            return readOnly;
        }

        protected internal virtual void SetReadOnly(bool readOnly) {
            this.readOnly = readOnly;
        }

        protected internal virtual bool IsModified() {
            return isModified;
        }

        protected internal virtual void SetModified(bool isModified) {
            this.isModified = isModified;
        }

        /// <summary>Sets the default color space.</summary>
        /// <param name="defaultCsKey"/>
        /// <param name="defaultCsValue"/>
        public virtual void SetDefaultColorSpace(PdfName defaultCsKey, PdfColorSpace defaultCsValue) {
            AddResource(defaultCsValue.GetPdfObject(), PdfName.ColorSpace, defaultCsKey);
        }

        public virtual void SetDefaultGray(PdfColorSpace defaultCs) {
            SetDefaultColorSpace(PdfName.DefaultGray, defaultCs);
        }

        public virtual void SetDefaultRgb(PdfColorSpace defaultCs) {
            SetDefaultColorSpace(PdfName.DefaultRGB, defaultCs);
        }

        public virtual void SetDefaultCmyk(PdfColorSpace defaultCs) {
            SetDefaultColorSpace(PdfName.DefaultCMYK, defaultCs);
        }

        public virtual PdfName GetResourceName<T>(PdfObjectWrapper<T> resource)
            where T : PdfObject {
            return resourceToName.Get(resource.GetPdfObject());
        }

        public virtual PdfName GetResourceName(PdfObject resource) {
            PdfName resName = resourceToName.Get(resource);
            if (resName == null) {
                resName = resourceToName.Get(resource.GetIndirectReference());
            }
            return resName;
        }

        public virtual ICollection<PdfName> GetResourceNames() {
            ICollection<PdfName> names = new SortedSet<PdfName>();
            // TODO: isn't it better to use HashSet? Do we really need certain order?
            foreach (PdfName resType in GetPdfObject().KeySet()) {
                names.AddAll(GetResourceNames(resType));
            }
            return names;
        }

        public virtual PdfArray GetProcSet() {
            return GetPdfObject().GetAsArray(PdfName.ProcSet);
        }

        public virtual void SetProcSet(PdfArray array) {
            GetPdfObject().Put(PdfName.ProcSet, array);
        }

        public virtual ICollection<PdfName> GetResourceNames(PdfName resType) {
            PdfDictionary resourceCategory = GetPdfObject().GetAsDictionary(resType);
            return resourceCategory == null ? new SortedSet<PdfName>() : resourceCategory.KeySet();
        }

        // TODO: TreeSet or HashSet enough?
        public virtual PdfDictionary GetResource(PdfName pdfName) {
            return GetPdfObject().GetAsDictionary(pdfName);
        }

        protected internal override bool IsWrappedObjectMustBeIndirect() {
            return false;
        }

        internal virtual PdfName AddResource<T>(PdfObjectWrapper<T> resource, PdfResources.ResourceNameGenerator nameGen
            )
            where T : PdfObject {
            return AddResource(resource.GetPdfObject(), nameGen);
        }

        protected internal virtual void AddResource(PdfObject resource, PdfName resType, PdfName resName) {
            if (resType.Equals(PdfName.XObject)) {
                CheckAndResolveCircularReferences(resource);
            }
            if (readOnly) {
                SetPdfObject(GetPdfObject().Clone(JavaCollectionsUtil.EmptyList<PdfName>()));
                BuildResources(GetPdfObject());
                isModified = true;
                readOnly = false;
            }
            if (GetPdfObject().ContainsKey(resType) && GetPdfObject().GetAsDictionary(resType).ContainsKey(resName)) {
                return;
            }
            resourceToName[resource] = resName;
            PdfDictionary resourceCategory = GetPdfObject().GetAsDictionary(resType);
            if (resourceCategory == null) {
                GetPdfObject().Put(resType, resourceCategory = new PdfDictionary());
            }
            resourceCategory.Put(resName, resource);
            PdfDictionary resDictionary = (PdfDictionary)GetPdfObject().Get(resType);
            if (resDictionary == null) {
                GetPdfObject().Put(resType, resDictionary = new PdfDictionary());
            }
            resDictionary.Put(resName, resource);
        }

        internal virtual PdfName AddResource(PdfObject resource, PdfResources.ResourceNameGenerator nameGen) {
            PdfName resName = GetResourceName(resource);
            if (resName == null) {
                resName = nameGen.Generate(this);
                AddResource(resource, nameGen.GetResourceType(), resName);
            }
            return resName;
        }

        protected internal virtual void BuildResources(PdfDictionary dictionary) {
            foreach (PdfName resourceType in dictionary.KeySet()) {
                if (GetPdfObject().Get(resourceType) == null) {
                    GetPdfObject().Put(resourceType, new PdfDictionary());
                }
                PdfDictionary resources = dictionary.GetAsDictionary(resourceType);
                if (resources == null) {
                    continue;
                }
                foreach (PdfName resourceName in resources.KeySet()) {
                    PdfObject resource = resources.Get(resourceName, false);
                    resourceToName[resource] = resourceName;
                }
            }
        }

        private void CheckAndResolveCircularReferences(PdfObject pdfObject) {
            // Consider the situation when an XObject references the resources of the first page.
            // We add this XObject to the first page, there is no need to resolve any circular references
            // and then we flush this object and try to add it to the second page.
            // Now there are circular references and we cannot resolve them because the object is flushed
            // and we cannot get resources.
            // On the other hand, this situation may occur any time when object is already flushed and we
            // try to add it to resources and it seems difficult to overcome this without keeping /Resources key value.
            if (pdfObject is PdfDictionary && !pdfObject.IsFlushed()) {
                PdfDictionary pdfXObject = (PdfDictionary)pdfObject;
                PdfObject pdfXObjectResources = pdfXObject.Get(PdfName.Resources);
                if (pdfXObjectResources != null && pdfXObjectResources.GetIndirectReference() != null) {
                    if (pdfXObjectResources.GetIndirectReference().Equals(GetPdfObject().GetIndirectReference())) {
                        PdfObject cloneResources = GetPdfObject().Clone();
                        cloneResources.MakeIndirect(GetPdfObject().GetIndirectReference().GetDocument());
                        pdfXObject.Put(PdfName.Resources, cloneResources.GetIndirectReference());
                    }
                }
            }
        }

        /// <summary>Represents a resource name generator.</summary>
        /// <remarks>
        /// Represents a resource name generator. The generator takes into account
        /// the names of already existing resources thus providing us a unique name.
        /// The name consists of the following parts: prefix (literal) and number.
        /// </remarks>
        internal class ResourceNameGenerator {
            private PdfName resourceType;

            private int counter;

            private String prefix;

            /// <summary>
            /// Constructs an instance of
            /// <see cref="ResourceNameGenerator"/>
            /// class.
            /// </summary>
            /// <param name="resourceType">
            /// Type of resource (
            /// <see cref="PdfName.XObject"/>
            /// ,
            /// <see cref="PdfName.Font"/>
            /// etc).
            /// </param>
            /// <param name="prefix">Prefix used for generating names.</param>
            /// <param name="seed">
            /// Seed for the value which is appended to the number each time
            /// new name is generated.
            /// </param>
            public ResourceNameGenerator(PdfName resourceType, String prefix, int seed) {
                this.prefix = prefix;
                this.resourceType = resourceType;
                this.counter = seed;
            }

            /// <summary>
            /// Constructs an instance of
            /// <see cref="ResourceNameGenerator"/>
            /// class.
            /// </summary>
            /// <param name="resourceType">
            /// Type of resource (
            /// <see cref="PdfName.XObject"/>
            /// ,
            /// <see cref="PdfName.Font"/>
            /// etc).
            /// </param>
            /// <param name="prefix">Prefix used for generating names.</param>
            public ResourceNameGenerator(PdfName resourceType, String prefix)
                : this(resourceType, prefix, 1) {
            }

            public virtual PdfName GetResourceType() {
                return resourceType;
            }

            /// <summary>Generates new (unique) resource name.</summary>
            /// <returns>New (unique) resource name.</returns>
            public virtual PdfName Generate(PdfResources resources) {
                PdfName newName = new PdfName(prefix + counter++);
                PdfDictionary r = resources.GetPdfObject();
                if (r.ContainsKey(resourceType)) {
                    while (r.GetAsDictionary(resourceType).ContainsKey(newName)) {
                        newName = new PdfName(prefix + counter++);
                    }
                }
                return newName;
            }
        }
    }
}
