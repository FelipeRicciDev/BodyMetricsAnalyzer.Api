namespace API.Services.Pdf;

public sealed class PdfTextExtractor : IPdfTextExtractor
{
    public Task<string> ExtractTextAsync(
        Stream pdfStream,
        CancellationToken ct = default)
    {
        using var doc = PdfDocument.Open(pdfStream);

        var sb = new StringBuilder();

        foreach (var page in doc.GetPages())
        {
            sb.AppendLine(page.Text);
        }

        return Task.FromResult(sb.ToString());
    }
}