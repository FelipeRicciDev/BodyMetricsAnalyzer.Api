using System.Diagnostics;

namespace API.Services.Ocr;

public sealed class OcrService
{
    public string ReadText(Stream imageStream)
    {
        var tempImage = Path.GetTempFileName() + ".png";
        var tempOut = Path.GetTempFileName();

        var tesseractExe =
            OperatingSystem.IsWindows()
                ? @"C:\Program Files\Tesseract-OCR\tesseract.exe"
                : "tesseract";

        try
        {
            using (var fs = File.Create(tempImage))
                imageStream.CopyTo(fs);

            var psi = new ProcessStartInfo
            {
                FileName = tesseractExe,
                Arguments = $"{tempImage} {tempOut} -l por+eng --psm 6 --oem 1",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi)!;

            if (!process.WaitForExit(60_000))
            {
                try
                {
                    process.Kill(entireProcessTree: true);
                }
                catch { }

                throw new TimeoutException("OCR processing timed out.");
            }

            return File.Exists(tempOut + ".txt")
                ? File.ReadAllText(tempOut + ".txt")
                : string.Empty;
        }
        finally
        {
            if (File.Exists(tempImage))
                File.Delete(tempImage);

            if (File.Exists(tempOut + ".txt"))
                File.Delete(tempOut + ".txt");
        }
    }
}
