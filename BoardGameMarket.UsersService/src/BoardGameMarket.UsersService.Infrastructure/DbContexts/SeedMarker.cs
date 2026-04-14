using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BoardGameMarket.UsersService.Infrastructure.DbContexts
{
    public class SeedMarker
    {
        public string Name { get; set; } = String.Empty;
        public DateTime SeededAt { get; set; }
    }
}
