namespace TicketingSystem.DAL.Models;

public class Manifest
{
    public int Id { get; set; }
    public int VenueId { get; set; }
    public Venue Venue { get; set; }
    public ICollection<Section> Sections { get; set; }
}

public class Venue
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public int ManifestId { get; set; }
    public Manifest Manifest { get; set; }
}

public class Section
{
    public int Id { get; set; }
    public int ManifestId { get; set; }
    public Manifest Manifest { get; set; }
    public ICollection<Row> Rows { get; set; }
}

public class Row
{
    public int Id { get; set; }
    public int SectionId { get; set; }
    public Section Section { get; set; }
    public ICollection<Seat> Seats { get; set; }
}

public class Seat
{
    public int Id { get; set; }
    public int RowId { get; set; }
    public Row Row { get; set; }
    public string SeatType { get; set; } // "Designated" or "General Admission"
}