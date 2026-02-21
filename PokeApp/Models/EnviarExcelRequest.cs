using System;

namespace PokeApp.Models;

public class EnviarExcelRequest
{
    public string FileBase64 { get; set; } = string.Empty;
    public string FileName   { get; set; } = string.Empty;
}
