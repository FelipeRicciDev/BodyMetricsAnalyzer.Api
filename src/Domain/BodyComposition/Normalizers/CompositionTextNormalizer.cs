namespace Domain.BodyComposition.Normalizers;

public static class CompositionTextNormalizer
{
    #region OCR Text Normalization
    public static string NormalizeOcrText(string text)
    {
        text = text.ToLowerInvariant();

        text = text
            .Replace("massaóssea", "Massa Óssea")
            .Replace("massaprotéica", "Massa Proteica")
            .Replace("massa protéica", "Massa Proteica")
            .Replace("águacorporal", "Água Corporal")
            .Replace("massamuscular", "Massa Muscular")
            .Replace("músculoesquelético", "Músculo Esquelético")
            .Replace("massagorda", "Massa Gorda")
            .Replace("peso(kg)", "Peso");

        text = Regex.Replace(text, @"[.\-]{3,}", " ");
        text = Regex.Replace(text, @"\s+", " ");

        return text;
    }

    public static string NormalizeKey(string key)
    {
        return key.ToLowerInvariant()
            .Replace(" ", "")
            .Replace("á", "a")
            .Replace("à", "a")
            .Replace("ã", "a")
            .Replace("é", "e")
            .Replace("ê", "e")
            .Replace("í", "i")
            .Replace("ó", "o")
            .Replace("ô", "o")
            .Replace("õ", "o")
            .Replace("ú", "u")
            .Replace("ç", "c");
    }

    public static string NormalizePercentual(string raw)
    {
        if (raw == "100.0")
            return "100.0%";

        var digits = Regex.Replace(raw, @"[^\d]", "");

        return digits.Length switch
        {
            3 => $"{digits[0]}{digits[1]}.{digits[2]}%",
            2 => $"{digits[0]}.{digits[1]}%",
            _ => $"{digits}%"
        };
    }
    #endregion
}