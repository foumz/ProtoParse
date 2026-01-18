using System.Text;
using UglyToad.PdfPig;

namespace ProtoParse.Parsing;

public sealed class PdfStatementReader
{
    public string ReadAllText(string pdfPath)
    {
        if (string.IsNullOrWhiteSpace(pdfPath))
        {
            throw new ArgumentException("Le chemin du PDF est vide.", nameof(pdfPath));
        }

        if (!File.Exists(pdfPath))
        {
            throw new FileNotFoundException("Le fichier PDF est introuvable.", pdfPath);
        }

        var builder = new StringBuilder();
        using var document = PdfDocument.Open(pdfPath);

        foreach (var page in document.GetPages())
        {
            builder.AppendLine(page.Text);
        }

        return builder.ToString();
    }
}
