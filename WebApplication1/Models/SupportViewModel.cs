using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace WebApplication1.Models
{
    public class SupportViewModel
    {
        public string Title { get; set; } = "Support Us";
        public string Description { get; set; } = "Help us preserve and share our natural heritage";
    }
}