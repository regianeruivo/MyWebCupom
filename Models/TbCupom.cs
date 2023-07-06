using System;
using System.Collections.Generic;

namespace MyWebCupom.Models;

public partial class TbCupom
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string ?UrlCupom { get; set; }
}
