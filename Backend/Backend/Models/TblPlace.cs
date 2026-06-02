using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class TblPlace
{
    public int PlaceId { get; set; }

    public string PlaceName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
