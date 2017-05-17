using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class CricketLog
    {
        [Key]
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int totalm { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public double NetRunRate { get; set; }
        public int Points { get; set; }
    }
}