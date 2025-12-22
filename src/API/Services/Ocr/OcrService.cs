namespace API.Services.Ocr;

public sealed class OcrService
{
    private const string Language = "por";

    private readonly string _tessdataPath =
        Path.Combine(AppContext.BaseDirectory, "tessdata");

    public string ReadText(Stream imageStream)
    {
        using var engine = new TesseractEngine(
            _tessdataPath,
            Language,
            EngineMode.LstmOnly
        );

        using var ms = new MemoryStream();
        imageStream.CopyTo(ms);

        using var pix = Pix.LoadFromMemory(ms.ToArray());
        using var page = engine.Process(pix);

        return page.GetText();
    }
}