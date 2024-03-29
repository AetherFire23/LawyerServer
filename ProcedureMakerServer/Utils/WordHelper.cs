﻿
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ProcedureMakerServer.TemplateManagement;

namespace ProcedureMakerServer.Utils;

public static class WordHelper
{
	private const string DocumentFolderPath = "DocsTemplates/";

	public static byte[] ReadDocumentBytesAt(string wordDocumentName)
	{
		string path = DocumentFolderPath + wordDocumentName;
		if (!File.Exists(path)) throw new Exception("Template did not exist, file should match enum type value");

		byte[] bytes = File.ReadAllBytes(path);

		return bytes;
	}

	public static WordDocGenerationInfo OpenDocumentFromBytes(byte[] bytes)
	{
		WordDocGenerationInfo wordInfo = new WordDocGenerationInfo();
		using (MemoryStream stream = new MemoryStream(bytes, 0, (int)bytes.Length))
		{
			using WordprocessingDocument readonlyCopy = WordprocessingDocument.Open(stream, false);
			using WordprocessingDocument pack = readonlyCopy.Clone(wordInfo.FilePath);
		}


		return wordInfo;
	}

	public static ParagraphProperties GetPropertiesOrCreate(this Paragraph self)
	{
		if (self.ParagraphProperties is null || !self.ParagraphProperties.Any())
		{
			self.ParagraphProperties = new ParagraphProperties();
		}
		return self.ParagraphProperties;
	}
}
