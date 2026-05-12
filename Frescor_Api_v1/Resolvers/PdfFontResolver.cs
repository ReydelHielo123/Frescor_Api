using PdfSharp.Fonts;

namespace Frescor_Api_v1.Resolvers
{
	public class PdfFontResolver : IFontResolver
	{
		private readonly byte[] _fontData;

		public PdfFontResolver()
		{
			var basePath = Path.Combine(
				Directory.GetCurrentDirectory(),
				"Assets",
				"Fonts");

			_fontData = File.ReadAllBytes(
				Path.Combine(basePath, "NotoSans-Variable.ttf"));
		}

		public byte[] GetFont(string faceName)
		{
			return _fontData;
		}

		public FontResolverInfo ResolveTypeface(
			string familyName,
			bool isBold,
			bool isItalic)
		{
			return new FontResolverInfo("NotoSans");
		}
	}
}
