﻿using System;
using System.Collections.Generic;

namespace ConsoleApp.Models;

/// <summary>
/// Album table
/// </summary>
public partial class Album
{
    public int AlbumId { get; set; }

    /// <summary>
    /// Title of album
    /// </summary>
    public string Title { get; set; }

    public int ArtistId { get; set; }

    public virtual Artist Artist { get; set; }

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
