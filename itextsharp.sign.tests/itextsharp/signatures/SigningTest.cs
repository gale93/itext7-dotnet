using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iTextSharp.Kernel.Geom;
using iTextSharp.Kernel.Pdf;
using iTextSharp.Kernel.Utils;
using iTextSharp.Test;
using Org.BouncyCastle.Pkcs;

namespace iTextSharp.Signatures
{
	public class SigningTest : ExtendedITextTest
	{
		public static readonly String sourceFolder = NUnit.Framework.TestContext.CurrentContext
			.TestDirectory + "/../../resources/itextsharp/signatures/";

		public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext
			.TestDirectory + "/test/itextsharp/signatures/";

		public static readonly String keystorePath = NUnit.Framework.TestContext.CurrentContext
			.TestDirectory + "/../../resources/itextsharp/signatures/ks.p12";

		public static readonly char[] password = "password".ToCharArray();

		private X509Certificate[] chain;

		private ICipherParameters pk;

		//TODO: add some validation of results in future
		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="Org.BouncyCastle.Security.SecurityUtilityException"/>
		[NUnit.Framework.OneTimeSetUp]
		public virtual void Init() {
            CreateOrClearDestinationFolder(destinationFolder);

            string alias = null;
            Pkcs12Store pk12;

            pk12 = new Pkcs12Store(new FileStream(keystorePath, FileMode.Open, FileAccess.Read), password);

		    foreach (var a in pk12.Aliases) {
                alias = ((string)a);
                if (pk12.IsKeyEntry(alias))
                    break;
            }
            pk = pk12.GetKey(alias).Key;
            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            chain = new X509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
                chain[k] = ce[k].Certificate;
        }

		/// <exception cref="Org.BouncyCastle.Security.GeneralSecurityException"/>
		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.Exception"/>
		[NUnit.Framework.Test]
		public virtual void SimpleSigningTest()
		{
			String src = sourceFolder + "simpleDocument.pdf";
			String fileName = "simpleSignature.pdf";
			String dest = destinationFolder + fileName;
			int x = 36;
			int y = 648;
			int w = 200;
			int h = 100;
			Rectangle rect = new Rectangle(x, y, w, h);
			String fieldName = "Signature1";
			Sign(src, fieldName, dest, chain, pk, DigestAlgorithms.SHA256, PdfSigner.CryptoStandard
				.CADES, "Test 1", "TestCity", rect, false);
			NUnit.Framework.Assert.IsNull(new CompareTool().CompareVisually(dest, sourceFolder
				 + "cmp_" + fileName, destinationFolder, "diff_", new _Dictionary_75()));
		}

		private sealed class _Dictionary_75 : Dictionary<int?, IList<Rectangle>>
		{
			public _Dictionary_75()
			{
				{
					this[1] = iTextSharp.IO.Util.JavaUtil.ArraysAsList(new Rectangle(67, 690, 155, 15
						));
				}
			}
		}

		/// <exception cref="Org.BouncyCastle.Security.GeneralSecurityException"/>
		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.Exception"/>
		[NUnit.Framework.Test]
		public virtual void SigningIntoExistingFieldTest01()
		{
			String src = sourceFolder + "emptySignature01.pdf";
			//field is merged with widget and has /P key
			String fileName = "filledSignatureFields01.pdf";
			String dest = destinationFolder + fileName;
			String fieldName = "Signature1";
			Sign(src, fieldName, dest, chain, pk, DigestAlgorithms.SHA256, PdfSigner.CryptoStandard
				.CADES, "Test 1", "TestCity", null, false);
			NUnit.Framework.Assert.IsNull(new CompareTool().CompareVisually(dest, sourceFolder
				 + "cmp_" + fileName, destinationFolder, "diff_", new _Dictionary_92()));
		}

		private sealed class _Dictionary_92 : Dictionary<int?, IList<Rectangle>>
		{
			public _Dictionary_92()
			{
				{
					this[1] = iTextSharp.IO.Util.JavaUtil.ArraysAsList(new Rectangle(67, 725, 155, 15
						));
				}
			}
		}

		/// <exception cref="Org.BouncyCastle.Security.GeneralSecurityException"/>
		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.Exception"/>
		[NUnit.Framework.Test]
		public virtual void SigningIntoExistingFieldTest02()
		{
			String src = sourceFolder + "emptySignature02.pdf";
			//field is merged with widget and widget doesn't have /P key
			String fileName = "filledSignatureFields02.pdf";
			String dest = destinationFolder + fileName;
			String fieldName = "Signature1";
			Sign(src, fieldName, dest, chain, pk, DigestAlgorithms.SHA256, PdfSigner.CryptoStandard
				.CADES, "Test 1", "TestCity", null, false);
			NUnit.Framework.Assert.IsNull(new CompareTool().CompareVisually(dest, sourceFolder
				 + "cmp_" + fileName, destinationFolder, "diff_", new _Dictionary_109()));
		}

		private sealed class _Dictionary_109 : Dictionary<int?, IList<Rectangle>>
		{
			public _Dictionary_109()
			{
				{
					this[1] = iTextSharp.IO.Util.JavaUtil.ArraysAsList(new Rectangle(67, 725, 155, 15
						));
				}
			}
		}

		/// <exception cref="Org.BouncyCastle.Security.GeneralSecurityException"/>
		/// <exception cref="System.IO.IOException"/>
		[NUnit.Framework.Test]
		public virtual void SigningIntoExistingReuseAppearanceTest()
		{
			String src = sourceFolder + "emptySigWithAppearance.pdf";
			String dest = destinationFolder + "filledSignatureReuseAppearanceFields.pdf";
			String fieldName = "Signature1";
			Sign(src, fieldName, dest, chain, pk, DigestAlgorithms.SHA256, PdfSigner.CryptoStandard
				.CADES, "Test 1", "TestCity", null, true);
		}

		/// <exception cref="Org.BouncyCastle.Security.GeneralSecurityException"/>
		/// <exception cref="System.IO.IOException"/>
		protected internal virtual void Sign(String src, String name, String dest, X509Certificate
			[] chain, ICipherParameters pk, String digestAlgorithm, PdfSigner.CryptoStandard
			 subfilter, String reason, String location, Rectangle rectangleForNewField, bool
			 setReuseAppearance)
		{
			PdfReader reader = new PdfReader(src);
			PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), false
				);
			// Creating the appearance
			PdfSignatureAppearance appearance = signer.GetSignatureAppearance().SetReason(reason
				).SetLocation(location).SetReuseAppearance(setReuseAppearance);
			if (rectangleForNewField != null)
			{
				appearance.SetPageRect(rectangleForNewField);
			}
			signer.SetFieldName(name);
			// Creating the signature
			IExternalSignature pks = new PrivateKeySignature(pk, digestAlgorithm);
			signer.SignDetached(pks, chain, null, null, null, 0, subfilter);
		}
	}
}
